using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinScore : MonoBehaviour
{
    public static CoinScore instance;
    public Text Cointext;
    public int coin = 0;
    // Start is called before the first frame update
    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public void SetCoinCount(int coinVaule)
    {
        coin += coinVaule;
        Cointext.text = coin.ToString();
    }
}
