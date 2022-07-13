using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class WaitingRoomManager : MonoBehaviourPunCallbacks
{
    [SerializeField] PlayerListItem _playerListItem;
    [SerializeField] Transform _playerListContent;

    [SerializeField] int _amountPlayersToStart = 3;

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);

        Instantiate(_playerListItem, _playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);

        if (PhotonNetwork.CurrentRoom.PlayerCount == _amountPlayersToStart)
        {
            StartGame();
        }
    }

    public void PopulateWaitingPlayerList()
    {
        Player[] _players = PhotonNetwork.PlayerList;

        for (int i = 0; i < _players.Length; i++)
        {
            Instantiate(_playerListItem, _playerListContent).GetComponent<PlayerListItem>().SetUp(_players[i]);
        }
    }

    public void ClearWaitingPlayersList()
    {
        foreach (Transform child in _playerListContent)
        {
            Destroy(child.gameObject);
        }
    }

    public void StartGame()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            if(PhotonNetwork.CurrentRoom.PlayerCount == _amountPlayersToStart)
            {
                //load the game level for everyone
                PhotonNetwork.LoadLevel(1);
            }
        }
    }
}
