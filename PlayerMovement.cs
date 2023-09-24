using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    // This script should really be named PlayerManager

    [SerializeField] private GameManager gm;

    [Header("Movement parameters")]
    [SerializeField] private float spawn_speed;
    [SerializeField] private float speed;
    [SerializeField] private float sprintSpeedModifier;
    [SerializeField] private float gravity;
    [SerializeField] public float jump;
    [SerializeField] private float rampSlopeLimit;
    [SerializeField] private float slopeSpeed;

    [SerializeField] public bool hasKey;

    private CharacterController controller;
    private Vector3 velocity;

    private GameObject pusher;

    // Jump Checks
    private Transform groundCheck;
    private float groundDistance = 0.4f;
    private LayerMask groundMask;
    private bool isGrounded;

    private bool bounced;

    // Slope/sliding parameters
    private Vector3 surfaceNormal;
    private bool onRamp;
    private LayerMask slopeMask;
    private bool isSliding
    {
        get
        {
            if (onRamp && Physics.Raycast(transform.position, Vector3.down, out RaycastHit rampHit, 2f))
            {
                surfaceNormal = rampHit.normal;
                return Vector3.Angle(surfaceNormal, Vector3.up) > rampSlopeLimit;
            } 
            else if (isGrounded && Physics.Raycast(transform.position, Vector3.down, out RaycastHit slopeHit, 2f))
            {
                surfaceNormal = slopeHit.normal;
                return Vector3.Angle(surfaceNormal, Vector3.up) > controller.slopeLimit;
            }
            else
            {
                return false;
            }
        }
    }

    [Header("Interaction Text HUD")]
    [SerializeField] public GameObject popUpTextCanvas;
    [SerializeField] public TextMeshProUGUI popUpText;

    // Start is called on the first frame
    void Awake()
    {
        // Setting up variables
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        controller = this.GetComponent<CharacterController>();
        pusher = transform.GetChild(3).gameObject;
        pusher.SetActive(false);

        groundCheck = GameObject.Find("GroundCheck").transform;
        groundMask = LayerMask.GetMask("Ground");
        slopeMask = LayerMask.GetMask("Ramp");

        popUpTextCanvas = GameObject.Find("PopUps");
        popUpText = popUpTextCanvas.GetComponent<TextMeshProUGUI>();
        popUpTextCanvas.SetActive(false);

        // Setting initial movement parameters
        spawn_speed = 12f;
        speed = spawn_speed;
        sprintSpeedModifier = 1.5f;
        gravity = -9.81f;
        jump = 2f;
        rampSlopeLimit = 12f;
        slopeSpeed = 1f;

        bounced = false;
        hasKey = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (gm.hp > 0)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                speed *= sprintSpeedModifier;
            }

            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                speed = spawn_speed;
            }

            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
            onRamp = Physics.CheckSphere(groundCheck.position, groundDistance, slopeMask);

            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }

            if ((Input.GetButtonDown("Jump") && (isGrounded || onRamp)) || bounced)
            {
                velocity.y = Mathf.Sqrt(jump * -2f * gravity);
            }

            velocity.y += gravity * Time.deltaTime;

            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 move = transform.right * x + transform.forward * z;

            if (isSliding)
            {
                move += new Vector3(surfaceNormal.x, -surfaceNormal.y, surfaceNormal.z) * slopeSpeed;
            }

            controller.Move(move * speed * Time.deltaTime);

            controller.Move(velocity * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Teleporter"))
        {
            popUpTextCanvas.SetActive(true);
            popUpText.text = "Teleport (E)";
        }

        if (other.CompareTag("Button"))
        {
            popUpTextCanvas.SetActive(true);
            popUpText.text = "Push the button... (E)";
        }

        if (other.CompareTag("Respawn"))
        {
            gm.respawnPoint.position = other.transform.position;

            other.GetComponent<BoxCollider>().enabled = false;
        }

        if (other.CompareTag("Finish"))
        {
            SceneManager.LoadScene("02_FPS_SecondLevel");
        }

        if (other.CompareTag("Platform"))
        {
            bounced = true;
        }

        if (other.CompareTag("Key"))
        {
            popUpTextCanvas.SetActive(true);
            popUpText.text = "Take the key (E)";
        }

        if (other.CompareTag("Push"))
        {
            pusher.SetActive(true);
        }

        if (other.CompareTag("Winner"))
        {
            // Go to winning screen
            SceneManager.LoadScene("03_YouWin");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Teleporter") && Input.GetKeyDown(KeyCode.E))
        {
            controller.enabled = false;
            transform.position = GameObject.Find("TeleportPos").transform.position;
            controller.enabled = true;
            popUpTextCanvas.SetActive(false);
        }

        if (other.CompareTag("Button") && Input.GetKeyDown(KeyCode.E))
        {
            popUpTextCanvas.SetActive(false);
            GameObject.Find("EndFloor").SetActive(false);
        }

        if (other.CompareTag("Key") && Input.GetKeyDown(KeyCode.E))
        {
            hasKey = true;

            // Set the key under the camera
            other.GetComponent<itemBounce_script>().enabled = false;
            other.transform.SetParent(transform.GetChild(0).GetChild(1));
            other.transform.localPosition = Vector3.zero;
            other.transform.localScale = Vector3.one * 0.3f;

            popUpTextCanvas.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Teleporter"))
        {
            popUpTextCanvas.SetActive(false);
        }

        if (other.CompareTag("Button"))
        {
            popUpTextCanvas.SetActive(false);
        }

        if (other.CompareTag("Platform"))
        {
            popUpTextCanvas.SetActive(false);
            bounced = false;
        }

        if (other.CompareTag("Key"))
        {
            popUpTextCanvas.SetActive(false);
        }
    }
}
