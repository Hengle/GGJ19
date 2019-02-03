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

    [Header("Player")]
    [SerializeField] PlayerController[] playerControllers;
    [SerializeField] TextMeshProUGUI[] playerControlText;
    [SerializeField] Transform[] playerReadyText;

    [Header("Setting Input")]
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
    string hold = "<color=#0000ffff>HOLD</color>";

    List<string> stringList = new List<string>();

    [Header("Canvas UI Select")]
    [SerializeField] Transform canvasSelect;

    [Header("Script Object")]
    [SerializeField] Transform scObject;

    [Header("Stuff Paren")]
    [SerializeField] Transform stuffParent;

    [Header("Goals Mask")]
    [SerializeField] Transform[] goalsMask;

    [Header("Alpha Fade Out")]
    [SerializeField] Transform canvas;
    [SerializeField] Transform[] playerSprite;

    ColorFunctions cf;
    TransformFunctions tf;

    string ChangeText(string moveP, string holP)
    {
        string result = moveP + " " + move + "\n" + holP + " " + hold;

        return result;
    }

    void Awake()
    {
        tf = FindObjectOfType<TransformFunctions>();
        cf = FindObjectOfType<ColorFunctions>();

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
        if (joypadCount <= 1)
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

    public void ReadyController()
    {
        for (int i = 0; i < playerControllers.Length; i++)
        {
            if (playerControllers[i].gameObject.activeSelf)
            {
                if (!playerControllers[i].isReady)
                {
                    print("Hazır olmayan var");
                    return;
                }
            }
        }

        //buraya kadar gelmis ise hepsi tamamdır.
        print("Hepsi Hazır oyun baslasın");

        StartCoroutine(FadeOut(.1f));
        StartCoroutine(StartGame(.1f));
    }

    IEnumerator StartGame(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        tf.SetActiveAfter(canvasSelect.gameObject, false, 0.85f); //Select UI kapanacak.
        
        tf.SetActiveAfter(stuffParent.gameObject, true, 0.8f); //Objeleri Aktif yap.

        //Oyuncuların position degiştirilecek
        //Oyuncuların layer değiştirilecek
        for (int i = 0; i < playerControllers.Length; i++)
        {
            if (playerControllers[i].gameObject.activeSelf)
            {
                playerControllers[i].SetPositionStart();
            }
        }

        scObject.GetComponent<CameraController>().enabled = true;
        scObject.GetComponent<PowerUpManager>().enabled = true;

        //camera follow active edilecek.
        //power manager active edilecek.

        for (int i = 0; i < goalsMask.Length; i++)
        {
            goalsMask[i].gameObject.SetActive(true);
        }

    }

    IEnumerator FadeOut(float delay)
    {
        yield return new WaitForSeconds(delay);
        cf.ColorTransition(canvas.GetComponent<CanvasGroup>(), 0, 0, .5f);
        for (int i = 0; i < playerSprite.Length; i++)
        {
            cf.ColorTransition(playerSprite[i].GetComponent<SpriteRenderer>(), Color.white.With(a: 0), .1f, 0.5f);
        }
    }

    void FadeIn()
    {
        cf.ColorTransition(canvas.GetComponent<CanvasGroup>(), 1, 0, .1f);
        for (int i = 0; i < playerSprite.Length; i++)
        {
            cf.ColorTransition(playerSprite[i].GetComponent<SpriteRenderer>(), Color.white.With(a: 1), .05f, 0.1f);
        }
    }

    public void ReadBig(Transform current)
    {
        tf.SetActiveAfter(current.gameObject, true, 0);
        tf.Scale(current, Vector3.one, 0, .3f);
    }

    public void ReadSmall(Transform current)
    {
        tf.Scale(current, Vector3.zero, 0, .1f);
        tf.SetActiveAfter(current.gameObject, false, .1f);
    }

    public void ChildScaleUp(Transform parent)
    {
        //Transform[] childs = new Transform[paren]
        //Vector3[]
    }


}
