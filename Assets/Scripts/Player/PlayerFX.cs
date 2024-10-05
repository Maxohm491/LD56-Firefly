using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace Firefly
{
    public class PlayerFX : MonoBehaviour
    {
        [field:SerializeField] public MMF_Player DeathFX { get; private set; }
        [field: SerializeField] public MMF_Player SlowFX { get; private set; }
        [field: SerializeField] public MMF_Player FlyFX { get; private set; }
    }
}