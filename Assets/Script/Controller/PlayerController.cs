using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Hand Image")]
    [SerializeField]
    Sprite spriteHold;
    [SerializeField]
    Sprite spriteNormal;

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
    bool isEnter;
    [SerializeField] float vertical;
    [SerializeField] float horizontal;

    public float stamina = 100;
    float staminaMax = 100;
    float speedDown = 1;
    float speedUp = 1;

    void Start()
    {
        rg = GetComponent<Rigidbody>();
        spriteRenderer = GetComponent<SpriteRenderer>();
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
                if (!isHold)
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
                if (!isHold)
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

    }

    void Hold(Transform current)
    {
        ChangeHandSprite(spriteHold);
        
        //Kenime bir joint componenti ekle.
        CharacterJoint joint = gameObject.AddComponent<CharacterJoint>();

        //Objenin Tutulma fonksiyonu cagırılıyor.
        currentStuff.GetComponent<StuffController>().Hold();

        //Connected body'sine current objeyi ver.
        joint.connectedBody = current.GetComponent<Rigidbody>();

        isHold = true;


        Down();
    }

    void Break(Transform currentStuff)
    {
        ChangeHandSprite(spriteNormal);

        if (currentStuff != null)
        {
            currentStuff.GetComponent<StuffController>().Break();
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
            stamina += Time.deltaTime * speedUp;
            yield return null;
        }
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
            stamina -= Time.deltaTime * speedDown;
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        print("OnTrigger Enter");
        if (other.tag == "Stuff" && isEnter == false)
        {
            print("isEnter : True");
            isEnter = true;
            currentStuff = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        print("OnTrigger Exit");
        if (other.tag == "Stuff" && other.transform == currentStuff)
        {
            currentStuff = null;
            isEnter = false;
            print("isEnter : False");
        }
    }

    void ChangeHandSprite(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }
}