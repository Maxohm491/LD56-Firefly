using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
namespace Firefly
{
    public class FireflyPlayer : MonoBehaviour
    {
        [Tooltip("Units/sec")]
        [SerializeField] private float _linearSpeed = 1f;

        [Tooltip("Degrees/sec")]
        [SerializeField] private float _rotationSpeed = 45f;

        enum LifeState
        {
            Spawn,
            Alive,
            Dead,
        }

        private LifeState _lifeState = LifeState.Spawn;

        private Vector2 _turning;

        private Nest _currentNest;

        private void OnEnable()
        {
            GameplayManager.Instance.OnUpdateNest.AddListener(HandleUpdateNest);
        }

        private void OnDisable()
        {
            GameplayManager.Instance.OnUpdateNest.RemoveListener(HandleUpdateNest);
        }

        private void Start()
        {
            //Respawn();
        }

        void FixedUpdate()
        {
            transform.Rotate(0, 0, -_turning.x * _rotationSpeed * Time.fixedDeltaTime);

            transform.position += _linearSpeed * Time.fixedDeltaTime * transform.up
                                    // do not move when not alive
                                    * (_lifeState == LifeState.Alive ? 1 : 0);
        }

        public void HandleMove(InputAction.CallbackContext context) 
        {
            // can only turn when not dead
            if(context.started && _lifeState != LifeState.Dead)
            {
                _turning = context.ReadValue<Vector2>();
            }
            if(context.canceled)
            {
                _turning = Vector2.zero;
            }
        }

        public void HandleSpace(InputAction.CallbackContext context)
        {
            if (_lifeState == LifeState.Alive)
            {
                // TODO: hide light
                if (context.performed)
                {

                }
                // TODO: show light
                else
                {

                }
            }
            if (_lifeState == LifeState.Spawn)
            {
                _lifeState = LifeState.Alive;
            }    
        }

        private void HandleUpdateNest(Nest newNest)
        {
            // respawn when set first nest
            if (_currentNest == null)
            {
                _currentNest = newNest;
                Respawn();
            }
            else
            {
                _currentNest = newNest;
            }
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            Debug.Log("collide");
            // death on collision with obstacles
            if (col.gameObject.CompareTag("Obstacle"))
            {
                Debug.Log("die");
                Die();
            }
        }

        private void Die()
        {
            // tell others the player dead
            GameplayManager.Instance.OnFireFlyDied?.Invoke(transform.position);
            // reset movement
            _turning = Vector2.zero;
            // TODO: potential animations before spawn
            // _lifeStatus = LifeStatus.Dead;
            Respawn();
        }

        private void Respawn()
        {
            // back to spawn
            _lifeState = LifeState.Spawn;
            // go back to nest
            transform.position = _currentNest.transform.position;
        }
    }
}
