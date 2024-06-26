using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Character player;
    [SerializeField] private float sensitivity = 2f;
    private float yRotationLimit = 88f;
    [SerializeField] private float smoothness = 150f;

    public float Sensitivity { get; set; }

    Vector2 rotation;
    Vector2 currentRotation;

    void Start()
    {
        rotation = Vector2.zero;
        currentRotation = Vector2.zero;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void FixedUpdate()
    {
        Vector2 rotationChange = player.Controller.GetCameraRotation();
        rotation.x += rotationChange.x * sensitivity;
        rotation.y += rotationChange.y * sensitivity;
        rotation.y = Mathf.Clamp(rotation.y, -yRotationLimit, yRotationLimit);

        currentRotation = Vector2.Lerp(currentRotation, rotation, smoothness);

        var yQuat = Quaternion.AngleAxis(currentRotation.y, Vector3.left);

        transform.localRotation = yQuat;

        player.transform.localRotation = Quaternion.Euler(0, currentRotation.x, 0);
    }
}