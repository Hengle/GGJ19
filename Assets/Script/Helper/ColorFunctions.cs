using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ColorFunctions : MonoBehaviour
{

    [SerializeField]
    AnimationCurve curveDefault;

    /*
        Sprite Functions
     */

    AnimationCurve defaultCurve;

    private void Awake()
    {
        defaultCurve = AnimationCurve.Linear(0, 0, 1, 1);
    }

    public void ChangeColor(SpriteRenderer changeThis, Color toThis, float delay = 0f)
    {
        StartCoroutine(_ChangeColor(changeThis, toThis, delay));
    }

    public IEnumerator _ChangeColor(SpriteRenderer changeThis, Color toThis, float delay = 0f)
    {
        yield return new WaitForSeconds(delay);

        changeThis.color = toThis;
    }

    public void ColorTransition(SpriteRenderer changeThis, Color toThis, float delay, float time, AnimationCurve curve)
    {
        StartCoroutine(_ColorTransition(changeThis, toThis, delay, time, curve));
    }

    public void ColorTransition(SpriteRenderer changeThis, Color toThis, float delay, float time)
    {
        StartCoroutine(_ColorTransition(changeThis, toThis, delay, time, curveDefault));
    }

    public IEnumerator _ColorTransition(SpriteRenderer changeThis, Color toThis, float delay, float time, AnimationCurve curve)
    {
        yield return new WaitForSeconds(delay);
        float passed = 0f;
        float rate = 0f;
        Color initColor = changeThis.color;

        while (passed < time)
        {
            passed += Time.deltaTime;
            rate = curve.Evaluate(passed / time);

            changeThis.color = Color.LerpUnclamped(initColor, toThis, rate);
            yield return null;
        }
    }

    public void ColorTransition(Image changeThis, Color toThis, float delay, float time, AnimationCurve curve)
    {
        StartCoroutine(_ColorTransition(changeThis, toThis, delay, time, curve));
    }

    public IEnumerator _ColorTransition(Image changeThis, Color toThis, float delay, float time, AnimationCurve curve)
    {
        yield return new WaitForSeconds(delay);
        float passed = 0f;
        float rate = 0f;
        Color initColor = changeThis.color;

        while (passed < time)
        {
            passed += Time.deltaTime;
            rate = curve.Evaluate(passed / time);

            changeThis.color = Color.LerpUnclamped(initColor, toThis, rate);
            yield return null;
        }
    }

    public void ColorTransition(CanvasGroup changeThis, float toThis, float delay, float time, AnimationCurve curve)
    {
        StartCoroutine(_ColorTransition(changeThis, toThis, delay, time, curve));
    }

    public void ColorTransition(CanvasGroup changeThis, float toThis, float delay, float time )
    {
        StartCoroutine(_ColorTransition(changeThis, toThis, delay, time, defaultCurve));
    }

    public IEnumerator _ColorTransition(CanvasGroup changeThis, float toThis, float delay, float time, AnimationCurve curve)
    {
        yield return new WaitForSeconds(delay);
        float passed = 0f;
        float rate = 0f;
        float initColor = changeThis.alpha;

        while (passed < time)
        {
            passed += Time.deltaTime;
            rate = curve.Evaluate(passed / time);

            changeThis.alpha = Mathf.LerpUnclamped(initColor, toThis, rate);
            yield return null;
        }
    }


    /*
        Material Functions
     */


    public void ChangeColor(Renderer changeThis, Color toThis, float delay = 0f)
    {
        StartCoroutine(_ChangeColor(changeThis, toThis, delay));
    }

    public IEnumerator _ChangeColor(Renderer changeThis, Color toThis, float delay = 0f)
    {
        yield return new WaitForSeconds(delay);

        changeThis.material.color = toThis;
    }

    public void ColorTransition(Renderer changeThis, Color toThis, float delay, float time, AnimationCurve curve)
    {
        StartCoroutine(_ColorTransition(changeThis, toThis, delay, time, curve));
    }

    public IEnumerator _ColorTransition(Renderer changeThis, Color toThis, float delay, float time, AnimationCurve curve)
    {
        yield return new WaitForSeconds(delay);
        float passed = 0f;
        float rate = 0f;
        Color initColor = changeThis.material.color;

        while (passed < time)
        {
            passed += Time.deltaTime;
            rate = curve.Evaluate(passed / time);

            changeThis.material.color = Color.LerpUnclamped(initColor, toThis, rate);
            yield return null;
        }
    }

    public void ChangeMaterial(Renderer changeThis, Material toThis, float delay = 0f)
    {
        StartCoroutine(_ChangeMaterial(changeThis, toThis, delay));
    }

    public IEnumerator _ChangeMaterial(Renderer changeThis, Material toThis, float delay = 0f)
    {
        yield return new WaitForSeconds(delay);

        changeThis.material = toThis;
    }

    public void MaterialTransition(Renderer changeThis, Material toThis, float delay, float time, AnimationCurve curve)
    {
        StartCoroutine(_MaterialTransition(changeThis, toThis, delay, time, curve));
    }

    public IEnumerator _MaterialTransition(Renderer changeThis, Material toThis, float delay, float time, AnimationCurve curve)
    {
        yield return new WaitForSeconds(delay);
        float passed = 0f;
        float rate = 0f;
        Material initMat = changeThis.material;

        while (passed < time)
        {
            passed += Time.deltaTime;
            rate = curve.Evaluate(passed / time);

            changeThis.material.Lerp(initMat, toThis, rate);
            yield return null;
        }
    }

    public IEnumerator _ChangeColorLoop(Transform current, float time, float minAlpha, float maxAlpha)
    {
        SpriteRenderer renderer = current.GetComponent<SpriteRenderer>();
        while (true)
        {
            print("Elimin Loop Alpha Degişimi..");
            ColorTransition(renderer, renderer.color.With(a: minAlpha), 0, time);
            yield return new WaitForSeconds(time);
            ColorTransition(renderer, renderer.color.With(a: maxAlpha), 0, time);
            yield return new WaitForSeconds(time);
        }
    }


}
