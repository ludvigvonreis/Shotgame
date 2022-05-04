using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempWeap : MonoBehaviour, WeaponSystem.Weapon.IOwner
{
	[SerializeField]
	WeaponSystem.Weapon weapon;

	void Start()
	{
		weapon.Setup(this);
	}
}
