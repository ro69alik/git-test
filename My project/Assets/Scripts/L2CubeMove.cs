using System;
using UnityEngine;
using UnityEngine.Serialization;

public class L2CubeMove : MonoBehaviour
{
    [SerializeField] private GameObject center;
    [SerializeField] private GameObject cubeToCopy;
    [SerializeField] private int cubesAmount;
    [SerializeField] private float cubeDistance;
    [SerializeField] private float radius;
    [SerializeField] private float rotationSpeed;
    
    private void Awake()
    {
        for (var i = 0; i < cubesAmount; i++)
        {
            var cubePos = new Vector3(MathF.Cos(cubeDistance * i) * radius, 0, MathF.Sin(cubeDistance * i) * radius);
            Instantiate(cubeToCopy, cubePos, Quaternion.identity, center.GetComponent<Transform>());
        }
    }
    private void Update()
    {
        center.transform.Rotate(new Vector3(0, rotationSpeed, 0));
    }
}
