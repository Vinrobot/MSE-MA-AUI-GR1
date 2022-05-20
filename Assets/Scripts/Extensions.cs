using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static void SetColor(this Transform transform, Color color)
    {
        var renderer = transform.GetComponentInChildren<Renderer>();
        if (renderer && renderer.material)
        {
            renderer.material.color = color;
        }
    }
}
