using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    [SerializeField] GameObject _MainMenuView;
    [SerializeField] GameObject _WaitingRoomView;

    [SerializeField] TextMeshProUGUI _connectionStatusText;
    [SerializeField] GameObject _playButton;

    public void OnConnected()
    {
        _connectionStatusText.text = "CONNECTED";
        _connectionStatusText.color = Color.green;

        //show play button on connected to server
        _playButton.SetActive(true);
    }

    public void OnConnecting()
    {
        _connectionStatusText.text = "CONNECTING...";
    }

    public void OnConnectionError()
    {
        _connectionStatusText.text = "DISCONNECTED";
        _connectionStatusText.color = Color.red;
    }

    public void TurnOnMainMenu()
    {
        _WaitingRoomView.SetActive(false);
        _MainMenuView.SetActive(true);
    }

    public void TurnOnWaitingRoomView()
    {
        _MainMenuView.SetActive(false);
        _WaitingRoomView.SetActive(true);
    }
}
