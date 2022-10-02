using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIBehaviour : MonoBehaviour
{
    [SerializeField]
    private Animator buttonsAnimator;

    [SerializeField]
    private Animator settingsDialogAnimator;

    [SerializeField]
    private Animator revialInputButtonAnimator;

    [SerializeField]
    private Animator inputFieldAnimator;

    private bool isChangingName = false;

    private bool isSettingsOpen = false;

    [SerializeField]
    private Text greetingsText;

    [SerializeField]
    private StringReference playerName;

    [SerializeField]
    private GameObject blockingPanel;

    private void Awake()
    {
        Time.timeScale = 1f;
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.ChangeMusicPitch(1f); 
        }
        ChangeGreetingsText();
    }

    public void OpenSettings()
    {
        isSettingsOpen = true;
        buttonsAnimator.SetBool(Constants.UIAnimationParameters.ISHIDDEN_BOOL, true);
        settingsDialogAnimator.SetBool(Constants.UIAnimationParameters.ISHIDDEN_BOOL, false);
        ChangeGreetingsAndButtonVisibility();
    }

    public void CloseSettings()
    {
        isSettingsOpen = false;
        buttonsAnimator.SetBool(Constants.UIAnimationParameters.ISHIDDEN_BOOL, false);
        settingsDialogAnimator.SetBool(Constants.UIAnimationParameters.ISHIDDEN_BOOL, true);
        Invoke(nameof(ChangeGreetingsAndButtonVisibility), 0.25f);
    }

    public void ChangeName()
    {
        isChangingName = !isChangingName;
        revialInputButtonAnimator.SetBool(Constants.UIAnimationParameters.ISCHANGINGNAME_BOOL, isChangingName);
        inputFieldAnimator.SetBool(Constants.UIAnimationParameters.ISCHANGINGNAME_BOOL, isChangingName);
        blockingPanel.SetActive(isChangingName);
    }

    public void ChangeGreetingsText()
    {
        greetingsText.text = "Hello, " + playerName.GetValue() + "\n" +
                            "Want to change\n" +
                            "name?";
    }

    public void ChangeGreetingsAndButtonVisibility()
    {
        greetingsText.gameObject.SetActive(!isSettingsOpen);
        revialInputButtonAnimator.gameObject.SetActive(!isSettingsOpen);
    }

    public void ExitApp()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif

        Application.Quit();
    }
}
