using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float cameraHeight;
    public float cameraSpeed;
    public bool isFrozen;
    public bool isRotating { get; private set; }
    public Vector3 viewDirection { get; private set; }
    public Quaternion flatRotation { get; private set; }
    
    [SerializeField] private GameObject targetObject;

    private Vector3 currentViewDirection;

    
    void Start()
    {
        isRotating = false;
        currentViewDirection = new Vector3(3, 9, -6);
        currentViewDirection.y = 0;
        currentViewDirection = currentViewDirection.normalized;
        viewDirection = currentViewDirection;

        flatRotation = Quaternion.LookRotation(Vector3.forward);
        isFrozen = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (isFrozen)
            return;

        if(!isRotating)
        {
            if (Input.GetKeyDown(KeyCode.E))
                RotateCamera(-90f);

            if (Input.GetKeyDown(KeyCode.Q))
                RotateCamera(90f);
            
        }
        
        transform.position = targetObject.transform.position + Vector3.up * 5 + currentViewDirection * cameraHeight;
        transform.rotation = Quaternion.LookRotation(Vector3.Normalize(targetObject.transform.position - transform.position));
        
    }

    void RotateCamera(float angle)
    {
        StartCoroutine(SmoothRotate(angle));
    }

    IEnumerator SmoothRotate(float angle)
    {
        Quaternion fullRotation = Quaternion.AngleAxis(angle, Vector3.up);

        Vector3 endDirection = fullRotation * currentViewDirection;

        flatRotation = flatRotation * fullRotation;
        viewDirection = endDirection;
        isRotating = true;
        
        float currentAngle = Vector3.SignedAngle(currentViewDirection, endDirection, Vector3.up);

        float step = 0f;
        while (Mathf.Abs(currentAngle) > 1f + step)
        {
        
            currentAngle = Vector3.SignedAngle(currentViewDirection, endDirection, Vector3.up);
            float progress = Mathf.Abs(currentAngle) / Mathf.Abs(angle);
            
            float turningRate = -Mathf.Pow((progress * 2 - 1), 2) + 1;
            turningRate = Mathf.Clamp(turningRate, 0.1f, 1.0f);
            step = Time.deltaTime * cameraSpeed * turningRate;


            currentViewDirection = Quaternion.AngleAxis(Mathf.Sign(currentAngle) * step, Vector3.up) * currentViewDirection;
            
            yield return null;
        }

        currentViewDirection = endDirection;
        isRotating = false;
    }
}
