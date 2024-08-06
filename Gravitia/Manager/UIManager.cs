using Michsky.UI.Heat;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    public Image FadeInOutPanel;
    [Header("마우스 커서 이미지")]
    public Texture2D mouseCursorImage;
    public void ContinueGame()
    {
        FadeInOutPanel.gameObject.SetActive(true);
        FadeInOutPanel.DOFade(1f, 1f).OnComplete(() => LoadingSceneManager.LoadScene("02.Main"));
    }

    public void NewGame()
    {
        SaveSystem.DeletePlayerData();
        FadeInOutPanel.gameObject.SetActive(true);
        FadeInOutPanel.DOFade(1f, 1f).OnComplete(() => LoadingSceneManager.LoadScene("02.Main"));
    }
    public void LoadData()
    {
        FindAnyObjectByType<PauseMenuManager>().ClosePauseMenu();
        SaveSystem.ReloadScene();
        GameManager.Instance.playerMovementManager.InputActionDisable();
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        GameManager.Instance.playerMovementManager.InputActionDisable();
        Destroy(GameManager.Instance.gameObject);
        SceneManager.LoadScene("01.Title");
    }

    public void DefaultMouseCorsor()
    {
        Cursor.SetCursor(default, new Vector2(0, 0), CursorMode.Auto);
    }

    public void ImageMouseCorsor()
    {
        Vector2 cursorOffset = new Vector2(mouseCursorImage.width / 2, mouseCursorImage.height / 2);
        Cursor.SetCursor(mouseCursorImage, cursorOffset, CursorMode.ForceSoftware);
    }
}
