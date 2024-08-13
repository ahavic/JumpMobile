using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphicsMenu : UIMenu
{
    public void SetQuality(int index)
    {
        QualitySettings.SetQualityLevel(index);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

}
