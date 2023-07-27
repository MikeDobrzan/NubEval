using NubEval.Game;
using NubEval.Game.Data;
using NubEval.Game.Networking;
using NubEval.Game.Networking.Payload;
using NubEval.PubNubWrapper;
using System.Threading.Tasks;
using UnityEngine;

namespace NubEval.DevTools
{
    /// <summary>
    /// MetaData Dashboard
    /// </summary>
    public class AppContextEditor : MonoBehaviour
    {
        [SerializeField] private Bootloader bootloader;
        [SerializeField] private UserAccountData _accountData;
        [SerializeField] private int matchID;

        private PNDevice _mainDevice;

        private void Awake()
        {
            bootloader.OnPubNubDeviceInitialized += Construct;
            if (_mainDevice == null)
            {
                _mainDevice = bootloader.PNDevice;
            }
        }

        public void Construct(PNDevice device)
        {
            _mainDevice = device;
        }

        public async void OnBtnClickAddUser()
        {
            if (string.IsNullOrEmpty(_accountData.PubNubUserID))
                return;

            bool success = await _mainDevice.MetadataUsers.SetUserData(_accountData);
            Debug.Log($"Modified: {success}");
        }

        public async void OnBtnCheckUser()
        {
            var r = await _mainDevice.MetadataUsers.GetAccountDataAsync(_accountData.PubNubUserID);

            if (r.Item1)
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
            var users = await _mainDevice.MetadataUsers.GetAllUserIDs();

            foreach (var data in users)
            {
                Debug.Log($"{data}");
            }
        }

        public async void OnBtnCheckMetadataChannel()
        {
            var announcement = await _mainDevice.MetadataChannels.GetDefaultCustomData<MatchRoomAnnouncement>(Channels.GetMatchChannel(matchID));

            Debug.Log($"matchID={matchID} | name={announcement.MatchConfig.Name}");
        }

        public async void OnBtnTestChanelMetadata()
        {
            var obj = new MatchRoomAnnouncement
            {
                MatchConfig = new MatchConfig(124, MatchType.Ranked, "test-2423"),
                MatchStatus = new MatchRoomStatus()
            };

            await _mainDevice.MetadataChannels.SetDefaultCustomData<MatchRoomAnnouncement>(Channels.GetMatchChannel(matchID), obj);
            await Task.Delay(2000);
            var match = await _mainDevice.MetadataChannels.GetDefaultCustomData<MatchRoomAnnouncement>(Channels.GetMatchChannel(matchID));
            Debug.Log(match.MatchConfig.Name);
        }
    }
}
