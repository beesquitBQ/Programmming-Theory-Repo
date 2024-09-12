using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public CameraController cameraController;

    private void Start()
    {
        if (cameraController == null)
        {
            cameraController = FindObjectOfType<CameraController>();
        }
    }
    private void OnDestroy()
    {
        if (cameraController != null)
        {
            cameraController.player = null;
        }
    }
}
