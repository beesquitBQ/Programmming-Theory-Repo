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

    [SerializeField] private GameObject swordObject;
    [SerializeField] private GameObject hammerObject;

    private bool swordIsActive = true;
    private Weapon currentWeapon;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        InitializeWeapons();
    }

    void InitializeWeapons()
    {
        if (swordObject != null && hammerObject != null)
        {
            swordObject.SetActive(true);
            hammerObject.SetActive(false);
            currentWeapon = swordObject.GetComponent<Weapon>();
            if (currentWeapon == null)
            {
                Debug.LogError("Sword object does not have a Weapon component!");
            }
        }
        else
        {
            Debug.LogError("Sword or Hammer object is not assigned in the inspector!");
        }
    }

    void Update()
    {
        isGrounded = controller.isGrounded;
        ProcessMove();
        ProcessJump();
        SwitchEquippedWeapon();
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
        if (Input.GetKeyDown(KeyCode.R) && currentWeapon != null && !currentWeapon.IsAttacking())
        {
            swordIsActive = !swordIsActive;
            if (swordObject != null && hammerObject != null)
            {
                swordObject.SetActive(swordIsActive);
                hammerObject.SetActive(!swordIsActive);
                currentWeapon = swordIsActive ? swordObject.GetComponent<Weapon>() : hammerObject.GetComponent<Weapon>();
                if (currentWeapon == null)
                {
                    Debug.LogError("Active weapon does not have a Weapon component!");
                }
            }
        }
    }

}