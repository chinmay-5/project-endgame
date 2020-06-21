using UnityEngine;
using UnityEngine.SceneManagement;
public class Rocket : MonoBehaviour
{
    [SerializeField] float thrustfactor = 100f;
    [SerializeField] float rotationfactor = 250f;

    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip explosion;
    [SerializeField] AudioClip lvlUp;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem explosionParticles;
    [SerializeField] ParticleSystem lvlUpParticles;

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
            ThrustForce();
        }
        else
        {
            audioSource.Stop();
            mainEngineParticles.Stop();
        }
    }

    private void ThrustForce()
    {
        float thrustframe = thrustfactor * Time.deltaTime;
        rigidBody.AddRelativeForce(Vector3.up * thrustframe);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
            mainEngineParticles.Play();
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
        if (state != State.alive) { return; }

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                StartNextLvl();
                break;
            default:
                StartRespawn();
                break;
        }
    }

    private void StartRespawn()
    {
        state = State.dead;
        audioSource.Stop();
        audioSource.PlayOneShot(explosion);
        mainEngineParticles.Stop();
        explosionParticles.Play();
        Invoke("Respawn", 1f);
    }

    private void StartNextLvl()
    {
        state = State.trans;
        audioSource.Stop();
        audioSource.PlayOneShot(lvlUp);
        mainEngineParticles.Stop();
        lvlUpParticles.Play();
        Invoke("LoadNextLvl", 1f);
    }

    private void Respawn()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNextLvl()
    {
        SceneManager.LoadScene(1);
    }
}
