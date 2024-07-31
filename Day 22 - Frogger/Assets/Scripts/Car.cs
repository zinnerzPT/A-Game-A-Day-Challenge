using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if(collision.gameObject.CompareTag("Player"))
		{
			collision.transform.GetComponent<Frog>().Die();
		}
	}
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.gameObject.CompareTag("Player"))
		{
			collision.transform.GetComponent<Frog>().Die();
		}
	}
}
