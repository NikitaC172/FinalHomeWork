using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [SerializeField] private Player _player = null;
    [SerializeField] private Mover _mover = null;
    [SerializeField] private GameObject _panelCompliteMission = null;
    [SerializeField] private GameObject _panelFailedMission = null;

    private void OnEnable()
    {
        _player.Died += OpenFailedGameMenu;
        _mover.LevelComplited += OpenCompliteGameMenu;
    }

    private void OnDisable()
    {
        _player.Died -= OpenFailedGameMenu;
        _mover.LevelComplited -= OpenCompliteGameMenu;
    }

    public void OpenPanel(GameObject panel)
    {
        panel.SetActive(true);
        Time.timeScale = 0;
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void ClosePanel(GameObject panel)
    {
        panel.SetActive(false);
        Time.timeScale = 1;
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OpenCompliteGameMenu()
    {
        OpenPanel(_panelCompliteMission);
    }

    private void OpenFailedGameMenu()
    {
        OpenPanel(_panelFailedMission);
    }
}
