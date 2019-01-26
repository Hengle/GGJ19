using System;
using System.Collections.Generic;
using UnityEngine;

public class AreaController : MonoBehaviour
{
	public Area                  area;
	public List<StuffController> stuffs = new List<StuffController>();
	public ScoreController       SC;

	[SerializeField] private Transform      moveToHere;
	[SerializeField] private AnimationCurve moveCurve;
	[Range(0, 5)]    public  float          moveDelay = 0.33f;
	[Range(0, 5)]    public  float          moveTime  = 1f;

	private TransformFunctions TF;

	void Awake()
	{
		TF = FindObjectOfType<TransformFunctions>();
	}

	public void Add(StuffController newSC)
	{
		if(area == Area.Neutral) return;

		if(!stuffs.Contains(newSC))
		{
			stuffs.Add(newSC);

			newSC.BreakAllPlayers();

			Collider[] cols = newSC.GetComponents<Collider>();

			foreach(Collider col in cols)
			{
				Destroy(col);
			}

			Destroy(newSC.GetComponent<Rigidbody>());

			TF.Move(newSC.transform, moveToHere, moveDelay, moveTime, moveCurve);
			TF.Scale(newSC.transform, moveToHere, moveDelay, moveTime, moveCurve);
			TF.Rotate(newSC.transform, moveToHere, moveDelay, moveTime, moveCurve);
		}

		SC.UpdateScore(area, CountStuffs());
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