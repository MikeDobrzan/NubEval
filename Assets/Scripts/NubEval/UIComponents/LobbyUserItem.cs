using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace UIComponents
{
    public class LobbyUserItem : MonoBehaviour
    {

        [SerializeField] private LobbyUserItemData data;

        [SerializeField] private TextMeshProUGUI userDisplayName;
        [SerializeField] private TextMeshProUGUI userPnID;
        [SerializeField] private TextMeshProUGUI userState;


        public void UpdateData(LobbyUserItemData data)
        {
            userPnID.text = data.UserID;
            userDisplayName.text = data.DisplayName;
            userState.text = $"[{data.LobbyState}] {data.MatchState}";
        }
    }
}
