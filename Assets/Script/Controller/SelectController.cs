using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    [SerializeField] TextMeshProUGUI[] playerControlText;

    [SerializeField] InputSetting[] joypads;
    [SerializeField] InputSetting[] keyboards;

    [HideInInspector]
    public List<InputSetting> inputList = new List<InputSetting>();

    [Header("String Button Show")]
    [SerializeField] string joypadMove;
    [SerializeField] string joypadHold;
    [SerializeField] string keyboard1Move;
    [SerializeField] string keyboard1Hold;
    [SerializeField] string keyboard2Move;
    [SerializeField] string keyboard2Hold;

    string move = "<color=#ff0000ff>Run</color>";
    string hold = "<color=#0000ffff>HOLD & RELEASE</color>";

    List<string> stringList = new List<string>();

    string ChangeText(string moveP, string holP)
    {
        string result = moveP + " " + move + "\n" + holP + " " + hold;

        return result;
    }

    void Awake()
    {
        int joypadCount = Input.GetJoystickNames().Length;
        print("joypad count : " + joypadCount);

        SetInput(joypadCount);
        SetPlayerControllerInput(joypadCount);

        for (int i = 0; i < playerControllers.Length; i++)
        {
            if (playerControllers[i].inputSetting == null)
            {
                playerControllers[i].gameObject.SetActive(false);
                playerControlText[i].gameObject.SetActive(false);
            }
        }
    }

    void SetInput(int joypadCount)
    {
        switch (joypadCount)
        {
            case 0:
                inputList.Add(keyboards[0]);
                inputList.Add(keyboards[1]);

                stringList.Add(ChangeText(keyboard1Move, keyboard1Hold)); //String atama işlemeleri
                stringList.Add(ChangeText(keyboard2Move, keyboard2Hold));
                break;
            case 1:
                inputList.Add(keyboards[0]);
                inputList.Add(joypads[0]);

                stringList.Add(ChangeText(keyboard1Move, keyboard1Hold));
                stringList.Add(ChangeText(joypadMove, joypadHold));
                break;
            case 2:
                inputList.Add(keyboards[0]);
                inputList.Add(keyboards[1]);
                inputList.Add(joypads[0]);
                inputList.Add(joypads[1]);

                stringList.Add(ChangeText(keyboard1Move, keyboard1Hold));
                stringList.Add(ChangeText(keyboard2Move, keyboard2Hold));
                stringList.Add(ChangeText(joypadMove, joypadHold));
                stringList.Add(ChangeText(joypadMove, joypadHold));
                break;
            case 3:
                inputList.Add(keyboards[0]);
                inputList.Add(joypads[0]);
                inputList.Add(joypads[1]);
                inputList.Add(joypads[2]);

                stringList.Add(ChangeText(keyboard1Move, keyboard1Hold));
                stringList.Add(ChangeText(joypadMove, joypadHold));
                stringList.Add(ChangeText(joypadMove, joypadHold));
                stringList.Add(ChangeText(joypadMove, joypadHold));
                break;
            case 4:
                inputList.Add(joypads[0]);
                inputList.Add(joypads[1]);
                inputList.Add(joypads[2]);
                inputList.Add(joypads[3]);

                stringList.Add(ChangeText(joypadMove, joypadHold));
                stringList.Add(ChangeText(joypadMove, joypadHold));
                stringList.Add(ChangeText(joypadMove, joypadHold));
                stringList.Add(ChangeText(joypadMove, joypadHold));
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

            playerControlText[0].text = stringList[0];
            playerControlText[2].text = stringList[1];
        }
        else
        {
            //Playerları 1. 2. 3. 4. ye ata
            playerControllers[0].inputSetting = inputList[0];
            playerControllers[1].inputSetting = inputList[1];
            playerControllers[2].inputSetting = inputList[2];
            playerControllers[3].inputSetting = inputList[3];

            playerControlText[0].text = stringList[0];
            playerControlText[1].text = stringList[1];
            playerControlText[2].text = stringList[2];
            playerControlText[3].text = stringList[3];
        }
    }


}
