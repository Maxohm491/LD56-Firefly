using System;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
namespace Firefly
{
    public class FireflyPlayer : MonoBehaviour, IEatable
    {
        [Tooltip("Units/sec")]
        [SerializeField] private float _linearSpeed = 5f;

        [Tooltip("Degrees/sec")]
        [SerializeField] private float _rotationSpeed = 270f;

        [Tooltip("How long after slowing down before it starts recharging")]
        [SerializeField] private float _slowCooldownTime = 0.2f;

        [Tooltip("What fraction of total it recharges per second")]
        [SerializeField] private float _slowRechargeRate = 0.5f;

        [Tooltip("How fast it drains while you're slow")]
        [SerializeField] private float _slowDrainRate = 0.5f;

        [Tooltip("Fraction speed is multiplied by when slow")]
        [SerializeField] private float _slowDownRatio = 0.2f;

        [Tooltip("Canvas that follows player prefab")]
        [SerializeField] private GameObject _UIPrefab;

        private GameObject _playerCanvas;
        private Slider _slowSlider;

        private Rigidbody2D _rigidBody;

        enum LifeState
        {
            Spawn,
            Alive,
            Dead,
        }

        enum SlowDownState
        {
            Normal,
            Cooldown,
            Slow
        }

        private SlowDownState _slowDown = SlowDownState.Normal;
        private float _slowCooldown;

        // 0 to 1 
        private float _slowDownStamina = 1.0f;

        private LifeState _lifeState = LifeState.Spawn;

        private Vector2 _turning;

        private Nest _currentNest;

        public Transform Transform => transform;

        private void Awake()
        {
            _rigidBody = GetComponent<Rigidbody2D>();
            _slowCooldown = _slowCooldownTime;
            _playerCanvas = Instantiate(_UIPrefab, transform.position, Quaternion.identity);
            _slowSlider = _playerCanvas.GetComponentInChildren<Slider>();
        }

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

            // do not move when not alive
            _rigidBody.velocity = _lifeState == LifeState.Alive ?
                _linearSpeed * (_slowDown == SlowDownState.Slow ? _slowDownRatio : 1) * transform.up :
                Vector2.zero;

            ModifySlowDown();

            // Make player canvas follow player
            _playerCanvas.transform.position = this.transform.position;
        }

        void ModifySlowDown()
        {
            switch (_slowDown)
            {
                case SlowDownState.Slow:
                    _slowDownStamina -= Time.fixedDeltaTime * _slowDrainRate;
                    if (_slowDownStamina <= 0)
                    {
                        _slowDown = SlowDownState.Cooldown;
                        _slowCooldown = _slowCooldownTime;
                    }
                    break;
                case SlowDownState.Normal:
                    if (_slowDownStamina != 1)
                    {
                        _slowDownStamina = Math.Min(_slowDownStamina + _slowRechargeRate * Time.fixedDeltaTime, 1f);
                    }
                    break;
                case SlowDownState.Cooldown:
                    // don't recharge during cooldown
                    _slowCooldown -= Time.fixedDeltaTime;
                    if (_slowCooldown <= 0) 
                    {
                        _slowDown = SlowDownState.Normal;
                    }
                    break;
            }

            _slowSlider.value = _slowDownStamina;
        }

        public void HandleMove(InputAction.CallbackContext context)
        {
            // can only turn when not dead
            if (context.started && _lifeState != LifeState.Dead)
            {
                _turning = context.ReadValue<Vector2>();
            }
            if (context.canceled)
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

        public void HandleShift(InputAction.CallbackContext context)
        {
            // Only change slowdown if alive
            if (_lifeState == LifeState.Alive)
            {
                if (context.performed && _slowDown != SlowDownState.Slow)
                {
                    _slowDown = SlowDownState.Slow;
                }
                else if (context.canceled && _slowDown == SlowDownState.Slow)
                {
                    _slowDown = SlowDownState.Cooldown;
                    _slowCooldown = _slowCooldownTime;
                }
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
            // death on collision with obstacles
            if (col.gameObject.CompareTag("Obstacle"))
            {
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
            transform.SetPositionAndRotation(_currentNest.transform.position, _currentNest.transform.rotation);
            _slowDown = SlowDownState.Normal;
            _slowDownStamina = 1;
        }

        public void GetCaught()
        {
            _lifeState = LifeState.Dead;
        }

        public void GetEaten()
        {
            // reset movement
            _turning = Vector2.zero;
            // TODO: potential animations before spawn
            // _lifeStatus = LifeStatus.Dead;
            Respawn();
        }
    }
}
