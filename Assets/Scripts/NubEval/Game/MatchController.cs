using NubEval.Game.Data;
using NubEval.Game.Networking.Payload;
using NubEval.PubNubWrapper;
using PubnubApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace NubEval.Game
{
    [System.Serializable]
    public class MatchController : MonoBehaviour
    {
        public int MatchID;
        public MatchConfig Config;
        public MatchRoomStatus MatchStatus;
        public DateTime MatchStart;

        [SerializeField] private PlayerInput playerInput;
        [SerializeField] private PlayerVisual thisPlayerAvatar;
        [SerializeField] private PlayerVisual remotePlayerAvatar;
        [SerializeField] private MatchSeed seed;
        [SerializeField] private PlayerState playerState;

        //private IMatchParticipant localPlayerParticipant;
        private Dictionary<int, IMatchParticipant> participantControllers;
        private Dictionary<int, ParticipantData> _participantDatas;
        private Dictionary<int, PlayerVisual> participantAvatars;
        
        //public UserId ThisPlayerId;
        //public ParticipantData thisPlayerInMatch;

        private MatchStateController _matchStateController;
        private PNDevice _device;

        private string CurrentUser => uuid;

        //public Dictionary<int, PlayerState> playerStates;

        [SerializeField] private PlayerData mockLocalPlayerData;
        [SerializeField] private PlayerData mockRemotePlayerData;
        [SerializeField] private List<ParticipantData> mock_participants;

        [Header("Debug")]
        [SerializeField] MatchStateData debug_matchStateData;
        
        [Range(0, 1)]
        public int IamIndex;
        public string uuid;

        public void Construct(PNDevice device)
        {
            _device = device;
        }

        public void OnBoot()
        {
            _matchStateController = new MatchStateController();

            OnReceiveParticipants(); //jsut create mock from boot for testing

            MatchStateData initialMatchState = SetInitialState(seed);
            _matchStateController.SetState(initialMatchState);


            OnNetworkMatchInitialStateReceived(_participantDatas);

            UpdateAvatars(initialMatchState);

            OnNetworkMatchStateReceived(initialMatchState);
        }

        private void OnReceiveParticipants()
        {
            _participantDatas = new Dictionary<int, ParticipantData>();
            foreach (var p in mock_participants)
            {
                _participantDatas.Add(p.Index, p);
            }
        }


        public void Update()
        {
            if (_participantDatas == null)
                return;

            uuid = _participantDatas[IamIndex].PnUser;
        }

        private async void OnNetworkMatchStateReceived(MatchStateData matchState)
        {           
            await Task.Delay(3000);

            _matchStateController.SetState(matchState);

            int nextPlayer = _matchStateController.NextPlayerIndex;

            if (_matchStateController.NextPlayerID == CurrentUser)
            {
                Debug.Log($"Make Your Turn! --> {nextPlayer}");
                var cts = new CancellationTokenSource(10000);

                var playerTurn = PlayerTurn.NewTurn();
                var action = await participantControllers[_matchStateController.GetPlayerIndex(CurrentUser)].RequestActionAsync(playerTurn, cts.Token);

                //Apply the action
                _matchStateController.ApplyPlayerAction(0, action);// .SetPlayerState(0, newState);

                Debug.Log($"Thank you for Your Turn!: {action.MoveDir}");
                await OnLocalPlayerEndTurn();
            }
            else
            {
                Debug.Log($"Not your Turn!  wait for --> {nextPlayer}");
            }

            UpdateAvatars(_matchStateController.CurrentStateData);
        }

        /// <summary>
        /// Create participant controller and avatar for each participant
        /// </summary>
        public void OnNetworkMatchInitialStateReceived(Dictionary<int, ParticipantData> participantDatas)
        {
            //Create participants (mocks for now)
            participantControllers = new Dictionary<int, IMatchParticipant>
            {
                { 0, new LocalPlayerParticipant(participantDatas[0], playerInput, mockLocalPlayerData) },
                { 1, new RemotePlayerParticipant(participantDatas[1], mockRemotePlayerData) }
            };

            //Spawn Avatars
            participantAvatars = new Dictionary<int, PlayerVisual>
            {
                { 0, thisPlayerAvatar },
                { 1, remotePlayerAvatar }
            };
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

        public async Task OnLocalPlayerEndTurn()
        {
            Debug.Log(_matchStateController.MatchStateJSON);

            await Task.CompletedTask;
        }


        private MatchStateData SetInitialState(MatchSeed seed)
        {
            Dictionary<int, PlayerState> playerStates = new Dictionary<int, PlayerState>();

            for (int i = 0; i < seed.StartLocations.Count; i++)
            {
                var state = new PlayerState(seed.StartLocations[i], false);
                playerStates.Add(i, state);
            }

            return new MatchStateData(0, _participantDatas, playerStates, seed.Script);
        }

        private void OnPlayerRequestAction(PlayerState currentState, PlayerAction action)
        {
            if (true)//Check if its current players move
            {
                Vector2 newPos = currentState.BoardPoistion + action.MoveDir;
                var newState = new PlayerState(newPos, false);

                thisPlayerAvatar.UpdateState(newState);
            }
            //else
            //    return;
        }

        private void Dispose()
        {
            thisPlayerAvatar.PlayerActionRequested -= OnPlayerRequestAction;
        }

        private void OnDisable()
        {
            Dispose();
        }
    }
}
