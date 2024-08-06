using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class AnimationEventFunction : MonoBehaviour
{
    public void StartPlayerMove(bool isTurn)
    {
        GameManager.Instance.StartPlayerMove(isTurn);
    }
    public void StartPlayerMoveWithTurn()
    {
        StartPlayerMove(true);
    }
    public void StopPlayerMove()
    {
        GameManager.Instance.StopPlayerMove();
    }
    public void TurnPlayer(float time)
    {
        GameManager.Instance.TurnPlayer(time);
    }
    public void ReleaseArmIK()
    {
        GameManager.Instance.ReleaseArmIK();
    }
    public void ChangeMainCamera(CinemachineVirtualCamera objectToChange)
    {
        CameraManager.Instance.ChangeMainCamera(objectToChange);
    }
    public void ChangeToBeforeCamera()
    {
        CameraManager.Instance.ChangeToBeforeCamera();
    }
    public void ReturnToTheDesiredCamera(CinemachineVirtualCamera desiredCamera)
    {
        CameraManager.Instance.ReturnToTheDesiredCamera(desiredCamera);
    }
    public void ShowCredit()
    {
        GetComponentInChildren<Ending>().ShowCredit();
    }
    public void SetOffSkipUI()
    {
        GameManager.Instance.animationManager.SetOffAllSkipUI();
    }
}
