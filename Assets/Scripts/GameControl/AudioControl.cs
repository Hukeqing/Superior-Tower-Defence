using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioControl : MonoBehaviour
{
    public Slider MusicSlider;
    public Slider MainSlider;
    public Toggle MusicToggle;
    public Toggle MouseToggle;
    public AudioSource BackGroundMusic;
    public AudioSource CameraAS;

    private float NormalVolume;

    private void Start()
    {
        NormalVolume = BackGroundMusic.volume;
        MusicSlider.value = 1f;
        MainSlider.value = 1;
        MusicToggle.isOn = false;
        MouseToggle.isOn = true;
    }
    public void ChangeBackGroundMusic() => BackGroundMusic.volume = MusicSlider.value * NormalVolume;
    public void ChangeMainMusic() => AudioListener.volume = MainSlider.value;
    public void Mute() => AudioListener.pause = MusicToggle.isOn;
    public void MouseMusic() => CameraAS.enabled = MouseToggle.isOn;
    public void ReStart()
    {
        MainSlider.value = 1;
        MusicSlider.value = 1;
        MusicToggle.isOn = false;
        MouseToggle.isOn = true;
        BackGroundMusic.volume = NormalVolume;
        AudioListener.volume = 1;
        AudioListener.pause = false;
        CameraAS.enabled = true;
    }
}
