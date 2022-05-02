using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Shotgun", menuName = "Weapons/Shotgun", order = 2)]
public class ShotgunStats : WeaponStats
{
	public int totalPellets;
	public float maxDevitation;
}