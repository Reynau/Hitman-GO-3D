using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connector : Activable {
    bool _connectableNodes;

    public bool isStatic = false;
    
    public Node activableNode;
    public Node targetNode;

    public float iTweenDelay;
    public iTween.EaseType easeType;
    public float animTime;

    private void Awake()
    {
        if (activableNode == null || targetNode == null)
        {
            return;
        }

        _connectableNodes = (Vector3.Distance(activableNode.transform.position, targetNode.transform.position) == 2);

        if (!_connectableNodes)
        {
            Debug.LogWarning("CONNECTOR Awake Error: Nodes are not connectable");
        }
    }

    public override void Activate()
    {
        if (!_connectableNodes)
        {
            return;
        }

        if (activableNode != null && targetNode != null)
        {
            if (_active)
            {
                if (!isStatic)
                {
                    _active = false;
                }
                activableNode.RemoveLink(targetNode);
            }
            else
            {
                if (!isStatic)
                {
                    _active = true;
                }
                activableNode.LinkNode(targetNode);
            }
        }
        else
        {
            Debug.LogWarning("ACTIVABLE Activate Error: activableNode or targetNode is null");
        }
    }
}
