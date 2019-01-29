using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneLoadMan : MonoBehaviour
{
    ScoreController scoreController;

    // Start is called before the first frame update
    void Start()
    {
        scoreController = FindObjectOfType<ScoreController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            SceneManager.LoadScene(1);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            scoreController.FinishGame();
        }
    }
}
