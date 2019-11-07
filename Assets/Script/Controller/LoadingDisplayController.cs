using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class LoadingDisplayController : MonoBehaviour
{
    private Animator _animator;
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        PlayAnimation(true);
    }

    public void PlayAnimation(bool play)
    {
        _animator.enabled = play;
    }

    public void SelfDestroy(float time)
    {
        IEnumerator _Destroy(float delay)
        {
            yield return new WaitForSeconds(delay);
            Destroy(this.gameObject);
        }

        StartCoroutine(_Destroy(time));
    }
    
}
