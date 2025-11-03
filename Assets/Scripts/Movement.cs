using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] InputAction thrust;
    [SerializeField] InputAction rotation;
    [SerializeField] float thrustStrength;
    [SerializeField] float rotationStrength;
    Rigidbody _rigidbody;

    void OnEnable()
    {
        thrust.Enable();
        rotation.Enable();
    }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
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
            _rigidbody.AddRelativeForce(Vector3.up * thrustStrength * Time.fixedDeltaTime);
        }
    }

    void ProcessRotation()
    {
       float rotationInput = rotation.ReadValue<float>();

       // For some reason the inputs are reverted, maybe because of the X axis origin which is
       // pointing to the left arrow by default.
       if (rotationInput < 0)
       {
           ApplyRotation(Vector3.right);
       }
       
       if (rotationInput > 0)
       {
           ApplyRotation(Vector3.left);
       }
    }

    private void ApplyRotation(Vector3 rotationDirection)
    {
        transform.Rotate(rotationDirection * rotationStrength * Time.fixedDeltaTime);
    }
}