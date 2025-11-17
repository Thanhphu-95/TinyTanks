using UnityEngine;

public class BarrelRotator : MonoBehaviour
{
    public Transform barrel;    
    public float rotateSpeed = 20f;
    public float minAngle;   
    public float maxAngle = 30f;   

    public LineRenderer line;
    public float laserLength = 20f;

    float currentAngle = 0f;

    void Update()
    {
        
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0f)
        {

            currentAngle -= scroll * rotateSpeed;
            currentAngle = Mathf.Clamp(currentAngle, minAngle, maxAngle);
            barrel.localRotation = Quaternion.Euler(currentAngle, 0, 0);
        }
        Line();
    }


    public void Line()
    {
        Vector3 start = barrel.position;

        Vector3 direction = barrel.forward;
        Vector3 end = start + direction * laserLength;

        line.SetPosition(0, start);
        line.SetPosition(1, end);
    }
}
