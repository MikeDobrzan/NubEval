using System.Collections.Generic;
using UnityEngine;

namespace NubEval.Game.UIComponents
{
    public class LobbyPanel : MonoBehaviour
    {
        [SerializeField] RectTransform listRoot;
        [SerializeField] LobbyUserItem listItemPrefab;

        [SerializeField] private List<LobbyUserItemData> lobbyPanelDatas = new List<LobbyUserItemData>();
        //[SerializeField] private List<LobbyUserItem> objects = new List<LobbyUserItem>();

        private Dictionary<string, LobbyUserItem> objects = new Dictionary<string, LobbyUserItem>();


        public void Refresh(List<LobbyUserItemData> list)
        {
            //delete old items
            foreach (var record in objects)
            {
                Destroy(record.Value.gameObject);
                objects.Remove(record.Key);
            }

            foreach (var data in list)
            {
                AddPlayerCard(data);
            }
        }

        public void AddPlayerCard(LobbyUserItemData data)
        {
            var obj = SpawnCard(data);
            obj.transform.SetParent(listRoot, false);
            objects.Add(data.UserID, obj);
        }

        public void RemovePlayerCard(LobbyUserItemData data)
        {
            if (objects.TryGetValue(data.UserID, out var obj))
            {
                DespawnCard(obj);
                objects.Remove(data.UserID);
            }
        }

        public void UpdatePlayerCardData(LobbyUserItemData data)
        {
            if (objects.TryGetValue(data.UserID, out var obj))
            {
                obj.UpdateData(data);
            }
        }

        private LobbyUserItem SpawnCard(LobbyUserItemData data)
        {
            var obj = Instantiate(listItemPrefab);
            obj.UpdateData(data);

            return obj;
        }

        private void DespawnCard(LobbyUserItem obj)
        {
            Destroy(obj.gameObject);
        }
    }
}
