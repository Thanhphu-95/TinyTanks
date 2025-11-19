using UnityEngine;

public class CameraAutoZoom : MonoBehaviour
{
    [Header("Zoom Settings")]
    public Transform target;
    public float moveThreshold = 0.1f;
    public float zoomIn = 8f;
    public float zoomOut = 12f;
    public float zoomSpeed = 5f;

    [Header("Look At Mouse")]
    public float lookStrength = 0.3f;   
    public float lookSmooth = 5f;       

    private Vector3 lastPos;
    private float currentZoom;

    private Camera cam;
    private Vector3 defaultLocalPos;

    void Start()
    {
        cam = Camera.main;
        lastPos = target.position;
        currentZoom = cam.orthographicSize;

        defaultLocalPos = transform.localPosition;
    }

    void LateUpdate()
    {
        AutoZoom();
        HardLookAtMouse();
    }

    void AutoZoom()
    {
        float speed = (target.position - lastPos).magnitude / Time.deltaTime;
        lastPos = target.position;

        float targetZoom = speed < moveThreshold ? zoomOut : zoomIn;
        currentZoom = Mathf.Lerp(currentZoom, targetZoom, Time.deltaTime * zoomSpeed);
        cam.orthographicSize = currentZoom;
    }

    void HardLookAtMouse()
    {
        Vector3 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = target.position.z;

        Vector3 dir = mouseWorld - target.position;

        // shift camera theo hướng con trỏ
        Vector3 desiredOffset = defaultLocalPos + new Vector3(dir.x, dir.y, 0) * lookStrength;

        transform.localPosition = Vector3.Lerp(
            transform.localPosition,
            desiredOffset,
            Time.deltaTime * lookSmooth
        );
    }
}
