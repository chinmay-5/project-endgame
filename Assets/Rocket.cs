using UnityEngine;
using UnityEngine.SceneManagement;
public class Rocket : MonoBehaviour
{
    [SerializeField] float thrustfactor = 100f;
    [SerializeField] float rotationfactor = 250f;

    Rigidbody rigidBody;
    AudioSource audioSource;
    enum State {alive, dead, trans};
    State state = State.alive;
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {   
        if (state == State.alive)
        {
            Thrust();
            Rotate();
        }
    }
    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up * thrustfactor);
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
    
    void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                state = State.trans;
                Invoke("LoadNextLvl",1f);
                break;
            default:
                state = State.dead;
                Invoke("Respawn",1.5f);
                break;
        }
    }

    private void Respawn()
    {
        SceneManager.LoadScene(0);
        state = State.alive;
    }

    private void LoadNextLvl()
    {
        SceneManager.LoadScene(1);
        state = State.alive;
    }
}
