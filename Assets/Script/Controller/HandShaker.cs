using System;
using System.Collections;
using System.Collections.Generic;
using Script.Controller;
using UnityEngine;

public class HandShaker : MonoBehaviour
{
    [SerializeField] private Sprite _redHand;
    [SerializeField] private Sprite _blueHand;
    [SerializeField] private Sprite _yellowHand;
    [SerializeField] private Sprite _greenHand;

    private SpriteRenderer _spriteRenderer;
    
    public void SetSprite(PlayerColor color)
    {
       _spriteRenderer = transform.GetComponentInChildren<SpriteRenderer>();
       
       if(_spriteRenderer == null)
           return;
       
       switch (color)
       {
           case PlayerColor.red:
               _spriteRenderer.sprite = _redHand;
               break;
           case PlayerColor.blue:
               _spriteRenderer.sprite = _blueHand;
               break;
           case PlayerColor.green:
               _spriteRenderer.sprite = _greenHand;
               break;
           case PlayerColor.yellow:
               _spriteRenderer.sprite = _yellowHand;
               break;
           default:
               throw new ArgumentOutOfRangeException(nameof(color), color, null);
       }
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
