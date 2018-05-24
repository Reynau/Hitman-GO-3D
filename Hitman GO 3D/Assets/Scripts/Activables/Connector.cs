using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connector : Activable {
    bool _connectableNodes;

    public bool isStatic = false;
    private bool _alreadyActivated = false;
    
    public Node activableNode;
    public Node targetNode;

    public float delay;

    public bool closeOnStart = true;

    private void Awake ()
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

    private void Start ()
    {
        _active = true; // Connectors start active to permit link propagation
    }

    public override void Init ()
    {
        if (activableNode != null && targetNode != null)
        {
            if (closeOnStart)
            {
                Disconnect();
            }
        }
    }

    public override void Activate ()
    {
        if (!_connectableNodes || (isStatic && _alreadyActivated))
        {
            return;
        }

        if (activableNode != null && targetNode != null)
        {
            _alreadyActivated = true;

            if (!_active)
            {
                Connect();
            }
            else
            {
                Disconnect();
            }
        }
        else
        {
            Debug.LogWarning("ACTIVABLE Activate Error: activableNode or targetNode is null");
        }
    }

    public void Connect ()
    {
        StartCoroutine(LinkNodeRoutine());
        _active = true;
    }

    public void Disconnect()
    {
        StartCoroutine(RemoveNodeRoutine());
        _active = false;
    }

    IEnumerator LinkNodeRoutine ()
    {
        yield return new WaitForSeconds(delay);
        activableNode.LinkNode(targetNode);
        activateActivableEvent.Invoke();
    }

    IEnumerator RemoveNodeRoutine ()
    {
        yield return new WaitForSeconds(delay);
        activableNode.RemoveLink(targetNode);
        activateActivableEvent.Invoke();
    }
}
