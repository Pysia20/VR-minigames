using UnityEngine;
using UnityEngine.Events;

public class PhysicalButton : MonoBehaviour
{
    public float pressDepth = 0.01f;
    private Vector3 startPos;
    private bool pressed;
    public UnityEvent action;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void FixedUpdate()
    {
        float depth = startPos.y - transform.localPosition.y;

        if (!pressed && depth >= pressDepth)
        {
            pressed = true;
            action.Invoke();
        }

        if (pressed && depth < pressDepth)
        {
            pressed = false;
        }

        // Debug.Log(depth);
    }
}
