using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float playerMovementSpeed = 5f;
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool isGrounded;
    public float gravity = -9.8f;
    public float jumpHeight = 3f;

    private GameObject sword;
    private GameObject hammer;

    private bool swordIsActive = true;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        sword = GameObject.Find("SwordPosition");
        hammer = GameObject.Find("HammerPosition");
    }

    void Update()
    {
        isGrounded = controller.isGrounded;
        ProcessMove();
        ProcessJump();
    }

    void ProcessMove()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 moveDir = new Vector3(moveHorizontal, 0f, moveVertical).normalized;
        controller.Move(transform.TransformDirection(moveDir) * playerMovementSpeed * Time.deltaTime);

        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }

        playerVelocity.y += gravity * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    void ProcessJump()
    {
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }
    }

    void SwitchEquippedWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1) && !swordIsActive)
        {
            hammer.SetActive(false);
            sword.SetActive(true);
            swordIsActive = true;
        }
        if (Input.GetKeyDown(KeyCode.Keypad2) && swordIsActive)
        {
            sword.SetActive(false);
            hammer.SetActive(true);
            swordIsActive= false;
        }
    }
}