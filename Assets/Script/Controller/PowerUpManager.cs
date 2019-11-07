using System;
using System.Collections;
using Script.Controller;
using UnityEngine;
using Random = UnityEngine.Random;

public class PowerUpManager : MonoBehaviour
{
    [SerializeField] private TransformFunctions TF;

    [Header("Power Up Creating")] public GameObject[] powerUpObject;
    [Range(0, 60)] public float minCreatingTime = 5f;
    [Range(0, 60)] public float maxCreatingTime = 15f;
    [SerializeField] private Transform minPoint;
    [SerializeField] private Transform maxPoint;
    [Range(0, 2)] private float showTime;
    [SerializeField] private AnimationCurve showCurve;
    public GameObject powerUpContainer;

    [Header("Speed Power Up")]
    [Range(0, 10)]
    public float resetSpeedAfter = 3f;

    [Range(1, 10)] public float setSpeed = 3f;

    [Header("Stamina Power Up")]
    [Range(0, 10)]
    public float resetStaminaAfter = 3f;

    [Range(1, 10)] public float setStamina = 3f;

    private float nextCreationTime;

    private PlayerController[] players;

    public PowerUpType _testType;

    [ContextMenu("Create Power Up")]
    public void TestCreatePowerUp()
    {
        GameObject newPowerUp = Instantiate(powerUpObject[(int)_testType]);
        GameObject newPowerUpParent = Instantiate(powerUpContainer);
        newPowerUp.transform.SetParent(newPowerUpParent.transform);
        newPowerUpParent.transform.localPosition = new Vector3(
            Random.Range(minPoint.transform.position.x, maxPoint.transform.position.x),
            0.1f,
            Random.Range(minPoint.transform.position.z, maxPoint.transform.position.z)
        );
        Vector3 originScale = newPowerUp.transform.localScale;
        newPowerUp.transform.localScale = Vector3.zero;
        TF.Scale(newPowerUp.transform, originScale, 0f, 0.5f, showCurve);
    }

    void Start()
    {
        nextCreationTime = Random.Range(minCreatingTime, maxCreatingTime);
        StartCoroutine(CreatePowerUp());
        players = FindObjectsOfType<PlayerController>();
    }

    IEnumerator CreatePowerUp()
    {
        yield return new WaitForSeconds(nextCreationTime);

        GameObject newPowerUp = Instantiate(powerUpObject[Random.Range(0,powerUpObject.Length)]);
        GameObject newPowerUpParent = Instantiate(powerUpContainer);
        newPowerUp.transform.SetParent(newPowerUpParent.transform);
        newPowerUpParent.transform.localPosition = new Vector3(
            Random.Range(minPoint.transform.position.x, maxPoint.transform.position.x),
            0.1f,
            Random.Range(minPoint.transform.position.z, maxPoint.transform.position.z)
        );
        Vector3 originScale = newPowerUp.transform.localScale;
        newPowerUp.transform.localScale = Vector3.zero;
        TF.Scale(newPowerUp.transform, originScale, 0f, 0.5f, showCurve);

        nextCreationTime = Random.Range(minCreatingTime, maxCreatingTime);
        StartCoroutine(CreatePowerUp());
    }

    public void PowerUp(PowerUpType type, PlayerController player)
    {
        switch (type)
        {
            case PowerUpType.StaminaUp:
                StaminaUp(player);
                break;
            case PowerUpType.Heavier:
                Heavier(player);
                break;
            case PowerUpType.Speed:
                Speed(player);
                break;
            case PowerUpType.Freeze:
                Freeze(player);
                break;
            case PowerUpType.Donkey:
                Donkey(player);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }

    void StaminaUp(PlayerController player)
    {
        player.Up(ChangeSpeed.quickly);
    }

    void Speed(PlayerController player)
    {
        player.SetSpeed(setSpeed);
        StartCoroutine(_ResetSpeed(player));
    }

    void Heavier(PlayerController player)
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].team != player.team) //Karsı oyuncular bulunur.
            {
                players[i].Down(ChangeSpeed.quickly);
            }
        }
    }

    void Freeze(PlayerController player)
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].team != player.team)
            {
                players[i].Freeze(5);
            }
        }
    }
    
    void Donkey(PlayerController player)
    {
        player.Donkey(5);
    }

    IEnumerator _ResetSpeed(PlayerController player)
    {
        yield return new WaitForSeconds(resetSpeedAfter);

        player.ResetSpeed();
    }
}