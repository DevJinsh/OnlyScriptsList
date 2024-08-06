using UnityEngine;

public class Respawn : MonoBehaviour
{
	private GameObject _player;

	private void Start()
	{
		_player = GameObject.Find("@@@Player@@@");
	}

	public void StartRespawn()
	{
		_player.transform.position = GameManager.Instance.respawnPoint;
	}
}