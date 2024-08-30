using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCameraMovement : MonoBehaviour
{
    [SerializeField] float rotationSpeed;
    [SerializeField] Camera cam;

    void LateUpdate()
    {
        transform.Rotate(Vector3.right, rotationSpeed * Time.deltaTime);
    }
}
