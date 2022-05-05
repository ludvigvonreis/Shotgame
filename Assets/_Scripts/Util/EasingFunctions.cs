using UnityEngine;

public static class EasingFunctions
{
	public static float EaseInOutQuad(float x)
	{
		return x < 0.5 ? 2 * x * x : 1 - Mathf.Pow(-2 * x + 2, 2) / 2;
	}
}