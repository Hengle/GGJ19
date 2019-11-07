using System;
using System.Collections;
using System.Collections.Generic;
using Script.Controller;
using UnityEngine;

public class MiniGameManager : MonoBehaviour
{
    public static MiniGameManager Instance;

    private float timeMiniGame = 3;

    [SerializeField] private LoadingDisplayController _loading;
    [SerializeField] private HandShaker _handShaker;

    private LoadingDisplayController load;

    [HideInInspector] public Element rock;
    [HideInInspector] public Element paper;
    [HideInInspector] public Element scissors;

    private void Awake()
    {
        Instance = this;

        rock = new Element(MiniGame.rock, MiniGame.scissors, MiniGame.paper);
        paper = new Element(MiniGame.paper, MiniGame.rock, MiniGame.scissors);
        scissors = new Element(MiniGame.scissors, MiniGame.paper, MiniGame.rock);
    }

    private StuffController _stuffController;
    private Vector3 _initialStuffPosition;

    private float neededDistance = 1;
    private Coroutine drawListener;
    private float timeDrawListener = 3f; //Tutulmadan ne kadar süre sonra minigame oynansın.

    private IEnumerator _Draw(StuffController stuffController)
    {
        print("Start Draw Listener Event");

        _stuffController = stuffController;
        _initialStuffPosition = stuffController.transform.position;

        //it's work draw count down
        yield return new WaitForSeconds(timeDrawListener);
        //if Stuff position don't change enough

        Vector3
            initPosition =
                _initialStuffPosition.With(y: 0); //Bana sadece x ve z'ler gerekli oldugu için y'leri olaydan çıkardım.
        Vector3 currentPosition = stuffController.transform.position.With(y: 0);

        if (Vector3.Distance(initPosition, currentPosition) <= neededDistance)
        {
            StartMiniGame();
            drawListener = null;
        }
        else
        {
            print("Yeteri kadar sabit kalmadı.");
            //Belki bir an çok hareket etti lakin sonra durdurdular ? Bunun için tekrar dinleme aktif edilir.
            StartDrawListener(stuffController);
        }
    }

    public void StopDrawListener()
    {
        print("Stop Draw Listener Event");
        if (drawListener != null)
        {
            StopCoroutine(drawListener);
            drawListener = null;
        }
    }

    public void StartDrawListener(StuffController stuffController)
    {
        if (drawListener == null)
        {
            drawListener = StartCoroutine(_Draw(stuffController));
        }
        else
        {
            print(" Draw sayacı - Zaten aktif o yuzden tekrar baslatmadım.");
        }
    }


    public void StartMiniGame()
    {
        //oyun basladıgı için artık dinleme yapmana gerek yok.
        StopDrawListener();

        List<PlayerController> listPlayer = PlayerManager.Instance.playerList;
        print("Start MiniGame");

        foreach (var player in listPlayer)
        {
            player.startMiniGame = true;
            player.StopAllCoroutines(); //kullanıcıların yorulma gibi seyleri yok sayılır.

            //Hand Shake Burada Çıkmalı.
            player.SpriteFlip(timeMiniGame);
            HandShaker hand = Instantiate(_handShaker, player.transform.position, Quaternion.identity);
            hand.transform.Rotate(90, -90, 0);
            hand.SetSprite(player._miniGameController._playerColor);
            hand.SelfDestroy(timeMiniGame);
        }

        //UI element animasyonları burada halledile
        load = Instantiate(_loading, _stuffController.transform.position, Quaternion.identity);
        load.transform.Rotate(90, 0, 0);

        //Secim yapması için geri sayım basladı.
        IEnumerator _StopSelectElement(float delay)
        {
            load.SelfDestroy(delay);
            yield return new WaitForSeconds(delay);
            //Oyuncularının yaptıklarını gördükleri kısım.
            foreach (var player in listPlayer)
            {
                player._miniGameController.Display(player.minigame);
            }

            //yield return new WaitForSeconds(delay / 2);
            StopMiniGame(listPlayer);
        }

        StartCoroutine(_StopSelectElement(timeMiniGame-0.25f));
    }


    private void StopMiniGame(List<PlayerController> listPlayer)
    {
        print("Over MiniGame");

        foreach (var player in listPlayer)
        {
            if(IsLose(player))
                player.LostMiniGame();
            else
            {
                player.startMiniGame = false;
                player.Hit();
            }
        }

    }

    public bool IsLose(PlayerController player)
    {
        var element = GetElement(player.minigame);
        foreach (var otherPlayer in PlayerManager.Instance.playerList)
        {
            if (otherPlayer.team != player.team) //benim takımından değilse bu savas yapılır.
            {
                var otherElement = GetElement(otherPlayer.minigame);
                if (element.week == otherElement.own) //Benim güçsüzlügüm baska birinde var ise ben öldüm.
                {
                    //Lanet olsun varmıs.
                    return true;
                }
            }
        }

        return false;
    }

    private Element GetElement(MiniGame miniGame)
    {
        switch (miniGame)
        {
            case MiniGame.paper:
                return paper;
            case MiniGame.rock:
                return rock;
            case MiniGame.scissors:
                return scissors;
            default:
                return null;
        }
    }
}