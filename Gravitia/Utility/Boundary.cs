using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundary : MonoBehaviour
{
    static public List<GameObject> objects = new List<GameObject>();
    public bool Draw;
    [SerializeField] Camera camera;
    [SerializeField] Transform player;
    [SerializeField] float padding_x;
    [SerializeField] float padding_y;
    float size_y;
    float size_x;

    public float Bottom
    {
        get 
        { 
            return size_y * -1 + camera.gameObject.transform.position.y - padding_y; 
        }
    }

    public float Top
    {
        get 
        {
            return size_y + camera.gameObject.transform.position.y + padding_y;
        }
    }

    public float Left
    {
        get 
        {
            return size_x * -1 + camera.gameObject.transform.position.x - padding_x; 
        }
    }

    public float Right
    {
        get 
        {
            return size_x + camera.gameObject.transform.position.x + padding_x; 
        }
    }

    public float Height
    {
        get 
        {
            return size_y * 2; 
        }
    }

    public float Width
    {
        get 
        {
            return size_x * 2; 
        }
    }

    public void SetSize()
    {
        size_y = Mathf.Abs(camera.transform.position.z) * Mathf.Tan(camera.fieldOfView / 2f * Mathf.Deg2Rad);
        size_x = size_y * Screen.width / Screen.height;
    }

    private bool CheckInBoundary(Vector3 position)
    {
        SetSize();
        if (position.x < Left || position.x > Right || position.y > Top || position.y < Bottom)
        {
            return false;
        }
        return true;
    }

    private void Update()
    {
        DeleteObjectNotInBoundary();
        if (Draw)
        {
            DrawRay();
        }
    }
    public void DeleteObjectNotInBoundary()
    {
        foreach(var obj in objects)
        {
            if (obj == null)
                continue;
            if (!CheckInBoundary(obj.transform.position))
            {
                Destroy(obj);
            }
        }
    }

    void DrawRay()
    {
        SetSize();
        Debug.DrawRay(new Vector3(Left, Top, 0), Vector3.right * (size_x + padding_x) * 2, Color.red);
        Debug.DrawRay(new Vector3(Left, Bottom, 0), Vector3.right * (size_x + padding_x) * 2, Color.red);

        Debug.DrawRay(new Vector3(Left, Bottom, 0), Vector3.up * (size_y + padding_y) * 2, Color.red);
        Debug.DrawRay(new Vector3(Right, Bottom, 0), Vector3.up * (size_y + padding_y) * 2, Color.red);
    }
}
