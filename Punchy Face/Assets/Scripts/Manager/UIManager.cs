using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] private TextMeshProUGUI currentLevelText;
    [SerializeField] private GameObject gamePlayPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject levelCompletePanel;

    void Start()
    {
        Init();
    }

    public void Init()
    {
        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
            instance = this;
        currentLevelText.color = Color.black;
    }

    public void ShowGameplayPanel(bool isShow)
    {
        gamePlayPanel.SetActive(isShow);
    }

    public void ShowGameOverPanel(bool isShow)
    {
        gameOverPanel.SetActive(isShow);
    }

    public void ShowLevelCompletePanel(bool isShow)
    {
        levelCompletePanel.SetActive(isShow);
    }
}
