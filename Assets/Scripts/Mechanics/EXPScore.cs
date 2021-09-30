using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EXPScore : MonoBehaviour
{
    public static EXPScore instance;
    public Text leveltext;
    int Level = 1;
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void SetLevel(int LevelVaule)
    {
        Level += LevelVaule;
        leveltext.text = Level.ToString();
    }
}
