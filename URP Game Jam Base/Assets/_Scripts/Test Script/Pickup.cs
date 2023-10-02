using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    private Vector3 offset;
    private bool isDragging = false;
    private bool isFalling = false;
    public Rigidbody2D rb;
    public float fallSpeed = 8;
    public Collider2D pickupCollider;
    public Collider2D collisions;

    private Camera _cam;

    private Vector3 leftBottomLimit;
    private Vector3 rightTopLimit;

    bool isEnterPot;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _cam = Camera.main;
        //rb.isKinematic = true; 
        isEnterPot = false;
        isFalling = true;
        leftBottomLimit = _cam.ViewportToWorldPoint(new Vector3(0, 0.1f, 0));
        rightTopLimit = _cam.ViewportToWorldPoint(new Vector3(1, 1, 0));
    }

    void OnMouseDown()
    {
        offset = gameObject.transform.position - GetCamPosition();
        isDragging = true;
        isFalling = false;
        AudioManager.instance.PlayOneShot("Pop");
    }

    void OnMouseUp()
    {
        isDragging = false;
        rb.isKinematic = false; 
        Vector3 mousePos = GetCamPosition();
        Vector3 velocity = (mousePos - transform.position).normalized * 10f; 
        //rb.velocity = new Vector3(velocity.x, velocity.y, 0f);
        rb.velocity = Vector3.zero;
        rb.gravityScale = 1;
        AudioManager.instance.PlayOneShot("Pop");
    }

    void Update()
    {
        if (isDragging)
        {
            Vector3 newPosition = GetCamPosition() + offset;
            transform.position = new Vector3(Mathf.Clamp(newPosition.x, leftBottomLimit.x, rightTopLimit.x), Mathf.Clamp(newPosition.y, leftBottomLimit.y, rightTopLimit.y), transform.position.z);
        }

        if (isFalling) {
            rb.velocity = new Vector3(0f, fallSpeed * -1, 0f);
            rb.gravityScale = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("trig " + collision.gameObject.tag);
        if (collision.gameObject.tag == "detect") {
            Debug.Log("enter");
            isEnterPot = true;
        }
     }

     private void OnTriggerExit2D(Collider2D collision)
    {
    Debug.Log("trig exit " + collision.gameObject.tag);
        if (isEnterPot) {
            if (collision.gameObject.tag == "barrier") {
            Debug.Log("in");
            gameObject.layer = LayerMask.NameToLayer("ball-inside");
            }
        }
        if (collision.gameObject.tag == "detect") {
            Debug.Log("exit");
            isEnterPot = false;
            gameObject.layer = LayerMask.NameToLayer("ball-outside");
        }
    }

    private Vector3 GetCamPosition()
    {
        return Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f));
    }
}

