using UnityEngine;
using UnityEngine.UI;

public class UIChessPiece : MonoBehaviour
{
    [SerializeField] private Image img;
    [SerializeField] private int nextPieceSize;
    [SerializeField] private int defaultPieceSize;
    
    public void Init(ChessPieceType type)
    {
        Set(type);
        GetComponent<RectTransform>().sizeDelta = new(defaultPieceSize, defaultPieceSize);
    }

    public void Set(ChessPieceType type)
    {
        img.sprite = ResourceManager.Instance.ChessPieceSpriteDict[type];
    }

    public void WaitNextPiece()
    {
        GetComponent<RectTransform>().sizeDelta  = new(nextPieceSize, nextPieceSize);
    }

    public void Reset()
    {
        img.sprite = null;
    }
}