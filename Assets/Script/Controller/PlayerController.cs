using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;


namespace Script.Controller
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        private HandDisplay _handDisplay;
        private CharacterUIDisplay _characterUIDisplay;
        private PlayerInputManager _playerInputManager;
        [HideInInspector] public InputSetting _inputSetting;
        
        [Header("Team")]
        public Team team;
        
        [Header("Player Ability")] 
        [SerializeField] private float speedMove;
        [SerializeField] private float speedDownStamina;
        [SerializeField] private float speedUpStamina;
        [SerializeField] private float speedUpStaminaQuickly;
        [SerializeField] private float speedDownStaminaQuickly;
        private float initialSpeed;

        private string keyHorizontal;
        private string keyVertical;
        private string keyJump;

        private Rigidbody _rigidbody;
        private SpriteRenderer _spriteRenderer;
        private Transform currentStuff;

        public bool isDead;
        public bool isEnter;
        private bool allowHold;
        private bool isHold;
        
        private float vertical;
        private float horizontal;

        private int limitTired = 40;
        private int limitDead = 10;
        
        private Coroutine currentCoroutine;

        public float Stamina
        {
            get => stamina;

            set
            {
                stamina = value;
                if (stamina > limitTired)
                {
                    if (State != HandState.normal)
                        State = HandState.normal;
                }
                else if (stamina > limitDead)
                {
                    if (State != HandState.tired)
                        State = HandState.tired;
                }
                else
                {
                    if (State != HandState.dead)
                        State = HandState.dead;
                }
            }
        }
        
        private float stamina = 100;
        private float staminaMax = 100;

        HandState State
        {
            get => state;
            set
            {
                state = value;
                switch (value)
                {
                    case HandState.normal:
                        _handDisplay.SetAlpha(HandColor.normal);
                        _characterUIDisplay.ChangeCharacterUI(CharacterUIState.idle);
                        isDead = false;
                        break;
                    case HandState.tired:
                        _handDisplay.StartTiredAnimation();
                        _characterUIDisplay.ChangeCharacterUI(CharacterUIState.tired);
                        isDead = false;
                        break;
                    case HandState.dead:
                        _handDisplay.SetAlpha(HandColor.dead);
                        _characterUIDisplay.ChangeCharacterUI(CharacterUIState.dead);
                        isDead = true;
                        Break(currentStuff);
                        StartUp(5f);
                        break;
                }
            }
        }

        HandState state = HandState.normal;

        
        [Header("Menu Screen")] 
        [Space(10)] 
        [Header("Start Position Target")] 
        [SerializeField] Transform startPosition;

        [Header("Border")] 
        [SerializeField] Transform limitVertical;
        [SerializeField] Transform limitHorizontal;

        [Header("Shorting Layer")]
        float selectUILayer = 25;
        float gameplayLayer = 5;

        [Header("Referans UI Select Start Position")]
        [SerializeField] Transform selectPositionStart;


        [Header("UI Select Collision Name")] [SerializeField]
        string collisonName;

        [Header("Select UI Animator")] [SerializeField]
        Animator selectAnim;


        [Header("Ready Text")] [SerializeField]
        Transform readText;

        public bool isReady;

        [Header("Stamina Display")]
        [SerializeField] Image imageStamina;


        [Header("Power Display")]
        [SerializeField] private GameObject freeze;
        [SerializeField] private GameObject gold;
        private bool isGold;
        
        [Header("Mini Game")]
        [HideInInspector] public bool startMiniGame;
        [HideInInspector] public MiniGameController _miniGameController;
        public MiniGame minigame;

        void Start()
        {
            PlayerManager.Instance.playerList.Add(this);
            
            _handDisplay = GetComponent<HandDisplay>();
            _characterUIDisplay = GetComponent<CharacterUIDisplay>();
            _miniGameController = GetComponent<MiniGameController>();

            _playerInputManager = PlayerInputManager.Instance;
            _rigidbody = GetComponent<Rigidbody>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            
            initialSpeed = speedMove;

            keyHorizontal = _inputSetting.keyHorizontal;
            keyVertical = _inputSetting.keyVertical;
            keyJump = _inputSetting.keyJump;

            SetPositionUISelect();

            _characterUIDisplay.ChangeCharacterUI(CharacterUIState.idle);
        }

        void SetPositionUISelect()
        {
            transform.position = selectPositionStart.position;
            _spriteRenderer.sortingOrder = 25;
        }

        public void SetPositionStart()
        {
            transform.position = startPosition.position;
            _spriteRenderer.sortingOrder = 5;

            speedMove = initialSpeed;

            _handDisplay.SetSprite(HandSpriteState.idle);
        }

        public void SpriteFlip(float time)
        {
            _spriteRenderer.enabled = false;
            IEnumerator _Active(float delay)
            {
                yield return new WaitForSeconds(delay);
                _spriteRenderer.enabled = true;    
            }
            StartCoroutine(_Active(time));
        }

        [Header("Force")]
        public  float force = 69.0f;
        public float radius = 5.0f;
        public float upwardsModifier = 0.0f;
        public ForceMode forceMode;
        public void Hit()
        {
            foreach (Collider col in Physics.OverlapSphere(transform.position, radius))
            {
                if (col.attachedRigidbody != null)
                {
                    if (col.transform.CompareTag("Player") && col.transform != transform)
                    {
                        col.attachedRigidbody.AddExplosionForce(force, transform.position, radius, upwardsModifier, forceMode);
                    }
                }
            }
        }

        void PositionLimitController()
        {
            //Horizontal Controller
            if (transform.position.x > limitHorizontal.position.x)
            {
                transform.position = transform.position.With(x: limitHorizontal.position.x);
            }
            else if (transform.position.x < -limitHorizontal.position.x)
            {
                transform.position = transform.position.With(x: -limitHorizontal.position.x);
            }

            //Vertical Controller
            if (transform.position.z > limitVertical.position.z)
            {
                transform.position = transform.position.With(z: limitVertical.position.z);
            }
            else if (transform.position.z < -limitVertical.position.z)
            {
                transform.position = transform.position.With(z: -limitVertical.position.z);
            }
        }

        private void InputController()
        {
            if (startMiniGame)
            {
                SetMiniGame();
                return;
            }


            //Buradan asagısı klasik oyun oyun gerekli kısım
            _rigidbody.velocity = GetVelocity();

            if (isDead)
                return;

            /* HOLD */

            if (isEnter)
            {
                if (Input.GetButton(keyJump))
                {
                    if (!isHold && !isDead)
                    {
                        Hold(currentStuff);
                    }
                }
                else if (isHold)
                {
                    Break(currentStuff);
                }
                else if (gameObject.GetComponent<CharacterJoint>() != null)
                {
                    Break(currentStuff);
                }
            }
            else
            {
                if (isHold)
                {
                    Break(currentStuff);
                }
                else if (gameObject.GetComponent<CharacterJoint>() != null)
                {
                    Break(currentStuff);
                }
            }
        }

        private Vector3 GetVelocity()
        {
            vertical = Input.GetAxis(keyVertical);
            horizontal = Input.GetAxis(keyHorizontal);

            Vector3 dir = Vector3.zero;

            if (Math.Abs(vertical) > 0.05f || Math.Abs(horizontal) > 0.05f)
            {
                dir = (Vector3.forward * vertical) + (Vector3.right * horizontal);
            }

            Vector3 velocity = dir * speedMove; //Normalized ettik.
            return velocity;
        }

        private void Update()
        {
            if (imageStamina != null)
                imageStamina.fillAmount = stamina / staminaMax;
        }

        void FixedUpdate()
        {
            InputController();
        }

        private void LateUpdate()
        {
            PositionLimitController();
        }

        public void Hold(Transform current)
        {
            if (Math.Abs(speedMove) < 0.01f) //isFreeze
            {
                return;
            }

            _characterUIDisplay.ChangeCharacterUI(CharacterUIState.hold);

            _handDisplay.SetSprite(HandSpriteState.hold);

            //Kendime bir joint component'i ekle.
            CharacterJoint joint = gameObject.AddComponent<CharacterJoint>();

            //Objenin Tutulma fonksiyonu cagırılıyor.
            currentStuff.GetComponent<StuffController>().Hold(this);

            //Connected body'sine current objeyi ver.
            joint.connectedBody = current.GetComponent<Rigidbody>();

            isHold = true;

            if (isGold)
                ConvertStuffToGold(currentStuff.GetComponent<StuffController>());


            Down();
        }

        public void Break(Transform currentStuff)
        {
            if (!isDead)
                _characterUIDisplay.ChangeCharacterUI(CharacterUIState.idle);

            _handDisplay.SetSprite(HandSpriteState.idle); //Hand turn Idle

            if (currentStuff != null)
            {
                currentStuff.GetComponent<StuffController>().Break(this);
            }

            Destroy(gameObject.GetComponent<CharacterJoint>());

            _rigidbody.angularVelocity = Vector3.zero;

            isHold = false;

            if (!isDead)
                Up();

        }

        #region mini game

        private void SetMiniGame()
        {
            vertical = Input.GetAxis(keyVertical);
            horizontal = Input.GetAxis(keyHorizontal);

            const float SENSEVITE = 0.2f;
            if (horizontal < -SENSEVITE) // Sol
            {
                minigame = MiniGame.rock;
            }
            else if (vertical > SENSEVITE) //Yukarı
            {
                minigame = MiniGame.paper;
            }
            else if (horizontal > SENSEVITE) //Sağ
            {
                minigame = MiniGame.scissors;
            }
        }

        public void LostMiniGame()
        {
            if (currentCoroutine != null)
                StopCoroutine(currentCoroutine);
        
            Stamina = 0;

            IEnumerator _InactiveMiniGame(float delay)
            {
                yield return new WaitForSeconds(delay);
                startMiniGame = false;
            }

            StartCoroutine(_InactiveMiniGame(2));
        }

        #endregion

        #region Trigger

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Stuff") && isEnter == false)
            {
                isEnter = true;
                currentStuff = other.transform;
            }

            if (other.CompareTag("PowerUp"))
            {
                if(!isDead)
                    other.GetComponent<PowerUpController>().Use(this);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Stuff") && !isHold)
            {
                currentStuff = null;
                isEnter = false;
            }
        }


        //Burası oyun baslagnıc ekranında oyuncular karakterleri secerken kullanıyor.
        bool isEventBig;
        bool isEventSmall;

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Select") && other.name == collisonName)
            {
                if (Input.GetButton(keyJump))
                {
                    //Hold Yapabilir.
                    _handDisplay.SetSprite(HandSpriteState.hold);
                    isReady = true;
                    selectAnim.enabled = true;
                    speedMove = 0;

                    isEventSmall = false;
                    if (!isEventBig)
                    {
                        isEventBig = !isEventBig;
                        _playerInputManager.ReadBig(readText);
                        _playerInputManager.ReadyController();
                    }
                }
                else
                {
                    isEventBig = false;
                    if (!isEventSmall)
                    {
                        isEventSmall = !isEventSmall;
                        _playerInputManager.ReadSmall(readText);
                    }

                    _handDisplay.SetSprite(HandSpriteState.idle);
                    selectAnim.enabled = false;
                    isReady = false;
                    speedMove = initialSpeed;
                }
            }
        }

        #endregion

        #region Stamina

        public void StartUp(float delay)
        {
            if (currentCoroutine != null)
                StopCoroutine(currentCoroutine);

            currentCoroutine = StartCoroutine(_StartUp(delay));
        }
    
        IEnumerator _StartUp(float delay)
        {
            yield return new WaitForSeconds(delay);
            Up();
        }

        public void Up(ChangeSpeed changeSpeed = ChangeSpeed.normal)
        {
            if (currentCoroutine != null)
                StopCoroutine(currentCoroutine);

            float speed = 1;
            switch (changeSpeed)
            {
                case ChangeSpeed.normal:
                    speed = speedUpStamina;
                    break;
                case ChangeSpeed.quickly:
                    speed = speedUpStaminaQuickly;
                    break;
            }

            currentCoroutine = StartCoroutine(_Up(speed));
        }

        IEnumerator _Up(float speed)
        {
            while (Stamina < staminaMax)
            {
                Stamina += Time.deltaTime * speed;
                yield return null;
            }
        }

        public void Down(ChangeSpeed changeSpeed = ChangeSpeed.normal)
        {
            if (currentCoroutine != null)
                StopCoroutine(currentCoroutine);

            float speed = 1;
            switch (changeSpeed)
            {
                case ChangeSpeed.normal:
                    speed = speedDownStamina;
                    break;
                case ChangeSpeed.quickly:
                    speed = speedDownStaminaQuickly;
                    break;
            }

            currentCoroutine = StartCoroutine(_Down(speed));
        }

        IEnumerator _Down(float speed)
        {
            while (Stamina > 0)
            {
                Stamina -= Time.deltaTime * speed;
                yield return null;
            }
        }

        #endregion

        #region PowerUp

        private void ConvertStuffToGold(StuffController stuff)
        {
            stuff.ConvertGold(10);
        }

        public void SetSpeed(float newSpeed)
        {
            speedMove = newSpeed;
        }

        public void ResetSpeed()
        {
            speedMove = initialSpeed;
        }

        public void Donkey(float time)
        {
            gold.SetActive(true);
            isGold = true;
            
            IEnumerator LostDonkey(float delay)
            {
                yield return new WaitForSeconds(delay);
                gold.SetActive(false);
                isGold = false;
            }
            StartCoroutine(LostDonkey(time));
        }


        public void Freeze(float time)
        {
            speedMove = 0;
            if (currentStuff != null)
            {
                Break(currentStuff);
            }
            
            freeze.SetActive(true);
            
            IEnumerator _WarmUp(float delay)
            {
                print("_warp up");
                yield return new WaitForSeconds(delay);
                freeze.SetActive(false);
                ResetSpeed();
            }
            StartCoroutine(_WarmUp(time));
        }


        #endregion
    }
}