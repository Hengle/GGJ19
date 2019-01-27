using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class PowerUpController : MonoBehaviour
{
	public  PowerUpType        type;
	private TransformFunctions TF;
	private PowerUpManager     PUM;
	public  Transform          targetScale;
	public  AnimationCurve     destroyCurve;

	[Range(5f, 120f)] public float destroyAfter = 12f;

	private float passedTime;

	void Start()
	{
		type        = RandomEnumValue<PowerUpType>();
		TF          = FindObjectOfType<TransformFunctions>();
		PUM         = FindObjectOfType<PowerUpManager>();
		targetScale = GameObject.Find("ZeroScale").transform;
	}

	private void LateUpdate()
	{
		if(passedTime >= destroyAfter)
		{
			DestroyThis();
			passedTime = -1000f;
		}

		passedTime += Time.deltaTime;
	}

	public void Use(PlayerController player)
	{
		PUM.PowerUp(type, player);
		DestroyThis();
	}

	void DestroyThis()
	{
		GetComponent<Collider>().enabled = false;
		StartCoroutine(_Destroy());
		TF.Scale(transform, targetScale, 0f, 0.33f, destroyCurve);
	}

	IEnumerator _Destroy()
	{
		yield return new WaitForSeconds(0.4f);
		Destroy(gameObject);
	}

	T RandomEnumValue<T>()
	{
		var v = Enum.GetValues(typeof(T));
		return (T) v.GetValue(Random.Range(0, v.Length));
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Player"))
		{
			Use(other.GetComponent<PlayerController>());
		}
	}
}