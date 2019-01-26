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

    // Update is called once per frame
    void Update()
    {

    }

    public void Break()
    {
        rg.velocity = Vector3.zero;
    }
}
