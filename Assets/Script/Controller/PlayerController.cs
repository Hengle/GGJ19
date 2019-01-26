using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody rg;

    [SerializeField]
    float speed;

    [SerializeField]
    Transform currentStuff;

    // Start is called before the first frame update
    void Start()
    {
        rg = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        Vector3 velocity = Vector2.zero;
        Vector3 dir = Vector2.zero;

        if (vertical != 0 || horizontal != 0)
        {
            dir = (Vector3.forward * vertical) + (Vector3.right * horizontal);
        }

        velocity = dir * speed;


        rg.velocity = velocity;


        /* HOLD */

        //if (allowHold)
        //{
        //    if (Input.GetKey(KeyCode.Space))
        //    {
        //        //Objeyi tut.
        //        Hold(currentStuff);
        //    }
        //}

        //if (Input.GetKeyUp(KeyCode.Space))
        //{
        //    if (hold)
        //    {
        //        print("Break");
        //        Break();
        //    }

        //}

        /* TEST */

        if (Input.GetKeyDown(KeyCode.Y))
        {
            print("Test Work : Press Y");
            print("Name current : " + currentStuff.name);
            Hold(currentStuff);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            print("Test Work : Press Y");
            Break();
        }

    }

    void Hold(Transform current)
    {
        //Kenime bir joint componenti ekle.
        CharacterJoint joint = gameObject.AddComponent<CharacterJoint>();

        //connected body'sine current objeyi ver.
        joint.connectedBody = current.GetComponent<Rigidbody>();

        hold = true;
    }

    void Break()
    {
        //Connected destroy
        Destroy(gameObject.GetComponent<CharacterJoint>());

        currentStuff.GetComponent<StuffController>().Break();

        hold = false;
    }

    bool allowHold;


    bool hold;

    private void OnTriggerEnter(Collider other)
    {
        print("Hit Somethign");
        if (other.tag == "Stuff")
        {
            allowHold = true;
            currentStuff = other.transform;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Stuff")
        {
            if (Input.GetKey(KeyCode.Space))
            {
                print("Tutuyorum");
            }
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                print("Bıraktım");
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Stuff"))
        {
            allowHold = false;
            currentStuff = null;
        }
    }
}
