using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;

public class PickupManager : Singleton<PickupManager>
{
    [SerializeField] private PhotonView _photonView;

    private void Start()
    {
        SetupPickupsSpawnPoints();
        SetupScoreOrbSpawnPoints();
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Starting the ScoreOrb Spawning Coroutine");
            StartCoroutine(PeriodicallySpawnPickups());
            StartCoroutine(PeriodicallySpawnScoreOrbs());
        }
    }

    public void FreeSpawnPoint(int index, Pickup pickup)
    {
        if(pickup is ScoreOrb)
        {
            _scoreOrbSpawnPointsStatus[_scoreOrbSpawnPoints[index]] = true;
            _activeScoreOrbs--;
        }
        else
        {
            _pickupsSpawnPointsStatus[_pickupsSpawnPoints[index]] = true;
            _activePickups--;
        }
    }
        

    #region Pickups
    [SerializeField] private float _pickupsCooldown;
    [SerializeField] private int _activePickups;
    [SerializeField] private int _maxPickups;


    [SerializeField] private GameObject _pickupsSpawnPointsHolder;
    [SerializeField] private List<GameObject> _pickupsPrefabs; // TODO: Change this to List<GO>

    // bool is for avilability of spawnpoint 
    [SerializeField] private Dictionary<Transform, bool> _pickupsSpawnPointsStatus = new Dictionary<Transform, bool>();
    private List<Transform> _pickupsSpawnPoints = new List<Transform>();

   

    private IEnumerator PeriodicallySpawnPickups()
    {
        while(true)
        {
            if(_activePickups < _maxPickups)
            {
                int indexToSpawnAt = AvailablePickupsSpawnPointIndex();

                if(indexToSpawnAt >= 0)
                {
                    int pickupsIndex = ChooseRandomPickupPrefabIndex();
                    _photonView.RPC("RPC_SpawnPickupsAtSpawnPoint", RpcTarget.All, indexToSpawnAt, pickupsIndex);
                }
            }



            yield return new WaitForSeconds(_pickupsCooldown);
        }
    }

    private int ChooseRandomPickupPrefabIndex()
    {
        return new System.Random().Next(_pickupsPrefabs.Count);
    }

    private int AvailablePickupsSpawnPointIndex()
    {
        bool foundPoint = false;
        System.Random rand = new System.Random();
        int index;
        Transform spawnPointTransform;

        List<int> _checkedIndexes = new List<int>();
        while (!foundPoint)
        {
            index = rand.Next(_pickupsSpawnPoints.Count);
            if(_checkedIndexes.Count == _pickupsSpawnPoints.Count)
            {
                return -1;
            }
            Debug.Log("random index: " + index);
            spawnPointTransform = _pickupsSpawnPoints[index];

            _checkedIndexes.Add(index);

            if(_pickupsSpawnPointsStatus[spawnPointTransform] == true)
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
    private void RPC_SpawnPickupsAtSpawnPoint(int spawnPointIndex, int pickupsIndex)
    {
        GameObject scoreOrb = Instantiate(_pickupsPrefabs[pickupsIndex], _pickupsSpawnPoints[spawnPointIndex]);
        scoreOrb.GetComponentInChildren<Pickup>().Setup(spawnPointIndex);
        scoreOrb.transform.parent = null;

        _activePickups++;
        _pickupsSpawnPointsStatus[_pickupsSpawnPoints[spawnPointIndex]] = false;
    }    
    private void SetupPickupsSpawnPoints()
    {
        foreach (Transform child in _pickupsSpawnPointsHolder.transform)
        {
            _pickupsSpawnPointsStatus.Add(child, true);
            _pickupsSpawnPoints.Add(child);
        }
    }


    #endregion

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
    #endregion
}
