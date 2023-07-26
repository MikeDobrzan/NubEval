using NubEval.Networking.Payloads;
using PubnubApi;
using System.Threading.Tasks;
using UnityEngine;

namespace NubEval
{
    /// <summary>
    /// MetaData Dashboard
    /// </summary>
    public class UserManagementInput : MonoBehaviour
    {
        [SerializeField] private UserAccountData _accountData;
        [SerializeField] private int matchID;

        private PNDevice _device;

        public void Cosntruct(PNDevice device)
        {
            _device = device;
        }

        public async void OnBtnClickAddUser()
        {
            if (string.IsNullOrEmpty(_accountData.PubNubUserID))
                return;

            bool success = await _device.MetadataUsers.SetUserData(_accountData);           
            Debug.Log($"Modified: {success}");
        }

        public async void OnBtnCheckUser()
        {
            var r = await _device.MetadataUsers.GetAccountDataAsync(_accountData.PubNubUserID);

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
            var users = await _device.MetadataUsers.GetAllUserIDs();

            foreach (var data in users)
            {
                Debug.Log($"{data}");
            }
        }

        public async void OnBtnCheckMetadataChannel()
        {
            var announcement = await _device.MetadataChannels.GetDefaultCustomData<MatchAnnouncement>(Channels.GetMatchChannel(matchID));

            Debug.Log($"matchID={matchID} | name={announcement.MatchConfig.Name}");
        }

        public async void OnBtnTestChanelMetadata()
        {
            var obj = new MatchAnnouncement
            {
                MatchConfig = new MatchConfig(124, MatchType.Ranked, "test-2423"),
                MatchStatus = new MatchStatus()                
            };

            await _device.MetadataChannels.SetDefaultCustomData<MatchAnnouncement>(Channels.GetMatchChannel(matchID), obj);
            await Task.Delay(2000);
            var match = await _device.MetadataChannels.GetDefaultCustomData<MatchAnnouncement>(Channels.GetMatchChannel(matchID));
            Debug.Log(match.MatchConfig.Name);
        }
    }
}
