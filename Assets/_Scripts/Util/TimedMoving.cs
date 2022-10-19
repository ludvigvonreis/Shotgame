using System;
using System.Collections;
using UnityEngine;

public static class TimedMoving
{
	public static IEnumerator MoveTransformPosition(Transform transform, Vector3 to, float duration, Func<float, float> easingFunction, bool local = false)
	{
		Vector3 startPosition = local ? transform.localPosition : transform.position;

		if (duration > Mathf.Epsilon)
		{
			for (float progress = 0; progress < duration; progress += Time.deltaTime)
			{
				if (local)
				{
					transform.localPosition = Vector3.Lerp(startPosition, to, easingFunction(progress / duration));
				}
				else
				{
					transform.position = Vector3.Lerp(startPosition, to, easingFunction(progress / duration));
				}
				yield return null;
			}
		}

		if (local)
		{
			transform.localPosition = to;
		}
		else
		{
			transform.position = to;
		}
	}

	public static IEnumerator MoveFov(Camera cam, float to, float duration, Func<float, float> easingFunction)
	{
		var start = cam.fieldOfView;

		for (float progress = 0; progress < duration; progress += Time.deltaTime)
		{
			cam.fieldOfView = Mathf.Lerp(start, to, easingFunction(progress / duration));
			yield return null;
		}

		cam.fieldOfView = to;
	}
}