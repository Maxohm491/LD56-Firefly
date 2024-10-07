using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateSkew : MonoBehaviour
{
    [SerializeField] private SkewedImage _skewedImage;


    [SerializeField]
    private float _maxSkewX;
    [SerializeField]
    private float _skewSpeed;

    private void Update()
    {
        _skewedImage.SkewX = Mathf.Sin(_skewSpeed * Time.time) * _maxSkewX;
    }
}
