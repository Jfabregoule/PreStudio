using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private InputManager _controllerMap;

    public delegate void Interact();
    public event Interact onInteract;

    public delegate void Jump();
    public event Interact onJump;

    public delegate void SprintEnter();
    public event SprintEnter onSprintEnter;

    public delegate void SprintExit();
    public event SprintExit onSprintExit;

    public delegate void ShootEnter();
    public event ShootEnter onShootEnter;

    public delegate void ShootExit();
    public event ShootExit onShootExit;

    public delegate void Reload();
    public event Interact onReload;

    private void Awake()
    {
        _controllerMap = new InputManager();
    }

    private void OnEnable()
    {
        _controllerMap.Enable();
        _controllerMap.Player.Interact.started += ctx => InteractEvent();
        _controllerMap.Player.Jump.started += ctx => JumpEvent();
        _controllerMap.Player.Sprint.started += ctx => SprintEnterEvent();
        _controllerMap.Player.Sprint.canceled += ctx => SprintExitEvent();
        _controllerMap.Player.Shoot.started += ctx => ShootEnterEvent();
        _controllerMap.Player.Shoot.canceled += ctx => ShootExitEvent();
        _controllerMap.Player.Reload.started += ctx => ReloadEvent();
    }

    private void OnDisable()
    {
        _controllerMap.Player.Interact.started -= ctx => InteractEvent();
        _controllerMap.Player.Jump.started -= ctx => JumpEvent();
        _controllerMap.Player.Sprint.started -= ctx => SprintEnterEvent();
        _controllerMap.Player.Sprint.canceled -= ctx => SprintExitEvent();
        _controllerMap.Player.Shoot.started -= ctx => ShootEnterEvent();
        _controllerMap.Player.Shoot.canceled -= ctx => ShootExitEvent();
        _controllerMap.Player.Reload.started -= ctx => ReloadEvent();
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

    private void JumpEvent()
    {
        onJump?.Invoke();
    }
    private void SprintEnterEvent()
    {
        onSprintEnter?.Invoke();
    }
    private void SprintExitEvent()
    {
        onSprintExit?.Invoke();
    }
    private void ShootEnterEvent()
    {
        onShootEnter?.Invoke();
    }
    private void ShootExitEvent()
    {
        onShootExit?.Invoke();
    }

    private void ReloadEvent()
    {
        onReload?.Invoke();
    }
}
