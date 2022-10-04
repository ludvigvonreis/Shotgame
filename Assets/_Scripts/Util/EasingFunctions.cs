using System;
using UnityEngine;

public static class EasingFunctions
{
	// Credit to all functions https://easings.net/

	public enum Easing
	{
		Linear,

		InQuad,
		OutQuad,
		InOutQuad,

		InCubic,
		OutCubic,
		InOutCubic,

		InQuint,
		OutQuint,
		InOutQuint
	}

	public static float Ease(Easing easing, float x)
	{
		switch (easing)
		{
			case Easing.Linear: return Linear(x);
			case Easing.InQuad: return InQuad(x);
			case Easing.OutQuad: return OutQuad(x);
			case Easing.InOutQuad: return InOutQuad(x);
			case Easing.InCubic: return InCubic(x);
			case Easing.OutCubic: return OutCubic(x);
			case Easing.InOutCubic: return OutCubic(x);
			case Easing.InQuint: return InQuint(x);
			case Easing.OutQuint: return OutQuint(x);
			case Easing.InOutQuint: return InOutQuint(x);
		}

		return 0f;
	}

	public static float Linear(float x)
	{
		return x;
	}

	public static float InOutCubic(float x)
	{
		return x < 0.5 ? 4 * x * x * x : 1 - Mathf.Pow(-2 * x + 2, 3) / 2;
	}

	public static float InCubic(float x)
	{
		return x * x * x;
	}

	public static float OutCubic(float x)
	{
		return 1 - Mathf.Pow(1 - x, 3);
	}

	public static float InOutQuad(float x)
	{
		return x < 0.5 ? 2 * x * x : 1 - Mathf.Pow(-2 * x + 2, 2) / 2;
	}

	public static float InQuad(float x)
	{
		return x * x;
	}

	public static float OutQuad(float x)
	{
		return 1 - (1 - x) * (1 - x);
	}

	public static float InOutQuint(float x)
	{
		return x < 0.5 ? 16 * x * x * x * x * x : 1 - Mathf.Pow(-2 * x + 2, 5) / 2;
	}

	public static float InQuint(float x)
	{
		return x * x * x * x * x;
	}

	public static float OutQuint(float x)
	{
		return 1 - Mathf.Pow(1 - x, 5);
	}
}