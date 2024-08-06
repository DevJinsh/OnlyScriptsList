using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class LoadingSceneManager : MonoBehaviour
{
    public static string nextScene;
    [SerializeField] Image progressImage;

    private void Awake()
    {
        AspectUtility.SetCamera();
    }
    private void Start()
    {
        StartCoroutine(LoadingScene());
        progressImage.rectTransform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("03.LoadingScene", LoadSceneMode.Additive);
    }

    private void Update()
    {
        progressImage.rectTransform.rotation *= new Quaternion(0, 0, -Time.deltaTime, 1);
    }
    IEnumerator LoadingScene()
    {
        yield return null;
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.completed += (asyncOperation) =>
        {
            progressImage.DOColor(new Color(0, 0, 0, 1), 1.5f).SetEase(Ease.InCirc).OnComplete(() => SceneManager.UnloadSceneAsync(2));
        };
    }
}
