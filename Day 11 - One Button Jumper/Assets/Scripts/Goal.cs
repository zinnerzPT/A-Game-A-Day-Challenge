using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
	[SerializeField] GameObject _gameOverPanel;
	[SerializeField] TMP_Text _gameOverScreen;


	float _time;

	BoxCollider2D _collider;
	private void Awake()
	{
		_time = 0.0f;
		_collider = GetComponent<BoxCollider2D>();
	}
	// Update is called once per frame
	void Update()
	{
		_time += Time.deltaTime;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.CompareTag("Player"))
		{
			string time = Mathf.FloorToInt(_time / 60).ToString() + ":" + (_time % 60).ToString("F2");
			_gameOverPanel.SetActive(true);
			_gameOverScreen.SetText("You beat the game in:\n" + time);

			_collider.enabled = false;
		}
	}

	public void ReloadGame()
	{
		SceneManager.LoadScene(0);
	}
}
