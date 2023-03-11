using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarMeter : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public float triggerValue;

    void Start() {
        gameObject.transform.parent = GameObject.Find("UI").transform;
    }
    public void SetBar (int maxFill, int fill, string name, int playerNum, int width) {
        Debug.Log("SETBAR");
        SetMaxFill(maxFill);
        SetFill(fill);
        gameObject.name = name;
        if (playerNum == 2) {
            slider.direction = Slider.Direction.RightToLeft;
            gameObject.transform.position -= new Vector3(width, 0);
        }
    }
    public void SetMaxFill(int maxFill) {
        slider.maxValue = maxFill;
        slider.value = maxFill;
        fill.color = gradient.Evaluate(1f);
    }
    public void SetFill(int fill) {
        slider.value = fill;
        this.fill.color = gradient.Evaluate(slider.normalizedValue);
    }
    public float GetFill() {
        return slider.value;
    }
    

}
