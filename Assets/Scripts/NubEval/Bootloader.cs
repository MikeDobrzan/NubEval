using UnityEngine;

namespace NubEval
{
    public class Bootloader : MonoBehaviour
    {
        [SerializeField] private PNWrapper pnWrapper;
        [SerializeField] private AddUserController addUserUI;

        private async void Start()
        {
            //Initialize PubNub
            await pnWrapper.Init();
            Debug.Log("Boot Complete!");

            addUserUI.Cosntruct(pnWrapper);           
        }
    }
}