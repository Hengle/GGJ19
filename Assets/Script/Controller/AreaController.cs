using System;
using System.Collections;
using System.Collections.Generic;
using Script.Controller;
using UnityEngine;
using UnityEngine.Serialization;

public class AreaController : MonoBehaviour
{
    public Area area;
    [FormerlySerializedAs("stuffs")] public List<StuffController> _listStuff = new List<StuffController>();
    [FormerlySerializedAs("SC")] public ScoreController _scoreController;

    [SerializeField] private Transform moveToHere;
    [SerializeField] private AnimationCurve moveCurve;
    [Range(0, 5)] public float moveDelay = 0.33f;
    [Range(0, 5)] public float moveTime = 1f;

    private TransformFunctions TF;

    void Awake()
    {
        TF = FindObjectOfType<TransformFunctions>();
    }

    public void Add(StuffController stuff)
    {
        if (area == Area.Neutral) return;

        if (!_listStuff.Contains(stuff))
        {
            _listStuff.Add(stuff);

            stuff.BreakAllPlayers();

            Collider[] cols = stuff.GetComponents<Collider>();

            foreach (Collider col in cols)
            {
                Destroy(col);
            }

            Destroy(stuff.GetComponent<Rigidbody>());

            if (stuff.GetComponent<AudioSource>() != null)
            {
                StartCoroutine(_SoundOut(stuff.GetComponent<AudioSource>()));
            }

            //is gold değişkeni kapanmaması için
            stuff.StopAllCoroutines();
            
            TF.Move(stuff.transform, moveToHere, moveDelay, moveTime, moveCurve);
            TF.Scale(stuff.transform, moveToHere, moveDelay, moveTime, moveCurve);
            TF.Rotate(stuff.transform, moveToHere, moveDelay, moveTime, moveCurve);
        }

        _scoreController.UpdateScore(area, CountStuffs());
    }

    IEnumerator _SoundOut(AudioSource AS)
    {
        GameObject.Find("Main Camera").GetComponent<AudioSource>().Play();
        float passed = 0f;

        ParticleSystem[] PSs = AS.GetComponentsInChildren<ParticleSystem>();

        for (int i = 0; i < PSs.Length; i++)
        {
            PSs[i].Stop();
        }

        while (passed < 2f)
        {
            passed += Time.deltaTime;
            AS.volume = 1f - passed / 2f;

            yield return null;
        }

        AS.volume = 0f;
        AS.Stop();
    }

    public void Remove(StuffController newSC)
    {
        if (area == Area.Neutral) return;

        throw new NotImplementedException();
    }

    private int CountStuffs()
    {
        if (area == Area.Neutral) return 0;

        int total = 0;

        for (int i = 0; i < _listStuff.Count; i++)
            total += _listStuff[i].stuffValue;

        return total;
    }
}