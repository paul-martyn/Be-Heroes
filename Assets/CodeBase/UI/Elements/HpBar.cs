using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Elements
{
    public class HpBar : MonoBehaviour 
    {
        public Slider Slider;
        public TMP_Text Value;

        public void SetValue(float current, float max)
        {
            Slider.value = (current / max) * 100f;
            Value.SetText($"{(int) current}");
        }
    }
}