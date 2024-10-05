using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Firefly.Utils
{
    public class SpriteSync : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteToOverride;

        private SpriteRenderer _masterSprite;

        private bool NeedUpdate => _spriteToOverride != null && _masterSprite != null;

        private void Awake()
        {
            _masterSprite = GetComponent<SpriteRenderer>();

            if (NeedUpdate)
            {
                _spriteToOverride.drawMode = _masterSprite.drawMode;
                _spriteToOverride.size = _masterSprite.size;
            }
        }

        private void Update()
        {
            if (NeedUpdate && _masterSprite.sprite != _spriteToOverride.sprite)
            {
                _spriteToOverride.sprite = _masterSprite.sprite;
                if (_spriteToOverride.drawMode == SpriteDrawMode.Tiled)
                    _spriteToOverride.size = _masterSprite.size;
            }
        }
    }
}