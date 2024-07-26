using UnityEngine;

public class Rock : MonoBehaviour
{
	[SerializeField] private float _scrollSpeed = 1.0f;

	[SerializeField] private float _minY = -1.5f;
	[SerializeField] private float _minX = -6;

	private GameManager _gameManager;
	private void Awake()
	{
		_gameManager = FindObjectOfType<GameManager>();
	}

	void Start()
	{
		RandomizeY();
	}

	void Update()
	{
		if(!_gameManager.isRunning)
			return;

		Vector3 pos = transform.position;

		pos.x -= _scrollSpeed * Time.deltaTime;

		if(pos.x < _minX)
		{
			pos.x += Mathf.Abs(_minX * 2.0f);
			RandomizeY();
		}

		transform.position = pos;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.gameObject.CompareTag("Player"))
		{
			// Score++
			_gameManager.AddScore();
		}
	}

	private void RandomizeY()
	{
		transform.position = new Vector3(transform.position.x, Random.Range(_minY, 0.0f));
	}
}
