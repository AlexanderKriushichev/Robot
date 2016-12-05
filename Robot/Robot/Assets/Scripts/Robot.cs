using UnityEngine;
using System.Collections;
using System;

public class Robot : MonoBehaviour {

    public GameObject leftWheel;
    public GameObject rightWheel;

    public float leftPower
    {
        get
        {
            return leftSlider.Value;
        }
    }
    public float rightPower
    {
        get
        {
            return rightSlider.Value;
        }
    }

    public float speed;

    public SliderPower leftSlider;
    public SliderPower rightSlider;

    public bool complite = false;

    public GameObject panelFinish;

    public void Complite()
    {

        complite = true;
        panelFinish.SetActive(true);

    }

    public void LeftForward()
    {
        if (complite)
            return;
        MoveLeftWheel(-leftPower);
    }

    public void LefBack()
    {
        if (complite)
            return;
        MoveLeftWheel(leftPower);
    }

    public void RightForward()
    {
        if (complite)
            return;
        MoveRightWheel(-rightPower);
    }

    public void RightBack()
    {
        if (complite)
            return;
        MoveRightWheel(rightPower);
    }

    public void Forward()
    {
        if (complite)
            return;
        RightForward();
        LeftForward();
        LeftForward();
        RightForward();
    }

    public void Back()
    {
        if (complite)
            return;
        RightBack();
        LefBack();
        LefBack();
        RightBack();
    }

    private void MoveLeftWheel(float left)
    {
        transform.RotateAround(rightWheel.transform.position, new Vector3(0, 0, 1), Time.deltaTime * speed * left);
    }

    private void MoveRightWheel(float right)
    {
        transform.RotateAround(leftWheel.transform.position, new Vector3(0, 0, 1), -Time.deltaTime * speed * right);
    }
 
}
