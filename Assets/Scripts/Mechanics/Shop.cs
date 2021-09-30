using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject items;

    private int coins;
    void Start()
    {
        
        items.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.I) && items.activeSelf == false)
        {
            items.SetActive(true);
        }
        else {
            if (Input.GetKeyDown(KeyCode.I)){
                items.SetActive(false);
            }
        }
        
    }

    
}

