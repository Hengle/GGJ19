using UnityEngine;
using UnityEngine.Animations;

public class CameraController : MonoBehaviour
{
	public               Transform          playersCenterPoint;
	public               PositionConstraint PositionConstraint;
	public               Camera             cam;
	[Range(0, 5)] public float              factor  = 1f;
	[Range(1, 7)] public float              minSize = 2.5f;
	[Range(1, 7)] public float              maxSize = 4.5f;

	void LateUpdate()
	{
		cam.orthographicSize = Mathf.Clamp(CalcSize(), minSize, maxSize);

		if(cam.orthographicSize.Equals(4.5f))
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
		float maxSize = 0f;

		for(int i = 1; i < PositionConstraint.sourceCount; i++)
		{
			float _calc = (PositionConstraint.GetSource(i).sourceTransform.position -
						   PositionConstraint.GetSource(0).sourceTransform.position).magnitude;

			if(_calc > maxSize) maxSize = _calc;
		}

		return maxSize * factor;
	}
}