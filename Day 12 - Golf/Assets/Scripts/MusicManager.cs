using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{

	private void Awake()
	{
		DontDestroyOnLoad(gameObject);
	}
}
