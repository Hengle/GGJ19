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

	public Sprite         staminaSprite;
	public Sprite         speedSprite;
	public Sprite         heavierSprite;
	public SpriteRenderer spriteRenderer;

	public Sprite         stamina2Sprite;
	public Sprite         speed2Sprite;
	public Sprite         heavier2Sprite;
	public SpriteRenderer sprite2Renderer;

	public Sprite         stamina3Sprite;
	public Sprite         speed3Sprite;
	public Sprite         heavier3Sprite;
	public SpriteRenderer sprite3Renderer;


	public  AudioClip   staminaClip;
	public  AudioClip   speedClip;
	public  AudioClip   heavierClip;
	private AudioSource audioSource;

	[Range(5f, 120f)] public float destroyAfter = 12f;

	private float passedTime;

	void Start()
	{
		type = RandomEnumValue<PowerUpType>();

		audioSource = FindObjectOfType<AudioSource>();

		switch(type)
		{
			case PowerUpType.Stamina:
				spriteRenderer.sprite  = staminaSprite;
				sprite3Renderer.sprite = stamina3Sprite;
				sprite2Renderer.sprite = stamina2Sprite;
				break;
			case PowerUpType.Heavier:
				spriteRenderer.sprite  = heavierSprite;
				sprite3Renderer.sprite = heavier3Sprite;
				sprite2Renderer.sprite = heavier2Sprite;
				break;
			case PowerUpType.Speed:
				spriteRenderer.sprite  = speedSprite;
				sprite3Renderer.sprite = speed3Sprite;
				sprite2Renderer.sprite = speed2Sprite;
				break;
		}

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
		switch(type)
		{
			case PowerUpType.Stamina:
				audioSource.clip = staminaClip;
				break;
			case PowerUpType.Heavier:
				audioSource.clip = heavierClip;
				break;
			case PowerUpType.Speed:
				audioSource.clip = speedClip;
				break;
		}

		audioSource.PlayOneShot(audioSource.clip);
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
		yield return new WaitForSeconds(2.5f);
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