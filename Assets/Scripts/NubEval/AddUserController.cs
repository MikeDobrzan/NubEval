using PubnubApi;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Profiling.Memory.Experimental;

namespace NubEval
{
    public class AddUserController : MonoBehaviour
    {
        [SerializeField] private Player _player;
        [SerializeField] private UserAccountData accountData;

        private Pubnub Pubnub => _player.Pubnub;

        public async void OnBtnClickAddUser()
        {
            await TryAddUser(accountData);
            Debug.Log("Modified");
        }

        public async void OnBtnCheckUser()
        {
            await PNCheckExists(accountData);
            Debug.Log("Exist?");
        }

        public async void GetAll()
        {
            var response = await Pubnub.GetAllUuidMetadata()
                .IncludeCustom(true)
                //.Filter($"ExternalId LIKE '{user}'")
                .ExecuteAsync();

            Debug.Log($"Result: count={response.Result.TotalCount}");

            foreach ( var data in response.Result.Uuids)
            {
                Debug.Log($"{data.Uuid} | {data.Name} | {data.ExternalId}");
            }           
        }

        private async Task TryAddUser(UserAccountData data)
        {
            string user = accountData.GameAccountId;

            var response = await Pubnub.SetUuidMetadata()
                .ExternalId(accountData.GameAccountId)
                .Uuid(accountData.PubNubUserID)
                .Name(accountData.DisplayName)
                .ExecuteAsync();
        }

        private async Task PNCheckExists(UserAccountData user)
        {
            string expression = $"Name == \"{user.DisplayName}\"";

            var response = await Pubnub.GetAllUuidMetadata()
                .IncludeCustom(true)
                //.Filter(expression)
                .ExecuteAsync();

            if (response.Result == null)
            {
                Debug.Log($"Not found <{expression}>");
                return;
            }

            foreach (var data in response.Result.Uuids)
            {
                Debug.Log($"{data.Uuid} | {data.Name} | {data.ExternalId}");
            }
        }
    }
}
