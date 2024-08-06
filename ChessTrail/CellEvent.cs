using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.ParticleSystem;

public class CellEvent : MonoBehaviour,
    IPointerClickHandler
    , IPointerEnterHandler
    , IPointerExitHandler
{
    private int correctCellCount = 0; //���� �� ����
    public int wrongCellCount = 0; //Ʋ�� �� ����

    public GameObject particle;
    public Sprite guessSprite;
    public Sprite guessWrongSprite;
    public Sprite guessQuestionSprite;
    public Sprite disableSprite;

    private TileState _tileState;
    public TileState TileState => _tileState ??= GetComponent<TileState>();
    public Image selectableImage;
    
    public bool IsPromotionTile { get; set; }
    bool isFlag = false;

    public void OnPointerClick(PointerEventData eventData) //Ŭ��
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (CheckSelectableCell())
            {
                GameManager.Instance._uiManager.isCloseHover = true;
                if (GameManager.Instance.CurrentMode == GameManager.Mode.Move)
                {
                    if (TileState.CurrentState != TileState.State.CanMove)
                    {
                        GameManager.Instance._uiManager.HoverNoticeMessage(MessageContext.CantMovePiece);
                        return;
                    }
                    gameObject.transform.GetChild(1).gameObject.SetActive(false);
                }
                int sibiling = transform.parent.Find(gameObject.name).GetSiblingIndex();
                int cellCount = (int)Mathf.Sqrt(GameManager.Instance.stageManager.currentStage.Map.Length);
                if (GameManager.Instance.stageManager.currentStage.Map[sibiling / cellCount, sibiling % cellCount] > 0)
                {
                    CorrectCell();
                }
                else
                {
                    WrongCell();
                }
            
                selectableImage.gameObject.SetActive(false);
            }
        }
        // if (eventData.button == PointerEventData.InputButton.Right)
        // {
        //     GuessCell();
        // }
    }

    public void OnPointerEnter(PointerEventData eventData) //���콺 ȣ��
    {
        if (CheckSelectableCell())
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData) //���콺 ȣ�� ����
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }

    #region
    //-----------------------------------Cell Information Check Management---------------------------------------//
    private void CorrectCell()
    {
        //GameManager�� ���� ��Ȳ ����
        GameManager.Instance.SelectedTile(this, true);
        if (GameManager.Instance.CurrentMode == GameManager.Mode.Move)
        {
            TileState.CurrentState = TileState.State.Correct;
            FillPreView(true);
            GameManager.Instance.correctCount++;
        }
        //�̹� �� �� Ŭ���� ���� ��Ŭ���� �� ����. 
    }

    private void WrongCell()
    {
        //GameManager�� ���� ����
        GameManager.Instance.SelectedTile(this, false);
        if (GameManager.Instance.CurrentMode == GameManager.Mode.Move)
        {
            TileState.CurrentState = TileState.State.Wrong;

            var obj = Instantiate(particle, transform);
            obj.transform.localScale = GameManager.Instance.DefaultScale();
            obj.GetComponent<ParticleSystem>().Play();
        }
    }

    private void GuessCell() //�� ������ ���� �÷���
    {
        if (TileState.CurrentState != TileState.State.Default && TileState.CurrentState != TileState.State.CanMove)
        {
            return;
        }
        GameObject obj = transform.GetChild(1).gameObject;
        if (!isFlag)
        {
            obj.SetActive(true);
            obj.GetComponent<Image>().sprite = guessSprite;
            isFlag = true;
        }
        else if (obj.GetComponent<Image>().sprite.name.Equals(guessSprite.name))
        {
            obj.GetComponent<Image>().sprite = guessWrongSprite;
        }
        else if (obj.GetComponent<Image>().sprite.name.Equals(guessWrongSprite.name))
        {
            obj.GetComponent<Image>().sprite = guessQuestionSprite;
        }
        else
        {
            obj.SetActive(false);
            isFlag = false;
        }
    }
    #endregion

    #region ü�� �⹰ �̵� ���� ǥ��
    private TileState.State _backup;
    public void SetSelectableSprite(bool state, ChessPieceType type)
    {
        if (!CheckSelectableCell()) return;
        
        if (state)
        {
            _backup = TileState.CurrentState;
            TileState.CurrentState = TileState.State.CanMove;
            
            selectableImage.gameObject.SetActive(true);
            selectableImage.sprite = ResourceManager.Instance.ChessPieceSpriteDict[type];
        }
        else
        {
            TileState.CurrentState = _backup;
            selectableImage.gameObject.SetActive(false);
        }
    }

    private bool CheckSelectableCell()
    {
        return (TileState.CurrentState == TileState.State.Default ||
                TileState.CurrentState == TileState.State.CanMove);

    }
    #endregion

    public void Undo()
    {
        if(TileState.CurrentState == TileState.State.Wrong)
        {
            return;
        }
        TileState.CurrentState = TileState.State.Default;
        GameManager.Instance.CurCrtTileCnt--;
        FillPreView(false);
    }

    public void FillPreView(bool isCorrect)
    {
        (int x, int y) curPos = GameManager.Instance.tileManager.FindItemIndex(GameManager.Instance.tileManager.Tiles, this);
        GameManager.Instance.preview.Tiles[curPos.x, curPos.y].sprite = isCorrect ? GameManager.Instance.preview.correctImage : GameManager.Instance.preview.defaultImage;
    }

    #region V2 Method
    /// <summary>
    /// 셀을 비활성화 합니다.
    /// </summary>
    public void DisableCell()
    {
        TileState.CurrentState = TileState.State.Disable;
    }
    

    #endregion
}

