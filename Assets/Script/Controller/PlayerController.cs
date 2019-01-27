using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum HandState
{
    normal,
    tired,
    dead
}

public class PlayerController : MonoBehaviour
{
    [Header("Hand Image")]
    [SerializeField]
    Sprite spriteHold;
    [SerializeField]
    Sprite spriteNormal;

    [SerializeField]
    float speedColorAlpha;

    SpriteRenderer spriteRenderer;

    [Header("This Is Keyboard")]
    [SerializeField]
    bool isKeyboard;

    [Header("Buttons String")]
    [SerializeField]
    string keyPlayer = "joystick 1 ";

    [SerializeField]
    string keyButton = "button 4";

    Rigidbody rg;

    [SerializeField]
    float speedMove;

    Transform currentStuff = null;

    bool allowHold;
    bool isHold;
    public bool isEnter;
    [SerializeField] float vertical;
    [SerializeField] float horizontal;

    public float stamina = 100;
    float staminaMax = 100;
    [SerializeField] float speedDownStamina = 1;
    [SerializeField] float speedUpStamina = 1;

    ColorFunctions cf;

    bool isDead;

    HandState state = HandState.normal;

    public bool isRedTeam;
    private float startSpeed;

    void Start()
    {
        cf = FindObjectOfType<ColorFunctions>();
        rg = GetComponent<Rigidbody>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        startSpeed = speedMove;
    }

    void JoystickController()
    {
        vertical = Input.GetAxis(keyPlayer + " Vertical");
        horizontal = Input.GetAxis(keyPlayer + " Horizontal");

        Vector3 velocity = Vector3.zero;
        Vector3 dir = Vector3.zero;

        if (vertical != 0 || horizontal != 0)
        {
            dir = (Vector3.forward * vertical) + (Vector3.right * horizontal);
        }

        velocity = dir * speedMove;
        rg.velocity = velocity;

        /* HOLD */

        if (isEnter)
        {
            if (!isKeyboard && Input.GetKey(keyPlayer + " " + keyButton))
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

    void KeyboardController()
    {
        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");

        Vector3 velocity = Vector3.zero;
        Vector3 dir = Vector3.zero;

        if (vertical != 0 || horizontal != 0)
        {
            dir = (Vector3.forward * vertical) + (Vector3.right * horizontal);
        }

        velocity = dir * speedMove;

        rg.velocity = velocity;

        /* HOLD */

        if (isEnter)
        {
            if (isKeyboard && Input.GetKey(KeyCode.Space))
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

    void FixedUpdate()
    {
        if (isKeyboard)
        {
            KeyboardController();
        }
        else
        {
            JoystickController();
        }

        if (Input.GetKeyDown(KeyCode.B))
        {

        }
        if (Input.GetKeyDown(KeyCode.C))
        {

        }

    }

    private void Update()
    {
        HandController();
    }

    public void Hold(Transform current)
    {
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

        ChangeHandSprite(spriteNormal);

        if (currentStuff != null)
        {
            currentStuff.GetComponent<StuffController>().Break(this);
        }

        Destroy(gameObject.GetComponent<CharacterJoint>());

        rg.angularVelocity = Vector3.zero;

        isHold = false;
        
        Up();
    }

    Coroutine current;

    public void Up()
    {
        if (current != null)
        {
            StopCoroutine(current);
        }

        current = StartCoroutine(_Up());
    }

    IEnumerator _Up()
    {
        while (stamina < staminaMax)
        {
            stamina += Time.deltaTime * speedUpStamina;
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

        current = StartCoroutine(_Down());
    }

    IEnumerator _Down()
    {
        while (stamina > 0)
        {
            stamina -= Time.deltaTime * speedDownStamina;
            yield return null;
        }

        isDead = true;
        print("Is Dead");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Stuff" && isEnter == false)
        {
            isEnter = true;
            currentStuff = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Stuff" && other.transform == currentStuff)
        {
            currentStuff = null;
            isEnter = false;
        }
    }

    void ChangeHandSprite(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }

    Coroutine colorFlip;
    IEnumerator _ChangeColor(Transform current, float time, float minAlpha, float maxAlpha)
    {
        while (true)
        {
            print("calısıyorum");
            cf.ColorTransition(spriteRenderer, spriteRenderer.color.With(a: minAlpha), 0, time);
            yield return new WaitForSeconds(time);
            cf.ColorTransition(spriteRenderer, spriteRenderer.color.With(a: maxAlpha), 0, time);
            yield return new WaitForSeconds(time);
        }
    }

    void HandTiredStart()
    {
        colorFlip = StartCoroutine(_ChangeColor(transform, 0.3f, 0.45f , 0.95f));
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
            }
        }

        if (BoolControl(1))
        {
            if (HandState.tired == state && stamina < 10)
            {
                state = HandState.dead;

                print("tireden dead'e ");

                StopHandTired();
                DeadColorHand();

                Break(currentStuff);

                SetArray(1);
            }
        }

        if (BoolControl(2))
        {
            if (HandState.dead == state && stamina > 40)
            {
                print("deadden normale");
                state = HandState.normal;

                StopHandTired();
                HandColorNormal();

                SetArray(2);
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

                SetArray(3);
            }
        }



    }

    public void SetSpeed(float newSpeed)
    {
        speedMove = newSpeed;
    }

    public void ResetSpeed()
    {
        speedMove = startSpeed;
    }
}