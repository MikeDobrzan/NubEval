using PubnubApi;
using UnityEngine;

namespace NubEval
{
    public class AppDashboard : MonoBehaviour
    {
        [SerializeField] private PNWrapper pl;

        public Pubnub Pubnub => pl.Pubnub;

        public async void AddSystemChannels()
        {
            var response = await Pubnub.SetChannelMetadata()
                .Channel(Channels.Lobby)
                .Name("server-lobby-matches")
                .ExecuteAsync();

            if (response != null)
            {
                bool success = !response.Status.Error;

                Debug.Log($"Success: {success}");

                if(!success)
                {
                    Debug.LogWarning($"{response.Status.ErrorData.Information}");
                }
            }

            //if (response != null)
            //{
            //    Debug.Log($"Created Channel Data: [{Channels.Lobby}]");
            //    Debug.Log($"{response.Status.Category} | {response.Result.Updated}");
            //}
            //else
            //{
            //    Debug.Log("Error!");
            //}
        }

        public async void GetAppChannels()
        {
            var response = await Pubnub.GetAllChannelMetadata()
                .IncludeCustom(true)
                .ExecuteAsync();

            if (response != null)
                Debug.Log($"Success: {!response.Status.Error}");

            var channels = response.Result.Channels;

            foreach (var channel in channels)
            {
                Debug.Log($"{channel.Name}");
            }
        }
    }
}
