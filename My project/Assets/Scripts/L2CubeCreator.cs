using System;
using UnityEngine;

public class L2CubeCreator : MonoBehaviour
{
    [SerializeField] private GameObject cubeToCopy;
    [SerializeField] private Transform cubeSpawnPosition;
    [SerializeField] private int cubesAmount;

    private void Awake()
    {
        for (var i = 0; i < cubesAmount; i++)
        {
            Instantiate(cubeToCopy, cubeSpawnPosition);
        }
    }
}