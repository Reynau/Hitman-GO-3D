using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour {
    protected GameManager _gameManager;

    protected bool _isTurnComplete = false;
    public bool IsTurnComplete { get { return _isTurnComplete; } set { _isTurnComplete = value; } }
	
	protected virtual void Awake () {
        _gameManager = Object.FindObjectOfType<GameManager>().GetComponent<GameManager>();
	}

    public virtual void FinishTurn()
    {
        _isTurnComplete = true;
        _gameManager.HasMoved = false;

        if (_gameManager != null)
        {
            _gameManager.UpdateTurn();
        }
    }
}
