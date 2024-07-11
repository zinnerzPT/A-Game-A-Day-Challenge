using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
	[SerializeField] List<AudioClip> _playerJumpClips;
	[SerializeField] List<AudioClip> _playerDeathClips;
	[SerializeField] List<AudioClip> _enemyDeathClips;

	[SerializeField] List<AudioClip> _uiClips;

	AudioSource _audioSource;
	private void Awake()
	{
		DontDestroyOnLoad(gameObject);

		_audioSource = GetComponent<AudioSource>();
	}

	public void PlayPlayerJumpSound()
	{
		_audioSource.PlayOneShot(_playerJumpClips[Random.Range(0, _playerJumpClips.Count)], 0.1f);
	}

	public void PlayPlayerDeathSound()
	{
		_audioSource.PlayOneShot(_playerDeathClips[Random.Range(0, _playerDeathClips.Count)], 0.1f);
	}
	public void PlayEnemyDeathSound()
	{
		_audioSource.PlayOneShot(_enemyDeathClips[Random.Range(0, _enemyDeathClips.Count)], 0.1f);
	}

	public void PlayUISound()
	{
		_audioSource.PlayOneShot(_uiClips[Random.Range(0, _uiClips.Count)], 0.1f);
	}
}
