using UnityEngine;
using UnityEngine.Animations;

public class CameraController : MonoBehaviour
{
	public Transform          playersCenterPoint;
	public PositionConstraint PositionConstraint;
	public Camera             cam;

	[Range(0, 5)] public float factor  = 1f;
	[Range(1, 7)] public float minSize = 2.5f;
	[Range(1, 7)] public float maxSize = 4.5f;

	Vector3 _Calc;

	void LateUpdate()
	{
		cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, Mathf.Clamp(CalcSize(), minSize, maxSize), 0.15f);

		if(cam.orthographicSize > maxSize - 0.02f)
		{
			transform.position = Vector3.Lerp(transform.position, new Vector3(0, transform.position.y, 0), 0.15f);
		}
		else
		{
			transform.position = Vector3.Lerp(transform.position,
											  new Vector3(playersCenterPoint.position.x, transform.position.y,
														  playersCenterPoint.position.z), 0.15f);
		}
	}

	float CalcSize()
	{
		float   maxSize    = 0f;
		Vector3 maxVector3 = Vector3.zero;

		for(int i = 0; i < PositionConstraint.sourceCount; i++)
		{
			for(int j = 0; j < PositionConstraint.sourceCount; j++)
			{
				if(i != j)
				{
					_Calc = (PositionConstraint.GetSource(i).sourceTransform.position -
							 PositionConstraint.GetSource(j).sourceTransform.position);
					float _calc = _Calc.magnitude;

					if(_calc > maxSize)
					{
						maxSize    = _calc;
						maxVector3 = _Calc;
					}
				}
			}
		}

		if(Mathf.Abs(maxVector3.z) > 3f) maxSize *= 1.25f;

		return maxSize * factor;
	}
}