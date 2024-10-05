using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Firefly
{
    public interface IEatable
    {
        public Transform Transform { get; }
        public void GetCaught();
        public void GetEaten();
    }
}