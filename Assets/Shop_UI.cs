using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


[Serializable]
public struct ColorToSell
{
    public Color color;
    public float price;
}

public class Shop_UI : MonoBehaviour
{
    [SerializeField] private GameObject platformColorButton;
    [SerializeField] private Transform platformColorParent;

    [SerializeField] private ColorToSell[] platformColors;
    void Start()
    {
        

        for (int i = 0; i < platformColors.Length; i++)
        {
            GameObject newButton = Instantiate(platformColorButton, platformColorParent);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
