using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class GameUtill
{
    public static IEnumerator Timer(float time, Delvoid del)
    {
        float Timer = 0;
        while (Timer < time)
        {
            Timer += Time.deltaTime;
            yield return null;
        }
        del();
    }
    public static Vector2 CoordCanvasPosition(Vector2 localpos)
    {
        Vector2 result = Vector2.zero;
        float width = MainSceneController.Instance.UIControl.UICanvas.pixelRect.width/2;
        float height = MainSceneController.Instance.UIControl.UICanvas.pixelRect.height/2;

        result = localpos + new Vector2(-width, height);

        return result;
    }
}
