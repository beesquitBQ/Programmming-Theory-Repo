using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCameraMovement : MonoBehaviour
{
    [SerializeField] float rotationSpeed;
    [SerializeField] GameObject mainCamera;



    void LateUpdate()
    {
        mainCamera.transform.Rotate(Vector3.right, rotationSpeed * Time.deltaTime);
    }
}
