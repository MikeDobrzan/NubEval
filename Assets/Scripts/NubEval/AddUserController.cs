using PubnubApi;
using UnityEngine;
using UnityEngine.Profiling.Memory.Experimental;

namespace NubEval
{
    public class AddUserController : MonoBehaviour
    {
        [SerializeField] private Player _player;
        [SerializeField] private UserAccountData accountData;

        private Pubnub Pubnub => _player.Pubnub;

        public async void GetAll()
        {
            string user = accountData.GameAccountId;

            var response = await Pubnub.GetAllUuidMetadata()
                .IncludeCustom(true)
                //.Filter($"ExternalId LIKE '{user}'")
                .ExecuteAsync();

            var metadatas = response.Result.Uuids;

            foreach ( var data in metadatas )
            {
                Debug.Log(data.Uuid);
            }           
        }

        //public void TryAddUser(UserAccountData data)
        //{

        //}


        //private async bool PNCheckExists(UserAccountData user)
        //{

        //}
    }
}
