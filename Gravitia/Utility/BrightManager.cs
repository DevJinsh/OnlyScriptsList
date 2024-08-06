using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Beautify.Universal;

public class BrightManager : MonoBehaviour
{
    public float bright;

    public void FixedUpdate()
    {
        BeautifySettings.settings.frameBandVerticalSize.value = bright/10;
    }
}
