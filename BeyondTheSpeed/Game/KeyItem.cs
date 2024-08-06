using UnityEngine;

public class KeyItem : MonoBehaviour
{
	private bool _isHandled;

	private void Start()
	{
		_isHandled = false;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (_isHandled)
		{
			return;
		}

		if (other.gameObject.CompareTag("Player"))
		{
			GameManager.Instance.IncreaseKeyItem(this);
			_isHandled = true;
		}
	}
}
