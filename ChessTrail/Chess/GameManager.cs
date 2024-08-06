using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [Header("Component")]
    [SerializeField] private ResourceManager _resourceManager;
    [SerializeField] private NonoGramHint _nonogramHint;
    public static GameManager Instance;
    public UIManager _uiManager;

    [SerializeField] public Preview preview;
    [SerializeField] public StageManager stageManager;
    [SerializeField] public TileManager tileManager;

    [Header("GamePlay Variables")]
    [SerializeField] private int _turnCount;
    [SerializeField] public int correctCount;

    [Header("Log")]
    [SerializeField] private int _currentDestroyCount;

    /*public int CurrentDestroyCount
    {
        get => _currentDestroyCount;
        set
        {
            _currentDestroyCount = value;
            var destroyCount = stageManager.currentStage.PieceTypes.Count;
            _uiManager.UpdateDestroyCount(_currentDestroyCount, destroyCount);

            if (_currentDestroyCount == destroyCount)
            {
                GameOver("파괴 횟수를 초과했습니다.");
            }
        }
    }*/
    private CellEvent _previousTile;
    private CellEvent _currentTile;
    private ChessPieceType _previousPiecetype;

    public int TurnCount
    {
        get => _turnCount;
        set
        {
            _turnCount = value;
            _uiManager.UpdateTurnText(_turnCount);
        }
    }

    private int _curCrtTileCnt;
    public int CurCrtTileCnt
    {
        get => _curCrtTileCnt;
        set
        {
            _curCrtTileCnt = value;
            if (stageManager.currentStage.CountStageTrue == _curCrtTileCnt)
            {
                GameClear();
            }
        }
    }
    
    public List<ChessPieceType> nextChessPieces = new();

    public GameObject chessPiecePrefab;
    public CellEvent beforeTile;

    private ChessPiece _currentChessPiece;

    public bool NotMove;
    public bool CompleteSelected { get; set; }
    public bool GameEnd { get; set; }
    public Mode CurrentMode { get; set; }

    [Header("Hold")]
    [SerializeField] private UIChessPiece uiHoldPiece;
    private ChessPieceType? _holdPieceType;
    
    [Header("Promotion")]
    ChessPieceType _promotionPieceType;

    private bool _beforePromotion;


    public enum Mode
    {
        Spawn,
        Move
    }


    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            stageManager.Init();
            _resourceManager.Init();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        StartCoroutine(nameof(MovePreSelectStage));
        //_uiManager.ResetScore();
    }

    private void PreSelectStage()
    {
        _uiManager.SetStageSelectMode();    // 스테이지 선택 UI 활성화 
        tileManager.NonoPanelClear();       // 노노 판넬 초기화
        stageManager.UpdateStageObj();      // 스테이지 정보 업데이트
    }

    public void SelectStage(Stage stage)
    {
        // 스테이지 정보에 맞게 설정
        tileManager.CreateTile(stage);
        _nonogramHint.CreateHint(stage);
        preview.Init();

        SetPieceList(stage.PieceTypes);
        stageManager.currentStage = stage;
        _uiManager.stageNameText.text = stage.StageName;
        
        // Hold Reset
        _holdPieceType = null;
        uiHoldPiece.Reset();
         

        
        // 초기 값 설정
        TurnCount = 0;
        //CurrentDestroyCount = 0;
        CurCrtTileCnt = 1;
        correctCount = 0;
        GameEnd = false;
        CurrentMode = Mode.Spawn;
        NotMove = false;
        
        // 게임 시작
        StartCoroutine(nameof(GameFlow));

        _uiManager.SetInGameMode();
    }

    public void GameOver(string reason)
    {
        Debug.Log($"게임오버: {reason}");
        GameEnd = true;
        _uiManager.gameOverObj.SetActive(true);
    }

    private void GameClear()
    {
        Debug.Log("클리어");

        var stage = stageManager.currentStage;
        stage.IsClear = true;
        Debug.Log("턴 카운트: " + TurnCount);
        _uiManager.gameClearObj.transform.GetChild(3).gameObject.SetActive(false);
        //stage.HighScore = (TurnCount, CurrentDestroyCount);
        if (stage.CountStageTrue >= (TurnCount + 2))
        {
            stage.PerfectClear = true;
            _uiManager.gameClearObj.transform.GetChild(3).gameObject.SetActive(true);
        }
        _uiManager.ClearImg.sprite = stage.Sprite; 
        
        GameEnd = true;
        _uiManager.gameClearObj.SetActive(true);
    }

    
    
    #region Game Flow
    IEnumerator GameFlow()
    {
        // 시작 지점 설정
        var pos = stageManager.currentStage.StartPosition;
        beforeTile = tileManager.Tiles[pos.y, pos.x];
        beforeTile.TileState.CurrentState = TileState.State.Correct;
        
        yield return new WaitForSeconds(.1f);
        
        do
        {
            if (CurrentMode == Mode.Spawn)
            {
                // 체스 기물 소환
                yield return SpawnPiece();
            }

            if (GameEnd) break;
            
            if (CurrentMode == Mode.Move)
            {
                // 기물 이동 위치 선택 대기
                yield return MovePiece();
            }

            TurnCount++; // [TODO] 턴 조건 수정 필요
            //_uiManager.UpdateScore();
        } while (!GameEnd);
        
        //_currentChessPiece?.Destroy();
        
        CompleteSelected = false;
        yield return new WaitUntil(() => CompleteSelected); // 확인 대기
        
        ResetStage();
        StartCoroutine(nameof(MovePreSelectStage));
    }

    IEnumerator SpawnPiece()
    {
        
        // 체스 기물 생성
        CreateChessPiece();
        if (beforeTile.TileState.CurrentState == TileState.State.Default)
        {
            _currentChessPiece.SetNewSpawnColor();
        }
        
        CurrentMode = Mode.Move;
        yield return null;
    }

    private void CreateChessPiece()
    {
        if (nextChessPieces.Count > 0)
        {
            var obj = Instantiate(chessPiecePrefab);
            obj.transform.localScale = DefaultScale() * 0.7f;
            
            var piece = nextChessPieces.First();
            _currentChessPiece = obj.GetComponent<ChessPiece>();

            _currentChessPiece.Init(piece);
            nextChessPieces.Remove(piece);

            if (!_beforePromotion) {
                _uiManager.RemoveNextChessPieceAtFirst();
            }
            else _beforePromotion = false;
            
            _currentChessPiece.Move(beforeTile.transform.position);
            tileManager.UpdateSelectableTiles(beforeTile, _currentChessPiece.Type); // 이동 가능 범위 출력
        }
        else
        {
            GameOver("사용 가능한 기물이 존재하지 않음");
        }
        
    }
    
    public Vector3 DefaultScale()
    {
        var size = 5f / tileManager.cellCount;

        return new(size, size, 1);
    }

    IEnumerator MovePiece()
    {
        var backupBeforeTile = beforeTile;

        CompleteSelected = false;
        yield return new WaitUntil(() => CompleteSelected);
        
        tileManager.ResetSelectableTiles(); // 이동 가능 범위 초기화
        
        if (!NotMove)
        {
            // 이동 
            _currentChessPiece.Move(beforeTile.transform.position);

            // 프로모션 이벤트
            if (_currentChessPiece.Type == ChessPieceType.Pawn && beforeTile.IsPromotionTile)
            {
                _uiManager.promotionPanel.SetActive(true);
                CompleteSelected = false;
                yield return new WaitUntil(() => CompleteSelected);
                _uiManager.promotionPanel.SetActive(false);
                nextChessPieces.Insert(0, _promotionPieceType);
                _beforePromotion = true;
            }
        }

        // 파괴 
        _previousPiecetype = _currentChessPiece.Type;
        _currentChessPiece.Destroy();

        NotMove = false;
        CurrentMode = Mode.Spawn;
        
    }

    /// <summary>
    /// [미사용] 다음 체스 기물 설정 
    /// </summary>
    // private void UpdatePieceList()
    // {
    //     if (nextChessPieces.Count < 4) // 큐 내 기술 후보가 4개 미만일 때, 섞어 넣기 
    //     {
    //         List<ChessPieceType> list = new();
    //         
    //         for (int i = 0; i < 6; i++)
    //         {
    //             var randomPiece = (ChessPieceType)Random.Range((int)ChessPieceType.Pawn, (int)ChessPieceType.Rook + 1);
    //             list.Add(randomPiece);
    //         }
    //
    //         // 스페셜 기물 넣기
    //         var special = Random.Range(0f, 1f) <= .7f ? ChessPieceType.King : ChessPieceType.Queen;
    //         list.Add(special);
    //         
    //         // 섞어 넣기
    //         for (int i = list.Count - 1; i >= 0; i--)
    //         {
    //             var enqIdx = Random.Range(0, list.Count);
    //             nextChessPieces.Add(list[enqIdx]);
    //             list.RemoveAt(enqIdx);
    //         }
    //     }
    //
    //     UpdateNextChessListUI();
    // }

    /// <summary>
    /// 체스 기물 리스트 설정
    /// </summary>
    public void SetPieceList(IEnumerable<ChessPieceType> pieceTypes)
    {
        nextChessPieces = new List<ChessPieceType>(pieceTypes);
        _uiManager.UpdateNextChessPieceList(pieceTypes);
    }

    /// <summary>
    /// 타일 선택 시, 호출
    /// </summary>
    public void SelectedTile(CellEvent tile, bool isCorrect = false)
    {
        beforeTile = tile;
        _currentTile = beforeTile;
        if (CurrentMode == Mode.Move)
        {
            if (isCorrect)
            {   
                CurCrtTileCnt++;
                
                
                
                // [TODO] 여부에 따라서 다른 애니메이션 출력
            }
            else
            {
                // [TODO] 여부에 따라서 다른 애니메이션 출력
                
                //CurrentDestroyCount ++;
            }
        }
        
        CompleteSelected = true;
    }

    public void B_HoldPiece()
    {
        NotMove = true;
        if (_holdPieceType == null) // First Hold
        {
            _holdPieceType = _currentChessPiece.Type;
            CompleteSelected = true;
        }
        else
        {
            var temp = (ChessPieceType)_holdPieceType;
            _holdPieceType = _currentChessPiece.Type;
            _currentChessPiece.UpdateType(temp);
            
            tileManager.ResetSelectableTiles();
            tileManager.UpdateSelectableTiles(beforeTile, _currentChessPiece.Type); // 이동 가능 범위 출력
        }
        
        uiHoldPiece.Set((ChessPieceType)_holdPieceType);
    }

    public void B_DeletePiece()
    {
        NotMove = true;
        CompleteSelected = true;
    }

    public void B_Saesdfasdf()
    {
        
    }
    public void B_SelectedPromotionPieceType(int typeIdx)
    {
        _promotionPieceType = (ChessPieceType)typeIdx;
        CompleteSelected = true;
    }
    
    
    #endregion

    #region External Button Function

    public void B_Restart()
    {
        ResetStage();
        StartCoroutine(nameof(RestartCo));   
    }
    IEnumerator RestartCo()
    {
        yield return new WaitForSeconds(.01f);
        SelectStage(stageManager.currentStage);  
    }

    public void B_ConfirmEnd()
    {
        CompleteSelected = true;
        _uiManager.gameOverObj.SetActive(false);
        _uiManager.gameClearObj.SetActive(false);
    }

    public void B_GoToStageSelect()
    {
        ResetStage();
        StartCoroutine(nameof(MovePreSelectStage));   
    }

    private void ResetStage()
    {
        if (_currentChessPiece != null)
        {
            _currentChessPiece?.Destroy();
        }
        foreach (Transform child in _nonogramHint.verticalHints.transform) // 세로 힌트 초기화
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in _nonogramHint.horizontalHints.transform) // 가로 힌트 초기화
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in tileManager.nonoPanel.transform) // nono패널 초기화
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in preview.transform) // 프리뷰 초기화
        {
            Destroy(child.gameObject);
        }
        correctCount = 0; // 카운팅 초기화
        TurnCount = 0; // 턴 수 초기화
        
        //_uiManager.ResetScore(); // 점수 초기화
        _uiManager.ResetNextChessPieceList();
        
        
        StopCoroutine(nameof(GameFlow));
    }

    IEnumerator MovePreSelectStage()
    {
        yield return new WaitForSeconds(.01f);
        PreSelectStage();
    }
    
    #endregion
}