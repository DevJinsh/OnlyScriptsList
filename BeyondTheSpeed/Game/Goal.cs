using UnityEngine;

public class Goal : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			GameManager.Instance.SetGameClear();
		}
	}
}
