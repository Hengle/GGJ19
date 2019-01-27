using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textLeft;
    [SerializeField] TextMeshProUGUI textRight;

    [SerializeField] Transform parentStuff;

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

    void FinishGame()
    {
        print("oyun biter");

    }

    public void UpdateUI()
    {
        textLeft.text = leftScore.ToString();
        textRight.text = rightScore.ToString();
    }
}