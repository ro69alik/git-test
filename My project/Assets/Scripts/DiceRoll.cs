using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField] private GameObject dicePrefab;
    [SerializeField] private List<GameObject> dices;
    [SerializeField] private PlayersControl rollKey;
    [SerializeField] private int minForce;
    [SerializeField] private int maxForce;
    [SerializeField] private int minDistance;
    [SerializeField] private int maxDistance;
    [SerializeField] private int score;
    [SerializeField] private LayerMask plane;
    [SerializeField] private bool isCalculated = true;
 
    void Awake()
    {
        dices = new List<GameObject>();
        score = 0;
        
        for (var i = 0; i < 5; i++)
        {
            var dice = Instantiate(dicePrefab, new Vector3(0, 1, i * 5), quaternion.identity);
            dices.Add(dice);
        }
        
        rollKey = new PlayersControl();
        rollKey.DiceControl.RollAllDices.performed += ctx => RollDices(dices);
    }

    public void RollDices(List<GameObject> dices)
    {
        var rng = new Random();
        foreach (var dice in dices)
        {
            var force = rng.Next(minForce, maxForce);
            var rb = dice.GetComponent<Rigidbody>();
            var direction = new Vector3(rng.Next(minDistance, maxDistance), rng.Next(minDistance, maxDistance),
                rng.Next(minDistance, maxDistance));
            rb.AddForce(direction * force, ForceMode.Impulse);
        }
    }

    public bool IsStop(GameObject dice)
    {
        return dice.GetComponent<Rigidbody>().linearVelocity == Vector3.zero;
    }

    private void Update()
    {
        if (dices.All(x => IsStop(x) && !isCalculated))
        {
            isCalculated = true;
            foreach (var dice in dices)
                score += CountScore(dice);
        }
    }
    
    // Реализацию подсчёта очков посмотрел у Изатуллина Кирилла
    public int CountScore(GameObject d)
    {
        Vector3[] directions =
        {
            -d.transform.forward,
            - d.transform.up,
            d.transform.right,
            -d.transform.right,
            d.transform.up,
            d.transform.forward
        };

        for (int i = 0; i < directions.Length; i++)
        {
            if (Physics.Raycast(d.transform.position, directions[i], 1, plane))
                return i + 1;
        }
        return 0;
    }
    
    void OnEnable()
    {
        rollKey.Enable();
    }
}
