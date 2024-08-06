using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gizmo : MonoBehaviour
{
    public enum GizmoColors { ChangeVcamPriorityColor, VibrationColor, ConfinerColor, TriggerCubeColor, SavePointColor, TurretColor}

    public GizmoColors color;
    void OnDrawGizmos()
    {
        switch (color)
        {
            case GizmoColors.ChangeVcamPriorityColor:
                Gizmos.color = new Color(0, 1, 0, 0.3f);
                break;
            case GizmoColors.VibrationColor:
                Gizmos.color = new Color(1, 0, 0, 0.3f);
                break;
            case GizmoColors.ConfinerColor:
                Gizmos.color = new Color(0, 0, 1, 0.3f);
                break;
            case GizmoColors.TriggerCubeColor:
                Gizmos.color = new Color(0, 1, 1, 0.3f);
                break;
            case GizmoColors.SavePointColor:
                Gizmos.color = new Color(1, 1, 0, 0.3f);
                break;
            case GizmoColors.TurretColor:
                Gizmos.color = new Color(1, 0, 1, 0.3f);
                break;
            default:
                break;
        }
        Gizmos.DrawCube(transform.position, this.transform.localScale);
    }
}
