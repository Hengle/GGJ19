using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StuffController : MonoBehaviour
{
	Rigidbody             rg;
	public AreaController activeAreaSide = null;

	void Start()
	{
		rg = GetComponent<Rigidbody>();
	}

	public void Break()
	{
		rg.velocity        = Vector3.zero;
		rg.angularVelocity = Vector3.zero;
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Stuff"))
		{
			print("Enter");
		}
		else if(other.name == "LeftSide" || other.name == "RightSide")
		{
			activeAreaSide = other.GetComponent<AreaController>();
		}
	}

	private void OnTriggerExit(Collider other)
	{
		switch(other.name)
		{
			case "Neutral":
				activeAreaSide.Add(this);
				break;
			case "LeftSide":
			case "RightSide":
				activeAreaSide = null;
				break;
		}
	}

	void Hit()
	{
		print("Hit");
	}
}