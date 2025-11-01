using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] InputAction thrust;
    Rigidbody _rigidbody;

    void OnEnable()
    {
        thrust.Enable();
    }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (thrust.IsPressed())
        {
            Debug.Log("WICKED");
        }
    }
}