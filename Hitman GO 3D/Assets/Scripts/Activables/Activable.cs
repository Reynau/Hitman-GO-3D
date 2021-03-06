﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Activable : MonoBehaviour {

    protected bool _active = false;
    public bool finishedRotations = true;

    public UnityEvent activateActivableEvent;

    public virtual void Init ()
    {

    }

	public virtual IEnumerator Activate ()
    {
        yield return new WaitForSeconds(0f);
    }
}
