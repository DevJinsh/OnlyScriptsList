using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CSVReader : MonoBehaviour
{
    Stage stageData;

    string stageName;
    int limitCount;
    int trueCount;

    (int y, int x) startPos;
    
    List<ChessPieceType> chessPieces;

    private bool debug()
    {
        return false;
    }

    public Stage ReadMapDataCsv(string csvFileName, int stageNum)
    {
        var textAsset = Resources.Load<TextAsset>("StageData/" + csvFileName);
        var stageInfo = Resources.Load<TextAsset>("StageData/" + "StageInfo");
        if (textAsset == null)
        {
            Debug.LogError($"Failed to load CSV file: {csvFileName}");
        }

        string[] lines = textAsset.text.Split('\n');
        string[] stageInfoLines = stageInfo.text.Split("\n");

        int lineCount = lines.Length;
        int[,] mapData = new int[lineCount, lineCount];
        for (int i = 0; i < lineCount; i++)
        {
            string trimData = lines[i].Trim();
            string[] data = trimData.Split(",");
            for (int j = 0; j < lineCount; j++)
            {
                try
                {
                    mapData[i, j] = Int32.Parse(data[j]);
                }
                catch when (debug())
                {

                }
            }
        }
        var stageData = stageInfoLines[stageNum].Split(",");
        stageName = stageData[0].ToUpper();
        trueCount = Int32.Parse(stageData[1]);
        
        //start pos
        var startPosText = stageData[2].Split('_');
        startPos = (Int32.Parse(startPosText[0]), Int32.Parse(startPosText[1]));

        chessPieces = new List<ChessPieceType>();
        for (int i = 2; i < stageData.Length; i++)
        {
            if (stageData[i] == "")
            {
                break;
            }
            switch (stageData[i])
            {
                case "Æù":
                    chessPieces.Add(ChessPieceType.Pawn);
                    break;
                case "³ªÀÌÆ®":
                    chessPieces.Add(ChessPieceType.Knight);
                    break;
                case "ºñ¼ó":
                    chessPieces.Add(ChessPieceType.Bishop);
                    break;
                case "·è":
                    chessPieces.Add(ChessPieceType.Rook);
                    break;
                case "Å·":
                    chessPieces.Add(ChessPieceType.King);
                    break;
                case "Äý":
                    chessPieces.Add(ChessPieceType.Queen);
                    break;
            }
        }
        return new Stage(stageName, startPos, mapData, trueCount, chessPieces, (0, 0));
    }
}
