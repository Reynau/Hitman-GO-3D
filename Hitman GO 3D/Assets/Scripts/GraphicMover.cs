using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GraphicMoverMode
{
    MoveTo,
    ScaleTo,
    MoveFrom
}

public class GraphicMover : MonoBehaviour {
    public GraphicMoverMode mode;

    public Transform startXfrom;
    public Transform endXfrom;

    public float moveTime = 1f;
    public float delay = 0f;
    public iTween.LoopType loopType = iTween.LoopType.none;

    public iTween.EaseType easeType = iTween.EaseType.easeOutExpo;

    private void Awake()
    {
        if (endXfrom == null)
        {
            endXfrom = new GameObject(gameObject.name + "XformEnd").transform;
            endXfrom.position = transform.position;
            endXfrom.rotation = transform.rotation;
            endXfrom.localScale = transform.localScale;
        }
        if (startXfrom == null)
        {
            startXfrom = new GameObject(gameObject.name + "XformStart").transform;
            startXfrom.position = transform.position;
            startXfrom.rotation = transform.rotation;
            startXfrom.localScale = transform.localScale;
        }

        Reset();
    }

    public void Reset ()
    {
        switch (mode)
        {
            case GraphicMoverMode.MoveTo:
                if (startXfrom != null)
                {
                    transform.position = startXfrom.position;
                }
                break;
            case GraphicMoverMode.MoveFrom:
                if (endXfrom != null)
                {
                    transform.position = endXfrom.position;
                }
                break;
            case GraphicMoverMode.ScaleTo:
                if (startXfrom != null)
                {
                    transform.localScale = startXfrom.localScale;
                }
                break;
        }
    }

    public void Move ()
    {
        switch (mode)
        {
            case GraphicMoverMode.MoveTo:
                iTween.MoveTo(gameObject, iTween.Hash(
                    "position", endXfrom.position,
                    "time", moveTime,
                    "delay", delay,
                    "easetype", easeType,
                    "looptype", loopType
                ));
                break;
            case GraphicMoverMode.MoveFrom:
                iTween.MoveFrom(gameObject, iTween.Hash(
                    "position", startXfrom.position,
                    "time", moveTime,
                    "delay", delay,
                    "easetype", easeType,
                    "looptype", loopType
                ));
                break;
            case GraphicMoverMode.ScaleTo:
                iTween.ScaleTo(gameObject, iTween.Hash(
                    "scale", endXfrom.localScale,
                    "time", moveTime,
                    "delay", delay,
                    "easetype", easeType,
                    "looptype", loopType
                ));
                break;
        }
    }
}
