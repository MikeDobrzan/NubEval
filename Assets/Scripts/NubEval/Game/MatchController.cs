using Newtonsoft.Json;
using NubEval.Game.Data;
using NubEval.Game.Networking;
using NubEval.Game.Networking.Payload;
using NubEval.PubNubWrapper;
using PubnubApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace NubEval.Game
{
    [System.Serializable]
    public class MatchController : MonoBehaviour,
        IMatchEventSubscriber
    {
        public int MatchID;
        public MatchConfig Config;
        public MatchRoomStatus MatchStatus;
        public DateTime MatchStart;

        [SerializeField] private PlayerInput playerInput;
        [SerializeField] private PlayerVisual avatarPrefab;
        //[SerializeField] private PlayerVisual remotePlayerAvatar;
        [SerializeField] private MatchSeed seed;
        [SerializeField] private PlayerState playerState;
        [SerializeField] private RemoteMatchController _remote;

        [SerializeField] public string _user;
        [SerializeField] public bool debug_isMatchHost;

        private Dictionary<int, IMatchParticipant> participantControllers;

        private Dictionary<int, PlayerVisual> participantAvatars;

        private MatchStateController _matchStateController;
        private PNDevice _device;

        private string CurrentUser => _user;
        [SerializeField] private List<ParticipantData> mock_participantDatas;
        [SerializeField] private List<PlayerData> mockPlayerData;

        [Header("Debug")]
        [SerializeField] MatchStateData debug_matchStateData;
        [SerializeField] bool historyMode;
        [Range(0, 10)]
        [SerializeField] int historyPointer;

        private bool _controllersInitialized;
        private bool matchEnded;

        public void Construct(PNDevice device, UserId user)
        {
            _user = user;
            _device = device;
            _remote.Construct(device);
        }

        public void OnBoot()
        {
            matchEnded = false;

            _matchStateController = new MatchStateController();
            _device.RemoteEventsMatch.SubscribeMatchEvents(this);
            participantAvatars = new Dictionary<int, PlayerVisual>();

            if (debug_isMatchHost)
            {
                MatchStateData initialMatchState = SetInitialState(seed, mock_participantDatas, mockPlayerData);
                _matchStateController.SetState(initialMatchState);
                _remote.PublishStateUpdate(initialMatchState);
            }
        }

        public void Update()
        {
            if (historyMode)
            {
                if (historyPointer > _remote.History.Count - 1)
                {
                    historyPointer = _remote.History.Count - 1;
                }

                UpdateAvatars(_remote.History[historyPointer]);
            }
        }

        private async void OnNetworkMatchStateReceived(MatchStateData matchState)
        {
            if (matchEnded)
            {
                Debug.Log($"The match is Over");
                return;
            }

            if (!_controllersInitialized)
                InitializeParticiapants(matchState);

            _matchStateController.SetState(matchState);
            UpdateAvatars(_matchStateController.CurrentStateData);

            int nextPlayer = _matchStateController.NextPlayerIndex;

            if (_matchStateController.NextPlayerID == CurrentUser)
            {
                int currentPlayerIndex = _matchStateController.GetPlayerIndex(CurrentUser);

                Debug.Log($"Make Your Turn! --> {nextPlayer}");
                var cts = new CancellationTokenSource(20000);

                var playerTurn = PlayerTurn.NewTurn();
                var action = await participantControllers[currentPlayerIndex].RequestActionAsync(playerTurn, cts.Token);

                //Apply the action
                _matchStateController.ApplyPlayerAction(currentPlayerIndex, action);// .SetPlayerState(0, newState);

                OnLocalPlayerEndTurn();
            }
            else
            {
                Debug.Log($"Not your Turn!  wait for --> {nextPlayer}");
            }
        }

        public void UpdateAvatars(MatchStateData matchState)
        {
            for (int i = 0; i < participantControllers.Count; i++)
            {
                participantControllers.TryGetValue(i, out IMatchParticipant participant);
                if (participant != null)
                {
                    bool isLocal = participant.ParticipantID.Index == _matchStateController.GetPlayerIndex(CurrentUser);

                    participantAvatars[i].UpdateState(matchState.PlayerStates[i]);
                    participantAvatars[i].UpdateData(participant.PlayerData, isLocal); //or get the data from AppContext every time
                }
            }
        }

        public void OnLocalPlayerEndTurn()
        {
            _remote.PublishStateUpdate(_matchStateController.CurrentStateData);
        }

        private MatchStateData SetInitialState(MatchSeed seed, List<ParticipantData> participantDatas, List<PlayerData> playerDatas)
        {
            Dictionary<int, PlayerState> playerStates = new Dictionary<int, PlayerState>();
            participantControllers = new Dictionary<int, IMatchParticipant>();

            Dictionary<int, ParticipantData> participantDict = new Dictionary<int, ParticipantData>();

            for (int i = 0; i < seed.StartLocations.Count; i++)
            {
                var state = new PlayerState(seed.StartLocations[i], false);
                playerStates.Add(i, state);
            }

            for (int i = 0; i < participantDatas.Count; i++)
            {
                var participantData = participantDatas[i];
                var playerData = playerDatas[i];

                IMatchParticipant pController;

                if (participantData.PnUser == CurrentUser)
                {
                    pController = new LocalPlayerParticipant(participantData, playerInput, playerData);
                    participantAvatars.Add(i, SpawnAvatar(playerData, playerInput));
                }
                else
                {
                    pController = new RemotePlayerParticipant(participantData, playerData);
                    participantAvatars.Add(i, SpawnAvatar(playerData));
                }

                participantControllers.Add(i, pController);
                participantDict.Add(i, participantData);
            }

            _controllersInitialized = true;

            return new MatchStateData(0, participantDict, playerStates, seed.Script);
        }

        private void InitializeParticiapants(MatchStateData state)
        {
            participantControllers = new Dictionary<int, IMatchParticipant>();

            var participantDataDict = state.Participants;


            foreach (var pData in participantDataDict)
            {
                PlayerData playerData = mockPlayerData[pData.Value.Index];
                IMatchParticipant pController;

                if (pData.Value.PnUser == CurrentUser)
                {
                    pController = new LocalPlayerParticipant(pData.Value, playerInput, playerData);
                    participantAvatars.Add(pData.Value.Index, SpawnAvatar(playerData, playerInput));
                }
                else
                {
                    pController = new RemotePlayerParticipant(pData.Value, playerData);
                    participantAvatars.Add(pData.Value.Index, SpawnAvatar(playerData));
                }

                participantControllers.Add(pData.Value.Index, pController);
            }

            _controllersInitialized = true;
        }


        private PlayerVisual SpawnAvatar(PlayerData playerData, PlayerInput input = null)
        {
            var obj = Instantiate(avatarPrefab);
            obj.name = $"--avatar ({playerData.Name})";

            if (input != null)
            {
                obj.AttachInput(input);
                obj.SetAsLocalPlayer(true, playerData);
            }

            return obj;
        }


        private void OnPlayerRequestAction(PlayerState currentState, PlayerAction action)
        {
            if (true)//Check if its current players move
            {
                Vector2 newPos = currentState.BoardPoistion + action.MoveDir;
                var newState = new PlayerState(newPos, false);

                avatarPrefab.UpdateState(newState);
            }
        }

        private void Dispose()
        {
            avatarPrefab.PlayerActionRequested -= OnPlayerRequestAction;
        }

        private void OnDisable()
        {
            Dispose();
        }

        async Task IMatchEventSubscriber.OnMatchStateUpdate(MatchStateData matchStateData)
        {
            matchEnded = matchStateData.CurrentScriptStep >= matchStateData.Script.Turns.Count - 1;
            OnNetworkMatchStateReceived(matchStateData);

            //Debug.Log($"[NetworkUpdate] (MatchStateData): {JsonConvert.SerializeObject(matchStateData)}");
        }
    }
}
