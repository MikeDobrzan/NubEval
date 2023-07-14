using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NubEval
{
    public class Bootloader : MonoBehaviour
    {
        [SerializeField] private PNManager userA;
        [SerializeField] private PNManager userB;

        void Start()
        {
            Debug.Log("Boot!");
        }
    }
}