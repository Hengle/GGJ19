using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StuffController : MonoBehaviour
{
	Rigidbody             rg;
	public AreaController activeAreaSide = null;

<<<<<<< HEAD
    public int mass;
    public int countHold;

    public int rate;

    void Start()
    {
        rg = GetComponent<Rigidbody>();
        rg.mass = mass;
    }

    public void Break()
    {
        rg.velocity = Vector3.zero;
        rg.angularVelocity = Vector3.zero;

        countHold--;
        SettingMass(countHold);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Stuff")
        {
            print("Enter");
        }
    }

    public void Hold()
    {
        countHold++;
        SettingMass(countHold);
    }

    void SettingMass(int count)
    {
        int value = mass;
        int factor = count * rate;

        rg.mass = mass - factor;
    }
}
=======
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
>>>>>>> 0e26d39696fbc131dc1424beacd38c88e623f968
