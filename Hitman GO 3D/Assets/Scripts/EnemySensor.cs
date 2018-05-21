using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySensor : MonoBehaviour {

    public Vector3 directionToSearch = new Vector3(0f, 0f, Board.spacing);

    Node _nodeToSearch;
    Board _board;

    bool _foundPlayer = false;
    public bool FoundPlayer { get { return _foundPlayer; } }

	// Use this for initialization
	void Awake () {
        _board = Object.FindObjectOfType<Board>().GetComponent<Board>();
	}
	
    public void UpdateSensor ()
    {
        Vector3 worldSpacePositionToSearch = transform.position + transform.TransformDirection(directionToSearch);

        if (_board != null)
        {
            _nodeToSearch = _board.FindNodeAt(worldSpacePositionToSearch);

            if (_nodeToSearch == _board.PlayerNode)
            {
                _foundPlayer = true;
            }
        }
    }

	//void Update () {
    //    UpdateSensor();		
	//}
}
