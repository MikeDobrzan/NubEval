using NubEval.Game.Networking;
using PubnubApi;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace NubEval.PubNubWrapper
{
    //It will be nice if we can get the total count of the data records before trying to download all of them 

    public class PNDatastoreUsers
    {
        private readonly Pubnub _pn;

        public PNDatastoreUsers(Pubnub pubnub)
        {
            _pn = pubnub;
        }

        private UserId CurrentUserID => _pn.GetCurrentUserId();

        public async Task<List<UserId>> GetAllUserIDs()
        {
            var response = await _pn.GetAllUuidMetadata()
                .IncludeCount(true) //<--- if this line is missing the count is always 0
                .IncludeCustom(true)
                //.Filter($"ExternalId LIKE '{user}'")
                .ExecuteAsync();

            List<UserId> users = new List<UserId>();

            foreach (var data in response.Result.Uuids)
                users.Add(new UserId(data.Uuid));

            return users;
        }

        public async Task<(bool, UserAccountData)> GetAccountDataAsync(UserId user)
        {
            var response = await _pn.GetUuidMetadata()
                .Uuid(user)
                .IncludeCustom(false)
                .ExecuteAsync();

            if (response != null && !response.Status.Error)
            {
                if (response.Result != null)
                {
                    var data = new UserAccountData(response.Result.ExternalId, response.Result.Uuid, response.Result.Name);
                    return (true, data);
                }
                else
                {
                    var data = new UserAccountData("null", response.Result.Uuid, "null");
                    return (true, data); // maybe return false?
                }
            }
            else
            {
                Debug.LogWarning($"No account data: {user}");
                return (false, default);
            }
        }

        public async Task<int> GetCountUsers()
        {
            var response = await _pn.GetAllUuidMetadata()
                .IncludeCount(true)
                .ExecuteAsync();

            return response.Result.TotalCount;
        }

        public async Task<bool> SetUserData(UserAccountData data)
        {
            var response = await _pn.SetUuidMetadata()
                .ExternalId(data.GameAccountId)
                .Uuid(data.PubNubUserID)
                .Name(data.DisplayName)
                .ExecuteAsync();

            return (response != null && !response.Status.Error);
        }
    }
}
