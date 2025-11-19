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
        _rigidbody.AddRelativeForce(Vector3.up * thrustStrength * Time.fixedDeltaTime);

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
       float rotationInput = rotation.ReadValue<float>();

       // For some reason the inputs are reverted, maybe because of the X axis origin which is
       // pointing to the left arrow by default.
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
        _rigidbody.freezeRotation = true;
        transform.Rotate(rotationDirection * rotationStrength * Time.fixedDeltaTime);
        _rigidbody.freezeRotation = false;
    }
}