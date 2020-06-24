using UnityEngine;
using UnityEngine.SceneManagement;
public class IronMan : MonoBehaviour
{
    [SerializeField] float thrustfactor = 100f;
    [SerializeField] float rotationfactor = 250f;
    [SerializeField] float lvlLoadDelay = 1f;

    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip explosion;
    [SerializeField] AudioClip lvlUp;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem mainEngineParticles1;
    [SerializeField] ParticleSystem mainEngineParticles2;
    [SerializeField] ParticleSystem explosionParticles;
    [SerializeField] ParticleSystem lvlUpParticles;

    Rigidbody rigidBody;
    AudioSource audioSource;
    
    bool isAlive = true;
    bool collisionsDisabled = false;
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        if (isAlive == true)
        {
            Thrust();
            Rotate();
        }
        if (Debug.isDebugBuild)
        {
            DebugKeys();
        }   
    }

    private void DebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            StartNextLvl();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            collisionsDisabled = !collisionsDisabled;
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
        rigidBody.AddRelativeForce(Vector3.up * thrustfactor * Time.deltaTime);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
            mainEngineParticles.Play();
            mainEngineParticles1.Play();
            mainEngineParticles2.Play();
        }
    }

    private void Rotate()
    {
        rigidBody.angularVelocity = Vector3.zero;

        float rotationframe = rotationfactor * Time.deltaTime;
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationframe);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationframe);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isAlive == false || collisionsDisabled) { return; }

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
        isAlive = false;
        audioSource.Stop();
        audioSource.PlayOneShot(explosion);
        mainEngineParticles.Stop();
        mainEngineParticles1.Stop();
        mainEngineParticles2.Stop();
        explosionParticles.Play();
        Invoke("Respawn", lvlLoadDelay);
    }

    private void StartNextLvl()
    {
        isAlive = false;
        audioSource.Stop();
        audioSource.PlayOneShot(lvlUp);
        mainEngineParticles.Stop();
        mainEngineParticles1.Stop();
        mainEngineParticles2.Stop();
        lvlUpParticles.Play();
        Invoke("LoadNextLvl", lvlLoadDelay);
    }

    private void Respawn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void LoadNextLvl()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        int totalScene = SceneManager.sceneCountInBuildSettings;
        if (currentScene == totalScene -1)
        {
            currentScene = -1;
        }
        SceneManager.LoadScene(currentScene + 1);
    }
}
