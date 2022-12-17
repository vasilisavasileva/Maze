using UnityEngine;
using TMPro;
using System;

public class PillsCollector : MonoBehaviour
{
    private int counter = 0;
    public TextMeshProUGUI counterText;
    void Start()
    {
        gameObject.tag = "Player";
        counterText.text = $"Pills: 0";
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PickUpPills")
        {
            //Debug.Log("Triggered");
            other.gameObject.SetActive(false);
            counter++;
            counterText.text = $"Pills: {counter}";
        }
    }
}