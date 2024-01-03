using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Merge : MonoBehaviour
{
    public static Merge instance { get; private set; }

    [SerializeField] private GameObject[] _units;

    [SerializeField] private Transform[] _tilesKey;
    [SerializeField] private bool[] _tilesValue;

    [SerializeField] private GameObject _mergeParticles;

    private List<Transform> _freeTiles = new List<Transform>();

    private void Awake()
    {
        SetSingleton();
    }

    public bool Connect(GameObject unitToDrag, GameObject unitToMerge)
    {
        MergeUnit unitOne = unitToDrag.GetComponent<MergeUnit>();
        MergeUnit unitTwo = unitToMerge.GetComponent<MergeUnit>();
        
        if (unitOne.Rank == unitTwo.Rank)
        {
            int rank = unitOne.Rank + 1;
            int randomUnit = RandomNumberInArray(_units.Length);

            Vector3 spawnPos = unitToMerge.transform.position;

            _freeTiles.Clear();
            SetTileKeyValue(unitOne.GetTile(), false);
            SetTileKeyValue(unitTwo.GetTile(), false);

            Destroy(unitToDrag.gameObject);
            Destroy(unitToMerge.gameObject);

            GameObject unitToSpawn = Instantiate(_units[randomUnit], spawnPos, Quaternion.identity);
            Instantiate(_mergeParticles, spawnPos, Quaternion.identity);
            unitToSpawn.GetComponent<MergeUnit>().Rank = rank;
            unitToSpawn.GetComponent<MergeUnit>().SetTile(unitTwo.GetTile());
            SetTileKeyValue(unitToSpawn.GetComponent<MergeUnit>().GetTile(), true);

            return true;
        }
        else
        {
            return false;
        }
    }

    public void Spawn()
    {
        GameObject randomUnit = _units[RandomNumberInArray(_units.Length)];

        _freeTiles.Clear();

        CheckFreeTiles();
        
        CreateRandomUnit(randomUnit);
    }

    private void SetSingleton()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        
        Destroy(gameObject);
    }

    private void CheckFreeTiles()
    {
        for (int i = 0; i < _tilesKey.Length; i++)
        {
            if (!_tilesValue[i])
            {
                _freeTiles.Add(_tilesKey[i]);
            }
        }
    }

    private void CreateRandomUnit(GameObject randomUnit)
    {
        Transform randomTile;
        if (_freeTiles.Count > 0)
        {
            randomTile = _freeTiles[RandomNumberInArray(_freeTiles.Count)];

            SetTileKeyValue(randomTile, true);
            
            GameObject spawnedUnit = Instantiate(randomUnit, randomTile.position, Quaternion.identity);
            spawnedUnit.GetComponent<MergeUnit>().SetTile(randomTile);
        }
    }
    
    private void SetTileKeyValue(Transform randomTile, bool value)
    {
        int index = Array.IndexOf(_tilesKey, randomTile);
        if (index != -1 && index < _tilesValue.Length)
        {
            _tilesValue[index] = value;
        }
    }
    
    private int RandomNumberInArray(int maxNumber)
    {
        return Random.Range(0, maxNumber);
    }
}
