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

    [SerializeField] private Weapon swordWeapon;
    [SerializeField] private Weapon hammerWeapon;

    private bool swordIsActive = true;
    private Weapon currentWeapon;

    private bool isDead = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        InitializeWeapons();
    }

    void InitializeWeapons()
    {
        if (swordWeapon != null && hammerWeapon != null)
        {
            swordWeapon.gameObject.SetActive(true);
            hammerWeapon.gameObject.SetActive(false);
            currentWeapon = swordWeapon;
        }
    }

    void Update()
    {
        if (isDead)
        {
        return; 
        }

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
            if (swordWeapon != null && hammerWeapon != null)
            {
                swordWeapon.gameObject.SetActive(swordIsActive);
                hammerWeapon.gameObject.SetActive(!swordIsActive);
                currentWeapon = swordIsActive ? swordWeapon : hammerWeapon;
            }
        }
    }

    public void Die()
    {
        isDead = true;
        playerVelocity = Vector3.zero;
        controller.enabled = false;
    }

    public void ResetPlayer()
    {
        isDead = false;
        controller.enabled = true;
        playerVelocity = Vector3.zero;
        transform.position = new Vector3(20, 2, 0);
    }

}