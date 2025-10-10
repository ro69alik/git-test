using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Random = System.Random;

public class DiceRoller : MonoBehaviour
{
    [SerializeField] private GameObject dicePrefab;
    [SerializeField] public int dicesAmount;
    [SerializeField] private int minForce;
    [SerializeField] private int maxForce;
    [SerializeField] private int rotateForce;
    [SerializeField] private int minDistance;
    [SerializeField] private int maxDistance;
    [SerializeField] private LayerMask plane;
    
    private bool isCalculated = true;
    public int pointsToDraw;
    public int pointsToWin;
    public UnityEvent needRedraw;
    public UnityEvent redrawWinCondition;
    public int score;
    public GameState gameState;
    
    private Random rng;
    private List<GameObject> dices = new List<GameObject>();
    

    public void Awake()
    {
        rng = new Random();
    }

    public void RollDices()
    {
        RespawnDices();
        foreach (var dice in dices)
            RollDice(dice);
        
        // буффер, чтобы флаг успел обновить состояние
        Invoke(nameof(NotCalculated), 0.2f);
    }
    private void RespawnDices()
    {
        foreach (var d in dices)
                    Destroy(d);
        dices.Clear();
                
        for (var i = 0; i < dicesAmount; i++)
        {
            // Кубики выстраиваются в "табличку" по x и z, шагом в 5
            var dice = Instantiate(dicePrefab, new Vector3(i/5 * 5, 1, i%5 * 5), quaternion.identity);
            dices.Add(dice);
        }
    }
    
    private void RollDice(GameObject dice)
    {
        var force = rng.Next(minForce, maxForce);
        var rb = dice.GetComponent<Rigidbody>();
        var forceDirection = new Vector3(0, rng.Next(minDistance, maxDistance), 0);
        var torqueDirection = new Vector3(rng.Next(-rotateForce, rotateForce + 1), rng.Next(-rotateForce, rotateForce + 1), rng.Next(-rotateForce, rotateForce + 1));
        rb.AddForce(forceDirection * force, ForceMode.Impulse);
        rb.AddTorque(torqueDirection, ForceMode.Impulse);
    }
    
    public void NotCalculated()
    {
        isCalculated = false;
    }
    
    // Количество очков для победы зависит от количества кубиков
    public void RandomizeStateValues()
    {
        pointsToWin = rng.Next(dicesAmount + 2, dicesAmount * 6 + 1);
        pointsToDraw = rng.Next(dicesAmount + 1, pointsToWin);
        redrawWinCondition.Invoke();
    }
    private void Update()
    {
        if (!isCalculated && dices!= null && dices.All(x => IsStop(x)))
        {
            isCalculated = true;
            score = dices.Sum(x => CountScore(x));
            UpdateGameState();
            needRedraw?.Invoke();
        }
    }
    
    private bool IsStop(GameObject dice)
    {
        var rb = dice.GetComponent<Rigidbody>();
        return rb.linearVelocity == Vector3.zero && rb.angularVelocity == Vector3.zero;
    }
    
    private void UpdateGameState()
    {
        if (score >= pointsToWin)
            gameState = GameState.win;
        else if (score >= pointsToDraw)
            gameState = GameState.draw;
        else gameState = GameState.lose;
    }

    // Реализацию подсчёта очков посмотрел у Изатуллина Кирилла
    private int CountScore(GameObject d)
    {
        Vector3[] directions =
        {
            -d.transform.forward,
            -d.transform.up,
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
    
}
