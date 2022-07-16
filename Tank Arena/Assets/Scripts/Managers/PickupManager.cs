using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;

public class PickupManager : Singleton<PickupManager>
{
    [SerializeField] private float _scoreOrbsCooldown;
    [SerializeField] private int _activeScoreOrbs;
    [SerializeField] private int _maxScoreOrbs;

    // bool is for avilability of spawnpoint
    [SerializeField] private Dictionary<Transform, bool> _spawnPoints = new Dictionary<Transform, bool>();

    [SerializeField] private GameObject _scoreOrbSpawnPointsHolder;
    [SerializeField] private GameObject _scorePrefab;
    private List<Transform> _sp = new List<Transform>();


    [SerializeField] private PhotonView _photonView;


    private void Start()
    {
        SetupSpawnPoints();
        if(PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Starting the ScoreOrb Spawning Coroutine");
            StartCoroutine(SpawnScoreOrbs());
        }
    }

    private IEnumerator SpawnScoreOrbs()
    {
        while(true)
        {
            if(_activeScoreOrbs < _maxScoreOrbs)
            {
                int indexToSpawnAt = AvailableSpawnPointIndex();

                if(indexToSpawnAt >= 0)
                {
                    _photonView.RPC("RPC_SpawnScorePrefabAtSpawnPoint", RpcTarget.All, indexToSpawnAt);
                    Debug.Log("Spawning Score Orb");
                }
            }



            yield return new WaitForSeconds(_scoreOrbsCooldown);
        }
    }


    private int AvailableSpawnPointIndex()
    {
        bool foundPoint = false;
        System.Random rand = new System.Random();
        int index;
        Transform spawnPointTransform;

        List<int> _checkedIndexes = new List<int>();
        while (!foundPoint)
        {
            index = rand.Next(_sp.Count);
            if(_checkedIndexes.Count == _sp.Count)
            {
                return -1;
            }
            Debug.Log("random index: " + index);
            spawnPointTransform = _sp[index];

            _checkedIndexes.Add(index);

            if(_spawnPoints[spawnPointTransform] == true)
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
    private void RPC_SpawnScorePrefabAtSpawnPoint(int spawnPointIndex)
    {
        GameObject scoreOrb = Instantiate(_scorePrefab, _sp[spawnPointIndex]);
        scoreOrb.GetComponentInChildren<Pickup>().Setup(spawnPointIndex);
        scoreOrb.transform.parent = null;

        _activeScoreOrbs++;
        _spawnPoints[_sp[spawnPointIndex]] = false;
    }    
    private void SetupSpawnPoints()
    {
        foreach (Transform child in _scoreOrbSpawnPointsHolder.transform)
        {
            _spawnPoints.Add(child, true);
            _sp.Add(child);
        }
    }

    public void FreeSpawnPoint(int index)
    {
        _spawnPoints[_sp[index]] = true;
        _activeScoreOrbs--;
    }
}
