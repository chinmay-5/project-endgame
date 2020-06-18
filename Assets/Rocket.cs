using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] float thrustfactor = 100f;
    [SerializeField] float rotationfactor = 200f;

    Rigidbody rigidBody;
    AudioSource audioSource;
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        Thrust();
        Rotate();
    }
    private void Thrust()
    {
        float thrustframe = thrustfactor * Time.deltaTime;
        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up * thrustframe);
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            audioSource.Stop();
        }
    }
    private void Rotate()
    {
        rigidBody.freezeRotation = true;

        float rotationframe = rotationfactor * Time.deltaTime;
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationframe);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationframe);
        }

        rigidBody.freezeRotation = false;
    }

}
