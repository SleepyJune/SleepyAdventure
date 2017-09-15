using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.EventSystems;

public static class HelperExtensions
{
    public static void SetTransparentMode(this Material material)
    {
        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        material.SetInt("_ZWrite", 0);
        material.DisableKeyword("_ALPHATEST_ON");
        material.DisableKeyword("_ALPHABLEND_ON");
        material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = 3000;
    }

    public static void DestroyChildren(this Transform root)
    {
        foreach (Transform transform in root.transform)
        {
            GameObject.Destroy(transform.gameObject);
        }
    }

    public static Vector3 ProjectionOnPoint2D(this Vector3 point, Vector3 start, Vector3 end)
    {
        var ap = (point.To2D() - start.To2D());
        var ab = (end.To2D() - start.To2D()).normalized;

        return start + Vector3.Project(ap, ab);
    }

    public static Vector3 ProjectionOnPoint(this Vector3 point, Vector3 start, Vector3 end)
    {
        var ap = (point - start);
        var ab = (end - start).normalized;

        return start + Vector3.Project(ap, ab);
    }

    public static float Distance(this Vector3 v1, Vector3 v2)
    {
        return Vector3.Distance(v1.To2D(), v2.To2D());
    }

    public static float GetAngleBetween2D(this Vector3 start, Vector3 v1, Vector3 v2)
    {
        start = start.To2D();
        v1 = v1.To2D();
        v2 = v2.To2D();

        return Vector3.Angle(v1 - start, v2 - start);
    }

    public static bool IsPointerOverUI(this Touch touch)
    {
        if (touch.fingerId >= 0)
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = touch.position;

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

            if (results.Count > 0)
            {
                return true;
            }
            return false;
        }
        else
        {
            return EventSystem.current.IsPointerOverGameObject();
        }
    }
}