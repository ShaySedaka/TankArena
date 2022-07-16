using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;

public class ScoreOrbManager : Singleton<ScoreOrbManager>
{
    [SerializeField] private PhotonView _photonView;

    private void Start()
    {
        SetupScoreOrbSpawnPoints();
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Starting the ScoreOrb Spawning Coroutine");
            StartCoroutine(PeriodicallySpawnScoreOrbs());
        }
    }

    #region Score Orb
    [SerializeField] private float _scoreOrbsCooldown;
    [SerializeField] private int _activeScoreOrbs;
    [SerializeField] private int _maxScoreOrbs;


    [SerializeField] private GameObject _scoreOrbSpawnPointsHolder;
    [SerializeField] private GameObject _scoreOrbPrefab;

    // bool is for avilability of spawnpoint 
    [SerializeField] private Dictionary<Transform, bool> _scoreOrbSpawnPointsStatus = new Dictionary<Transform, bool>();
    private List<Transform> _scoreOrbSpawnPoints = new List<Transform>();



    private IEnumerator PeriodicallySpawnScoreOrbs()
    {
        while (true)
        {
            if (_activeScoreOrbs < _maxScoreOrbs)
            {
                int indexToSpawnAt = AvailableScoreOrbSpawnPointIndex();

                if (indexToSpawnAt >= 0)
                {
                    _photonView.RPC("RPC_SpawnScoreOrbAtSpawnPoint", RpcTarget.All, indexToSpawnAt);
                    Debug.Log("Spawning Score Orb");
                }
            }



            yield return new WaitForSeconds(_scoreOrbsCooldown);
        }
    }

    private int AvailableScoreOrbSpawnPointIndex()
    {
        bool foundPoint = false;
        System.Random rand = new System.Random();
        int index;
        Transform spawnPointTransform;

        List<int> _checkedIndexes = new List<int>();
        while (!foundPoint)
        {
            index = rand.Next(_scoreOrbSpawnPoints.Count);
            if (_checkedIndexes.Count == _scoreOrbSpawnPoints.Count)
            {
                return -1;
            }
            Debug.Log("random index: " + index);
            spawnPointTransform = _scoreOrbSpawnPoints[index];

            _checkedIndexes.Add(index);

            if (_scoreOrbSpawnPointsStatus[spawnPointTransform] == true)
            {
                foundPoint = true;
                Debug.Log("Available Spawn Point Index: " + index);
                return index;
            }
        }

        Debug.Log("Available Spawn Point Index NOT FOUND");
        return -1;
    }

    [PunRPC]
    private void RPC_SpawnScoreOrbAtSpawnPoint(int spawnPointIndex)
    {
        GameObject scoreOrb = Instantiate(_scoreOrbPrefab, _scoreOrbSpawnPoints[spawnPointIndex]);
        scoreOrb.GetComponentInChildren<Pickup>().Setup(spawnPointIndex);
        scoreOrb.transform.parent = null;

        _activeScoreOrbs++;
        _scoreOrbSpawnPointsStatus[_scoreOrbSpawnPoints[spawnPointIndex]] = false;
    }
    private void SetupScoreOrbSpawnPoints()
    {
        foreach (Transform child in _scoreOrbSpawnPointsHolder.transform)
        {
            _scoreOrbSpawnPointsStatus.Add(child, true);
            _scoreOrbSpawnPoints.Add(child);
        }
    }

    public void FreeSpawnPoint(int index)
    {
        _scoreOrbSpawnPointsStatus[_scoreOrbSpawnPoints[index]] = true;
        _activeScoreOrbs--;
    }
    #endregion
}
