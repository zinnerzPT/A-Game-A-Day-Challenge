using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessAssets : MonoBehaviour
{

	[SerializeField] private Sprite[] _sprites;

	public Sprite GetSprite(Chesspiece piece, TeamColor color)
	{
		return _sprites[(int)piece % 6 + (int)color * 6];
	}
}
