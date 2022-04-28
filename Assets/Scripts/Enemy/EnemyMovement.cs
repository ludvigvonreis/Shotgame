using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ECM.Components;

public class EnemyMovement : MonoBehaviour
{
	private GameObject player;

	[SerializeField] private float speed;
	private CharacterMovement characterMovement;
	private Vector3 cachedPlayerPos;

	private GameObject box;

	// Start is called before the first frame update
	void Start()
	{
		player = GameObject.Find("Player");
		characterMovement = GetComponent<CharacterMovement>();
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		cachedPlayerPos = new Vector3(player.transform.position.x, 0, player.transform.position.z);

		Vector3 direction = (cachedPlayerPos - this.transform.position).normalized;

		characterMovement.Move(direction * speed, 10f);
	}
}
