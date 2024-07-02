using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
	[SerializeField] private LayerMask _layerToHit;
	[SerializeField] private float _speed;

	[SerializeField] private float _yLimit = 5.0f;

	private void Update()
	{
		transform.position += transform.up * _speed * Time.deltaTime;

		if(Mathf.Abs(transform.position.y) >= _yLimit)
			Destroy(gameObject);
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if((_layerToHit & (1 << collision.gameObject.layer)) > 0)
		{
			Destroy(collision.gameObject);
			Destroy(gameObject);
		}
	}
}
