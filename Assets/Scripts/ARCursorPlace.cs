using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARCursorPlace : MonoBehaviour
{
    private readonly float SnapToValue = 0.1f;
    private readonly string SnapToTag = "__Block__";

    private readonly Color ColorSelected = Color.green;
    private readonly Color ColorNotSelected = Color.blue;

    public ARCursor cursor;
    public GameObject objectToPlace;
    public ARRaycastManager raycastManager;

    private GameObject _next;

    void Start()
    {
        _next = Instantiate(objectToPlace, Vector3.zero, Quaternion.identity);
    }

    void Update()
    {
        // TODO: Handle duplicate on same position (Collider + Distance ?)
        // TODO: Handle Raycast on specific face
        // TODO: Bypass Planes in Raycast (Using Mask)

        Vector3 nextPosition;
        Quaternion nextRotation = Quaternion.identity;

        foreach (var obj in GameObject.FindGameObjectsWithTag(SnapToTag))
        {
            SetColor(obj.transform, ColorNotSelected);
        }

        Vector2 screenPosition = Camera.main.ViewportToScreenPoint(Vector2.one / 2);
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity) && hit.transform.tag == SnapToTag)
        {
            SetColor(hit.transform, ColorSelected);

            nextPosition = hit.transform.position + Vector3.up * SnapToValue / 2;
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

        _next.GetComponentInChildren<Collider>().transform.tag = SnapToTag;
        _next = Instantiate(objectToPlace, nextPosition, nextRotation);
    }

    void SetColor(Component component, Color color)
    {
        var renderer = component.GetComponentInChildren<Renderer>();
        if (renderer && renderer.material)
        {
            renderer.material.color = ColorSelected;
        }
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
