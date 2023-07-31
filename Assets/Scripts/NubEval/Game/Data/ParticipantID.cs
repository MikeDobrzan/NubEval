using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NubEval
{
    public struct ParticipantID
    {
        public ParticipantID(int index)
        {
            Index = index;
        }

        public int Index { get; set; }
    }
}
