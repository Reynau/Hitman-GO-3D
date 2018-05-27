using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Linq;

public class MenuManager : MonoBehaviour {

    public UnityEvent mainMenuEvent;
    public UnityEvent selectLevelEvent;
    public UnityEvent controlsEvent;

    private void Start()
    {
        Debug.Log("START");
        mainMenuEvent.Invoke();
    }

    public void ChangeToMainMenu ()
    {
        Debug.Log("Change to main menu");
        mainMenuEvent.Invoke();
    }

    public void ChangeToSelectLevel ()
    {
        Debug.Log("Change to select level");
        selectLevelEvent.Invoke();
    }

    public void ChangeToControls()
    {
        Debug.Log("Change to controls");
        controlsEvent.Invoke();
    }

    public void LoadMenu ()
    {
        Debug.Log("Loading menu");
        SceneManager.LoadScene("Menu");
    }

    public void LoadLevel (int level)
    {
        Debug.Log("Loading level " + level);
        SceneManager.LoadScene(level);
    }

    public void LoadCredits ()
    {
        Debug.Log("Loading credits");
        SceneManager.LoadScene("Credits");
    }
}
