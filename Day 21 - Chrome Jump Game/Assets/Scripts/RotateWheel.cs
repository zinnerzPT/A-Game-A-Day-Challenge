using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWheel : MonoBehaviour
{
	void Update()
	{
		transform.Rotate(new Vector3(0.0f, 0.0f, 180.0f * Time.deltaTime));
	}
}
