using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AspectUtility : MonoBehaviour
{
    static float wantedAspectRatio = (float)16 / 9;
    static Camera cam;
    static Camera backgroundCam;
    public static void SetCamera()
    {
        float currentAspectRatio = (float)Screen.width / Screen.height;
        cam = Camera.main;

        if ((int)(currentAspectRatio * 100) / 100.0f == (int)(wantedAspectRatio * 100) / 100.0f)
        {
            cam.rect = new Rect(0.0f, 0.0f, 1.0f, 1.0f);
            if (backgroundCam)
            {
                Destroy(backgroundCam.gameObject);
            }

            return;
        }
        // Pillarbox
        if (currentAspectRatio > wantedAspectRatio)
        {
            float inset = 1.0f - wantedAspectRatio / currentAspectRatio;
            //Debug.Log(new Rect(inset / 2, 0.0f, 1.0f - inset, 1.0f));
            cam.rect = new Rect(inset / 2, 0.0f, 1.0f - inset, 1.0f);
        }
        // Letterbox
        else
        {
            float inset = 1.0f - currentAspectRatio / wantedAspectRatio;
            cam.rect = new Rect(0.0f, inset / 2, 1.0f, 1.0f - inset);
        }

        if (!backgroundCam)
        {
            // Make a new camera behind the normal camera which displays black; otherwise the unused space is undefined
            backgroundCam = new GameObject("BackgroundCam", typeof(Camera)).GetComponent<Camera>();
            backgroundCam.depth = int.MinValue;
            backgroundCam.clearFlags = CameraClearFlags.SolidColor;
            backgroundCam.backgroundColor = Color.black;
            backgroundCam.cullingMask = 0;
        }
    }
}
