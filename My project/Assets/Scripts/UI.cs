using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField] private DiceRoller diceRoller;
    [SerializeField] private Button rerollDiceButton;
    [SerializeField] private Button randomizeStateValues;
    [SerializeField] private TMP_InputField dicesAmount;
    [SerializeField] private TMP_Text pointsToWin;
    [SerializeField] private TMP_Text pointsToDraw;
    [SerializeField] private TMP_Text pointsToLose;
    [SerializeField] private TMP_Text currentPoints;
    [SerializeField] private TMP_Text gameState;
    
    void Awake()
    {
        dicesAmount.contentType = TMP_InputField.ContentType.IntegerNumber;
        dicesAmount.onEndEdit.AddListener(OnDiceValuesChange);
        
        randomizeStateValues.onClick.AddListener(OnRandomizeStateValuesClick);
        rerollDiceButton.onClick.AddListener(OnRerollButtonClick);
        diceRoller.needRedraw.AddListener(Draw);
        diceRoller.redrawWinCondition.AddListener(DrawWinCon);
        
    }
    private void OnDiceValuesChange(string arg0)
    {
        diceRoller.dicesAmount = Convert.ToInt32(arg0);
    }

    private void OnRandomizeStateValuesClick()
    {
        diceRoller.RandomizeStateValues();
    }

    public void OnRollAllDices()
    {
        diceRoller.RollDices();
    }
    
    private void OnRerollButtonClick()
    {
        diceRoller.RollDices();
    }

    private void Draw()
    {
        currentPoints.text = "Очки: " + diceRoller.score;
        switch (diceRoller.gameState)
        {
            case GameState.win:
                gameState.text = "чиназес!";
                gameState.color = Color.red;
                break;
            case GameState.draw:
                gameState.text = "нормик";
                gameState.color = Color.yellow;
                break;
            case GameState.lose:
                gameState.text = "наср(";
                gameState.color = Color.green;
                break;
        }
    }
    private void DrawWinCon()
    {
        pointsToWin.text = "Очков для победы:\nот " + diceRoller.pointsToWin + " и больше";
        pointsToDraw.text = "Очков для ничьи:\nот " + diceRoller.pointsToDraw + " до " + (diceRoller.pointsToWin - 1);
        pointsToLose.text = "Очков для поражения:\n" + (diceRoller.pointsToDraw - 1) + " и меньше";
    }
}
