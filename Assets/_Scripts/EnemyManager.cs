using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
	public List<Transform> spawnpoints = new List<Transform>();
	public List<GameObject> enemies = new List<GameObject>();
	public GameObject enemyObject;

	public int maxEnemies = 0;

	void Start()
	{
		for (int i = 0; i < maxEnemies; i++)
		{
			var enemy = Instantiate(enemyObject, spawnpoints[i % spawnpoints.Count].position, Quaternion.identity);
			enemies.Add(enemy);
		}
	}
}
