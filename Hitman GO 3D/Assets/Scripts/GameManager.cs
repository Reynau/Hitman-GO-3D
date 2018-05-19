using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    Board _board;
    PlayerManager _player_manager;

    bool _hasLevelStarted = false;
    public bool HasLevelStarted
    {
        get { return _hasLevelStarted; }
        set { _hasLevelStarted = value; }
    }
    bool _isGamePlaying = false;
    public bool IsGamePlaying
    {
        get { return _isGamePlaying; }

        set { _isGamePlaying = value; }
    }
    bool _isGameOver = false;
    public bool IsGameOver
    {
        get { return _isGameOver; }

        set { _isGameOver = value; }
    }
    bool _hasLevelFinished = false;
    public bool HasLevelFinished
    {
        get { return _hasLevelFinished; }

        set { _hasLevelFinished = value; }
    }

    public float delay = 1f;

    public UnityEvent startLevelEvent;
    public UnityEvent playLevelEvent;
    public UnityEvent endLevelEvent;

    void Awake () {
        _board = Object.FindObjectOfType<Board>().GetComponent<Board>();
        _player_manager = Object.FindObjectOfType<PlayerManager>().GetComponent<PlayerManager>();
    }

    void Start()
    {
        if (_player_manager != null && _board != null)
        {
            StartCoroutine("RunGameLoop");
        }
        else
        {
            Debug.LogWarning("GAMEMANAGER Error: no player or board found!");
        }
    }

    IEnumerator RunGameLoop()
    {
        yield return StartCoroutine("StartLevelRoutine");
        yield return StartCoroutine("PlayLevelRoutine");
        yield return StartCoroutine("EndLevelRoutine");
    }

    IEnumerator StartLevelRoutine()
    {
        Debug.Log("START LEVEL");
        _player_manager.playerInput.InputEnabled = false;
        while(!_hasLevelStarted)
        {
            // show start screen
            //   user presses button to start
            //   _hasLevelStarted = true
            yield return null;
        }

        if (startLevelEvent != null)
        {
            startLevelEvent.Invoke();
        }
    }

    IEnumerator PlayLevelRoutine()
    {
        Debug.Log("PLAY LEVEL");
        _isGamePlaying = true;
        yield return new WaitForSeconds(delay);
        _player_manager.playerInput.InputEnabled = true;

        if (playLevelEvent != null)
        {
            playLevelEvent.Invoke();
        }
        while (!_isGameOver)
        {
            // check for Game Over condition

            // win
            // reach the end of the level

            // lose
            // player dies

            // _isGameOver = true
            yield return null;
        }
    }

    IEnumerator EndLevelRoutine()
    {
        Debug.Log("END LEVEL");
        _player_manager.playerInput.InputEnabled = false;

        if (endLevelEvent != null)
        {
            endLevelEvent.Invoke();
        }


        // show end screen
        while (!_hasLevelFinished)
        {
            // user presses button to continue

            // _hasLevelFinished = true
            yield return null;
        }

        RestartLevel();
    }

    void RestartLevel ()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void PlayLevel ()
    {
        _hasLevelStarted = true;
    }
}
