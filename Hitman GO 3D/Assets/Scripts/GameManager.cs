using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Linq;

[System.Serializable]
public enum Turn
{
    Player,
    Enemy
}

public class GameManager : MonoBehaviour {
    Board _board;
    PlayerManager _player;

    List<EnemyManager> _enemies;

    Turn _currentTurn = Turn.Player;
    public Turn CurrentTurn { get { return _currentTurn; } }

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

    public UnityEvent setupEvent;
    public UnityEvent startLevelEvent;
    public UnityEvent playLevelEvent;
    public UnityEvent endLevelEvent;

    void Awake () {
        _board = Object.FindObjectOfType<Board>().GetComponent<Board>();
        _player = Object.FindObjectOfType<PlayerManager>().GetComponent<PlayerManager>();

        EnemyManager[] enemies = Object.FindObjectsOfType<EnemyManager>() as EnemyManager[];
        _enemies = enemies.ToList();
    }

    void Start()
    {
        if (_player != null && _board != null)
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
        Debug.Log("SETUP LEVEL");
        if (setupEvent != null)
        {
            setupEvent.Invoke();
        }
        Debug.Log("START LEVEL");
        _player.playerInput.InputEnabled = false;
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
        _player.playerInput.InputEnabled = true;

        if (playLevelEvent != null)
        {
            playLevelEvent.Invoke();
        }
        while (!_isGameOver)
        {
            yield return null;
            // check for Game Over condition

            // win
            // reach the end of the level
            _isGameOver = IsWinner();

            // lose
            // player dies

            // _isGameOver = true
        }
    }

    IEnumerator EndLevelRoutine()
    {
        _player.playerInput.InputEnabled = false;
        Debug.Log("END LEVEL");

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

    bool IsWinner ()
    {
        if (_board.PlayerNode != null && _board.GoalNode != null)
        {
            return (_board.PlayerNode == _board.GoalNode);
        }
        return false;
    }

    void PlayPlayerTurn ()
    {
        _currentTurn = Turn.Player;
        _player.IsTurnComplete = false;

        // alow Player to move
    }

    void PlayEnemyTurn()
    {
        _currentTurn = Turn.Enemy;

        foreach (EnemyManager enemy in _enemies)
        {
            if (enemy != null)
            {
                enemy.IsTurnComplete = false;
                enemy.PlayTurn();
            }
        }
    }

    bool IsEnemyTurnComplete ()
    {
        foreach (EnemyManager enemy in _enemies)
        {
            if (!enemy.IsTurnComplete)
            {
                return false;
            }
        }
        return true;
    }

    public void UpdateTurn ()
    {
        if (_currentTurn == Turn.Player && _player != null)
        {
            if (_player.IsTurnComplete)
            {
                PlayEnemyTurn();
            }
        }
        else if (_currentTurn == Turn.Enemy)
        {
            if (IsEnemyTurnComplete())
            {
                PlayPlayerTurn();
            }
        }
    }
}
