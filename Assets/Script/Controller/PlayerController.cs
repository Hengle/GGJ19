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

public class PlayerController : MonoBehaviour
{
    [Header("Hand Image")]
    [SerializeField] Sprite spriteHold;
    [SerializeField] Sprite spriteIdle;

    [Header("Player Ability")]
    [SerializeField] float speedMove;
    [SerializeField] float speedDownStamina = 15;
    [SerializeField] float speedUpStamina = 15;
    [SerializeField] float speedUpStaminaQuickly = 20;
    [SerializeField] float speedDownStaminaQuickly = 20;

    [Header("Animation UI")]
    [SerializeField] Transform uiIdle;
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

    public float stamina = 100;
    float staminaMax = 100;

    ColorFunctions cf;

    Coroutine current;
    Coroutine colorFlip;

    public bool isDead;

    HandState state = HandState.normal;

    public bool isRedTeam;
    float startSpeed;

    [Header("Start Position Target")]
    [SerializeField] Transform startPosition;

    [Header("Border")]
    [SerializeField] Transform limitVertical;
    [SerializeField] Transform limitHorizontal;

    [Header("Shorting Layer")]
    float selectUILayer = 25;
    float gameplayLayer = 5;


    public InputSetting inputSetting;

    [Header("Referans UI Select Start Position")]
    [SerializeField] Transform selectPositionStart;

    void Start()
    {
       
        cf = FindObjectOfType<ColorFunctions>();
        rg = GetComponent<Rigidbody>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        startSpeed = speedMove;

        keyHorizontal = inputSetting.keyHorizontal;
        keyVertical = inputSetting.keyVertical;
        keyJump = inputSetting.keyJump;

        transform.position = selectPositionStart.position;
        spriteRenderer.sortingOrder = 25;

        ChangeUI(UIState.idle);
    }

    void SetPositionStart()
    {
        transform.position = startPosition.position;
        spriteRenderer.sortingLayerID = 5;
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
            default:
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


    void InputController()
    {
        rg.velocity = GetVelocity();

        if (isDead)
        {
            print("Öldün");
            return;
        }

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
        HandController();

        PositionLimitController();
    }

    public void Hold(Transform current)
    {
        print("Hold");

        ChangeUI(UIState.hold);

        ChangeHandSprite(spriteHold);

        //Kenime bir joint componenti ekle.
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

        Up();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Stuff" && isEnter == false)
        {
            isEnter = true;
            currentStuff = other.transform;
        }

        if (other.tag == "PowerUp")
        {
            print("Power Up Aldım");
            other.GetComponent<PowerUpController>().Use(this);
        }

        if (other.tag == "Select")
        {
            print(other.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Stuff" && !isHold)
        {
            currentStuff = null;
            isEnter = false;
        }
    }

    #region Stamina

    public void Up()
    {
        if (current != null)
        {
            StopCoroutine(current);
        }

        current = StartCoroutine(_Up(speedUpStamina));
    }

    public void UpQuickly()
    {
        if (current != null)
        {
            StopCoroutine(current);
        }

        current = StartCoroutine(_Up(speedUpStaminaQuickly));
    }

    IEnumerator _Up(float speed)
    {
        while (stamina < staminaMax)
        {
            stamina += Time.deltaTime * speed;
            yield return null;
        }

        isDead = false;
    }

    public void Down()
    {
        if (current != null)
        {
            StopCoroutine(current);
        }

        current = StartCoroutine(_Down(speedDownStamina));
    }

    public void DownQuickly()
    {
        if (current != null)
        {
            StopCoroutine(current);
        }

        current = StartCoroutine(_Down(speedDownStaminaQuickly));
    }

    IEnumerator _Down(float speed)
    {
        while (stamina > 0)
        {
            stamina -= Time.deltaTime * speed;
            yield return null;
        }

        isDead = true;
        print("Is Dead");
    }

    #endregion


    #region Hand Color

    void ChangeHandSprite(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }

    void HandTiredStart()
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

    #region Listener

    bool[] onWorked = new bool[4] { true, true, true, true };

    void SetArray(int index)
    {
        for (int i = 0; i < 4; i++)
        {
            onWorked[i] = true;
        }

        onWorked[index] = false;
    }

    bool BoolControl(int index)
    {
        for (int i = 0; i < 4; i++)
        {
            if (onWorked[i] == true)
            {
                if (i == index)
                {
                    return true;
                }
            }
        }

        return false;
    }

    void HandController()
    {
        if (BoolControl(0))
        {
            if (HandState.normal == state && stamina < 40)
            {
                print("normalde tired 'e ");
                state = HandState.tired;

                HandTiredStart();

                SetArray(0);

                ChangeUI(UIState.tired);

                return;
            }
        }

        if (BoolControl(1))
        {
            if (HandState.tired == state && stamina < 10)
            {
                state = HandState.dead;
                isDead = true;
                print("tireden dead'e ");

                StopHandTired();
                DeadColorHand();

                Break(currentStuff);

                ChangeUI(UIState.dead);

                SetArray(1);

                return;
            }
        }

        if (BoolControl(2))
        {
            if (HandState.dead == state && stamina > 98)
            {
                print("deadden normale");
                state = HandState.normal;

                StopHandTired();
                HandColorNormal();

                ChangeUI(UIState.idle);

                SetArray(2);

                return;
            }
        }

        if (BoolControl(3))
        {
            if (HandState.tired == state && stamina > 40)
            {
                print("tridden normal");
                state = HandState.normal;

                StopHandTired();
                HandColorNormal();

                ChangeUI(UIState.idle);

                SetArray(3);

                return;
            }
        }
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