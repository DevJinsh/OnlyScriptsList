using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stage
{
    public Stage(string stageName, (int y, int x) startPos, int[,] map, int countTrue, List<ChessPieceType> pieceTypes, (int turn, int destroyCnt) highScore, bool isclear = false)
    {
        this.StageName = stageName;
        this.Map = map;
        this.CountStageTrue = countTrue;
        this.IsClear = isclear;
        this.HighScore = highScore;
        this.PieceTypes = pieceTypes;
        StartPosition = startPos;
    }

    public string StageName { get; set; }               // 스테이지 이름
    public int[,] Map { get; set; }                     // 맵 모양 (0, 1) or (true, false)? 
    public int CountStageTrue { get; set; }             // 클리어 True 갯수
    public bool IsClear { get; set; }                   // 클리어 정보
    public (int turn, int destroyCnt) HighScore { get; set; }
    
    public List<ChessPieceType> PieceTypes { get; set; }
    public Sprite Sprite { get; set; }

    public (int y, int x) StartPosition { get; set; } = (4, 0); // [TODO] 임시

    public bool PerfectClear { get; set; }


}