using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpgrade : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject upgrades;
    void Start()
    {
        upgrades.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && upgrades.activeSelf == false)
        {
            upgrades.SetActive(true);
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                upgrades.SetActive(false);
            }
        }
    }
}
