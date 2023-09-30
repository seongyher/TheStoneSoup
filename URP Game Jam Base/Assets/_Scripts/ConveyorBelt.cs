using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConveyorBelt : MonoBehaviour
{
    RectTransform rt;
    Vector3 movement;


    private void Awake()
    {
        rt = GetComponent<RectTransform>();
        movement = new Vector3(-200, 0, 0);
    }

    private void Update()
    {
        rt.position += movement * Time.deltaTime;
    }


    public void Click()
    {
        Debug.Log("Clicked!");
    }

    public void Release()
    {
        Debug.Log("Released!");
    }
}
