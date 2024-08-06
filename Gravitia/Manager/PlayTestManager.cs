using Beautify.Universal;
using System;
using System.Collections;
using System.IO;
using UnityEngine;

public class PlayTestManager : MonoBehaviour
{
    public static PlayTestManager Instance = null;

    private int _section = 0;
    private float _sectionPlayTime;
    private Coroutine _sectionCoroutine;

    private float _playTime = 0f;
    private Coroutine _gameCoroutine;

    private StreamWriter _writer;
    private string _filePath;
    private string _fileName;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
            _filePath = $@"{Application.persistentDataPath}\\PlayTestResult";
            _fileName = $"{DateTime.Now.ToString("yyyyMMdd-HHmmss")}.txt";
            if (!Directory.Exists(_filePath))
            {
                Directory.CreateDirectory(_filePath);
            }
            _writer = new StreamWriter(Path.Combine(_filePath, _fileName));
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    private void Start()
    {
        _gameCoroutine = StartCoroutine(GameTimer());
    }
    public void ClearGame()
    {
        Instance.GameClear();
    }

    public void SectionStart()
    {
        if (_sectionCoroutine != null)
            StopCoroutine(_sectionCoroutine);
        _sectionCoroutine = StartCoroutine(SectionTimer());
    }

    public void SectionEnd()
    {
        StopCoroutine(_sectionCoroutine);
        string log = $"{(_section < 1 ? "튜토리얼 " : _section) }구간: {((int)(_sectionPlayTime / 60))}분 {((int)(_sectionPlayTime % 60))}초";
        Debug.Log(log);
        _writer.WriteLine(log);
        _section++;
    }

    public IEnumerator SectionTimer()
    {
        _sectionPlayTime = 0f;
        while (true)
        {
            yield return null;
            _sectionPlayTime += Time.deltaTime;
        }
    }
    public void GameClear()
    {
        StopCoroutine(_gameCoroutine);
        string log = $"총 플레이 타임: {((int)(_playTime / 60))}분 {((int)(_playTime % 60))}초";
        Debug.Log(log);
        _writer.WriteLine(log);
    }
    private IEnumerator GameTimer()
    {
        _playTime = 0f;
        while (true)
        {
            yield return null;
            _playTime += Time.deltaTime;
        }
    }
    private void OnApplicationQuit()
    {
        Debug.Log("종료");
        _writer.Flush();
        _writer.Close();
    }
}
