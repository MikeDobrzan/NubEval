using NubEval.Game.Data;
using NubEval.Game.Networking.Payload;
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
        private Dictionary<int, IMatchParticipant> participants;
        private Dictionary<int, ParticipantData> participantDatas;
        private Dictionary<int, PlayerVisual> avatars;
        
        //public UserId ThisPlayerId;
        //public ParticipantData thisPlayerInMatch;

        private MatchStateController _matchState;

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


        public void Update()
        {
            if (participantDatas == null)
                return;

            uuid = participantDatas[IamIndex].PnUser;
        }


        private async void Start()
        {
            participantDatas = new Dictionary<int, ParticipantData>();

            foreach (var p in mock_participants)
            {
                participantDatas.Add(p.Index, p);
            }

            _matchState = new MatchStateController();
            //thisPlayerInMatch = new ParticipantData(0, ThisPlayerId);

            MatchStateData initialMatchState = SetInitialState(seed);

            //set initial player states
            OnNetworkMatchInitialStateReceived(initialMatchState);


            //localPlayerParticipant = new LocalPlayerParticipant(playerInput);

            bool isHost = true;



            //Add remote players
            _matchState.NetworkPublishState(initialMatchState);

            int nextPlayer = _matchState.NextPlayerIndex;


            OnNetworkMatchStateReceived(initialMatchState);
            //thisPlayerAvatar.UpdateState(_matchState.GetParticipantState(0));




            await Task.Delay(3000);

            if (_matchState.NextPlayerID == CurrentUser)
            {
                //Wait for the player


                Debug.Log($"Make Your Turn! --> {nextPlayer}");
                var cts = new CancellationTokenSource(10000);

                var playerTurn = PlayerTurn.NewTurn();
                var action = await participants[_matchState.GetPlayerIndex(CurrentUser)].RequestActionAsync(playerTurn, cts.Token);

                //Apply the action
                _matchState.ApplyPlayerAction(0, action);// .SetPlayerState(0, newState);

                //Update Avatar
                //thisPlayerAvatar.UpdateState(_matchState.GetParticipantState(0));

                Debug.Log($"Thank you for Your Turn!: {action.MoveDir}");
                await OnLocalPlayerEndTurn();
            }
            else
            {
                Debug.Log($"Not your Turn!  wait for --> {nextPlayer}");
            }

            OnNetworkMatchStateReceived(_matchState.CurrentStateData);

        }


        //private bool IsCurrentUser(int index)
        //{
        //    _matchState.NextPlayerIndex .CurrentStateData.cur

        //    return participantDatas.Any(p => p.Value.PnUser == uuid);
        //}

        public void OnNetworkMatchInitialStateReceived(MatchStateData matchState)
        {


            //Create participants (mocks for now)
            participants = new Dictionary<int, IMatchParticipant>
            {
                { 0, new LocalPlayerParticipant(participantDatas[0], playerInput, mockLocalPlayerData) },
                { 1, new RemotePlayerParticipant(participantDatas[1], mockRemotePlayerData) }
            };

            //Spawn Avatars
            avatars = new Dictionary<int, PlayerVisual>
            {
                { 0, thisPlayerAvatar },
                { 1, remotePlayerAvatar }
            };
        }

        public void OnNetworkMatchStateReceived(MatchStateData matchState)
        {
            for (int i = 0; i < participants.Count; i++)
            {
                participants.TryGetValue(i, out IMatchParticipant participant);
                if (participant != null)
                {
                    bool isLocal = participant.ParticipantID.Index == _matchState.GetPlayerIndex(CurrentUser);

                    avatars[i].UpdateState(matchState.PlayerStates[i]);
                    avatars[i].UpdateData(participant.PlayerData, isLocal); //or get the data from AppContext every time
                }
            }
        }

        public async Task OnLocalPlayerEndTurn()
        {
            Debug.Log(_matchState.MatchStateJSON);

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

            return new MatchStateData(0, participantDatas, playerStates, seed.Script);
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
