using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    public void SetMaxTimer(int Timer)
    {
        slider.maxValue = Timer;
        slider.value = Timer;

        fill.color = gradient.Evaluate(1f);
    }
    public void SetHealth(int Timer)
    {
        slider.value = Timer;

        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}


