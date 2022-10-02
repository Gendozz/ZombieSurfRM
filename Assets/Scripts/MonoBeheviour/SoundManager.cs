using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource soundsSource;

    [SerializeField]
    private AudioSource musicSource;

    [SerializeField]
    private AudioMixer audioMixer;

    [SerializeField]
    private float lowPitchRange = 0.95f;

    [SerializeField]
    private float highPitchRange = 1.05f;

    private float soundsVolume = -20f;
    
    private float musicVolume = -20f;

    public static SoundManager Instance = null;

    private Slider[] volumeSliders;

    private Slider soundsVolumeSlider;

    private Slider musicVolumeSlider;

    private void Awake()
    {
        if (Instance == null)
        {
            SoundManager.Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        GetSliders();
        SetVolume();
        musicSource.Play();
    }

    public void MusicVolume(float sliderValue)
    {
        audioMixer.SetFloat(Constants.AudioMixer.MUSIC_VOLUME, sliderValue);
        PlayerPrefs.SetFloat(Constants.AudioMixer.MUSIC_VOLUME, sliderValue);
    }

    public void SoundsVolume(float sliderValue)
    {
        audioMixer.SetFloat(Constants.AudioMixer.SOUNDS_VOLUME, sliderValue);
        PlayerPrefs.SetFloat(Constants.AudioMixer.SOUNDS_VOLUME, sliderValue);
    }

    public void Play(AudioClip clip)
    {
        soundsSource.clip = clip;
        soundsSource.Play();
    }    
    
    public void Play(AudioClip clip, bool isRandomPitch)
    {
        if (isRandomPitch)
        {
            soundsSource.pitch = Random.Range(lowPitchRange, highPitchRange); 
        }
        soundsSource.clip = clip;
        soundsSource.Play();
    }

    // Play a single clip through the music source.
    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.Play();
    }

    public void ChangeMusicPitch(float pitch)
    {
        musicSource.pitch = pitch;
    }

    // Play a random clip from an array, and randomize the pitch slightly.
    public void RandomSoundEffect(params AudioClip[] clips)
    {
        int randomIndex = Random.Range(0, clips.Length);
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);

        soundsSource.pitch = randomPitch;
        soundsSource.clip = clips[randomIndex];
        soundsSource.Play();
    }

    private void OnLevelWasLoaded()
    {        
        GetSliders();
        SetVolume();
    }

    private void GetSliders()
    {
        volumeSliders = FindObjectsOfType<Slider>();

        foreach (Slider slider in volumeSliders)
        {
            if (slider.name.Equals("MusicVolumeSlider"))
            {
                musicVolumeSlider = slider;
            }
            if (slider.name.Equals("SoundsVolumeSlider"))
            {
                soundsVolumeSlider = slider;
            }
        }

        musicVolumeSlider.onValueChanged.AddListener(MusicVolume);
        soundsVolumeSlider.onValueChanged.AddListener(SoundsVolume);
    }

    private void SetVolume()
    {
        if (PlayerPrefs.HasKey(Constants.AudioMixer.SOUNDS_VOLUME))
        {
            soundsVolume = PlayerPrefs.GetFloat(Constants.AudioMixer.SOUNDS_VOLUME);
        }

        if (PlayerPrefs.HasKey(Constants.AudioMixer.MUSIC_VOLUME))
        {
            musicVolume = PlayerPrefs.GetFloat(Constants.AudioMixer.MUSIC_VOLUME);
        }

        audioMixer.SetFloat(Constants.AudioMixer.MUSIC_VOLUME, musicVolume);
        audioMixer.SetFloat(Constants.AudioMixer.SOUNDS_VOLUME, soundsVolume);
    }
}