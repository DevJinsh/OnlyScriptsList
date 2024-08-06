using UnityEngine;
using Beautify.Universal;
using System.Collections;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    public PlayerMovementManager playerMovementManager;
    public AnimationManager animationManager;
    public HapticManager hapticManager;
    public Vector3 playerSavePoint;
    float _waitSeconds = 2f;
    #region Device
    public enum Device
    {
        GamePad,
        PC,
    }
    private Device _currentDevice;
    public Device CurrentDevice
    {
        set
        {
            if (_currentDevice == value)
            {
                return;
            }
            _currentDevice = value;

            Cursor.visible = _currentDevice == Device.PC;
            Cursor.lockState = _currentDevice == Device.PC ? CursorLockMode.Confined : CursorLockMode.Locked;
            if (_currentDevice == Device.GamePad)
                playerMovementManager.CancelAming();
            if (_currentDevice == Device.PC)
                hapticManager.StopRumbling();
        }
        get { return _currentDevice; }
    }
    #endregion
    private void Awake()
    {
        playerMovementManager = FindAnyObjectByType<PlayerMovementManager>();
        animationManager = FindAnyObjectByType<AnimationManager>();
        hapticManager = GetComponent<HapticManager>();
        BeautifySettings.UnloadBeautify();
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
            Instance.animationManager.Init();
            RestartWithLocalData();
        }
        else
        {
            Instance.playerMovementManager = FindAnyObjectByType<PlayerMovementManager>();
            Instance.animationManager = FindAnyObjectByType<AnimationManager>();
            Instance.animationManager.Init();
            Instance.animationManager.FadeIn();
            Destroy(this.gameObject);
        }
    }

    public void StartPlayerMove(bool isTurn)
    {
        playerMovementManager.canMove = true;
        if(isTurn)
        {
            animationManager.TurnPlayerReverse();
        }
        //playerMovementManager.enabled = false;
    }

    public void StopPlayerMove()
    {
        playerMovementManager.canMove = false;
        playerMovementManager.GetComponent<Rigidbody>().velocity = Vector3.zero;
        animationManager.Move(0, false);
        hapticManager.StopRumbling();
    }

    public void TurnPlayer(float time)
    {
        animationManager.TurnPlayer(time);
    }

    public void ReleaseArmIK()
    {
        animationManager.ReleaseArmIK();
    }

    public void ReloadScene(GameObject player)
    {
        StartCoroutine(IRagDollCoroutine(player));
        SoundManager.instance.SoundDeadAfter();
    }

    IEnumerator IRagDollCoroutine(GameObject player)
    {
        player.GetComponent<PlayerMovementManager>().InputActionDisable();
        player.GetComponent<RagDollOnOff>().RagdollModeOn();
        animationManager.Fadeout();
        yield return new WaitForSeconds(_waitSeconds);
        SaveSystem.ReloadScene();
    }
    public void RestartWithLocalData()
    {
        PlayerData data = SaveSystem.LoadPlayer();
        if (data == null)
        {
            Instance.playerSavePoint = new Vector3(16.33f, 2.58f, 0);
            Instance.animationManager.Intro();
            return;
        }
        Vector3 respawnPosition = new Vector3(data.savePosition[0], data.savePosition[1], data.savePosition[2]);
        //this.transform.position = respawnPosition;
        Instance.playerSavePoint = respawnPosition;
    }
    public void RestartWithGameManager()
    {
        Instance.playerMovementManager.transform.position = Instance.playerSavePoint;
        FindAnyObjectByType<LoadGameObject>().SetGameObjects(Instance.playerSavePoint);
    }
}
