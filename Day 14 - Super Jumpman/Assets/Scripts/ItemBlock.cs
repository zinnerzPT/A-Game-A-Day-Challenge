using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBlock : Block
{

	[SerializeField] PowerUp _powerUp;

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	public override void Activate(Strength strength)
	{
		if(_isDepleted)
			return;

		//Play Animation

		SetActivated();

		SpawnPowerUp();
	}

	private void SpawnPowerUp()
	{
		Instantiate(_powerUp, transform.position + Vector3.up, Quaternion.identity);
	}
}
