using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public GameObject boardpartsPrefab;

    public int gridSize = 4;

    void Start()
    {
        for (int i = 0; i < gridSize; i++){
            for (int j = 0; j < gridSize; j++){
                Vector3 spawnPos = new Vector3(j, 0, i);
                GameObject boardparts = Instantiate(boardpartsPrefab, spawnPos, Quaternion.identity, transform);
                boardparts.tag = "BoardParts";
            }
        }        
    }
}
