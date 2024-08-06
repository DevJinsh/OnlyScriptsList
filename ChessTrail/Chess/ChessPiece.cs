using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SubsystemsImplementation;

public class ChessPiece : MonoBehaviour
{
    [Header("기본 설정")]
    private SpriteRenderer _renderer;
    public Color color;
        
    [Header("데이타 정보")]
    public ChessPieceType Type;

    /// <summary>
    /// 기물 생성된 직후 호출되는 초기화 함수
    /// </summary>
    public void Init(ChessPieceType type)
    {
        Type = type;
        _renderer = GetComponent<SpriteRenderer>();
        _renderer.sprite = ResourceManager.Instance.ChessPieceSpriteDict[type];
    }

    /// <summary>
    /// 지정된 위치로 기물 이동
    /// </summary>
    public void Move(Vector3 pos)
    {
        transform.position = pos;
    }

    public void UpdateType(ChessPieceType type)
    {
        Type = type;
        _renderer.sprite = ResourceManager.Instance.ChessPieceSpriteDict[type];
    }
    
    
    
    /// <summary>
    /// 기물 파괴
    /// </summary>
    public void Destroy()
    {
        if(gameObject != null) Destroy(gameObject);    
    }

    public void SetNewSpawnColor()
    {
        _renderer.color = color;
    }
}