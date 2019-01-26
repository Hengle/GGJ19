using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
	[Header("This Is Keyboard")] [SerializeField]
	bool isKeyboard;


	[Header("Buttons String")] [SerializeField]
	string keyPlayer = "joystick 1 ";

	[SerializeField] string keyButton = "button 4";


	Rigidbody rg;

	[SerializeField] float speed;

	Transform currentStuff = null;

	bool                   allowHold;
	bool                   isHold;
	bool                   isEnter;
	[SerializeField] float vertical;
	[SerializeField] float horizontal;

	private void Awake()
	{
		if(isKeyboard)
		{
			keyPlayer = "";
		}
	}

	// Start is called before the first frame update
	void Start()
	{
		rg = GetComponent<Rigidbody>();
	}

	void FixedUpdate()
	{
		if(isKeyboard)
		{
			vertical   = Input.GetAxis("Vertical");
			horizontal = Input.GetAxis("Horizontal");
		}
		else
		{
			vertical   = Input.GetAxis(keyPlayer + " Vertical");
			horizontal = Input.GetAxis(keyPlayer + " Horizontal");
		}

		Vector3 velocity = Vector2.zero;
		Vector3 dir      = Vector2.zero;

		if(vertical != 0 || horizontal != 0)
		{
			dir = (Vector3.forward * vertical) + (Vector3.right * horizontal);
		}

		velocity = dir * speed;


		rg.velocity = velocity;

		/* HOLD */

		if(isEnter)
		{
			if(
				(isKeyboard && Input.GetKey(KeyCode.Space)) ||
				(!isKeyboard && Input.GetKey(keyPlayer + " " + keyButton))
			)
			{
				if(!isHold)
				{
					Hold(currentStuff);
				}
			}
			else if(isHold)
			{
				Break(currentStuff);
			}
			else if(gameObject.GetComponent<CharacterJoint>() != null)
			{
				Break(currentStuff);
			}
		}
		else
		{
			if(isHold)
			{
				Break(currentStuff);
			}
			else if(gameObject.GetComponent<CharacterJoint>() != null)
			{
				Break(currentStuff);
			}
		}


		///* TEST */

		//if (Input.GetKeyDown(KeyCode.Y))
		//{
		//    print("Test Work : Press Y");
		//    print("Name current : " + currentStuff.name);
		//    Hold(currentStuff);
		//}

		//if (Input.GetKeyDown(KeyCode.K))
		//{
		//    print("Test Work : Press Y");
		//    Break(currentStuff);
		//}
	}

	void Hold(Transform current)
	{
		//Kenime bir joint componenti ekle.
		CharacterJoint joint = gameObject.AddComponent<CharacterJoint>();

		//connected body'sine current objeyi ver.
		joint.connectedBody = current.GetComponent<Rigidbody>();

		isHold = true;
	}

	void Break(Transform currentStuff)
	{
		if(currentStuff != null)
		{
			//Connected destroy
			currentStuff.GetComponent<StuffController>().Break();

			Destroy(gameObject.GetComponent<CharacterJoint>());
		}

		rg.angularVelocity = Vector3.zero;

		isHold = false;
	}


	private void OnTriggerEnter(Collider other)
	{
		print("Enter");
		if(other.tag == "Stuff" && isEnter == false)
		{
			print("is Enter t");
			isEnter      = true;
			currentStuff = other.transform;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		print("Exit");
		if(other.tag == "Stuff" && other.transform == currentStuff)
		{
			currentStuff = null;
			isEnter      = false;
			print("isEnter : False");
		}
	}
}