using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameUIManager : Singleton<GameUIManager>
{
    [SerializeField] private TMP_Text _timerText;

    [SerializeField] private List<GameObject> _tankScorePanel;
    [SerializeField] private List<TMP_Text> _tankNicknames;
    [SerializeField] private List<TMP_Text> _tankScores;
    [SerializeField] public RespawnPopup RespawnPopup;
    [SerializeField] public ReloadingPanel ReloadingPanel;

    public void Setup()
    { 

        Dictionary<int, int> playerScores = ScoreManager.Instance.PlayerScores;
        List<string> nickNames = new List<string>();

        foreach (KeyValuePair<int, int> entry in playerScores)
        {
            PhotonView pv = PhotonView.Find(entry.Key);
            string nickName = pv.Owner.NickName;
            nickNames.Add(nickName);
        }

        for (int i  = 0; i  < playerScores.Count; i ++)
        {
            _tankNicknames[i].text = nickNames[i];
            _tankScores[i].text = 0.ToString();
            _tankScorePanel[i].SetActive(true);
        }
    }

    public void UpdateScoreForTank(string nickName, int score)
    {
        for (int i = 0; i < _tankNicknames.Count; i++)
        {
            if(_tankNicknames[i].text == nickName)
            {
                _tankScores[i].text = score.ToString();
            }
        }
    }
}
