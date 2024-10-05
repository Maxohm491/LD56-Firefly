using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Firefly
{
    public interface IEatable
    {
        public Transform Transform { get; }
        public bool Eatable { get; }
        public void GetCaught();
        public void GetEaten();
    }
}