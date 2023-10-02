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

    bool isEnterPot;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //rb.isKinematic = true; 
        isEnterPot = false;
        isFalling = true;
    }

    void OnMouseDown()
    {
        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f));
        isDragging = true;
        isFalling = false;
    }

    void OnMouseUp()
    {
        isDragging = false;
        rb.isKinematic = false; 
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f));
        Vector3 velocity = (mousePos - transform.position).normalized * 10f; 
        //rb.velocity = new Vector3(velocity.x, velocity.y, 0f);
        rb.velocity = Vector3.zero;
        rb.gravityScale = 1;
    }

    void Update()
    {
        if (isDragging)
        {
            Vector3 newPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f)) + offset;
            transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
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
    }

}

