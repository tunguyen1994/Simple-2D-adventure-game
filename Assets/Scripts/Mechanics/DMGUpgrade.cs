using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DMGUpgrade : MonoBehaviour
{
    public Button button;
    void Start()
    {
        Button btn = button.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    private void TaskOnClick()
    {
        if (EXP.instance.skillpoint > 0)
        {
            EXP.instance.skillpoint--;
            BulletOne.instance.damage += 0.5;
        }
    }

}
