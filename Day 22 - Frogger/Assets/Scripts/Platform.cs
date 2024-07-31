using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
	private Rigidbody2D _rigidbody;
	private void Awake()
	{
		_rigidbody = GetComponent<Rigidbody2D>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.gameObject.CompareTag("Player"))
		{
			// Grab player
			collision.transform.parent = transform;
			collision.transform.GetComponent<Rigidbody2D>().velocity = _rigidbody.velocity;
			collision.transform.GetComponent<Frog>().AddPlatform(this);
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if(collision.gameObject.CompareTag("Player"))
		{
			// Release player
			collision.transform.parent = null;
			collision.transform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
			collision.transform.GetComponent<Frog>().RemovePlatform(this);
		}
	}
}
