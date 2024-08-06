using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 오브젝트가 뭘 할지만 정함.
/// 게임 로직은 여기서 구현하지 말 것.
/// Component를 만들어서 구현할 것.
/// </summary>
public abstract class BaseController : MonoBehaviour, IController
{
    public Dictionary<string, ScriptableObject> DataDict => dataDict;

    /// <summary>
    /// Data Dictionary for interaction
    /// </summary>
    private Dictionary<string, ScriptableObject> dataDict = new Dictionary<string, ScriptableObject>();

    /// <summary>
    /// Add all scriptable object Data
    /// </summary>
    protected abstract void AddDatas();

    protected virtual void Awake()
    {
        AddDatas();
    }
}
