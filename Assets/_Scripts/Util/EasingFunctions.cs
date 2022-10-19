using System;
using UnityEngine;

public static class EasingFunctions
{
	// Credit to all functions https://easings.net/

	public enum Ease
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

	public static Func<float, float> GetEase(Ease easing)
	{
		switch (easing)
		{
			case Ease.Linear: return Linear;
			case Ease.InQuad: return InQuad;
			case Ease.OutQuad: return OutQuad;
			case Ease.InOutQuad: return InOutQuad;
			case Ease.InCubic: return InCubic;
			case Ease.OutCubic: return OutCubic;
			case Ease.InOutCubic: return OutCubic;
			case Ease.InQuint: return InQuint;
			case Ease.OutQuint: return OutQuint;
			case Ease.InOutQuint: return InOutQuint;
		}

		return null;
	}

	public static float PerformEase(Ease easing, float x)
	{
		return GetEase(easing)(x);
	}

	private static float Linear(float x)
	{
		return x;
	}

	private static float InOutCubic(float x)
	{
		return x < 0.5 ? 4 * x * x * x : 1 - Mathf.Pow(-2 * x + 2, 3) / 2;
	}

	private static float InCubic(float x)
	{
		return x * x * x;
	}

	private static float OutCubic(float x)
	{
		return 1 - Mathf.Pow(1 - x, 3);
	}

	private static float InOutQuad(float x)
	{
		return x < 0.5 ? 2 * x * x : 1 - Mathf.Pow(-2 * x + 2, 2) / 2;
	}

	private static float InQuad(float x)
	{
		return x * x;
	}

	private static float OutQuad(float x)
	{
		return 1 - (1 - x) * (1 - x);
	}

	private static float InOutQuint(float x)
	{
		return x < 0.5 ? 16 * x * x * x * x * x : 1 - Mathf.Pow(-2 * x + 2, 5) / 2;
	}

	private static float InQuint(float x)
	{
		return x * x * x * x * x;
	}

	private static float OutQuint(float x)
	{
		return 1 - Mathf.Pow(1 - x, 5);
	}
}