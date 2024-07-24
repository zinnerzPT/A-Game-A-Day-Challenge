using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pellet : MonoBehaviour
{
	public int points = 10;

	private void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.CompareTag("Player"))
		{
			Eat();
		}
	}

	protected virtual void Eat()
	{
		FindObjectOfType<GameManager>().PelletEaten(this);
	}
}
