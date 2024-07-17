using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
	[SerializeField] List<AudioClip> _bounceClips;
	[SerializeField] List<AudioClip> _scoreClips;
	[SerializeField] List<AudioClip> _uiClips;


	AudioSource _audioSource;
	private void Awake()
	{
		DontDestroyOnLoad(gameObject);

		_audioSource = GetComponent<AudioSource>();
	}

	public void PlayBounceSound()
	{
		_audioSource.clip = _bounceClips[Random.Range(0, _bounceClips.Count)];
		_audioSource.pitch = Random.Range(0.9f, 1.1f);
		_audioSource.Play();
	}

	public void PlayScoreSound()
	{
		_audioSource.clip = _scoreClips[Random.Range(0, _scoreClips.Count)];
		_audioSource.pitch = Random.Range(0.9f, 1.1f);
		_audioSource.Play();
	}

	public void PlayUISound()
	{
		_audioSource.clip = _uiClips[Random.Range(0, _uiClips.Count)];
		_audioSource.pitch = Random.Range(0.9f, 1.1f);
		_audioSource.Play();
	}
}
