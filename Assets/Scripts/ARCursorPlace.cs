﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARCursorPlace : MonoBehaviour
{
    private readonly float SnapToValue = 0.1f;
    private readonly string SnapToLayer = "Block";
    private readonly Color ColorSelected = Color.green;
    private readonly Color ColorNotSelected = Color.blue;

    public ARCursor cursor;
    public GameObject objectToPlace;

    private GameObject _next;
    private Transform _selected;
    private int _snapToLayer;

    void Start()
    {
        _snapToLayer = LayerMask.NameToLayer(SnapToLayer);

        _next = Instantiate(objectToPlace, Vector3.zero, Quaternion.identity);
    }

    void Update()
    {
        // TODO: Handle duplicate on same position (Collider + Distance ?)

        Vector3 nextPosition;
        Quaternion nextRotation = Quaternion.identity;

        if (_selected)
        {
            _selected.SetColor(ColorNotSelected);
            _selected = null;
        }

        Vector2 screenPosition = Camera.main.ViewportToScreenPoint(Vector2.one / 2);
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, 1 << _snapToLayer))
        {
            hit.transform.SetColor(ColorSelected);
            _selected = hit.transform;

            nextPosition = hit.transform.position + hit.normal * SnapToValue / 3 * 2;
        }
        else
        {
            nextPosition = cursor.transform.position;
        }

        nextPosition = SnapTo(nextPosition, SnapToValue);

        _next.transform.position = nextPosition;
        _next.transform.rotation = nextRotation;

        if (!TryGetTouchPosition(out _))
            return;

        _next.transform.SetLayerRecursively(_snapToLayer);
        _next.transform.SetColor(ColorNotSelected);
        _next = Instantiate(objectToPlace, nextPosition, nextRotation);
    }

    bool TryGetTouchPosition(out Vector2 touchPosition)
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                touchPosition = Input.GetTouch(0).position;
                return true;
            }
        }

        touchPosition = default;
        return false;
    }

    public static Vector3 SnapTo(Vector3 a, float snap)
    {
        Vector3 b = a / snap;
        b.x = Mathf.Round(b.x) * snap;
        b.y = Mathf.Round(b.y) * snap;
        b.z = Mathf.Round(b.z) * snap;
        return b;
    }
}
