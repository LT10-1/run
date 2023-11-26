using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGame : MonoBehaviour
{
    private Player player;

    [SerializeField] private TextMeshProUGUI distanceText;
    [SerializeField] private TextMeshProUGUI coinsText;

    [SerializeField] private Image heartEmpty;
    [SerializeField] private Image heartFull;

    private float distance;
    private float coins;

    void Start()
    {
        player = GameManager.Instance.player;
        InvokeRepeating("UpdateInfo", 0, .15f);
    }

    // Update is called once per frame
    private void UpdateInfo()
    {
        distance = GameManager.Instance.distance;
        coins = GameManager.Instance.coins;

        if(distance > 0)
                distanceText.text = distance.ToString("#,#") + "  m";

        if(coins > 0)
                coinsText.text = GameManager.Instance.coins.ToString("#,#");

        heartEmpty.enabled = !player.extraLife;
        heartFull.enabled = player.extraLife;
    }
}
