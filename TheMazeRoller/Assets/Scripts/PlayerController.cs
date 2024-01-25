using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private int count;
    private float movementX;
    private float movementY;
    private bool isGrounded;
    private float jumpForce = 5f;
    public float speed;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;
    public TextMeshProUGUI timerText;
    private float maxTime;
    private float currentTime;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        SetCountText();
        winTextObject.SetActive(false);
        isGrounded = true;

        // Obtener el tiempo máximo según la escena
        switch (SceneManager.GetActiveScene().name)
        {
            case "Tutorial":
                maxTime = 60f;
                break;
            case "LVL1":
                maxTime = 35f;
                break;
            case "LVL2":
                maxTime = 40f;
                break;
            case "LVL3":
                maxTime = 45f;
                break;
            default:
                maxTime = 60f; // Tiempo predeterminado si no se reconoce la escena
                break;
        }

        currentTime = maxTime;
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;

        if (Keyboard.current.spaceKey.isPressed)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        rb.AddForce(movement * speed);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count = count + 1;
            SetCountText();
        }
    }
    void SetCountText()
    {
        countText.text = SceneManager.GetActiveScene().name + " Count: " + count.ToString();

        if (count >= 15)
        {
            winTextObject.SetActive(true);
            StartCoroutine(ReturnToMainMenu(5f));
        }
    }
    void Update()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            timerText.text = "Time: " + Mathf.Round(currentTime).ToString();
        }
        else if (currentTime <= 0 && !winTextObject.activeSelf)
        {
            timerText.text = "You lose";
            winTextObject.SetActive(false);
            StartCoroutine(ReturnToMainMenu(5f));
        }
        if (Keyboard.current.mKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene("Menu");
        }
    }
    IEnumerator ReturnToMainMenu(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene("Menu");
    }
}




