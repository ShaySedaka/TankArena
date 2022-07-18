using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Timer : MonoBehaviour
{
    [SerializeField] private float _timeRemaining = 10;
    private bool _timerIsRunning = false;

    void Update()
    {
        if (_timerIsRunning)
        {
            if (_timeRemaining > 0)
            {
                _timeRemaining -= Time.deltaTime;
            }
            else
            {
                _timeRemaining = 0;
                _timerIsRunning = false;
                EndMatch();
            }
        }
        GameUIManager.Instance.TimerToString(_timeRemaining);
    }

    public void StartTimer()
    {
        _timerIsRunning = true;
    }

    private void EndMatch()
    {
        RoomManager.Instance.LocalTank.InputHandler.enabled = false;
        GameUIManager.Instance.GameOverPanel.Show(ScoreManager.Instance.GetWinner());
    }
}