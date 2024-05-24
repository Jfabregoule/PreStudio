using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private InputManager _controllerMap;
    private bool _isMovingHorizontal = false;
    private bool _isMovingVertical = false;

    public delegate void Interact();
    public event Interact onInteract;

    private void Awake()
    {
        _controllerMap = new InputManager();
    }

    private void OnEnable()
    {
        _controllerMap.Enable();
        _controllerMap.Player.Interact.started += ctx => InteractEvent();
    }

    private void OnDisable()
    {
        _controllerMap.Player.Interact.started -= ctx => InteractEvent();
        _controllerMap.Disable();
    }

    public Vector2 GetMoveDirection()
    {
        if (!gameObject.activeSelf) return Vector2.zero;
        return _controllerMap.Player.Movement.ReadValue<Vector2>();
    }

    public Vector2 GetCameraRotation()
    {
        if (!gameObject.activeSelf) return Vector2.zero;
        return _controllerMap.Player.RotateCamera.ReadValue<Vector2>();
    }

    private void InteractEvent()
    {
        onInteract?.Invoke();
    }
}
