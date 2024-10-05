using System.Collections;
using System.Collections.Generic;
using Firefly.Utils;
using UnityEngine;

namespace Firefly.UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private SceneAsset _startGameScene;

        public void OnStartButtonClick()
        {
            GameManager.Instance.LoadScene(gameObject.scene.name, _startGameScene, null, null);
        }
    }
}