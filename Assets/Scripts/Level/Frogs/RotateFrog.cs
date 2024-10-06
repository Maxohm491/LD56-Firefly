using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Firefly
{
    public class RotateFrog : MonoBehaviour
    {
        [SerializeField, Range(-90, 90)] private float _deltaRotation = 90;
        [SerializeField] private float _rotateInterval = 3f;
        [SerializeField] private float _rotateDuration = .5f;

        private void Start()
        {
            StartCoroutine(Rotate());
        }

        IEnumerator Rotate()
        {
            while(true)
            {
                yield return transform.DORotate(new Vector3(0, 0, _deltaRotation), _rotateDuration, RotateMode.LocalAxisAdd).WaitForCompletion();
                yield return new WaitForSeconds(_rotateInterval);
            }
        }
    }
}