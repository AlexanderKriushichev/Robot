using UnityEngine;
using System.Collections;
using UnityEngine.UI;
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

    public Toggle automaticMode;

    public bool complite = false;

    public GameObject panelFinish;

    public Transform leftRay;
    public Transform rightRay;

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

    private bool LeftRayAction()
    {
        RaycastHit2D hit = Physics2D.Raycast(leftRay.position, (leftRay.TransformPoint(Vector3.up) - leftRay.position).normalized, 0.6f);
        Debug.DrawRay(leftRay.position, (leftRay.TransformPoint(Vector3.up) - leftRay.position).normalized * 0.6f, Color.red);
        if (hit.collider != null)
        {
            if (hit.collider.tag != "Finish")
                return false;
        }
        return true;
    }

    private bool RightRayAction()
    {
        RaycastHit2D hit = Physics2D.Raycast(rightRay.position, (rightRay.TransformPoint(Vector3.up) - rightRay.position).normalized, 0.6f);
        Debug.DrawRay(rightRay.position, (rightRay.TransformPoint(Vector3.up) - rightRay.position).normalized * 0.6f, Color.red);
        if (hit.collider != null)
        {
            if (hit.collider.tag != "Finish")
                return false;
        }
        return true;
    }

    void Update()
    {
        if (automaticMode.isOn && !complite)
        {
            if (RightRayAction() && LeftRayAction())
            {
                Forward();
                LeftForward();
                RightBack();
                RightBack();
                LeftForward();
            }
            else
            {
                LefBack();
                RightForward();
                RightForward();
                LefBack();
            }
        }
    }
 
}
