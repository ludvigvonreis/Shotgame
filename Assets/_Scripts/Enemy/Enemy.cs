using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	/*
	[SerializeField] private float maxHealth;
	[SerializeField] private float health;

	void Start()
	{
		health = maxHealth;

		EventManager.Instance.m_HitEvent.AddListener(HitListener);
	}

	void Update()
	{
		if (health <= 0)
		{
			Death();
		}
	}

	void HitListener(Hit hit)
	{
		if (hit.raycastHit.transform.gameObject == this.gameObject)
		{
			Debug.LogFormat("I {0} got hit by {1}", this.name, hit.from.name);

			Damage(hit.weaponStats.damage);
		}
	}

	void Damage(float amount)
	{
		health -= amount;
	}

	void Death()
	{
		Debug.Log("I just dieded");

		Destroy(this.gameObject);
	}
	*/
}
