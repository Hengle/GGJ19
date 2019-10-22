using System;
using System.Collections;
using UnityEngine;

public enum UIState
{
    idle,
    hold,
    tired,
    dead
}

public enum HandState
{
    normal,
    tired,
    dead
}

public enum ChangeSpeed
{
    normal,
    quickly
}

public enum Team
{
    hot,
    cold
}

public class PlayerController : MonoBehaviour
{
    [Header("Hand Image")] [SerializeField]
    Sprite spriteHold;

    [SerializeField] Sprite spriteIdle;

    [Header("Player Ability")] [SerializeField]
    float speedMove;

    [SerializeField] float speedDownStamina = 15;
    [SerializeField] float speedUpStamina = 15;
    [SerializeField] float speedUpStaminaQuickly = 20;
    [SerializeField] float speedDownStaminaQuickly = 20;

    [Header("Animation UI")] [SerializeField]
    Transform uiIdle;

    [SerializeField] Transform uiHold;
    [SerializeField] Transform uiTired;
    [SerializeField] Transform uiDead;

    string keyHorizontal;
    string keyVertical;
    string keyJump;

    Rigidbody rg;
    SpriteRenderer spriteRenderer;
    Transform currentStuff = null;

    bool allowHold;
    bool isHold;
    public bool isEnter;
    float vertical;
    float horizontal;

