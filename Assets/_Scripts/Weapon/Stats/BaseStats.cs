using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FireMode
{
	Single,
	Rapid,
	Burst,
}

public class BaseStats : ScriptableObject
{
	[ReadOnly]
	public string ID;
	public new string name;

	[Header("Visual")]
	public GameObject model;

	void OnValidate()
	{
		if (!System.Guid.TryParse(ID, out _))
			ID = System.Guid.NewGuid().ToString();
	}
}