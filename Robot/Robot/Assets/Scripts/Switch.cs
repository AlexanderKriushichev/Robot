using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class Switch : MonoBehaviour {


    public List<GameObject> switchObject = new List<GameObject>();

    public Sprite spriteOn, spriteOff;

    private Image image;

    public bool activate = false;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(SwitchState);
        image = GetComponent<Image>();
    }

    public void SwitchState()
    {
        foreach (GameObject obj in switchObject)
        {
            obj.SetActive(!obj.activeSelf);
        }

        if (activate)
        {
            image.sprite = spriteOff;
        }
        else
        {
            image.sprite = spriteOn;
        }

        activate = !activate;
    }
	
}
