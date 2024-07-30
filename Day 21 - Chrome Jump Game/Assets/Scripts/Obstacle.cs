using UnityEngine;

public class Obstacle : MonoBehaviour
{
	[SerializeField] private float _minX;

	private float _scrollSpeed = 1.0f;

	private GameManager _gameManager;
	private void Awake()
	{
		_gameManager = FindObjectOfType<GameManager>();
	}

	void Update()
	{
		if(!_gameManager.isRunning)
			return;

		Vector3 pos = transform.position;

		pos.x -= _scrollSpeed * Time.deltaTime;

		if(pos.x < _minX) {
			_gameManager.RemoveObstacle(this);
			Destroy(gameObject);
		}

		transform.position = pos;
	}

	public void SetSpeed(float speed)
	{
		_scrollSpeed = speed;
	}
}
