using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectController : MonoBehaviour
{

    /* Device sayısına göre sekillenecek.  
     
         4 adet joypad var ise random chacterlere joypad atama

         3 adet joypad var ise ilk sıra keyboard sonraları joypadlara atanır.
         
         2 adet joypad var ise ilk sıra keyboard ikincisine keyboard geri kalan 2 charter joypad'e

         1 adet joypad var ise ilk sıra keyboard ikinci keyboard , sonrasına joypad

         0 adet joypad var ise ilk keyboard , sonra 3. character'e yani karsı team'e ikinci joypad
         
         */

    // Start is called before the first frame update

    [SerializeField] PlayerController[] playerControllers;


    [SerializeField] InputSetting[] joypads;
    [SerializeField] InputSetting[] keyboards;

    [HideInInspector]
    public List<InputSetting> inputList = new List<InputSetting>();

    void Start()
    {
        int joypadCount = Input.GetJoystickNames().Length;
        print("joypad count : " + joypadCount);

        SetInput(joypadCount);
        SetPlayerControllerInput(joypadCount);
    }

    void SetInput(int joypadCount)
    {
        switch (joypadCount)
        {
            case 0:
                inputList.Add(keyboards[0]);
                inputList.Add(keyboards[1]);
                break;
            case 1:
                inputList.Add(keyboards[0]);
                inputList.Add(joypads[0]);
                break;
            case 2:
                inputList.Add(keyboards[0]);
                inputList.Add(keyboards[1]);
                inputList.Add(joypads[0]);
                inputList.Add(joypads[1]);
                break;
            case 3:
                inputList.Add(keyboards[0]);
                inputList.Add(joypads[0]);
                inputList.Add(joypads[1]);
                inputList.Add(joypads[2]);
                break;
            case 4:
                inputList.Add(joypads[0]);
                inputList.Add(joypads[1]);
                inputList.Add(joypads[2]);
                inputList.Add(joypads[3]);
                break;

            default:
                break;
        }

        print("Complated Setting");
    }

    void SetPlayerControllerInput(int joypadCount)
    {
        if (joypadCount <= 2)
        {
            //Playerları 1. ve 3. ye ata
            playerControllers[0].inputSetting = inputList[0];
            playerControllers[2].inputSetting = inputList[1];
        }
        else
        {
            //Playerları 1. 2. 3. 4. ye ata
            playerControllers[0].inputSetting = inputList[0];
            playerControllers[1].inputSetting = inputList[1];
            playerControllers[2].inputSetting = inputList[2];
            playerControllers[3].inputSetting = inputList[3];
        }
    }


}
