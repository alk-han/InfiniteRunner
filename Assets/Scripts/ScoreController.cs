using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    public event Action<int> OnScoreChanged;
    private int score;


    public void ChangeScore(int amount)
    {
        score += amount;
        if (score < 0)
        {
            score = 0;
        }
        OnScoreChanged?.Invoke(score);
    }
}
