using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using DG.Tweening;
using MoreMountains.Feedbacks;
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


        private GameObject _light;
        private GameObject _playerCanvas;
        private CanvasGroup _uiGroup;
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
        private bool _stopMove;

        [SerializeField, ReadOnly]
        private int _totalNestCount;

        public Transform Transform => transform;

        private bool _lightOn = true;

        [SerializeField]
        private AudioSource _ambientSource;
        [SerializeField] private float _maxAmbientVolume;

        [Title("Sprite Renders")]
        [SerializeField] private SpriteRenderer _spriteRend;
        [SerializeField] private SpriteRenderer _reflectionRend;

        bool IEatable.Eatable { get { return LightOn; } }

        private SlowDownState SlowDown
        {
            get => _slowDown;
            set
            {
                if (_slowDown == value) return;

                _slowDown = value;
                _playerFX.SwitchFlyMode(_slowDown == SlowDownState.Slow);
            }
        }

        public bool LightOn
        {
            get => _lightOn; set
            {
                if (_lightOn == value) return;

                _lightOn = value;
                _light.SetActive(_lightOn);
                _playerFX.ToggleTrail(_lightOn);
            }

        }

        private void Awake()
        {
            _rigidBody = GetComponent<Rigidbody2D>();
            _slowCooldown = _slowCooldownTime;
            _playerCanvas = Instantiate(_UIPrefab, transform.position, Quaternion.identity);
            _uiGroup = _playerCanvas.GetComponent<CanvasGroup>();

            _slowSlider = _playerCanvas.GetComponentInChildren<Slider>();
            _light = GetComponentInChildren<FireflyLight>().gameObject;

            _playerFX = GetComponentInChildren<PlayerFX>();

            var allNests = GameObject.FindGameObjectsWithTag("Nest");
            _totalNestCount = allNests.Length;
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

            _reflectionRend.sprite = _spriteRend.sprite;
        }

        void FixedUpdate()
        {
            transform.Rotate(0, 0, -_turning.x * _rotationSpeed * Time.fixedDeltaTime);

            // do not move when not alive
            _rigidBody.velocity = (_lifeState == LifeState.Alive && !_stopMove) ?
                _linearSpeed * (SlowDown == SlowDownState.Slow ? _slowDownRatio : 1) * transform.up :
                Vector2.zero;

            _playerCanvas.transform.position = this.transform.position;
        }

        void ModifySlowDown()
        {
            switch (SlowDown)
            {
                case SlowDownState.Slow:
                    _slowDownStamina -= Time.fixedDeltaTime * _slowDrainRate;
                    if (_slowDownStamina <= 0)
                    {
                        SlowDown = SlowDownState.Emptied;
                        //_playerFX.SlowFX.StopFeedbacks();
                        //_playerFX.FlyFX.PlayFeedbacks();
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
                        SlowDown = SlowDownState.Normal;
                    }
                    break;
                case SlowDownState.Emptied:
                    if (_slowDownStamina != 1)
                    {
                        _slowDownStamina = Math.Min(_slowDownStamina + _slowRechargeRate * Time.fixedDeltaTime, 1f);
                    }
                    else
                    {
                        SlowDown = SlowDownState.Normal;
                    }
                    break;
            }

            if (_slowDownStamina != 1f) {
                if(!_slowSlider.gameObject.activeSelf) _slowSlider.gameObject.SetActive(true);
                _slowSlider.value = _slowDownStamina;
                _slowSlider.transform.position = transform.position + new Vector3(-_slowDownStamina / 2 + 0.5f, 1, 0);
            }
            else {
                if(_slowSlider.gameObject.activeSelf) _slowSlider.gameObject.SetActive(false);
            }
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
            if (offset == _activatedNests.Count) return;

            _currentNest.ToggleSelection(false);
            var idx = _activatedNests.FindIndex(n => n == _currentNest);
            idx = (idx + offset) % _activatedNests.Count;
            _currentNest = _activatedNests[idx];
            _currentNest.ToggleSelection(true);

            _playerFX.NestSelectFX.PlayFeedbacks();
        }


        public void HandleSpace(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                if (_lifeState == LifeState.Spawn)
                {
                    _lifeState = LifeState.Alive;
                    _playerFX.StartFly();
                    //_playerFX.FlyFX.PlayFeedbacks();
                    
                    GameplayManager.Instance.OnPlayerRespawn.Invoke();
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

        public void HandleToggleLight(InputAction.CallbackContext context)
        {
            if (_lifeState == LifeState.Alive)
            {
                if (context.performed)
                {
                    LightOn = false;
                }
                if (context.canceled)
                {
                    LightOn = true;
                }
            }
        }

        public void HandleSlowDownKey(InputAction.CallbackContext context) 
        {
            // Only change slowdown if alive
            if (_lifeState == LifeState.Alive)
            {
                if (context.performed && (SlowDown == SlowDownState.Normal || SlowDown == SlowDownState.Cooldown))
                {
                    SlowDown = SlowDownState.Slow;
                    //_playerFX.FlyFX.StopFeedbacks();
                    //_playerFX.SlowFlyFX.PlayFeedbacks();
                }
                else if (context.canceled && SlowDown == SlowDownState.Slow)
                {
                    SlowDown = SlowDownState.Cooldown;
                    //_playerFX.SlowFlyFX.StopFeedbacks();
                    //_playerFX.FlyFX.PlayFeedbacks();
                    _slowCooldown = _slowCooldownTime;
                }
            }
        }

        private void HandleUpdateNest(Nest newNest)
        {
            if (!_activatedNests.Contains(newNest))
            {
                _activatedNests.Add(newNest);
                if (_activatedNests.Count >= _totalNestCount)
                {
                    GameplayManager.Instance.OnLevelClear.Invoke();
                }

                _ambientSource.volume = (float)_activatedNests.Count / _totalNestCount * _maxAmbientVolume;
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
                _playerFX.DeathFX.PlayFeedbacks();
                StartCoroutine(Die());
                Invoke(nameof(PlantFlower), .2f);
            }
        }

        private void PlantFlower()
        {
            // tell others the player dead
            GameplayManager.Instance.OnFireFlyDied?.Invoke(transform.position);
        }

        private IEnumerator Die()
        {
            // fade out
            _spriteRend.DOFade(0, .2f);
            _uiGroup.DOFade(0, .2f);

            //_playerFX.FlyFX.StopFeedbacks();
            //_playerFX.SlowFlyFX.StopFeedbacks();
            _playerFX.StopFly();
            // reset movement
            _turning = Vector2.zero;
            // TODO: potential animations before spawn
            // start nest selection and respawn after it
            _currentNest.ToggleSelection(true);
            _stopMove = true;
            yield return new WaitForSeconds(.5f);
            GameplayManager.Instance.EnterMapMode();
            _lifeState = LifeState.Dead;
            _stopMove = false;
        }

        private void Respawn()
        {
            // back to spawn
            _lifeState = LifeState.Spawn;
            // go back to nest
            transform.SetPositionAndRotation(_currentNest.transform.position, _currentNest.transform.rotation);
            SlowDown = SlowDownState.Normal;
            _slowCooldown = _slowCooldownTime;
            _slowDownStamina = 1;

            // fade in
            _spriteRend.DOFade(1, .25f);
            _uiGroup.DOFade(1, .2f);

            LightOn = true;
        }

        public void GetCaught()
        {
            _lifeState = LifeState.Dead;
            _playerFX.DeathFX.PlayFeedbacks();
        }

        public void GetEaten() => StartCoroutine(Die());
    }
}
