using System.Collections;
using Script.Controller;
using UnityEngine;

public class MiniGameController : MonoBehaviour
{
    public PlayerColor _playerColor;
    
    [SerializeField] private Sprite _rock;
    [SerializeField] private Sprite _paper;
    [SerializeField] private Sprite _scissors;

    [SerializeField] private SpriteRenderer _spriteRenderer;

    [SerializeField] private TransformFunctions _transformFunctions;

    [SerializeField] private float _scale;
    [SerializeField] private float _time;
    [SerializeField] private AnimationCurve _curve;

    private void Start()
    {
        _transformFunctions = TransformFunctions.Instance;
    }

    public void Display(MiniGame minigame)
    {
        _spriteRenderer.gameObject.SetActive(true);
        switch (minigame)
        {
            case MiniGame.paper:
                _spriteRenderer.sprite = _paper;
                break;
            case MiniGame.rock:
                _spriteRenderer.sprite = _rock;
                break;
            case MiniGame.scissors:
                _spriteRenderer.sprite = _scissors;
                break;
        }

        switch (_playerColor)
        {
            case PlayerColor.red:
                _spriteRenderer.color = Color.red;
                break;
            case PlayerColor.blue:
                _spriteRenderer.color = Color.blue;
                break;
            case PlayerColor.green:
                _spriteRenderer.color = Color.green;
                break;
            case PlayerColor.yellow:
                _spriteRenderer.color = Color.yellow;
                break;
        }
        
        _transformFunctions.Scale(_spriteRenderer.transform , Vector3.one * _scale , 0 , _time , _curve );

        IEnumerator _Close()
        {
            const float  _TIME_DISPLAY = 1.5f; // ne kadar süre sonra kapanacak.
            yield return new WaitForSeconds(_TIME_DISPLAY); 
            Close();
        }

        StartCoroutine(_Close());
    }

    public void Close()
    {
        _transformFunctions.Scale(_spriteRenderer.transform , Vector3.zero , 0 , _time , _curve );
        _transformFunctions.SetActiveAfter(_spriteRenderer.gameObject , false , _time);
    }
}
