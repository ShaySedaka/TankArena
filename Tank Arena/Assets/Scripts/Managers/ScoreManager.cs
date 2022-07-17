using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>
{
    private Dictionary<int, int> _playerScores = new Dictionary<int, int>();

    public void AddTankToScoreBoard(int viewID)
    {
        _playerScores.Add(viewID, 0);
    }

    public void AddScoreToTank(int viewID, int scoreToAdd)
    {
        _playerScores[viewID] += scoreToAdd;
    }
}
