using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaController : MonoBehaviour
{
	public Area                  area;
	public List<StuffController> stuffs;
	public ScoreController       SC;

	public void Add(StuffController newSC)
	{
		throw new NotImplementedException();
	}

	public void Remove(StuffController newSC)
	{
		throw new NotImplementedException();
	}

	public int CountStuffs()
	{
		return 0;
	}
}

public enum Area
{
	Neutral,
	Left,
	Right
}