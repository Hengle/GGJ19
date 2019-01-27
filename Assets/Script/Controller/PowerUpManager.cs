using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
	private TransformFunctions TF;

	[Header("Power Up Creating")] public  GameObject     powerUpObject;
	[Range(0, 60)]                public  float          minCreatingTime = 5f;
	[Range(0, 60)]                public  float          maxCreatingTime = 15f;
	[SerializeField]              private Transform      minPoint;
	[SerializeField]              private Transform      maxPoint;
	[Range(0, 2)]                 private float          showTime;
	[SerializeField]              private AnimationCurve showCurve;

	private float nextCreationTime;


	void Start()
	{
		TF               = FindObjectOfType<TransformFunctions>();
		nextCreationTime = Random.Range(minCreatingTime, maxCreatingTime);
		StartCoroutine(CreatePowerUp());
	}

	IEnumerator CreatePowerUp()
	{
		yield return new WaitForSeconds(nextCreationTime);

		GameObject newPU = Instantiate(powerUpObject);
		newPU.transform.localPosition = new Vector3(
			Random.Range(minPoint.transform.position.x, maxPoint.transform.position.x),
			0.1f,
			Random.Range(minPoint.transform.position.z, maxPoint.transform.position.z)
		);
		newPU.transform.localScale = Vector3.zero;
		TF.Scale(newPU.transform, Vector3.one, 0f, 0.5f, showCurve);

		nextCreationTime = Random.Range(minCreatingTime, maxCreatingTime);
		StartCoroutine(CreatePowerUp());
	}

	public void PowerUp(PowerUpType type, PlayerController player)
	{
		switch(type)
		{
			case PowerUpType.Stamina:
				Stamina(player);
				break;
			case PowerUpType.Heavier:
				Heavier(player);
				break;
			case PowerUpType.Speed:
				Speed(player);
				break;
		}
	}

	void Stamina(PlayerController player) {}

	void Speed(PlayerController player) {}

	void Heavier(PlayerController player) {}
}

public enum PowerUpType
{
	Stamina,
	Speed,
	Heavier
}