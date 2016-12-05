using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SliderPower : MonoBehaviour {

    private Slider slider;
    public Text text;
    public Switch switchState;
    public SliderPower sliderPower;
    public Robot robot;
	// Use this for initialization
	void Start () {

        slider = GetComponent<Slider>();
        slider.onValueChanged.AddListener(OnChange);
	}

    void OnChange(float value)
    {
        text.text = ((value - 0.5f) * 200).ToString("0");
        if (switchState.activate)
        {
            sliderPower.slider.value = value;
        }
    }

    public float Value
    {
        get
        {
            return (slider.value - 0.5f) * 2;
        }
    }
	
}
