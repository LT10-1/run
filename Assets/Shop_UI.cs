using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct ColorToSell
{
    public Color color;
    public int price;
}

public class Shop_UI : MonoBehaviour
{
    [SerializeField] private GameObject platformColorButton;
    [SerializeField] private Transform platformColorParent;
    [SerializeField] private Image platformDisplay;

    [SerializeField] private ColorToSell[] platformColors;
    void Start()
    {
        

        for (int i = 0; i < platformColors.Length; i++)
        {
            Color color = platformColors[i].color;
            int price = platformColors[i].price;

            GameObject newButton = Instantiate(platformColorButton, platformColorParent);

            newButton.transform.GetChild(0).GetComponent<Image>().color = color;
            newButton.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = price.ToString("#,#");

            newButton.GetComponent<Button>().onClick.AddListener(() => PurchaseColor(color, price));
        }
    }

    // Update is called once per frame
    public void PurchaseColor(Color color, int price)
    {
        if(EnoughMoney(price))
        {
            GameManager.Instance.platformColor = color;
            platformDisplay.color = color;
        }
    }

    private bool EnoughMoney(int price)
    {
        int myCoins = PlayerPrefs.GetInt("Coins");
        
        if(myCoins > price)
        {
            int newAmountOfCoins = myCoins - price;
            PlayerPrefs.SetInt("Coins", newAmountOfCoins);
            
            Debug.Log("Purchased successful");
            return true;
        }
        Debug.Log("Not enough money");
        return false;
    }
}
