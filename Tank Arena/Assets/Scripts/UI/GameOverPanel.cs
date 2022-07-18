using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text _winnerNickname;
    [SerializeField] private TMP_Text _objectiveText;

    public void Show(string nickname)
    {
        _objectiveText.gameObject.SetActive(false);
        _winnerNickname.text = nickname;
        gameObject.SetActive(true);
    }
}
