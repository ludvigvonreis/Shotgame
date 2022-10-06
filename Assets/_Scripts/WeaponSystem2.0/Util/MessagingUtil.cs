using UnityEngine;
using UnityEngine.EventSystems;

public static class MessagingUtil
{
	public static void ExecuteRecursive<T>(GameObject target, ExecuteEvents.EventFunction<T> functor, int depth = 0)
	where T : IEventSystemHandler
	{
		// Execute on root aswell
		if (depth == 0) ExecuteEvents.Execute<T>(target, null, functor);

		foreach (Transform child in target.transform)
		{
			ExecuteEvents.Execute<T>(child.gameObject, null, functor);
			ExecuteRecursive<T>(child.gameObject, functor, depth + 1);
		}
	}
}