using UnityEngine;

public class Block : MonoBehaviour
{
    public Color ColorTemporary = Color.white;
    public Color ColorSelected = Color.green;
    public Color ColorNormal = Color.blue;

    public enum State
    {
        Temporary,
        Selected,
        Normal,
    }

    public Renderer targetRenderer;

    public State currentState = State.Normal;
    private State _previousState = State.Normal;

    void Start()
    {
        if (!targetRenderer) targetRenderer = GetComponent<Renderer>();

        Update();
    }

    void Update()
    {
        if (currentState == _previousState) return;
        _previousState = currentState;

        targetRenderer.material.color = getColor(currentState);
    }

    Color getColor(State state)
    {
        switch (currentState)
        {
            case State.Normal:
                return ColorNormal;
            case State.Selected:
                return ColorSelected;
            case State.Temporary:
                return ColorTemporary;
        }
        return ColorNormal;
    }
}
