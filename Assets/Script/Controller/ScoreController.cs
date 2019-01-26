using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
	public int leftScore  = 0;
	public int rightScore = 0;

	public void UpdateScore(Area side, int newScore)
	{
		switch(side)
		{
			case Area.Left:
				leftScore = newScore;
				break;
			case Area.Right:
				rightScore = newScore;
				break;
		}

		UpdateUI();
	}

	public void UpdateUI()
	{
		//Updating the UI thingymings.
	}
}