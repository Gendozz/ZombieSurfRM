using UnityEngine;

public class Pause : MonoBehaviour
{
    public static bool isGamePaused = false;

    [SerializeField]
    private Animator settingsDialogAnimator;

    [SerializeField]
    private Animator fadeBackgroundAnimator;

    [SerializeField]
    private GameObject toMainMenuButton; 

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGamePaused)
            {
                UnpauseGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    private void PauseGame()
    {
        isGamePaused = true;
        Time.timeScale = 0;
        SoundManager.Instance.ChangeMusicPitch(0.5f);
        settingsDialogAnimator.SetBool(Constants.UIAnimationParameters.ISHIDDEN_BOOL, false);
        fadeBackgroundAnimator.SetBool(Constants.UIAnimationParameters.ISPAUSED_BOOL, true);
        toMainMenuButton.SetActive(true);
    }

    public void UnpauseGame()
    {
        isGamePaused = false;
        Time.timeScale = 1;
        SoundManager.Instance.ChangeMusicPitch(1f);
        settingsDialogAnimator.SetBool(Constants.UIAnimationParameters.ISHIDDEN_BOOL, true);
        fadeBackgroundAnimator.SetBool(Constants.UIAnimationParameters.ISPAUSED_BOOL, false);
        toMainMenuButton.SetActive(false);
    }
}
