using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class StageManager : MonoBehaviour
{
    [Header("Stage List Control")]
    public Transform selectStageListTr;       // 스테이지 생성 위치
    public GameObject selectStagePrefab;      // 스테이지 프리팹
    
    public List<Stage> StageList { get; private set; }

    private CSVReader _reader;
    private Stage stages;

    public Stage currentStage;
    private int stageCount;
    /// <summary>
    /// 게임 시작시, GameManager에서 호출되는 초기화 함수
    /// </summary>
    public void Init()
    {
        LoadData();
        CreateStageObj();
    }
    
    /// <summary>
    /// 스테이지 정보를 읽어서 저장하는 함수
    /// </summary>
    private void LoadData()
    {
        _reader = GetComponent<CSVReader>();
        List<TextAsset> files = Resources.LoadAll<TextAsset>("StageData").ToList();
        StageList = new List<Stage>();
        for(int i = 0; i < files.Count - 1; i++)
        {
            var name = $"Stage {i}";
            stages = _reader.ReadMapDataCsv(name, i+1);
            stages.Sprite = Resources.Load<Sprite>($"SelectStage/SelectStage_{name}");
            StageList.Add(stages);
        }
        // [TODO] Stage 정보를 읽음
    }

    /// <summary>
    /// 읽혀진 스테이지 정보를 읽어, 스테이지 전체 리스트를 화면에 생성하는 함수
    /// </summary>
    private void CreateStageObj()
    {
        foreach (var stage in StageList)
        {
            var obj = Instantiate(selectStagePrefab, selectStageListTr);
            obj.GetComponent<UIStage>().Init(stage);
            obj.GetComponentInChildren<TMP_Text>().text = stage.StageName;
        }

    }

    /// <summary>
    /// 스테이지 클리어 등 정보 업데이트 시, 호출되는 함수
    /// </summary>
    public void UpdateStageObj()
    {
        selectStageListTr.BroadcastMessage("UpdateInfo");
    }
}