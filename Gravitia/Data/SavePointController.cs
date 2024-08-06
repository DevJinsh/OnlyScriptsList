using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

public class SavePointController : MonoBehaviour
{
    public Canvas SavePointCanvas;
    public GameObject viewPortContent;
    public GameObject buttonPrefab;
    public GameObject savePoints;
    bool showConsole = false;

    private void Awake()
    {
        SavePointCanvas.gameObject.SetActive(true);
        int index = 1;
        foreach (Transform tf in savePoints.transform.GetComponentsInChildren<Transform>())
        {
            if (tf.name == savePoints.name)
                continue;
            CreateButtons(index++);
        }
        CreateButtonResetAchievement();
        SavePointCanvas.gameObject.SetActive(false);
    }
    public void ShowSavePointMenu()
    {
        showConsole = !showConsole;
    }

    private void OnGUI()
    {
        SavePointCanvas.gameObject.SetActive(showConsole);
        if (!showConsole)
        {
            return;
        }
    }

    void CreateButtons(int index)
    {
        GameObject button = Instantiate(buttonPrefab);
        button.transform.parent = viewPortContent.transform;
        button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "SavePoint " + index;
        button.transform.GetComponent<Button>().onClick.AddListener(() => SetSavePoint(savePoints.transform.GetChild(index - 1).transform.position));
    }
    void CreateButtonResetAchievement()
    {
        GameObject button = Instantiate(buttonPrefab);
        button.transform.parent = viewPortContent.transform;
        button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Reset Achievement ";
        button.transform.GetComponent<Button>().onClick.AddListener(() => Steamworks.SteamUserStats.ResetAll(true));
    }
    void SetSavePoint(Vector3 pos)
    {
        showConsole = false;
        GameManager.Instance.playerSavePoint = pos;
        GameManager.Instance.ReloadScene(FindAnyObjectByType<PlayerMovementManager>().gameObject);
    }
}
