using System;
using UnityEngine;

// Class for moving object between two positions
public class Oscillator : MonoBehaviour
{
    [SerializeField] private Vector3 movementVector;
    [SerializeField] private float speed;
    
    private Vector3 startPosition;
    private Vector3 endPosition;
    private float movementFactor;
    
    void Start()
    {
        startPosition = transform.position;
        endPosition = startPosition + movementVector;
    }

    void Update()
    {
        // PingPong creates oscillation between 0 and 1
        // Time.time * speed controls oscillation speed
        movementFactor = Mathf.PingPong(Time.time * speed, 1f);
        
        // Lerp smoothly interpolates between two positions
        // movementFactor (0 to 1) determines blend ratio
        transform.position = Vector3.Lerp(startPosition, endPosition, movementFactor);
    }
}
