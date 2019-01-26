using System;
using System.Collections.Generic;
using UnityEngine;

public class AreaController : MonoBehaviour
{
	public Area                  area;
	public List<StuffController> stuffs;
	public ScoreController       SC;

	public void Add(StuffController newSC)
	{
		if(area == Area.Neutral) return;

		if(!stuffs.Contains(newSC))
			stuffs.Add(newSC);

		SC.UpdateScore(area, CountStuffs());

//		throw new NotImplementedException();
	}

	public void Remove(StuffController newSC)
	{
		if(area == Area.Neutral) return;

		throw new NotImplementedException();
	}

	private int CountStuffs()
	{
		if(area == Area.Neutral) return 0;

		int total = 0;

		for(int i = 0; i < stuffs.Count; i++)
			total += stuffs[i].stuffValue;

		return total;
	}
}

public enum Area
{
	Neutral,
	Left,
	Right
}