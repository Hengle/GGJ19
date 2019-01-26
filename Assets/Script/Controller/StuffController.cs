using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StuffController : MonoBehaviour
{
    Rigidbody rg;

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
