using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {
    float _h;
    public float H { get { return _h; } }
    float _v;
    public float V { get { return _v; } }

    bool _inputEnabled = false;
    public bool InputEnabled { get { return _inputEnabled; } set { _inputEnabled = value; } }

	public void GetKeyInput ()
    {
        if (_inputEnabled)
        {
            _h = Input.GetAxisRaw("Horizontal");
            _v = Input.GetAxisRaw("Vertical");
        }
    }
}
