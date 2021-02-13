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
    public static Vector2 CoordCanvasMousePosition(GameObject Canvas)
    {
        Vector3 screenPos = Input.mousePosition;
        float widthRate = Canvas.GetComponent<RectTransform>().rect.size.x / Camera.main.pixelRect.width;
        float HeightRate = Canvas.GetComponent<RectTransform>().rect.size.y / Camera.main.pixelRect.height;
        screenPos = new Vector3((widthRate * screenPos.x) - (Canvas.GetComponent<RectTransform>().rect.size.x / 2),
            (HeightRate * screenPos.y) - (Canvas.GetComponent<RectTransform>().rect.size.y / 2), 0);

        return screenPos;
    }
    public static Vector2 DrawBezierCurve(Vector2 p0, Vector2 p1, Vector2 p2, float t)
    {
        Vector2 result;

        result = (1 - t) * (1 - t) * p0 + 2 * t * (1 - t) * p1 + t * t * p2;

        return result;
    }
    public static Vector3 ChildCoordCanvas(GameObject obj,GameObject TargetCanvas)
    {
        Transform tmp = obj.transform;
        Vector3 result = tmp.localPosition;
        while (tmp.parent != null && tmp.parent != TargetCanvas.transform)
        {
            result += tmp.parent.localPosition;
            tmp = tmp.parent;
        }
        return result;
    }
}
