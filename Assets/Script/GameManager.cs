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
        //LoadColor();
    }

    public void SaveColor(float r, float g, float b)
    {

        PlayerPrefs.SetFloat("ColorR", r);
        PlayerPrefs.SetFloat("ColorG", g);
        PlayerPrefs.SetFloat("ColorB", b);
    }

    private void LoadColor()
    {
        
        SpriteRenderer sr = player.GetComponent<SpriteRenderer>();

        Color newColor = new Color(PlayerPrefs.GetFloat("ColorR"),
                                   PlayerPrefs.GetFloat("ColorG"),
                                   PlayerPrefs.GetFloat("ColorB"),
                                   PlayerPrefs.GetFloat("ColorA", 1));
        sr.color = newColor;
    }

    private void Update()
    {
         if(player.transform.position.x > distance)
        {
            distance = player.transform.position.x;
        }
    }

    public void UnlockPlayer() => player.playerUnlocked = true;

    public void RestartLevel()
    {
        SaveInfo();
        SceneManager.LoadScene(0);
    }
    public void SaveInfo()
    {

        int savedCoins = PlayerPrefs.GetInt("Coins");

        PlayerPrefs.SetInt("Coins", savedCoins + coins);

        float Score = distance * coins;
        PlayerPrefs.SetFloat("LastScore", Score);

        if (PlayerPrefs.GetFloat("HighScore") < Score)
            PlayerPrefs.SetFloat("HighScore", Score);
        
    }
}
