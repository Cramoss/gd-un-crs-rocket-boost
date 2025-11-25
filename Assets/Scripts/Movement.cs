using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] InputAction thrust;
    [SerializeField] InputAction rotation;
    [SerializeField] float thrustStrength;
    [SerializeField] float rotationStrength;
    [SerializeField] AudioClip mainEngineClip;
    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem leftEngineParticles;
    [SerializeField] ParticleSystem rightEngineParticles;
    Rigidbody _rigidbody;
    AudioSource _audioSource;

    // Enable InputActions when component becomes active
    void OnEnable()
    {
        thrust.Enable();
        rotation.Enable();
    }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _audioSource =  GetComponent<AudioSource>();
    }

    // FixedUpdate for physics - runs at fixed intervals (default 0.02s)
    void FixedUpdate()
    {
        ProcessThrust();
        ProcessRotation();
    }

    void ProcessThrust()
    {
        if (thrust.IsPressed())
        {
            StartThrusting();
        }
        else
        {
            StopThrusting();
        }
    }
    
    private void StartThrusting()
    {
        // AddRelativeForce uses local space (Vector3.up = local Y-axis)
        // Time.fixedDeltaTime makes movement frame-rate independent
        _rigidbody.AddRelativeForce(Vector3.up * thrustStrength * Time.fixedDeltaTime);

        // Check if already playing to prevent audio restart stuttering.
        // PlayOneShot is for one-shot sounds, not continuous streams.
        if (!_audioSource.isPlaying)
        {
            _audioSource.PlayOneShot(mainEngineClip);
        }

        if (!mainEngineParticles.isPlaying)
        {
            mainEngineParticles.Play();
        }
    }
    
    private void StopThrusting()
    {
        _audioSource.Stop();
        mainEngineParticles.Stop();
    }
    
    void ProcessRotation()
    { 
        // Read input from the rotation action
       float rotationInput = rotation.ReadValue<float>();

       // Input is inverted - negative value rotates right, positive rotates left
       if (rotationInput < 0)
       {
           RotateRight();
       }
       else if (rotationInput > 0)
       {
           RotateLeft();
       }
       else
       {
           StopRotation();
       }
    }
    
    private void RotateRight()
    {
        ApplyRotation(Vector3.right);
        if (!leftEngineParticles.isPlaying)
        {
            rightEngineParticles.Stop();
            leftEngineParticles.Play();
        }
    }

    private void RotateLeft()
    {
        ApplyRotation(Vector3.left);
        if (!rightEngineParticles.isPlaying)
        {
            leftEngineParticles.Stop();
            rightEngineParticles.Play();
        }
    }
    
    private void StopRotation()
    {
        leftEngineParticles.Stop();
        rightEngineParticles.Stop();
    }
    
    private void ApplyRotation(Vector3 rotationDirection)
    {
        // Freeze physics rotation to prevent conflicts with manual rotation
        _rigidbody.freezeRotation = true;
        
        // Rotation calculation: direction * degrees_per_second * Time.fixedDeltaTime
        // Example: 100 * 0.02 = 2 degrees per physics step
        transform.Rotate(rotationDirection * rotationStrength * Time.fixedDeltaTime);
        
        // Re-enable physics rotation
        _rigidbody.freezeRotation = false;
    }
}