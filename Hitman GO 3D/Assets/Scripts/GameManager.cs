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
    public bool HasMoved
    {
        set { _player.playerMover.hasMoved = value; }
    }

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

    public float startPlayingDelay = 1f;
    public string nextLevel;
    public string menuName = "Menu";

    public UnityEvent setupEvent;
    public UnityEvent startLevelEvent;
    public UnityEvent playLevelEvent;
    public UnityEvent endLevelEvent;
    public UnityEvent loseLevelEvent;


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
            setupEvent.Invoke();    // Activates start menu
        }

        _player.playerInput.InputEnabled = false;

        Debug.Log("START LEVEL");

        while(!_hasLevelStarted)    // Waiting for the player to click the play button
        {
            yield return null;
        }

        if (startLevelEvent != null)
        {
            startLevelEvent.Invoke();   // Hide start menu and init board and player compass
        }
    }

    IEnumerator PlayLevelRoutine()
    {
        Debug.Log("PLAY LEVEL");

        _isGamePlaying = true;

        yield return new WaitForSeconds(startPlayingDelay);

        _player.playerInput.InputEnabled = true;

        if (playLevelEvent != null)
        {
            playLevelEvent.Invoke();
        }
        while (!_isGameOver)
        {
            yield return null;

            _isGameOver = IsWinner();
        }
    }

    public void LoseLevel()
    {
        StartCoroutine(LoseLevelRoutine());
    }

    IEnumerator LoseLevelRoutine ()
    {
        _isGameOver = true;

        yield return new WaitForSeconds(1.5f);

        if (loseLevelEvent != null)
        {
            loseLevelEvent.Invoke();
        }

        yield return new WaitForSeconds(2f);

        RestartLevel();
    }

    IEnumerator EndLevelRoutine()
    {
        PlayerManager.totalhealthyCount = PlayerManager.totalhealthyCount + _player.healthyCount;
        PlayerManager.totalfastFoodCount = PlayerManager.totalfastFoodCount + _player.fastFoodCount;
        if (PlayerManager.totalhealthyCount > 3)
        {
            PlayerManager.totalhealthyCount = 3;
        }
        if (PlayerManager.totalfastFoodCount > 3)
        {
            PlayerManager.totalfastFoodCount = 3;
        }
        _player.healthyCount = 0;
        _player.fastFoodCount = 0;
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
    }

    public void BackMenu()
    {
        SceneManager.LoadScene(menuName);
    }

    public void NextLevel ()
    {
        SceneManager.LoadScene(nextLevel);
    }

    public void RestartLevel ()
    {
        _player.healthyCount = 0;
        _player.fastFoodCount = 0;
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

        if (_player.playerMover.playerCompass != null)
        {
            _player.playerMover.playerCompass.ShowArrows(true);
        }
        // alow Player to move
    }

    void PlayEnemyTurn()
    {
        _currentTurn = Turn.Enemy;

        foreach (EnemyManager enemy in _enemies)
        {
            if (enemy != null && !enemy.IsDead)
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
            if (enemy.IsDead)
            {
                continue;
            }
            if (!enemy.IsTurnComplete)
            {
                return false;
            }
        }
        return true;
    }

    bool AreEnemiesAllDead ()
    {
        foreach (EnemyManager enemy in _enemies)
        {
            if (!enemy.IsDead)
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
            if (_player.IsTurnComplete && !AreEnemiesAllDead())
            {
                PlayEnemyTurn();
            }
            else if (AreEnemiesAllDead() && _player.playerMover.playerCompass != null)
            {
                _player.playerMover.playerCompass.ShowArrows(true);
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
