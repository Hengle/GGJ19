using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textLeft;
    [SerializeField] TextMeshProUGUI textRight;

    [SerializeField] Transform parentStuff;

    [SerializeField] Transform panelOrigin;
    [SerializeField] Transform panelFinish;
    [SerializeField] Transform winRed;
    [SerializeField] Transform winBlue;

    public int leftScore = 0;
    public int rightScore = 0;

    int totalStuff;

    private void Start()
    {
        totalStuff = parentStuff.childCount;
    }

    public void UpdateScore(Area side, int newScore)
    {
        switch (side)
        {
            case Area.Left:
                leftScore = newScore;
                break;
            case Area.Right:
                rightScore = newScore;
                break;
        }

        StuffController();

        UpdateUI();
    }

    void StuffController()
    {
        totalStuff--;

        if (totalStuff <= 0)
        {
            FinishGame();
        }
    }

    bool isWinBlue;
    void FinishGame()
    {
        if (leftScore > rightScore)
        {
            isWinBlue = true;
        }

        panelFinish.gameObject.SetActive(true);
        panelOrigin.gameObject.SetActive(false);

        if (!isWinBlue)
        {
            winRed.gameObject.SetActive(true);
        }
        else
        {
            winBlue.gameObject.SetActive(true);
        }

    }

    public void UpdateUI()
    {
        textLeft.text = leftScore.ToString();
        textRight.text = rightScore.ToString();
    }
}