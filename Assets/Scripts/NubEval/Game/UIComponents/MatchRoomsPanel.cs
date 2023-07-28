using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NubEval.Game.UIComponents
{
    public class MatchRoomsPanel : MonoBehaviour
    {
        [SerializeField] RectTransform listRoot;
        [SerializeField] MatchRoomListItem listItemPrefab;

        [SerializeField] private List<MatchRoomListItemData> lobbyPanelDatas = new List<MatchRoomListItemData>();
        private Dictionary<int, MatchRoomListItem> objects = new Dictionary<int, MatchRoomListItem>();


        public void Refresh(List<MatchRoomListItemData> list)
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

        public void AddPlayerCard(MatchRoomListItemData data)
        {
            var obj = SpawnCard(data);
            obj.transform.SetParent(listRoot, false);
            objects.Add(data.MatchID, obj);
        }

        public void RemovePlayerCard(MatchRoomListItemData data)
        {
            if (objects.TryGetValue(data.MatchID, out var obj))
            {
                DespawnCard(obj);
                objects.Remove(data.MatchID);
            }
        }

        public void UpdatePlayerCardData(MatchRoomListItemData data)
        {
            if (objects.TryGetValue(data.MatchID, out var obj))
            {
                obj.UpdateData(data);
            }
        }

        private MatchRoomListItem SpawnCard(MatchRoomListItemData data)
        {
            var obj = Instantiate(listItemPrefab);
            obj.UpdateData(data);

            return obj;
        }

        private void DespawnCard(MatchRoomListItem obj)
        {
            Destroy(obj.gameObject);
        }
    }
}
