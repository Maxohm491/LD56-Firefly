using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Firefly
{
    public class EventTriggerVolume : MonoBehaviour
    {
        [SerializeField, TextArea]
        private string _infoText;

        [SerializeField]
        private bool _oneShot;

        private bool _acitvated = false;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player") && !_acitvated)
            {
                GameplayManager.Instance.OnDisplayInfo.Invoke(_infoText);

                if (_oneShot)
                {
                    _acitvated = true;
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                GameplayManager.Instance.OnHideInfo.Invoke();
            }
        }
    }
}