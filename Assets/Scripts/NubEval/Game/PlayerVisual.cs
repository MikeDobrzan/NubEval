using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace NubEval
{
    public class PlayerVisual : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private SpriteRenderer mainSprite;
        [SerializeField] private SpriteRenderer deadSprite;

        [Header("Debug")]
        [SerializeField] private PlayerState playerState;
        [SerializeField] private PlayerData plData;

        // Start is called before the first frame update
        private void SetState(PlayerState state)
        {          
            gameObject.transform.position = state.BoardPoistion;
            deadSprite.enabled = state.IsKilled;          
        }

        private void SetVisual(PlayerData data)
        {
            nameText.text = data.Name;
            mainSprite.color = data.Color;
        }

        // Update is called once per frame
        void Update()
        {
            SetState(playerState);
            SetVisual(plData);
        }
    }
}
