using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    public Slider hpSlider;
    public TMP_Text hpText;

    public void UpdateHP(int current, int max)
    {
        hpSlider.maxValue = max;
        hpSlider.value = current;
        hpText.text = current + " / " + max;
    }
}
