using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
	[SerializeField] List<AudioClip> _playerLaserClips;
	[SerializeField] List<AudioClip> _enemyLaserClips;
	[SerializeField] List<AudioClip> _playerDeathClips;
	[SerializeField] List<AudioClip> _enemyDeathClips;

	[SerializeField] List<AudioClip> _uiClips;

	AudioSource _audioSource;
	private void Awake()
	{
		DontDestroyOnLoad(gameObject);

		_audioSource = GetComponent<AudioSource>();
	}

	public void PlayPlayerLaserSound()
	{
		_audioSource.PlayOneShot(_playerLaserClips[Random.Range(0, _playerLaserClips.Count)], 0.1f);
	}

	public void PlayEnemyLaserSound()
	{
		_audioSource.PlayOneShot(_enemyLaserClips[Random.Range(0, _enemyLaserClips.Count)], 0.1f);
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
		_audioSource.clip = _uiClips[Random.Range(0, _uiClips.Count)];
		_audioSource.pitch = Random.Range(0.9f, 1.1f);
		_audioSource.Play();
	}
}