    public float Stamina
    {
        get { return stamina; }

        set
        {
            stamina = value;
            if (stamina > 40)
            {
                if (State != HandState.normal)
                    State = HandState.normal;
            }
            else if (stamina > 10)
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

    float staminaMax = 100;

    ColorFunctions cf;
    SelectController selectController;

    Coroutine currentCoroutine;
    Coroutine colorFlip;

    public bool isDead;

    HandState State
    {
        get { return state; }
        set
        {
            state = value;
            switch (value)
            {
                case HandState.normal:
                    StopHandTired();
                    HandColorNormal();
                    ChangeUI(UIState.idle);
                    isDead = false;
                    break;
                case HandState.tired:
                    StartHandTired();
                    ChangeUI(UIState.tired);
                    isDead = false;
                    break;
                case HandState.dead:
                    StopHandTired();
                    DeadColorHand();
                    ChangeUI(UIState.dead);
                    isDead = true;
                    Break(currentStuff);
                    break;
            }
        }
    }

    HandState state = HandState.normal;

    public Team team;
    float startSpeed;

    [Header("Start Position Target")] [SerializeField]
    Transform startPosition;

    [Header("Border")] [SerializeField] Transform limitVertical;
    [SerializeField] Transform limitHorizontal;

    [Header("Shorting Layer")] float selectUILayer = 25;
    float gameplayLayer = 5;

    public InputSetting inputSetting;

    [Header("Referans UI Select Start Position")] [SerializeField]
    Transform selectPositionStart;


    [Header("UI Select Collision Name")] [SerializeField]
    string collisonName;

    [Header("Select UI Animator")] [SerializeField]
    Animator selectAnim;


    [Header("Ready Text")] [SerializeField]
    Transform readText;

    public bool isReady;
    private GameObject freeze;

    void Start()
    {
        print("Hello i am " + transform.name);
        selectController = FindObjectOfType<SelectController>();
        cf = GetComponent<ColorFunctions>();
        rg = GetComponent<Rigidbody>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        startSpeed = speedMove;

        keyHorizontal = inputSetting.keyHorizontal;
        keyVertical = inputSetting.keyVertical;
        keyJump = inputSetting.keyJump;

        SetPositionUISelect();

        ChangeUI(UIState.idle);

        freeze = transform.GetChild(0).gameObject;
    }

    void SetPositionUISelect()
    {
        transform.position = selectPositionStart.position;
        spriteRenderer.sortingOrder = 25;
    }

    public void SetPositionStart()
    {
        transform.position = startPosition.position;
        spriteRenderer.sortingOrder = 5;

        speedMove = startSpeed;

        ChangeHandSprite(spriteIdle);
    }

    void ChangeUI(UIState state)
    {
        uiIdle.gameObject.SetActive(false);
        uiHold.gameObject.SetActive(false);
        uiTired.gameObject.SetActive(false);
        uiDead.gameObject.SetActive(false);

        switch (state)
        {
            case UIState.idle:
                uiIdle.gameObject.SetActive(true);
                break;
            case UIState.hold:
                uiHold.gameObject.SetActive(true);
                break;
            case UIState.tired:
                uiTired.gameObject.SetActive(true);
                break;
            case UIState.dead:
                uiDead.gameObject.SetActive(true);
                break;
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


    
    public void Freeze(float time)
    {
        speedMove = 0;
        freeze.SetActive(true);
        StartCoroutine(WarmUp(time));
    }

    IEnumerator WarmUp(float delay)
    {
        yield return new WaitForSeconds(delay);
        freeze.SetActive(false);
        ResetSpeed();
    }

    void InputController()
    {
        rg.velocity = GetVelocity();

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

        Vector3 velocity = Vector3.zero;
        Vector3 dir = Vector3.zero;

        if (vertical != 0 || horizontal != 0)
        {
            dir = (Vector3.forward * vertical) + (Vector3.right * horizontal);
        }

        velocity = dir * speedMove; //Normalized ettik.
        return velocity;
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
        print("Hold");

        ChangeUI(UIState.hold);

        ChangeHandSprite(spriteHold);

        //Kendime bir joint component'i ekle.
        CharacterJoint joint = gameObject.AddComponent<CharacterJoint>();

        //Objenin Tutulma fonksiyonu cagırılıyor.
        currentStuff.GetComponent<StuffController>().Hold(this);

        //Connected body'sine current objeyi ver.
        joint.connectedBody = current.GetComponent<Rigidbody>();

        isHold = true;

        Down();
    }

    public void Break(Transform currentStuff)
    {
        print("Break");
        ChangeUI(UIState.idle);

        ChangeHandSprite(spriteIdle); //Hand turn Idle

        if (currentStuff != null)
        {
            currentStuff.GetComponent<StuffController>().Break(this);
        }

        Destroy(gameObject.GetComponent<CharacterJoint>());

        rg.angularVelocity = Vector3.zero;

        isHold = false;

        if (!isDead)
            Up();
        else
           StartCoroutine(StartUp(3));
    }

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
            print("Power Up Aldım");
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
                ChangeHandSprite(spriteHold);
                isReady = true;
                selectAnim.enabled = true;
                speedMove = 0;

                isEventSmall = false;
                if (!isEventBig)
                {
                    isEventBig = !isEventBig;
                    selectController.ReadBig(readText);
                    selectController.ReadyController();
                }
            }
            else
            {
                isEventBig = false;
                if (!isEventSmall)
                {
                    isEventSmall = !isEventSmall;
                    selectController.ReadSmall(readText);
                }

                ChangeHandSprite(spriteIdle);
                selectAnim.enabled = false;
                isReady = false;
                speedMove = startSpeed;
            }
        }
    }

    #endregion

    #region Stamina

    IEnumerator StartUp(float delay)
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

    #region Hand Color

    void ChangeHandSprite(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }

    void StartHandTired()
    {
        colorFlip = StartCoroutine(cf._ChangeColorLoop(transform, 0.3f, 0.45f, 0.95f));
    }

    void StopHandTired()
    {
        if (colorFlip != null)
        {
            StopCoroutine(colorFlip);
        }
    }

    void DeadColorHand()
    {
        cf.ColorTransition(spriteRenderer, spriteRenderer.color.With(a: .35f), 0, 0.2f);
    }

    void HandColorNormal()
    {
        cf.ColorTransition(spriteRenderer, spriteRenderer.color.With(a: 1f), 0, 0.2f);
    }

    #endregion

    #region Speed

    public void SetSpeed(float newSpeed)
    {
        speedMove = newSpeed;
    }

    public void ResetSpeed()
    {
        speedMove = startSpeed;
    }

    #endregion
}