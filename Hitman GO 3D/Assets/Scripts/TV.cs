using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TV : MonoBehaviour {

    GameObject _screen;
    Material _screen_material;

    Color purpleColor = new Color(0.57f, 0f, 1f);
    Color blueColor = new Color(0.57f, 0.57f, 1f);
    Color newColor = new Color(1f, 0.57f, 0.57f);

    public float changeColorDelay = 3f;

    private void Awake()
    {
        _screen = gameObject.transform.Find("Screen").gameObject;
        _screen_material = _screen.GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Start () {
        StartCoroutine(ChangeColor());
	}

    IEnumerator ChangeColor()
    {
        for (;;)
        {
            _screen_material.SetColor("_EmissionColor", Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f));
            yield return new WaitForSeconds(changeColorDelay);
        }
    }
}
