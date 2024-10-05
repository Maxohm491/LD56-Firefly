using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Firefly
{
    public class ZOrderAdjust : MonoBehaviour
    {
        private void Awake()
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);
        }
    }
}