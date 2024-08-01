using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Cell
{
	public enum Type
	{
		Invalid,
		Water,
		Ship
	}

	public Vector3Int position;
	public Type type;
	public bool revealed;
}
