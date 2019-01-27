using System.Collections;
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
	public                                GameObject     powerUpContainer;

	[Header("Speed Power Up")] [Range(0, 10)]
	public float resetSpeedAfter = 3f;

	[Range(1, 10)] public float setSpeed = 3f;

	[Header("Stamina Power Up")] [Range(0, 10)]
	public float resetStaminaAfter = 3f;

	[Range(1, 10)] public float setStamina = 3f;


	private float nextCreationTime;

	private PlayerController[] players;

	void Start()
	{
		TF               = FindObjectOfType<TransformFunctions>();
		nextCreationTime = Random.Range(minCreatingTime, maxCreatingTime);
		StartCoroutine(CreatePowerUp());
		players = FindObjectsOfType<PlayerController>();
	}

	IEnumerator CreatePowerUp()
	{
		yield return new WaitForSeconds(nextCreationTime);

		GameObject newPU       = Instantiate(powerUpObject);
		GameObject newPUParent = Instantiate(powerUpContainer);
		newPU.transform.SetParent(newPUParent.transform);
		newPUParent.transform.localPosition = new Vector3(
			Random.Range(minPoint.transform.position.x, maxPoint.transform.position.x),
			0.1f,
			Random.Range(minPoint.transform.position.z, maxPoint.transform.position.z)
		);
		Vector3 origS = newPU.transform.localScale;
		newPU.transform.localScale = Vector3.zero;
		TF.Scale(newPU.transform, origS, 0f, 0.5f, showCurve);

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

	void Stamina(PlayerController player)
	{
		player.UpQuickly();
	}

	void Speed(PlayerController player)
	{
		player.SetSpeed(setSpeed);
		StartCoroutine(_ResetSpeed(player));
	}

	void Heavier(PlayerController player)
	{
		for(int i = 0; i < players.Length; i++)
		{
			if(players[i].isRedTeam != player.isRedTeam)
			{
				player.DownQuickly();
			}
		}
	}

	IEnumerator _ResetSpeed(PlayerController player)
	{
		yield return new WaitForSeconds(resetSpeedAfter);

		player.ResetSpeed();
	}
}

public enum PowerUpType
{
	Stamina,
	Speed,
	Heavier
}