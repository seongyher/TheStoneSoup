using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CooldownScript : MonoBehaviour
{
    public float timerDuration;
    private float currentTime = 0;

    public Slider cooldownSlider;
    public GameObject cookButton;

    private bool isReady = false;
    // Start is called before the first frame update
    void Start()
    {
        cooldownSlider.maxValue = timerDuration;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTime <= timerDuration)
        {
            currentTime += Time.deltaTime;
            updateSlider();
        } else if(!isReady)
        {
            ReadyToCook();
        }
    }

    public void updateSlider()
    {
        cooldownSlider.value = currentTime;
    }

    public void resetTimer()
    {
        cooldownSlider.gameObject.SetActive(true);
        cookButton.SetActive(false);
        currentTime = 0;
        isReady = false;
    }

    private void ReadyToCook()
    {
        cookButton.SetActive(true);
        cooldownSlider.gameObject.SetActive(false);
        isReady = true;
    }
}
