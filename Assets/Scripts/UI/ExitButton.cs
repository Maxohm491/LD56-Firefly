using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Firefly
{
    public class ExitButton : MonoBehaviour
    {
        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(HandleButtonClick);
        }

        private void HandleButtonClick()
        {
            EventSystem.current.SetSelectedGameObject(null);
            Application.Quit();
        }
    }
}