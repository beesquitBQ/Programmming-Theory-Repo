using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCameraMovement : MonoBehaviour
{
    [SerializeField] float rotationSpeed;
    [SerializeField] Camera mainCamera;



    void LateUpdate()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
