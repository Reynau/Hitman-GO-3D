using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorButton : MonoBehaviour {
    GameObject _glow;
    Material _glow_material;

    Color purpleColor = new Color(0.57f, 0f, 1f);

    bool _glowing = true;
    bool _active = true;

    public float changeColorDelay = 1.5f;

    private void Awake()
    {
        _glow = gameObject.transform.Find("Glow").gameObject;
        _glow_material = _glow.GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Start()
    {
        StartCoroutine(ChangeColor());
    }

    IEnumerator ChangeColor()
    {
        for (; ; )
        {
            if (_active)
            {
                if (_glowing)
                {
                    iTween.ValueTo(gameObject, iTween.Hash(
                        "from", 1f,
                        "to", 0f,
                        "time", changeColorDelay,
                        "onupdate", "UpdateColor"
                    ));
                }
                else
                {
                    iTween.ValueTo(gameObject, iTween.Hash(
                        "from", 0f,
                        "to", 1f,
                        "time", changeColorDelay,
                        "onupdate", "UpdateColor"
                    ));
                }
                _glowing = !_glowing;
            }
            yield return new WaitForSeconds(changeColorDelay);
        }
    }

    void UpdateColor(float val)
    {
        _glow_material.SetColor("_EmissionColor", new Color(val, 0f, 0f));
    }

    public void Deactivate ()
    {
        _active = false;
    }
}
