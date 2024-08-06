using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileManager : MonoBehaviour
{
    public int cellCount;
    public GameObject cell;
    public GameObject nonoPanel;
    public Sprite promotionTileImage;

    private GridLayoutGroup _nonoGridGroup;
    

    public CellEvent[,] Tiles { get; private set; }

    public void CreateTile(Stage stage)
    {
        _nonoGridGroup = nonoPanel.GetComponent<GridLayoutGroup>();
        cellCount = (int)Mathf.Sqrt(stage.Map.Length);

        CreateCrossLine(cellCount);

        _nonoGridGroup = nonoPanel.GetComponent<GridLayoutGroup>();
        _nonoGridGroup.constraintCount = cellCount;
        
        float cellsize = nonoPanel.GetComponent<RectTransform>().sizeDelta.x / cellCount;
        _nonoGridGroup.cellSize = Vector2.one * cellsize;
        

        Tiles = new CellEvent[cellCount, cellCount];

        for (int y = 0; y < cellCount; y++)
        {
            for (int x = 0; x < cellCount; x++)
            {
                var obj = Instantiate(cell, nonoPanel.transform);
                obj.name = "Cell" + y + "," + x;
                var tile = obj.GetComponent<CellEvent>();
                Tiles[y, x] = tile;

                if (stage.Map[y, x] == 0) tile.DisableCell();
                if (stage.Map[y, x] == 2)
                {
                    tile.IsPromotionTile = true;
                    tile.GetComponent<Image>().sprite = promotionTileImage;
                }
            }
        }
    }


    private List<CellEvent> _beforeSelectableTiles;
    public void ResetSelectableTiles()
    {
        _beforeSelectableTiles?.ForEach(x=> x.SetSelectableSprite(false, default));
        _beforeSelectableTiles = null;
    }
    public void UpdateSelectableTiles(CellEvent cell, ChessPieceType type)
    {
        (int x, int y) curPos = FindItemIndex(Tiles, cell);

        if (curPos != (-1, -1))
        {
            var moves = type switch
            {
                ChessPieceType.Pawn => GetPawnMoves(curPos),
                ChessPieceType.Bishop => GetBishopMoves(curPos),
                ChessPieceType.Rook => GetRookMoves(curPos),
                ChessPieceType.Knight => GetKnightMoves(curPos),
                ChessPieceType.Queen => GetQueenMoves(curPos),
                ChessPieceType.King => GetKingMoves(curPos),
            };

            moves.ForEach(x => x.SetSelectableSprite(true, type));
            _beforeSelectableTiles = moves;

            if (moves.Count == 0)
            {
                GameManager.Instance._uiManager.HoverNoticeMessage(MessageContext.NoWay);
            }
        }
    }
    
    public (int x, int y) FindItemIndex(CellEvent[,] array, CellEvent item)
    {
        int rows = array.GetLength(0);
        int cols = array.GetLength(1);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (array[i, j].Equals(item))
                {
                    return (i, j);
                }
            }
        }

        return (-1, -1);
    }

    private List<CellEvent> GetPawnMoves((int x, int y) curPos)
    {
        List<CellEvent> moves = new List<CellEvent>();

        int newX = curPos.x - 1;
        int newY = curPos.y;

        if (IsInsideBoard(newX, newY)) moves.Add(Tiles[newX, newY]);
        newX -= 1;
        if (IsInsideBoard(newX, newY)) moves.Add(Tiles[newX, newY]);

        return moves;
    }

    private List<CellEvent> GetKnightMoves((int x, int y) curPos)
    {
        List<CellEvent> moves = new List<CellEvent>();

        int[] dx = { 1, 2, 2, 1, -1, -2, -2, -1 };
        int[] dy = { 2, 1, -1, -2, -2, -1, 1, 2 };

        for (int i = 0; i < dx.Length; i++)
        {
            int newX = curPos.x + dx[i];
            int newY = curPos.y + dy[i];

            if (IsInsideBoard(newX, newY))
            {
                CellEvent targetCell = Tiles[newX, newY];
                if (targetCell != null)
                {
                    moves.Add(targetCell);
                }
            }
        }

        return moves;
    }

    private List<CellEvent> GetBishopMoves((int x, int y) curPos)
    {
        List<CellEvent> moves = new List<CellEvent>();

        int[] dx = { -1, -1, 1, 1 };
        int[] dy = { -1, 1, -1, 1 };

        for (int i = 0; i < dx.Length; i++)
        {
            for (int j = 1; j < Tiles.GetLength(0); j++)
            {
                int newX = curPos.x + (j * dx[i]);
                int newY = curPos.y + (j * dy[i]);

                if (!IsInsideBoard(newX, newY))
                {
                    break;
                }

                CellEvent targetCell = Tiles[newX, newY];
                moves.Add(targetCell);
            }
        }

        return moves;
    }

    private List<CellEvent> GetRookMoves((int x, int y) curPos)
    {
        List<CellEvent> moves = new List<CellEvent>();

        int[] dx = { -1, 1, 0, 0 };
        int[] dy = { 0, 0, -1, 1 };

        for (int i = 0; i < dx.Length; i++)
        {
            for (int j = 1; j < Tiles.GetLength(0); j++)
            {
                int newX = curPos.x + (j * dx[i]);
                int newY = curPos.y + (j * dy[i]);

                if (!IsInsideBoard(newX, newY))
                {
                    break;
                }

                CellEvent targetCell = Tiles[newX, newY];
                moves.Add(targetCell);
            }
        }

        return moves;
    }

    private List<CellEvent> GetQueenMoves((int x, int y) curPos)
    {
        List<CellEvent> moves = new List<CellEvent>();
        List<CellEvent> rookMoves = GetRookMoves(curPos);
        List<CellEvent> bishopMoves = GetBishopMoves(curPos);

        moves.AddRange(rookMoves);
        moves.AddRange(bishopMoves);

        return moves;
    }

    private List<CellEvent> GetKingMoves((int x, int y) curPos)
    {
        List<CellEvent> moves = new List<CellEvent>();

        int[] dx = { -1, -1, -1, 0, 0, 1, 1, 1 };
        int[] dy = { -1, 0, 1, -1, 1, -1, 0, 1 };

        for (int i = 0; i < dx.Length; i++)
        {
            int newX = curPos.x + dx[i];
            int newY = curPos.y + dy[i];

            if (IsInsideBoard(newX, newY))
            {
                CellEvent targetCell = Tiles[newX, newY];
                if (targetCell != null)
                {
                    moves.Add(targetCell);
                }
            }
        }

        return moves;
    }

    private bool IsInsideBoard(int x, int y)
    {
        return x >= 0 && x < Tiles.GetLength(0) && y >= 0 && y < Tiles.GetLength(1);
    }


    public void NonoPanelClear()
    {
        for (int i = nonoPanel.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(nonoPanel.transform.GetChild(i).gameObject);
        }   
    }

    void CreateCrossLine(int count)
    {
       // crossLine.sprite = crossLineSprite[count / 5 - 1];
    }
}