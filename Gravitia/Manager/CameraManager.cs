using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance = null;

    public static Stack<CinemachineVirtualCamera> Vcams;
    public const int BasePriority = 10;

    private CinemachineBrain _cinemachineBrain;
    private void Awake()
    {
        _cinemachineBrain = FindAnyObjectByType<CinemachineBrain>();
        Vcams = new Stack<CinemachineVirtualCamera>();
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void VcamsClear()
    {
        while (Vcams.Count != 0)
        {
            PopInVcams();
        }
        Vcams.Clear();
    }

    public void ChangeMainCamera(CinemachineVirtualCamera objectToChange)
    {
        if (Vcams.Count == 0)
        {
            Vcams.Push((CinemachineVirtualCamera)_cinemachineBrain.ActiveVirtualCamera);
        }
        else if (Vcams.Peek() == null) //임시방편
        {
            Vcams.Push((CinemachineVirtualCamera)_cinemachineBrain.ActiveVirtualCamera);
        }
        objectToChange.m_Priority = Vcams.Peek().m_Priority + 1;
        Vcams.Push(objectToChange);
    }

    public void ChangeToBeforeCamera()
    {
        PopInVcams();
    }
    public void ReturnToTheDesiredCamera(CinemachineVirtualCamera desiredCamera)
    {
        if (!Vcams.Contains(desiredCamera))
        {
            Debug.Log("The required virtual camera does not exist on the stack.");
        }
        while (Vcams.Peek() != desiredCamera)
        {
            PopInVcams();
        }
    }

    public CinemachineVirtualCamera PopInVcams()
    {
        if (Vcams.Count == 0)
        {
            return null;
        }
        CinemachineVirtualCamera poped = Vcams.Pop();
        poped.m_Priority = BasePriority;
        return poped;
    }
}
