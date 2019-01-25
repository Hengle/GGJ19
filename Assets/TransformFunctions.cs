using System.Collections;
using UnityEngine;

public class TransformFunctions : MonoBehaviour
{

    #region Singleton
    public static TransformFunctions initial;

    void Singleton()
    {
        if (initial == null)
        {
            initial = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }

    #endregion

    [SerializeField]
    AnimationCurve curveDefaultMove;

    [SerializeField]
    AnimationCurve curveDefaultScale;
    
    private void Awake()
    {
        Singleton();
    }

    #region Position Functions
    /*
        Position Functions
     */

    public void Teleport(Transform teleportThis, Transform toThis, float delay = 0f)
    {
        StartCoroutine(_Teleport(teleportThis, toThis, delay));
    }

    public IEnumerator _Teleport(Transform teleportThis, Transform toThis, float delay = 0f)
    {
        yield return new WaitForSeconds(delay);

        teleportThis.position = toThis.position;
    }

    public void Move(Transform moveThis, Transform toThis, float delay, float time, AnimationCurve curve)
    {
        StartCoroutine(_Move(moveThis, toThis, delay, time, curve));
    }
    public void Move(Transform moveThis, Transform toThis, float delay, float time)
    {
        StartCoroutine(_Move(moveThis, toThis, delay, time, curveDefaultMove));
    }

    public void Move(Transform moveThis, Vector3 toThis, float delay, float time, AnimationCurve curve)
    {
        StartCoroutine(_Move(moveThis, toThis, delay, time, curve));
    }

    public void Move(Transform moveThis, Vector3 toThis, float delay, float time)
    {
        StartCoroutine(_Move(moveThis, toThis, delay, time, curveDefaultMove));
    }

    public IEnumerator _Move(Transform moveThis, Transform toThis, float delay, float time, AnimationCurve curve)
    {
        yield return new WaitForSeconds(delay);
        float passed = 0f;
        float rate = 0f;
        Vector3 initPos = moveThis.position;

        while (passed < time)
        {
            passed += Time.deltaTime;
            rate = curve.Evaluate(passed / time);

            moveThis.position = Vector3.LerpUnclamped(initPos, toThis.position, rate);
            yield return null;
        }
    }

    public IEnumerator _Move(Transform moveThis, Vector3 toThis, float delay, float time, AnimationCurve curve)
    {
        yield return new WaitForSeconds(delay);
        float passed = 0f;
        float rate = 0f;
        Vector3 initPos = moveThis.localPosition;

        while (passed < time)
        {
            passed += Time.deltaTime;
            rate = curve.Evaluate(passed / time);

            moveThis.localPosition = Vector3.LerpUnclamped(initPos, toThis, rate);
            yield return null;
        }
    }

    #endregion

    #region Extra

    public void MoveX(Transform moveThis, Transform toThis, float delay, float time, AnimationCurve curve)
    {
        StartCoroutine(_MoveX(moveThis, toThis, delay, time, curve));
    }

    public IEnumerator _MoveX(Transform moveThis, Transform toThis, float delay, float time, AnimationCurve curve)
    {
        yield return new WaitForSeconds(delay);
        float passed = 0f;
        float rate = 0f;
        float initPos = moveThis.position.x;

        while (passed < time)
        {
            passed += Time.deltaTime;
            rate = curve.Evaluate(passed / time);

            float newPosX = Mathf.LerpUnclamped(initPos, toThis.position.x, rate);

            moveThis.position = new Vector3(newPosX, moveThis.position.y, moveThis.position.z);

            yield return null;
        }
    }

    public void MoveY(Transform moveThis, Transform toTop, Transform toBot, float delay, float time, AnimationCurve curve)
    {
        StartCoroutine(_MoveY(moveThis, toTop, toBot, delay, time, curve));
    }

    public IEnumerator _MoveY(Transform moveThis, Transform toTop, Transform toBot, float delay, float time, AnimationCurve curve)
    {
        yield return new WaitForSeconds(delay);
        float passed = 0f;
        float rate = 0f;
        float initPos = moveThis.position.y;

        while (true)
        {
            passed = 0f;
            rate = 0f;
            initPos = moveThis.position.y;

            while (passed < time)
            {
                passed += Time.deltaTime;
                rate = curve.Evaluate(passed / time);

                float newPosY = Mathf.LerpUnclamped(initPos, toTop.position.y, rate);

                moveThis.position = new Vector3(moveThis.position.x, newPosY, moveThis.position.z);

                yield return null;
            }

            passed = 0f;
            rate = 0f;
            initPos = moveThis.position.y;

            while (passed < time)
            {
                passed += Time.deltaTime;
                rate = curve.Evaluate(passed / time);

                float newPosY = Mathf.LerpUnclamped(initPos, toBot.position.y, rate);

                moveThis.position = new Vector3(moveThis.position.x, newPosY, moveThis.position.z);

                yield return null;
            }
        }

    }

    #endregion

    #region Rotation Functions

    public void ChangeRotation(Transform changeThis, Transform toThis, float delay = 0f)
    {
        StartCoroutine(_ChangeRotation(changeThis, toThis, delay));
    }

    public IEnumerator _ChangeRotation(Transform changeThis, Transform toThis, float delay = 0f)
    {
        yield return new WaitForSeconds(delay);

        changeThis.rotation = toThis.rotation;
    }

    public void Rotate(Transform rotateThis, Transform toThis, float delay, float time, AnimationCurve curve)
    {
        StartCoroutine(_Rotate(rotateThis, toThis, delay, time, curve));
    }

    public IEnumerator _Rotate(Transform rotateThis, Transform toThis, float delay, float time, AnimationCurve curve)
    {
        yield return new WaitForSeconds(delay);
        float passed = 0f;
        float rate = 0f;
        Quaternion initRot = rotateThis.rotation;

        while (passed < time)
        {
            passed += Time.deltaTime;
            rate = curve.Evaluate(passed / time);

            rotateThis.rotation = Quaternion.LerpUnclamped(initRot, toThis.rotation, rate);
            yield return null;
        }
    }

    #endregion



    #region Scale Functions
    /*
        Scale Functions
     */

    public void ChangeScale(Transform changeThis, Transform toThis, float delay = 0f)
    {
        StartCoroutine(_ChangeScale(changeThis, toThis, delay));
    }

    public void ChangeScale(Transform changeThis, Vector3 toThis, float delay = 0f)
    {
        StartCoroutine(_ChangeScale(changeThis, toThis, delay));
    }

    public IEnumerator _ChangeScale(Transform changeThis, Transform toThis, float delay = 0f)
    {
        yield return new WaitForSeconds(delay);

        changeThis.localScale = toThis.localScale;
    }
    
    public IEnumerator _ChangeScale(Transform changeThis, Vector3 toThis, float delay = 0f)
    {
        yield return new WaitForSeconds(delay);

        changeThis.localScale = toThis;
    }

    public void Scale(Transform scaleThis, Transform toThis, float delay, float time, AnimationCurve curve)
    {
        StartCoroutine(_Scale(scaleThis, toThis, delay, time, curve));
    }

    public void Scale(Transform scaleThis, Vector3 toThis, float delay, float time, AnimationCurve curve)
    {
        StartCoroutine(_Scale(scaleThis, toThis, delay, time, curve));
    }

    public void Scale(Transform scaleThis, Transform toThis, float delay, float time)
    {
        StartCoroutine(_Scale(scaleThis, toThis, delay, time, curveDefaultScale));
    }

    public void Scale(Transform scaleThis, Vector3 toThis, float delay, float time)
    {
        StartCoroutine(_Scale(scaleThis, toThis, delay, time, curveDefaultScale));
    }
    
    public IEnumerator _Scale(Transform scaleThis, Transform toThis, float delay, float time, AnimationCurve curve)
    {
        yield return new WaitForSeconds(delay);
        float passed = 0f;
        float rate = 0f;
        Vector3 initScale = scaleThis.localScale;

        while (passed < time)
        {
            passed += Time.deltaTime;
            rate = curve.Evaluate(passed / time);

            scaleThis.localScale = Vector3.LerpUnclamped(initScale, toThis.localScale, rate);
            yield return null;
        }
    }
    
    public IEnumerator _Scale(Transform scaleThis, Vector3 toThis, float delay, float time, AnimationCurve curve)
    {
        yield return new WaitForSeconds(delay);
        float passed = 0f;
        float rate = 0f;
        Vector3 initScale = scaleThis.localScale;

        while (passed < time)
        {
            passed += Time.deltaTime;
            rate = curve.Evaluate(passed / time);

            scaleThis.localScale = Vector3.LerpUnclamped(initScale, toThis, rate);
            yield return null;
        }
    }
    #endregion


    #region Parent Functions
    /*
        Parent Functions
     */

    public void SetParentAfter(Transform thisIsChild, Transform thisIsParent, float delay = 0f)
    {
        StartCoroutine(_SetParentAfter(thisIsChild, thisIsParent, delay));
    }

    public IEnumerator _SetParentAfter(Transform thisIsChild, Transform thisIsParent, float delay = 0f)
    {
        yield return new WaitForSeconds(delay);

        thisIsChild.SetParent(thisIsParent);
    }

    #endregion


    #region GameObject/Component Functions
    /*
        GameObject/Component Functions
     */
     
    public void SetActiveAfter(GameObject activateThis, bool isActive = true, float delay = 0f)
    {
        StartCoroutine(_SetActiveAfter(activateThis, isActive, delay));
    }

    public IEnumerator _SetActiveAfter(GameObject activateThis, bool isActive = true, float delay = 0f)
    {
        yield return new WaitForSeconds(delay);

        activateThis.SetActive(isActive);
    }

    IEnumerator _SetEnabledAfter<T>(GameObject go, bool isActive, float delay = 0f) where T : Component
    {
        T component;
        component = go.GetComponent<T>();
        yield return new WaitForSeconds(delay);

        if (typeof(T).IsSubclassOf(typeof(MonoBehaviour)))
        {
            (component as MonoBehaviour).enabled = isActive;
        }

    }

    #endregion
}
