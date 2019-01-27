using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
	public void LoadOpening()
	{
		SceneManager.LoadScene(0);
	}

	public void LoadGame()
	{
		SceneManager.LoadScene(1);
	}
}