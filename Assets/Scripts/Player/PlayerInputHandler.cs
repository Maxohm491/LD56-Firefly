using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
namespace Firefly
{
    public class PlayerInputHandler : MonoBehaviour
    {
        [field:SerializeField, ReadOnly]
        public int Steering { get; private set; }

        public void HandleMoveInput(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Steering = (int)Mathf.Sign(context.ReadValue<Vector2>().x);
            }
            if (context.canceled)
            {
                Steering = 0;
            }
        }
    }
}