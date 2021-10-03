using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public UnityEngine.UI.Button nextLevelButton = null;

    public bool muted = false;
    public UnityEngine.UI.Text muteMusicButtonText = null;

    private void Start()
    {
        if (nextLevelButton != null) nextLevelButton.interactable = (!isLastScene());

        muted = (PlayerPrefs.GetInt("mute", 1) == 0);
        if (muteMusicButtonText != null) muteMusicButtonText.text = (muted ? "UnMute Music" : "Mute Music");
    }

    public void LoadLevel(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void NextLevel()
    {
        int index = 1 + SceneManager.GetActiveScene().buildIndex;
        if (index < SceneManager.sceneCountInBuildSettings) SceneManager.LoadScene(index);
    }

    public bool isLastScene() { return SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 1;  }

    public void Restart()
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        if (index < SceneManager.sceneCountInBuildSettings) SceneManager.LoadScene(index);
    }

    public void ToggleMute()
    {
        muted = !muted;
        PlayerPrefs.SetInt("mute", muted ? 0 : 1);
        if (muteMusicButtonText != null) muteMusicButtonText.text = (muted ? "UnMute Music" : "Mute Music");
    }
}
