using NubEval.Game.Data;
using NubEval.Game.Networking.Payload;
using PubnubApi;
using System;
using System.Collections.Generic;
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

        //private IMatchParticipant localPlayerParticipant;
        private Dictionary<ParticipantID, IMatchParticipant> participants;
        private Dictionary<ParticipantID, PlayerVisual> avatars;
        private Dictionary<ParticipantID, PlayerData> participantDatas;

        public UserId ThisPlayerId;
        public ParticipantID thisPlayerInMatch = new ParticipantID(0);

        private MatchStateController _matchState;

        //public Dictionary<int, PlayerState> playerStates;

        [SerializeField] private PlayerData mockLocalPlayerData;
        [SerializeField] private PlayerData mockRemotePlayerData;


        [Header("Debug")]
        [SerializeField] MatchStateData debug_matchStateData;

        private async void Start()
        {
            _matchState = new MatchStateController();

            MatchStateData initialMatchState = SetInitialState(seed);

            //set initial player states
            OnNetworkMatchInitialStateReceived(initialMatchState);


            //localPlayerParticipant = new LocalPlayerParticipant(playerInput);

            bool isHost = true;



            //Add remote players
            _matchState.NetworkPublishState(initialMatchState);

            ParticipantID nextPlayer = new ParticipantID(_matchState.NextPlayer);


            OnNetworkMatchStateReceived(initialMatchState);
            //thisPlayerAvatar.UpdateState(_matchState.GetParticipantState(0));




            await Task.Delay(3000);

            if (nextPlayer.Index == thisPlayerInMatch.Index)
            {
                //Wait for the player


                Debug.Log($"Make Your Turn! --> {nextPlayer}");
                var cts = new CancellationTokenSource(10000);

                var playerTurn = PlayerTurn.NewTurn();
                var action = await participants[thisPlayerInMatch].RequestActionAsync(playerTurn, cts.Token);

                //Apply the action
                _matchState.ApplyPlayerAction(0, action);// .SetPlayerState(0, newState);

                //Update Avatar
                //thisPlayerAvatar.UpdateState(_matchState.GetParticipantState(0));

                Debug.Log($"Thank you for Your Turn!: {action.MoveDir}");
            }
            else
            {
                Debug.Log($"Not your Turn!  wait for --> {nextPlayer}");
            }

            OnNetworkMatchStateReceived(_matchState.CurrentStateData);

        }

        public void OnNetworkMatchInitialStateReceived(MatchStateData matchState)
        {
            //Create participants (mocks for now)
            participants = new Dictionary<ParticipantID, IMatchParticipant>
            {
                { new ParticipantID(0), new LocalPlayerParticipant(new ParticipantID(0), playerInput, mockLocalPlayerData) },
                { new ParticipantID(1), new RemotePlayerParticipant(new ParticipantID(1), mockRemotePlayerData) }
            };

            //Spawn Avatars
            avatars = new Dictionary<ParticipantID, PlayerVisual>
            {
                { new ParticipantID(0), thisPlayerAvatar },
                { new ParticipantID(1), remotePlayerAvatar }
            };
        }

        public void OnNetworkMatchStateReceived(MatchStateData matchState)
        {
            for (int i = 0; i < participants.Count; i++)
            {
                var pID = new ParticipantID(i);
                participants.TryGetValue(pID, out IMatchParticipant participant);
                if (participant != null)
                {
                    bool isLocal = participant.ParticipantID.Index == thisPlayerInMatch.Index;

                    avatars[pID].UpdateState(matchState.PlayerStates[pID]);
                    avatars[pID].UpdateData(participant.PlayerData, isLocal); //or get the data from AppContext every time
                }
            }
        }

        private MatchStateData SetInitialState(MatchSeed seed)
        {
            Dictionary<ParticipantID, PlayerState> playerStates = new Dictionary<ParticipantID, PlayerState>();

            for (int i = 0; i < seed.StartLocations.Count; i++)
            {
                Debug.Log(i);

                var state = new PlayerState(seed.StartLocations[i], false);

                playerStates.Add(new ParticipantID(i), state);
            }

            return new MatchStateData(0, playerStates, seed.Script);
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
