using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Player player;
    public static GameManager Instance;
    [Header("Color Info")]
    public Color platformColor;
    public Color playerColor = Color.white;

    [Header("Score info")]
    public int coins;
    public float distance;

    private void Awake()
    {
        Instance = this;

    }
    private void Start()
    {
        PlayerPrefs.GetInt("Coins");
    }

    private void Update()
    {
         if(player.transform.position.x > distance)
        {
            distance = player.transform.position.x;
        }
    }

    public void UnlockPlayer() => player.playerUnlocked = true;

    public void RestartLevel() => SceneManager.LoadScene(0);
    public void Save()
    {
        PlayerPrefs.SetInt("TotalAmountOfCoins", coins);
    }
}
