using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
namespace Firefly
{
    public class FireflyMover : MonoBehaviour
    {
        [Tooltip("Units/sec")]
        [SerializeField] private float _linearSpeed = 1f;

        [Tooltip("Degrees/sec")]
        [SerializeField] private float _rotationSpeed = 45f;

        private Vector2 _turning;

        void FixedUpdate()
        {
            transform.Rotate(0, 0, -_turning.x * _rotationSpeed * Time.deltaTime);

            transform.position += _linearSpeed * Time.deltaTime * transform.up;
        }

        public void HandleMove(InputAction.CallbackContext context) 
        {
            if(context.started)
            {
                _turning = context.ReadValue<Vector2>();
            }
            if(context.canceled) 
            {
                _turning = new();
            }
        }
    }
}
