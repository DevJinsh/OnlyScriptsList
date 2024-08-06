using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;
    public Dictionary<ChessPieceType, Sprite> ChessPieceSpriteDict;

    public Dictionary<string, Sprite> StageSpriteDict;
    
    public void Init()
    {
        Instance = this;
        ReadChessPieceSprite();
    }


    void ReadChessPieceSprite()
    {
        ChessPieceSpriteDict = new();

        for (int i = 0, cnt = Enum.GetNames(typeof(ChessPieceType)).Length; i < cnt; i++)
        {
            var type = (ChessPieceType)i;
            var sprite = Resources.Load<Sprite>($"ChessPiece/ChessPiece_{type.ToString()}");

            if (sprite != null)
            {
                ChessPieceSpriteDict.Add(type, sprite);
            }
        }
    }
}