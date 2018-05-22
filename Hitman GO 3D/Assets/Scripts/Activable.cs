using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activable : MonoBehaviour {
    public Node activableNode;
    public Node targetNode;

    public float iTweenDelay;
    public iTween.EaseType easeType;
    public float animTime;

    bool _active = false;

    GameObject _glow;

    private void Awake()
    {
        _glow = gameObject.transform.Find("Upper").gameObject;
    }

    public void Init ()
    {
        if (_glow != null)
        {
            
        }
        else
        {
            Debug.LogWarning("ACTIVABLE Init Error: _glow is null");
        }
    }

    public void Activate ()
    {
        if (activableNode != null && targetNode != null)
        {
            if (!_active)
            {
                _active = true;
                activableNode.LinkNode(targetNode);
            }
        }
        else
        {
            Debug.LogWarning("ACTIVABLE Activate Error: activableNode or targetNode is null");
        }
    }
}
