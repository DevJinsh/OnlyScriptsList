using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicator : MonoBehaviour
{
    public LineRenderer shootLineRenderer;
    public GameObject GravityOutLine;
    public LayerMask layerMask;
    public float ShotRange;
    [SerializeField] [Range(3, 100)] int polygonPoints = 3;
    [SerializeField] [Min(0.1f)] float radius = 7f;
    [SerializeField] float sphereRadius;
    public Vector3 hittedPos;
    bool drawGravityFieldOutLine;

    private void Start()
    {
        shootLineRenderer.enabled = true;
        GravityOutLine.SetActive(true);
        shootLineRenderer.widthMultiplier = 0.05f;
    }
    public void DrawShootLine(Vector3 originPos, Vector3 targetdir)
    {
        Vector3 dir = targetdir;
        RaycastHit hit;

        if (Physics.SphereCast(originPos, sphereRadius, dir, out hit, ShotRange, layerMask))
        {
            hittedPos = hit.point;
            drawGravityFieldOutLine = true;
            shootLineRenderer.enabled = true;
            //Debug.DrawRay(originPos, dir * hit.distance, Color.red);
        }
        else
        {
            drawGravityFieldOutLine = false;
            shootLineRenderer.enabled = false;
        }
        shootLineRenderer.SetPosition(0, originPos);
        shootLineRenderer.SetPosition(1, originPos + dir * (hit.distance + sphereRadius));
    }

    public void DrawGravityFieldOutLineWithShader(float ratio)
    {
        if (!drawGravityFieldOutLine)
        {
            GravityOutLine.SetActive(false);
            return;
        }
        GravityOutLine.transform.position = hittedPos;
    }

    public void SetShotRange(float value)
    {
        ShotRange = value;
    }
}
