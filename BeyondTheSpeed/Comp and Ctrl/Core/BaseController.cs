using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������Ʈ�� �� ������ ����.
/// ���� ������ ���⼭ �������� �� ��.
/// Component�� ���� ������ ��.
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
