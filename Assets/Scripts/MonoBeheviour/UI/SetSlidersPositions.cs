using UnityEngine;
using UnityEngine.UI;

public class SetSlidersPositions : MonoBehaviour
{
    [SerializeField]
    private Slider musicVolumeSlider;
    
    [SerializeField]
    private Slider soundsVolumeSlider;

    void Awake()
    {
        if (PlayerPrefs.HasKey(Constants.AudioMixer.SOUNDS_VOLUME))
        {
            soundsVolumeSlider.value = PlayerPrefs.GetFloat(Constants.AudioMixer.SOUNDS_VOLUME);
        }

        if (PlayerPrefs.HasKey(Constants.AudioMixer.MUSIC_VOLUME))
        {
            musicVolumeSlider.value = PlayerPrefs.GetFloat(Constants.AudioMixer.MUSIC_VOLUME);
        }
    }
}
