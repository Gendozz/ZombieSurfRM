using UnityEngine;

/// <summary>
/// Loads all service objects (spawners, pools etc)
/// </summary>
public class Loader : MonoBehaviour
{
    public ListReference movingPrefabsToLoad;

    [SerializeField]
    private DefaultSettingsSO defaultSettings;

    [Header("Difficulty settings")]
    [SerializeField]
    private FloatReference difficulty;

    [SerializeField]
    private FloatReference difficultyIncrement;

    [SerializeField]
    private FloatReference changeDifficultyPeriod;

    private DifficultyChanger difChanger;

    void Start()
    {
        LoadMovingPrefabs();
        SetDefaults();
        HandleDifficulty();
    }

    private void LoadMovingPrefabs()
    {
        GameObject container = new GameObject("Spawners_pools_etc");
        foreach (var obj in movingPrefabsToLoad.GetList())
        {
            Instantiate(obj, container.transform);
        }
    }

    private void SetDefaults()
    {
        difficulty.value = defaultSettings.GetDefaultDifficulty();
    }

    private void HandleDifficulty()
    {
        difChanger = new DifficultyChanger(difficulty, difficultyIncrement, changeDifficultyPeriod);
        difChanger.StartChangeDifficulty();
    }

    private void OnDestroy()
    {
        difChanger.StopChangeDifficulty();
    }
}
