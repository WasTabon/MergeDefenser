using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Merge : MonoBehaviour
{
    public static Merge instance { get; private set; }

    [SerializeField] private GameObject[] _units;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        
        Destroy(gameObject);
    }

    public bool Connect(GameObject unitToDrag, GameObject unitToMerge)
    {
        MergeUnit unitOne = unitToDrag.GetComponent<MergeUnit>();
        MergeUnit unitTwo = unitToMerge.GetComponent<MergeUnit>();

        if (unitOne.Rank == unitTwo.Rank)
        {
            int rank = unitOne.Rank + 1;
            int randomUnit = Random.Range(0, _units.Length);

            Vector3 spawnPos = unitToMerge.transform.position;
            Destroy(unitToDrag.gameObject);
            Destroy(unitToMerge.gameObject);

            GameObject unitToSpawn = Instantiate(_units[randomUnit], spawnPos, Quaternion.identity);
            unitToSpawn.GetComponent<MergeUnit>().Rank = rank;

            return true;
        }
        else
        {
            return false;
        }
    }
}
