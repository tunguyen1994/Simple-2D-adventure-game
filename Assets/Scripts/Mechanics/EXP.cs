using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EXP : MonoBehaviour
{
    public static EXP instance;
    public Slider slider;
    public int skillpoint;
    public int currentEXP;
    public int MaxEXP = 100;

    public void Start()
    {
        SetMaxEXP(MaxEXP);
        currentEXP = 0;
        skillpoint= 0;
        if (instance == null)
        {
            instance = this;
        }
    }
    private void SetMaxEXP(int exp)
    {
        slider.maxValue = exp;
        slider.value = 0;
    }
    public void SetEXP(int exp)
    {
        currentEXP += exp;
        slider.value = currentEXP;
        if (LevelUp(currentEXP))
        {
            SetSkillPoint(1);
            EXPScore.instance.SetLevel(1);
            currentEXP = 0;
            slider.value = 0;
        }
    }
    public void SetSkillPoint(int skillpointValue)
    {
        skillpoint += skillpointValue;
    }
    public bool LevelUp(int EXP)
    {
        if (EXP == MaxEXP)
        {
            return true;
        }
        else return false;
    } 
}
