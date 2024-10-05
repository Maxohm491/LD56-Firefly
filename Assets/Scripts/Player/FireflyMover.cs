using UnityEngine;
namespace Firefly
{
    public class FireflyMover : MonoBehaviour
    {
        [Tooltip("Units/sec")]
        [SerializeField] private float _linearSpeed = 1f;

        [Tooltip("Degrees/sec")]
        [SerializeField] private float _rotationSpeed = 45f;


        void FixedUpdate()
        {
            // TEMPORARY OLD INPUT
            float horizontal = Input.GetAxisRaw("Horizontal");

            // if right arrow
            transform.Rotate(0, 0, -horizontal * _rotationSpeed * Time.deltaTime);

            transform.position += _linearSpeed * Time.deltaTime * transform.up;
        }
    }
}
