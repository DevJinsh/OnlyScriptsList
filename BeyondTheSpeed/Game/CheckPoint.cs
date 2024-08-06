using UnityEngine;

public class CheckPoint : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Player"))
		{
			GameManager.Instance.SetRespawnPoint(transform.position);
			Destroy(gameObject);
			Debug.Log("세이브 되었습니다.");
		}
	}
}