using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    float speed;

    [SerializeField]
    Transform lookAtPoint;

    [SerializeField]
    Transform targetParent;

    [SerializeField]
    float offsetFlat;
    [SerializeField]
    float offset;

    [SerializeField]
    Transform[] target;

    Rigidbody2D rg;

    TransformFunctions tf;

    [SerializeField]


    int index = 0;

    // Start is called before the first frame update
    void Start()
    {
        rg = GetComponent<Rigidbody2D>();
        tf = FindObjectOfType<TransformFunctions>();
    }

    private void FixedUpdate()
    {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        rg.velocity = (Vector3.up * vertical * speed) + (Vector3.right * horizontal* speed);
    }
}
