using System.Collections;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	private static GameManager s_instance;

    private TMP_Text timerText;
    private TMP_Text dashboardText;

	public Canvas canvas;
	public bool _isClear = false;

	private GameObject _player;
    private Rigidbody _rigidbody;
	private Respawn _respawn;
	private GameObject _pauseUI;

	private float _currentTime;
    private int _keyItem;
    private Coroutine gameTimer;

	public static GameManager Instance => s_instance;
	public Vector3 respawnPoint;

	private void Awake()
	{
		if (s_instance == null)
		{
			s_instance = this;
		}
	}

	void Start()
    {
        Time.timeScale = 1f;
        _player = GameObject.Find("@@@Player@@@");
        _rigidbody = _player.GetComponent<Rigidbody>();
		_respawn = GetComponent<Respawn>();

        dashboardText = canvas.gameObject.FindChild<TMP_Text>("DashBoard");
        timerText = canvas.gameObject.FindChild<TMP_Text>("Timer");
        _pauseUI = canvas.gameObject.FindChild<Transform>("PauseUI").gameObject;

        _currentTime = 0f;
        _keyItem = 0;
        gameTimer = StartCoroutine(StartGameTimer());
    }

    void Update()
    {
		dashboardText.text = $"Speed: {_rigidbody.velocity.GetFlatMagnitude():N1}Km/h";

		if (_player.transform.position.y < -10f)
		{
			_respawn.StartRespawn();
		}
    }

	public void IncreaseKeyItem(KeyItem item)
	{
		_keyItem++;
		Destroy(item.gameObject);
	}

	public void SetRespawnPoint(Vector3 point)
	{
		respawnPoint = point;
	}

	public void SetGameClear()
	{
		_isClear = true;
	}

    public void Pause()
    {
        Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
        _pauseUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
        _pauseUI.SetActive(false);
        Time.timeScale = 1f;
    }

    void GameClear()
    {
        Time.timeScale = 0f;
		Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        canvas.gameObject.FindChild<Transform>("ScoreBoard").gameObject.SetActive(true);
        canvas.gameObject.GetComponent<ScoreBoard>().Resulting(_currentTime, _keyItem);
    }

    IEnumerator StartGameTimer()
    {
        while(!_isClear)
        {
            yield return new WaitForSeconds(1);
            _currentTime += 1;
            timerText.text = (int)(_currentTime / 60f) + ":" + (int)(_currentTime % 60f);
        }
        GameClear();
    }
}
