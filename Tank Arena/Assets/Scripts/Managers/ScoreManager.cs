using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>
{
    [SerializeField] private int _scoreForKill = 1;

    private Dictionary<int, int> _playerScores = new Dictionary<int, int>();

    public Dictionary<int, int> PlayerScores { get => _playerScores; }

    public void AddTankToScoreBoard(int viewID)
    {
        PlayerScores.Add(viewID, 0);
        if(PlayerScores.Count == PhotonNetwork.CurrentRoom.PlayerCount)
        {
            RoomManager.Instance.PlayersAreReady();
            GameUIManager.Instance.Setup();
        }
    }

    public void AddScoreToTank(PhotonView tankPV, int scoreToAdd)
    {
        PlayerScores[tankPV.ViewID] += scoreToAdd;
        GameUIManager.Instance.UpdateScoreForTank(tankPV.Owner.NickName, PlayerScores[tankPV.ViewID]);
    }

    public string GetWinner()
    {
        int winnerID = RoomManager.Instance.LocalTank.PhotonView.ViewID;
        foreach (KeyValuePair<int, int> entry in PlayerScores)
        {
            if(entry.Value > PlayerScores[winnerID])
            {
                winnerID = entry.Key;
            }
        }

        return PhotonView.Find(winnerID).Owner.NickName;

    }

    public void AddScoreForKill(PhotonView tankPV)
    {
        AddScoreToTank(tankPV, _scoreForKill);
    }
}
