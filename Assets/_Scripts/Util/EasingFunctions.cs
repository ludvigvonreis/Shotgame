using System;
using UnityEngine;

public static class EasingFunctions
{
	// Credit to all functions https://easings.net/

	public static float Linear(float x)
	{
		return x;
	}

	public static float EaseInOutCubic(float x)
	{
		return x < 0.5 ? 4 * x * x * x : 1 - Mathf.Pow(-2 * x + 2, 3) / 2;
	}

	public static float EaseInCubic(float x)
	{
		return x * x * x;
	}

	public static float EaseOutCubic(float x)
	{
		return 1 - Mathf.Pow(1 - x, 3);
	}

	public static float EaseInOutQuad(float x)
	{
		return x < 0.5 ? 2 * x * x : 1 - Mathf.Pow(-2 * x + 2, 2) / 2;
	}

	public static float EaseInQuad(float x)
	{
		return x * x;
	}

	public static float EaseOutQuad(float x)
	{
		return 1 - (1 - x) * (1 - x);
	}

	public static float EaseInOutQuint(float x)
	{
		return x < 0.5 ? 16 * x * x * x * x * x : 1 - Mathf.Pow(-2 * x + 2, 5) / 2;
	}

	public static float EaseInQuint(float x)
	{
		return x * x * x * x * x;
	}

	public static float EaseOutQuint(float x)
	{
		return 1 - Mathf.Pow(1 - x, 5);
	}

	public static float EasedLerp(float a, float b, float t, Func<float, float> easer)
	{
		return Mathf.Lerp(a, b, easer(t));
	}

	public static Vector3 EasedLerp(Vector3 a, Vector3 b, float t, Func<float, float> easer)
	{
		return Vector3.Lerp(a, b, easer(t));
	}
}