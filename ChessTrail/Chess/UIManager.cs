using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    [Header("Stage UI Related")]
    public GameObject StageUIObj;
    private StageManager _stageManager;
    
    [Header("Game UI Related")]
    public GameObject GameUIObj;
    public GameObject HoverNoticePanel;
    public TextMeshProUGUI HoverNoticeTMP;
    public RectTransform hoverNoticeRectTransform;

    [Header("ChessPiece")] 
    public Transform ChessPiecesSlotTr;
    public GameObject ChessPiecesPrefab;
    public List<UIChessPiece> ChessPieces;

    [Header("Other UI Text")] 
    public TextMeshProUGUI stageNameText;
    public TextMeshProUGUI destroyText;
    public TextMeshProUGUI turnText;
    public TextMeshProUGUI scoreText;
    public int score;
    
    [Header("Game Flow")] 
    public GameObject gameOverObj;
    public GameObject gameClearObj;
    public Image ClearImg;

    [Header("Game Guide")] 
    public TextMeshProUGUI guideNonoTMP;
    public TextMeshProUGUI guideChessTMP;
    public GameObject guideNonoImage;
    public GameObject guideChessImage;

    [Header("UNDO")] 
    public TextMeshProUGUI unDOPointText;
    
    [Header("Promotion")] 
    public GameObject promotionPanel;
    
    private void Awake()
    {
        _stageManager = GetComponent<StageManager>();
    }

    public void Update()
    {
        hoverNoticeRectTransform.anchoredPosition = Input.mousePosition;
    }
    public void SetStageSelectMode()
    {
        GameUIObj.SetActive(false);
        StageUIObj.SetActive(true);
        
        // 단순 UI 띄우기
    }

    public void SetInGameMode()
    {
        StageUIObj.SetActive(false);
        GameUIObj.SetActive(true);
    }

    /// <summary>
    /// 게임 플레이 화면 첫 세팅
    /// </summary>
    public void SetGamePlayMode(int limitCount)
    {
        StageUIObj.SetActive(false);
        GameUIObj.SetActive(true);
        
        // 각 텍스트 초기화 필요
    }

    public void UpdateTurnText(int turn)
    {
        turnText.text = turn.ToString();
    }

    /*public void UpdateDestroyCount(int cnt, int limitCount)
    {
        destroyText.text = $"{cnt}/{limitCount}";
    }*/

    #region Next Chess Piece 관리
    /// <summary>
    /// 
    /// </summary>
    public void UpdateNextChessPieceList(IEnumerable<ChessPieceType> chessPieces)
    {
        ChessPieces = new();
        
        foreach (var piece in chessPieces)
        {
            var obj = Instantiate(ChessPiecesPrefab, ChessPiecesSlotTr);
            var uipiece = obj.GetComponent<UIChessPiece>();
            uipiece.Init(piece);
            ChessPieces.Add(uipiece);
        }
        
        ChessPieces.First().WaitNextPiece();
    }

    public void RemoveNextChessPieceAtFirst()
    {
        if (ChessPieces.Count > 1)
        {
            ChessPieces[1].WaitNextPiece();
        }
        
        // 제거
        var first = ChessPieces.FirstOrDefault();
        if (first != null)
        {
            Destroy(first.gameObject);
            ChessPieces.RemoveAt(0);
        }
    }
    public void ResetNextChessPieceList()
    {
        Debug.Log(ChessPiecesSlotTr.childCount );
        for (int i = ChessPiecesSlotTr.childCount - 1; i >= 0; i--)
        {
            Destroy(ChessPieces[i].gameObject);
        }
    }
    #endregion
    

    /// <summary>
    /// Hover Notice 내용 출력 관리
    /// </summary>
    public bool isCloseHover = false;
    public void HoverNoticeMessage(MessageContext context)
    {
        string message = string.Empty;
        HoverNoticePanel.transform.position = Input.mousePosition;

        switch (context)
        {
            case MessageContext.SpawnPiece:
                message = "체스말을 소환할 위치를 선택해 주세요";
                break;

            case MessageContext.MovePiece:
                message = "체스말을 이동할 위치를 선택해 주세요";
                break;
            
            case MessageContext.CantMovePiece:
                message = "이동할 수 없는 위치입니다";
                break;

            case MessageContext.NoWay:
                message = "더 이상 둘 수 있는 수가 없습니다";
                break;
            case MessageContext.NoUnDo:
                message = "더 이상 되돌릴 수 없습니다.";
                break;
            case MessageContext.NotEnoughUnDoPoint:
                message = "UNDO 포인트가 모자랍니다.";
                break;
            
            default:
                break;
        }

        if (!string.IsNullOrEmpty(message))
        {
            HoverNoticePanel.SetActive(true);
            HoverNoticeTMP.text = message;
            StartCoroutine(CloseHover());
        }
        else
        {
            HoverNoticePanel.SetActive(false);
        }
    }
    public void B_ClickClearInfoView()
    {
        
    }
    
    // Guide 출력 관리
    #region NonoChessGuide
    private Coroutine guideNonoCoroutine, guideChessCoroutine;

    public void OnPointerEnterNonoTMP()
    {
        if (guideNonoCoroutine != null) StopCoroutine(guideNonoCoroutine);
        guideNonoCoroutine = StartCoroutine(ShowImageDelayed(guideNonoImage, 0.2f));
    }

    public void OnPointerExitNonoTMP()
    {
        if (guideNonoCoroutine != null) StopCoroutine(guideNonoCoroutine);
        guideNonoCoroutine = StartCoroutine(HideImageDelayed(guideNonoImage, 0.1f));
    }

    public void OnPointerEnterChessTMP()
    {
        if (guideChessCoroutine != null) StopCoroutine(guideChessCoroutine);
        guideChessCoroutine = StartCoroutine(ShowImageDelayed(guideChessImage, 0.2f));
    }

    public void OnPointerExitChessTMP()
    {
        if (guideChessCoroutine != null) StopCoroutine(guideChessCoroutine);
        guideChessCoroutine = StartCoroutine(HideImageDelayed(guideChessImage, 0.1f));
    }

    private IEnumerator ShowImageDelayed(GameObject image, float delay)
    {
        yield return new WaitForSeconds(delay);
        image.SetActive(true);
    }

    private IEnumerator HideImageDelayed(GameObject image, float delay)
    {
        yield return new WaitForSeconds(delay);
        image.SetActive(false);
    }
    #endregion

    //Score 관리
    /*public void ResetScore()
    {
        score = 0;
    }
    public void UpdateScore()
    {
        float acc = ((float)GameManager.Instance.correctCount / (float)GameManager.Instance.TurnCount) * 100f;
        scoreText.text = Mathf.Round(acc).ToString();
    }*/
    
    IEnumerator CloseHover()
    {
        isCloseHover = false;
        yield return new WaitUntil(() => isCloseHover);
        HoverNoticePanel.SetActive(false);
    }
}



public enum MessageContext
{
    SpawnPiece,
    MovePiece,
    CantMovePiece,
    NoWay,
    NoUnDo,
    NotEnoughUnDoPoint
}