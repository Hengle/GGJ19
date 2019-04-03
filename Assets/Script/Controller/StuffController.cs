using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StuffController : MonoBehaviour
{
    Rigidbody rg;
    public AreaController activeAreaSide = null;

    public int mass;
    public int countHold;

    public int rate;

    [Range(1, 100)] public int stuffValue = 1;

    private List<PlayerController> PCs = new List<PlayerController>();

    public bool isGone;

    void Start()
    {
        rg = GetComponent<Rigidbody>();
        rg.mass = mass;
    }



    public void Break(PlayerController PC)
    {
        if (rg != null)
        {
            rg.velocity = Vector3.zero;
            rg.angularVelocity = Vector3.zero;
        }

        if (PCs.Contains(PC)) PCs.Remove(PC);

        countHold--;
        SettingMass(countHold);
    }


    public void Hold(PlayerController PC)
    {
        countHold++;

        if (!PCs.Contains(PC)) PCs.Add(PC);

        SettingMass(countHold);
    }

    void SettingMass(int count)
    {
        int value = mass;
        int factor = count * rate;
        int x = mass - factor;
        if (x < 0)
        {
            rg.mass = 1;
        }
        else
        {
            if (rg != null)
            {
                rg.mass = x;
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Stuff"))
        {
            print("Enter");
        }
        else if (other.name == "LeftSide" || other.name == "RightSide")
        {
            activeAreaSide = other.GetComponent<AreaController>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        switch (other.name)
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

    public void BreakAllPlayers()
    {
        for (int i = 0; i < PCs.Count; i++)
        {
            PCs[0].isEnter = false;
            PCs[0].Break(transform);
        }
    }
}