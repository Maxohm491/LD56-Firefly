using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Sirenix.OdinInspector;
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
        [SerializeField] private float _slowCooldownTime = 0.4f;

        [Tooltip("What fraction of total it recharges per second")]
        [SerializeField] private float _slowRechargeRate = 0.6f;

        [Tooltip("How fast it drains while you're slow")]
        [SerializeField] private float _slowDrainRate = 0.6f;

        [Tooltip("Fraction speed is multiplied by when slow")]
        [SerializeField] private float _slowDownRatio = 0.2f;

        [Tooltip("Canvas that follows player prefab")]
        [SerializeField] private GameObject _UIPrefab;

        private GameObject _playerCanvas;
        private Slider _slowSlider;

        private Rigidbody2D _rigidBody;

        private PlayerFX _playerFX;
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
            Slow,
            Emptied
        }

        private SlowDownState _slowDown = SlowDownState.Normal;
        private float _slowCooldown;

        // 0 to 1 
        private float _slowDownStamina = 1.0f;

        private LifeState _lifeState = LifeState.Spawn;

        private Vector2 _turning;

        private Nest _currentNest;
        [SerializeField, ReadOnly]
        private List<Nest> _activatedNests = new List<Nest>();

        public Transform Transform => transform;

        private void Awake()
        {
            _rigidBody = GetComponent<Rigidbody2D>();
            _slowCooldown = _slowCooldownTime;
            _playerCanvas = Instantiate(_UIPrefab, transform.position, Quaternion.identity);
            _slowSlider = _playerCanvas.GetComponentInChildren<Slider>();

            _playerFX = GetComponentInChildren<PlayerFX>();
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

        private void Update()
        {
            ModifySlowDown();
        }

        void FixedUpdate()
        {
            transform.Rotate(0, 0, -_turning.x * _rotationSpeed * Time.fixedDeltaTime);

            // do not move when not alive
            _rigidBody.velocity = _lifeState == LifeState.Alive ?
                _linearSpeed * (_slowDown == SlowDownState.Slow ? _slowDownRatio : 1) * transform.up :
                Vector2.zero;

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
                        _slowDown = SlowDownState.Emptied;
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
                case SlowDownState.Emptied:
                    if (_slowDownStamina != 1)
                    {
                        _slowDownStamina = Math.Min(_slowDownStamina + _slowRechargeRate * Time.fixedDeltaTime, 1f);
                    }
                    else
                    {
                        _slowDown = SlowDownState.Normal;
                    }
                    break;
            }

            _slowSlider.value = _slowDownStamina;
            _slowSlider.transform.position = transform.position + new Vector3(-_slowDownStamina / 2 + 0.5f, 1, 0);
        }

        public void HandleMove(InputAction.CallbackContext context)
        {
            // can only turn when not dead
            if (context.started && _lifeState != LifeState.Dead)
            {
                _turning = context.ReadValue<Vector2>();
            }
            if (context.started && _lifeState == LifeState.Dead)
            {
                var select = context.ReadValue<Vector2>();
                if (select.x > 0)
                {
                    SelectNest(1);
                }
                else if (select.x < 0)
                {
                    SelectNest(_activatedNests.Count - 1);
                }
            }

            if (context.canceled)
            {
                _turning = Vector2.zero;
            }
        }

        private void SelectNest(int offset)
        {
            _currentNest.ToggleSelection(false);
            var idx = _activatedNests.FindIndex(n => n == _currentNest);
            idx = (idx + offset) % _activatedNests.Count;
            _currentNest = _activatedNests[idx];
            _currentNest.ToggleSelection(true);
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
                return;
            }

            if (context.performed)
            {
                if (_lifeState == LifeState.Spawn)
                {
                    _lifeState = LifeState.Alive;
                    _playerFX.FlyFX.PlayFeedbacks();
                    return;
                }
                if (_lifeState == LifeState.Dead)
                {
                    Respawn();
                    GameplayManager.Instance.ExitMapMode();
                    return;
                }
            }
        }

        public void HandleShift(InputAction.CallbackContext context)
        {
            // Only change slowdown if alive
            if (_lifeState == LifeState.Alive)
            {
                if (context.performed && (_slowDown == SlowDownState.Normal || _slowDown == SlowDownState.Cooldown))
                {
                    _slowDown = SlowDownState.Slow;
                    _playerFX.SlowFX.PlayFeedbacks();
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
            if (!_activatedNests.Contains(newNest))
            {
                _activatedNests.Add(newNest);
            }

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
                // tell others the player dead
                GameplayManager.Instance.OnFireFlyDied?.Invoke(transform.position);
                _playerFX.DeathFX.PlayFeedbacks();
                StartCoroutine(Die());
            }
        }

        private IEnumerator Die()
        {
            _playerFX.FlyFX.StopFeedbacks();
            // reset movement
            _turning = Vector2.zero;
            // TODO: potential animations before spawn
            // start nest selection and respawn after it
            _lifeState = LifeState.Dead;
            _currentNest.ToggleSelection(true);

            yield return new WaitForSeconds(.5f);
            GameplayManager.Instance.EnterMapMode();
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
            _playerFX.DeathFX.PlayFeedbacks();
        }

        public void GetEaten() => StartCoroutine(Die());
    }
}
