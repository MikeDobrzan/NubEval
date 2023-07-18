using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIComponents
{
    public class LobbyPanel : MonoBehaviour
    {
        [SerializeField] RectTransform listRoot;
        [SerializeField] LobbyUserItem listItemPrefab;

        [SerializeField] private List<LobbyUserItemData> lobbyPanelDatas = new List<LobbyUserItemData>();
        [SerializeField] private List<LobbyUserItem> objects = new List<LobbyUserItem>();

        //private void Start()
        //{
        //    foreach (var data in lobbyPanelDatas)
        //    {
        //        var obj = SpawnCard(data);
        //        obj.transform.SetParent(listRoot, false);
        //        objects.Add(obj);
        //    }
        //}

        public void Refresh(List<LobbyUserItemData> list)
        {
            //delete old items
            foreach (var obj in objects)
            {
                Destroy(obj.gameObject);
            }

            foreach (var data in list)
            {
                var obj = SpawnCard(data);
                obj.transform.SetParent(listRoot, false);
                objects.Add(obj);
            }
        }

        private LobbyUserItem SpawnCard(LobbyUserItemData data)
        {
            var obj = Instantiate(listItemPrefab);
            obj.UpdateData(data);

            return obj;
        }
    }
}
