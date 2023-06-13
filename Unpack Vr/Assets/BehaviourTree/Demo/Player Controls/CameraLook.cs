
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraLook : MonoBehaviour
{


    PlayerControl playerControl;

    private InputAction look;
    public PlayerInput playerInput;


    public float sensX = 2;
    public float sensY = 2;

    public float currentSens;

    public Transform orientaion;

    float xRotation;
    float yRotation;

    float rotation = 0;



    private void Start()
    {

        playerControl = FindObjectOfType<PlayerControl>();
        look = playerInput.actions["Look"];
        look.Enable();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;


    }

    // Update is called once per frame
    void Update()
    {

        
        currentSens = sensX;


        xRotation = look.ReadValue<Vector2>().x * currentSens * Time.deltaTime;
        yRotation = look.ReadValue<Vector2>().y * currentSens * Time.deltaTime;

        rotation -= yRotation;

        rotation = Mathf.Clamp(rotation, -90, 90);





        Quaternion localRotation = Quaternion.Euler(rotation, 0.0f, 0.0f);

        transform.localRotation = localRotation;




        orientaion.Rotate(Vector3.up * xRotation);


        //orientaion.Rotate(Vector3.up * xRotation);

        //Debug.Log(localRotation);
    }
}

