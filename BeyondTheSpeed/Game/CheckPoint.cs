using UnityEngine;

public class CheckPoint : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Player"))
		{
			GameManager.Instance.SetRespawnPoint(transform.position);
			Destroy(gameObject);
			Debug.Log("���̺� �Ǿ����ϴ�.");
		}
	}
}