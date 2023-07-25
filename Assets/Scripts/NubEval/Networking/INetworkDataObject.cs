using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NubEval.Networking
{
    public interface INetworkDataObject
    {

    }


    public interface IPayloadObject<TObject>
        where TObject : struct
    {
        public TObject Value { get; set; }
        public string RawJson { get; set; }
    }
}
