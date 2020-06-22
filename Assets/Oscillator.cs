using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector = new Vector3(10,0,0);
    [Range(0, 1)] [SerializeField] float movementFactor;
    [SerializeField] float period = 2f;

    Vector3 startPos;
    void Start()
    {
        startPos = transform.position;
    }
    void Update()
    {
        if (period <= Mathf.Epsilon) { return; }
        float cycleInstance = Time.time / period;
        float rawSineInput = Mathf.Sin(cycleInstance * 2 * Mathf.PI);
        movementFactor = rawSineInput / 2f; 
        Vector3 offset = movementVector * movementFactor;
        transform.position = startPos + offset;
    }
}
