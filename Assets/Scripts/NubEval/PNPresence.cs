using NubEval.Networking.PubNubWrapper;
using PubnubApi;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace NubEval
{
    public class PNPresence : PNServiceBase
    {
        public PNPresence(Pubnub pubnub, PNDevice device) : base(pubnub, device) { }

        private UserId CurrentUser => PNApi.GetCurrentUserId();

        public async Task<Dictionary<UserId, ConnectedUser>> GetUsersInChannel(Channel channel)
        {
            var response = await PNApi.HereNow()
                .Channels(new[] { channel.PubNubAddress })
                .IncludeState(true)
                .IncludeUUIDs(true)
                .ExecuteAsync();

            Dictionary<UserId, ConnectedUser> users = new Dictionary<UserId, ConnectedUser>();

            foreach (KeyValuePair<string, PNHereNowChannelData> record in response.Result.Channels)
            {
                PNHereNowChannelData channelData = record.Value;

                if (channelData.Occupants != null && channelData.Occupants.Count > 0)
                {
                    foreach (var oData in channelData.Occupants)
                    {

                        List<PresenceState> pStates = new List<PresenceState>();

                        if (oData.State != null)
                        {
                            Dictionary<string, object> statesDict = oData.State as Dictionary<string, object>;

                            pStates = ResponseNormalization.ToPresenceStates(statesDict);
                        }
                        var user = new ConnectedUser(oData.Uuid, pStates);
                        users.Add(oData.Uuid, user);
                    }
                }
            }

            return users;
        }

        public async Task<List<UserId>> GetUserIDsInChannel(Channel channel)
        {
            Debug.Log($"Getting users in: {channel.PubNubAddress} ");

            var response = await PNApi.HereNow()
                .Channels(new string[] { channel.PubNubAddress })
                .IncludeState(true)
                .IncludeUUIDs(true)
                .ExecuteAsync();

            Debug.Log($"ocupancy={response.Result.TotalOccupancy}");

            List<UserId> users = new List<UserId>();

            response.Result.Channels.TryGetValue(channel.PubNubAddress, out PNHereNowChannelData chanelContent);

            foreach (var oData in chanelContent.Occupants)
            {
                users.Add(oData.Uuid);
                Debug.Log($"Found: {oData.Uuid}");
            }


            //foreach (KeyValuePair<string, PNHereNowChannelData> record in response.Result.Channels)
            //{
            //    PNHereNowChannelData channelData = record.Value;

            //    if (channelData.Occupants != null && channelData.Occupants.Count > 0)
            //    {
            //        foreach (var oData in channelData.Occupants)
            //            users.Add(oData.Uuid);
            //    }
            //}

            return users;
        }

        public async Task<List<PresenceState>> GetStates(string channel, UserId user)
        {
            var responce = await PNApi.GetPresenceState()
                .Channels(new[] { channel })
                .Uuid(user)
                .ExecuteAsync();

            List<PresenceState> states = ResponseNormalization.ToPresenceStates(responce.Result.StateByUUID);

           return states;
        }


        public async Task<List<PresenceState>> GetStatesCurrentUser(string channel)
        {
            return await GetStates(channel, CurrentUser);
        }


        public async Task SubscribePresence(Channel channel)
        {
            if (channel.ChannelType != ChannelType.PresenceChannel)
            {
                PNDevice.Console.Log("Can't join channels without presence");
                return;
            }

            PNApi.Subscribe<string>()
                .Channels(new string[] { channel.PubNubAddress, channel.PresenceAddress })
                //.WithPresence()
                .Execute();

            await Task.Delay(2000); //Note: this is workaround because Subscribe is not Async
        }


        /// <summary>
        /// Will automatically subscribe to it
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public async Task SetPresenceState(Channel channel, List<PresenceState> state)
        {
            PNDevice.Console.Log($"[SetPresenceState]: UserID={PNApi.GetCurrentUserId()}");

            if (channel.ChannelType != ChannelType.PresenceChannel)
            {
                PNDevice.Console.Log("Can't join channels without presence");
                return;
            }

            var newState = ResponseNormalization.BuildStatesDict(state);

            var responce = await PNApi.SetPresenceState()
                .Uuid(PNApi.GetCurrentUserId())
                .Channels(new string[] { channel.PubNubAddress })
                .State(newState)
                .ExecuteAsync();

            if (responce.Status.Error)
                PNDevice.Console.Log("ErrorSettingStates");
        }

        public async Task LeaveChannel(Channel channel)
        {
            PNApi.Unsubscribe<string>()
                .Channels(new string[] { channel.PubNubAddress })
                .Execute();

            await Task.Delay(1000);

            PNApi.Unsubscribe<string>()
                .Channels(new string[] { channel.PresenceAddress, channel.PresenceAddress })
                .Execute();

            await Task.Delay(1000);
        }
    }
}
