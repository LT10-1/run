using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class CoinGenerator : MonoBehaviour
{
    [SerializeField] private int amountOfCoins;
    [SerializeField] private GameObject coinPrefab;
    
    [SerializeField] private float chanceToSpawn;

    void Start()
    {
        //amountOfCoins = Random.Range(mincoins, maxcoins);
        int additionalOffset = amountOfCoins / 2;

        for (int i = 0; i < amountOfCoins; i++)
        {
            bool canSpawn = chanceToSpawn > Random.Range(0,100);
            Vector3 offset = new Vector2(i - additionalOffset, 0);

            if (canSpawn)
            {
                Instantiate(coinPrefab, transform.position + offset, Quaternion.identity, transform);
            }

        }
    }

    
}
