using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    // public static PlayerInput playerInput;
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    public bool topDownMode;
    private float lookVert = 0, lookHor = 0;

    [SerializeField]
    private float playerSpeed = 2.0f, jumpHeight = 1.0f, gravityValue = -9.81f, Xsensitivity = 1.77f, Ysensitivity = 1f;
    [SerializeField]
    private GameObject[] weapons;
    private Camera mainCam;
    public static Action startShooting, startCharging, stopShooting, stopAim;
    
    void Awake()
    {
        instance = this;
        controller = GetComponent<CharacterController>();
        mainCam = Camera.main;
    }

    void Start()
    {
        int playerWeapon = PlayerPrefs.GetInt("PlayerWeapon", 0);

        foreach (GameObject weapon in weapons)
        {
            weapon.SetActive(false);
        }
        weapons[playerWeapon].SetActive(true);
    }

    void Update()
    {
        //Char Controller
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        if (!topDownMode) //FPS move/aim
        {
            //FPS Movement
            Vector2 movInput = GameManager.playerInput.PlayerMain.Move.ReadValue<Vector2>();
            Vector3 move = mainCam.transform.forward * movInput.y + mainCam.transform.right * movInput.x;
            move.y = 0;
            //Move Forward
            controller.Move(move * Time.deltaTime * playerSpeed);

            //FPS Aim
            Vector2 look = GameManager.playerInput.PlayerMain.Aim.ReadValue<Vector2>();
            lookVert += look.y;
            lookHor = look.x;
            lookVert = Mathf.Clamp(lookVert, -89f, 89f);

            transform.Rotate(Vector3.up * lookHor * Xsensitivity);
            Vector3 camAngle = new Vector3(-lookVert * Ysensitivity, 0f, 0f);
            mainCam.transform.localEulerAngles = camAngle;
        } else { //Top Down Aim
            //Top Down Movement
            Vector2 movInput = GameManager.playerInput.PlayerMain.Move.ReadValue<Vector2>();
            Vector3 move = new Vector3 (movInput.x, 0, movInput.y);
            //Move with input
            controller.Move(move * Time.deltaTime * playerSpeed);

            //Top Down Aim
            Vector2 lookInput = GameManager.playerInput.PlayerMain.Aim.ReadValue<Vector2>();
            Vector3 look = new Vector3 (lookInput.x * Xsensitivity * 1.77f, 0, lookInput.y * Ysensitivity);
        if (look != Vector3.zero)
        {
            transform.forward = look;
        } else if (move != Vector3.zero)
        {
            transform.forward = move;
        }
        }


        // Changes the height position of the player.. //Adds jump
        if (GameManager.playerInput.PlayerMain.Jump.triggered && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        //Gravity
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        //Shoot actions
        GameManager.playerInput.PlayerMain.Shoot.performed += _ => startShooting();
        GameManager.playerInput.PlayerMain.Shoot.canceled += _ => stopShooting();
        GameManager.playerInput.PlayerMain.Aim.performed += _ => startCharging();
        GameManager.playerInput.PlayerMain.Aim.canceled += _ => stopAim();
    }

    void setSensitivity(float x, float y)
    {
        Xsensitivity = x;
        Ysensitivity = y;
    }

    void OnEnable()
    {
        settingsUI.playerSens += setSensitivity;
    }

    void OnDisable()
    {
        settingsUI.playerSens -= setSensitivity;
    }
}