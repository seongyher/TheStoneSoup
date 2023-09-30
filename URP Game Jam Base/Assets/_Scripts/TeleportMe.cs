using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportMe : MonoBehaviour
{
    Transform teleportReceiver;


    private void Awake()
    {
        teleportReceiver = GameObject.FindWithTag("TeleportReceiver").transform;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Teleporter")
        {
            //transform.SetAsLastSibling();
            //Instantiate(clone, transform.parent.transform);
            transform.position = teleportReceiver.position;
        }
    }
}
