using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthPotion : MonoBehaviour
{
    public Button button;
    private int price;
    void Start()
    {
        price = 5;
        Button btn = button.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    private void TaskOnClick()
    {
        if (CoinScore.instance.coin >= price)
        {
            PlayerController.instance.SetHealth();
            CoinScore.instance.SetCoinCount(-price);
            
        }
    }
}
