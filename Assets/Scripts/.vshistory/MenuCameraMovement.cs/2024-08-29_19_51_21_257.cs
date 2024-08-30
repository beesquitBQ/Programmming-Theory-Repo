using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCameraMovement : MonoBehaviour
{
    [SerializeField] float rotationSpeed;
    [SerializeField] Camera cam;

    private void CameraRotation()
    {
        transform.Rotate(Vector3.right, rotationSpeed * Time.deltaTime);
    }
}
