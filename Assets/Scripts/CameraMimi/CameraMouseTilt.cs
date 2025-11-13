using System;
using UnityEngine;

[Serializable]
public enum ExecutionMode
{
    Update,
    LateUpdate,
    FixedUpdate
}

public class CameraMouseTilt : MonoBehaviour
{
    [SerializeField] private float strength = 1f;
    [SerializeField] private float speed = 1f;
    [SerializeField] public ExecutionMode execution;

    public Vector3 Offset { get; private set; } = Vector3.zero;

    private void Update()
    {
        if (execution == ExecutionMode.Update)
            Execute(Time.deltaTime);
    }

    private void LateUpdate()
    {
        if (execution == ExecutionMode.LateUpdate)
            Execute(Time.deltaTime);
    }

    private void FixedUpdate()
    {
        if (execution == ExecutionMode.FixedUpdate)
            Execute(Time.fixedDeltaTime);
    }

    private void Execute(float dt)
    {
        transform.eulerAngles -= Offset;

        Vector2 mousePos = Input.mousePosition;
        Vector2 resolution = new Vector2(Screen.width, Screen.height);
        Vector2 center = resolution / 2f;
        Vector2 diff = (mousePos - center) / resolution.y;

        Vector3 newOffset = new Vector2(-diff.y, diff.x) * strength;
        Offset = Vector2.Lerp(Offset, newOffset, speed * dt);
        transform.eulerAngles += Offset;
    }
}
