using UnityEngine;

public class ClickToDelete : MonoBehaviour
{
    private int _snapToLayer;

    void Start()
    {
        _snapToLayer = LayerMask.NameToLayer(ARCursorPlace.SnapToLayer);
    }

    public void OnClick()
    {
        Vector2 screenPosition = Camera.main.ViewportToScreenPoint(Vector2.one / 2);
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, 1 << _snapToLayer))
        {
            Block block = hit.transform.GetComponentInParent<Block>();
            if (block) Destroy(block.gameObject);
        }
    }
}
