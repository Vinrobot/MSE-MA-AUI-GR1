using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;

public class ARCursorPlace : MonoBehaviour
{
    public static readonly float SnapToValue = 0.1f;
    public static readonly string SnapToLayer = "Block";

    public ARCursor cursor;
    public Block objectToPlace;
    public ColorSelector colorSelector;

    private Block _next;
    private Block _selected;
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
            _selected.currentState = Block.State.Normal;
            _selected = null;
        }

        Vector2 screenPosition = Camera.main.ViewportToScreenPoint(Vector2.one / 2);
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, 1 << _snapToLayer))
        {
            _selected = hit.transform.GetComponentInParent<Block>();
            _selected.currentState = Block.State.Selected;

            nextPosition = _selected.transform.position + SnapToValue * hit.normal;
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
        _next.currentState = Block.State.Normal;
        _next.ColorNormal = colorSelector.value.color;

        _next = Instantiate(objectToPlace, nextPosition, nextRotation);
        _next.currentState = Block.State.Temporary;
    }

    bool TryGetTouchPosition(out Vector2 touchPosition)
    {
        if (Input.touchCount > 0 && EventSystem.current.currentSelectedGameObject == null)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                touchPosition = touch.position;
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
