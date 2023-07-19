using PubnubApi;
using System.Threading.Tasks;
using UnityEngine;

namespace NubEval
{
    public class UserManagementInput : MonoBehaviour
    {
        [SerializeField] private UserAccountData _accountData;

        private PNDevice _pn;

        public void Cosntruct(PNDevice pubnub)
        {
            _pn = pubnub;
        }

        public async void OnBtnClickAddUser()
        {
            if (string.IsNullOrEmpty(_accountData.PubNubUserID))
                return;

            bool success = await _pn.DataUsers.SetUserData(_accountData);           
            Debug.Log($"Modified: {success}");
        }

        public async void OnBtnCheckUser()
        {
            var r = await _pn.DataUsers.GetAccountData(_accountData.PubNubUserID);

            if(r.Item1)
            {
                var data = r.Item2;
                Debug.Log($"{data.GameAccountId} | {data.DisplayName} | {data.PubNubUserID}");
            }
            else
            {
                Debug.Log("ERR");
            }            
        }

        public async void OnBtnGetAll()
        {
            var users = await _pn.DataUsers.GetAllUserIDs();

            foreach (var data in users)
            {
                Debug.Log($"{data}");
            }
        }
    }
}
