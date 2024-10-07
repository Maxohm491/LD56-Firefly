using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Firefly
{
    [ExecuteAlways]
    public class ZOrderAdjust : MonoBehaviour
    {
        [SerializeField] private float _offset;

        private void Awake()
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y + _offset);
            //Debug.Log(transform.position.y);
        }

#if UNITY_EDITOR
        private void Update()
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y + _offset);
        }
#endif
    }
}