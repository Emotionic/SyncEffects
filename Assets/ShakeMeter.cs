using System;
using UnityEngine;
using UnityEngine.UI;

public class ShakeMeter : MonoBehaviour
{

    public float Value;

    // Use this for initialization
    private void Start()
    {
        Value = 0.0f;
    }

    // Update is called once per frame
    private void Update()
    {
        Value = Math.Max(0.0f, Value);
        Value = Math.Min(1.0f, Value);

        this.GetComponent<Slider>().value = Value;

    }
}
