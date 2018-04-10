using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Setting : MonoBehaviour {
    public const float DEFAULT_BGM = 0.2f;
    public const float DEFAULT_SFX = 1f;

    [SerializeField]
    private VolumeType volume;

    [SerializeField]
    private Text value;

    [SerializeField]
    private Slider slider;

    private string Key {
        get {
            string key = string.Empty;
            switch (volume) {
                case VolumeType.BGM:
                    key = SoundManager.BGM_KEY;
                    break;

                case VolumeType.SFX:
                    key = SoundManager.SFX_KEY;
                    break;
            }
            return key;
        }
    }

    private void Start() {
        slider.value = PlayerPrefs.GetFloat(Key, volume == VolumeType.BGM ? DEFAULT_BGM : DEFAULT_SFX);
        OnValueChange(slider.value);
    }

    public static float GetBGM() {
        return PlayerPrefs.GetFloat(SoundManager.BGM_KEY, DEFAULT_BGM);
    }

    public static float GetSFX() {
        return PlayerPrefs.GetFloat(SoundManager.SFX_KEY, DEFAULT_SFX);
    }

    public void OnValueChange(float value) {
        PlayerPrefs.SetFloat(Key, value);
        this.value.text = Mathf.FloorToInt(value * 100).ToString();
    }
}