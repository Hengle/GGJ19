using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StuffController : MonoBehaviour
{
    Rigidbody rg;

    void Start()
    {
        rg = GetComponent<Rigidbody>();
    }
    
    public void Break()
    {
        rg.velocity = Vector3.zero;
        rg.angularVelocity = Vector3.zero;
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Stuff" )
        {
            print("Enter");
        }
    }

    void Hit()
    {
        print("Hit");
    }
}
