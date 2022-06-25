using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject _panelLevel = null;
    [SerializeField] private GameObject _panelShop = null;
    [SerializeField] private PlayerStats _playerStats = null;

    private void Awake()
    {
        _playerStats.GetSaveData();
    }

    public void OpenPanel(GameObject panel)
    {
        CloseAllPanel();
        panel.SetActive(true);
    }

    public void ClosePanel(GameObject panel)
    {
        panel.SetActive(false);
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void CloseAllPanel()
    {
        ClosePanel(_panelLevel);
        ClosePanel(_panelShop);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
