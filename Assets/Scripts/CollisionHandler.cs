using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 2f;
    [SerializeField] AudioClip crashSFX;
    [SerializeField] AudioClip successSFX;
    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem crashParticles;
    
    AudioSource _audioSource;

    bool isControllable = true;
    bool isCollidable = true; // Debug flag for collision toggle

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        RespondToDebugKeys();
    }
    
    // OnCollisionEnter is Unity's collision callback
    private void OnCollisionEnter(Collision other)
    {
        // Prevent multiple collision triggers
        if (!isControllable || !isCollidable)
        {
            return;
        }

        // Handle collisions based on object tags
        switch (other.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            case "Fuel":
                break;
            default:
                StartCrashSequence();
                break;
        }
    }

    private void StartSuccessSequence()
    {
        _audioSource.Stop();
        successParticles.Play();
        isControllable = false;
       _audioSource.PlayOneShot(successSFX); 
        GetComponent<Movement>().enabled = false;
        
        // Invoke delays method execution without blocking
        Invoke("LoadNextLevel", levelLoadDelay);
    }

    void StartCrashSequence()
    {
        _audioSource.Stop();
        crashParticles.Play();
        isControllable = false;
        _audioSource.PlayOneShot(crashSFX); 
        GetComponent<Movement>().enabled = false;
        Invoke("ReloadLevel", levelLoadDelay);
    }

    void ReloadLevel()
    {
        // buildIndex is the scene number in Build Settings
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene, LoadSceneMode.Single);
    }

    void LoadNextLevel()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        // Loop back to first level if we've completed all scenes
        if (nextSceneIndex >= SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        
        SceneManager.LoadScene(nextSceneIndex, LoadSceneMode.Single);
    }

    // Debug keys: L = skip level, C = toggle collisions
    void RespondToDebugKeys()
    {
        if (Keyboard.current.lKey.wasPressedThisFrame)
        {
            LoadNextLevel();
        }
        else if (Keyboard.current.cKey.wasPressedThisFrame)
        {
            isCollidable = !isCollidable;
        }
    }
}
