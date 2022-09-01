using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController3 : MonoBehaviour
{
    //Objects
    [SerializeField] GameObject Player;
    [SerializeField] GameObject MainCamera;
    [SerializeField] CharacterController Controller;
    [SerializeField] LayerMask GroundMask;
    [SerializeField] Slider oxygenSlider;
    [SerializeField] RectTransform oxygenRect;
    //Script Constants
    [SerializeField] float MoveSpeed;
    [SerializeField] float Sensitivity;
    [SerializeField] float Gravity;
    [SerializeField] float JumpHeight;
    [SerializeField] bool isUnderwater;
    [SerializeField] float oxygen = 30;
    private float vSpeed = 0;
    private bool[] runes = { 
        false, //Ice Rune
        false //Fire Rune
    };
    // Start is called before the first frame update
    public void setGravity(float a)
    {
        Gravity = a;
    }
    public void setMoveSpeed(float a)
    {
        MoveSpeed = a;
    }
    public void setisUnderwater(bool a)
    {
        isUnderwater = a;
    }
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        HandleGravity();
        HandleCamera();
        HandleMovement();
        HandleActions();
        HandleOxygen();
        
        Controller.Move(new Vector3(0, vSpeed * Time.deltaTime, 0));
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(Player.transform.position, Player.transform.position + Player.transform.up * -1.1f);
    }

    void die()
    {
        Debug.Log("You Died");
        Destroy(this);
    }
    void HandleOxygen()
    {
        if (isUnderwater)
        {
            oxygen -= Time.deltaTime;
            oxygenRect.localScale = new Vector3(1, 1, 1);
            if (oxygen <= 0)
            {
                die();
            }
        }
        else
        {
            oxygen = Mathf.Clamp(oxygen + Time.deltaTime * 5,0,30);
            if (oxygen == 30)
            {
                oxygenRect.localScale = new Vector3(0, 0, 0);
            }
        }
        oxygenSlider.value = oxygen;
    }

    void HandleActions()
    {
        //Jump
        if(Input.GetButton("Jump") && isGrounded(Player.transform))
        {
            vSpeed = JumpHeight;
        }
        if(Input.GetButton("Fire1") && runes[0])
        {
            GameObject iceturret = new GameObject("iceturret");
            iceturret.AddComponent<iceturretcontroller>();
            iceturret.transform.position = Player.transform.position;
        }
        if(Input.GetButtonDown("Fire2") && runes[1])
        {
            GameObject fireball = new GameObject("fireball");
            fireball.AddComponent<fireballcontroller>();
            fireball.transform.position = Player.transform.position;
            fireball.transform.localEulerAngles = new Vector3(
                MainCamera.transform.localEulerAngles.x,
                Player.transform.localEulerAngles.y,
                Player.transform.localEulerAngles.z
                );
        }
    }
    void HandleGravity()
    {
        if (isGrounded(Player.transform))
        {
            vSpeed = 0;
        }
        vSpeed -= Gravity * Time.deltaTime;
    }

    void HandleCamera()
    {
        float MouseX = Input.GetAxis("Mouse X");
        float MouseY = Input.GetAxis("Mouse Y");

        MainCamera.transform.localEulerAngles = new Vector3(
                EulerClamp(MainCamera.transform.localEulerAngles.x - MouseY * Sensitivity * Time.deltaTime,-90,90),
                MainCamera.transform.localEulerAngles.y,
                MainCamera.transform.localEulerAngles.z
            );

        Player.transform.localEulerAngles = new Vector3(
                Player.transform.localEulerAngles.x,
                Player.transform.localEulerAngles.y + MouseX * Sensitivity * Time.deltaTime,
                Player.transform.localEulerAngles.z
            );
    }

    void HandleMovement()
    {
        float MoveX = Input.GetAxis("Horizontal");
        float MoveY = Input.GetAxis("Vertical");

        Controller.Move(Player.transform.forward * MoveY * MoveSpeed * Time.deltaTime);
        Controller.Move(Player.transform.right * MoveX * MoveSpeed * Time.deltaTime);
    }

    float EulerClamp(float value, float min, float max)
    {
        if (value > 180)
        {
            value -= 360;
        }
        value = Mathf.Clamp(value, min, max);

        return value;
    }

    bool isGrounded(Transform Transform)
    {
        return Physics.Linecast(Transform.position, Transform.position + Transform.up * -1.2f, GroundMask);
    }
}
