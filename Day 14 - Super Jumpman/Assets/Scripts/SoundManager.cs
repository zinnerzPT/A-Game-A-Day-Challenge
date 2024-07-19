using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
	[SerializeField] List<AudioClip> _playerJumpClips;
	[SerializeField] List<AudioClip> _playerDeathClips;
	[SerializeField] List<AudioClip> _enemyDeathClips;

	[SerializeField] List<AudioClip> _uiClips;

	[SerializeField] List<AudioClip> _coinClips;
	[SerializeField] List<AudioClip> _powerUpClips;
	[SerializeField] List<AudioClip> _powerDownClips;
	[SerializeField] List<AudioClip> _blockBreakClips;

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

	public void PlayCoinSound()
	{
		_audioSource.PlayOneShot(_coinClips[Random.Range(0, _coinClips.Count)], 0.1f);
	}

	public void PlayPowerUpSound()
	{
		_audioSource.PlayOneShot(_powerUpClips[Random.Range(0, _powerUpClips.Count)], 0.1f);
	}

	public void PlayPowerDownSound()
	{
		_audioSource.PlayOneShot(_powerDownClips[Random.Range(0, _powerDownClips.Count)], 0.1f);
	}

	public void PlayBlockBreakSound()
	{
		_audioSource.PlayOneShot(_blockBreakClips[Random.Range(0, _blockBreakClips.Count)], 0.1f);
	}
}
