using PubnubApi;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace NubEval
{
    public class PNPresence
    {
        private readonly Pubnub _pn;
        private readonly UserId currentUser;

        public PNPresence(Pubnub pubnub, UserId userID)
        {
            _pn = pubnub;
            currentUser = userID;
        }

        public async Task<List<PresenceState>> GetStates(string channel, UserId user)
        {
            var responce = await _pn.GetPresenceState()
                .Channels(new[] { channel})
                .Uuid(user)
                .ExecuteAsync();

            List<PresenceState> states = new List<PresenceState>();

            string debugStr = $"States [{user}] ";

            foreach (var state in responce.Result.StateByUUID.Keys)
            {
                responce.Result.StateByUUID.TryGetValue(state, out var value);
                debugStr += $"<{state}>:{value} | ";
                Debug.Log(debugStr);

                states.Add(new PresenceState(state, value.ToString()));
            }                       

            return states;
        }

        public async Task<List<PresenceState>> GetStatesCurrentUser(string channel)
        {
            return await GetStates(channel, currentUser);
        }


        public async Task ChannelJoin(string channel, PresenceState state = default)
        {
            var newState = new Dictionary<string, object>
            {
                { state.StateType, state.State }
            };

            var responce = await _pn.SetPresenceState()
                .Channels(new string[] { channel })
                .State(newState)
                .ExecuteAsync();

            if (responce.Status.Error)
                Debug.Log(responce.Status.ErrorData.Information);
            else
            {
                responce.Result.State.TryGetValue(state.StateType, out object obj);
                Debug.Log($"<{state.StateType}> set to: {obj}");
            }
        }
    }
}
