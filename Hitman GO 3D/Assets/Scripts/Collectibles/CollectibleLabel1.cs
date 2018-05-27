using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class CollectibleLabel1 : MonoBehaviour
{
    Text _text;
    PlayerManager _player;
    void Awake()
    {
        _text = GetComponent<Text>();
        _player = Object.FindObjectOfType<PlayerManager>().GetComponent<PlayerManager>();
    }

    public void SetCount()
    {
        if (_text != null && _player != null)
        {
            _text.text = "Fast Food: " + _player.fastFoodCount.ToString() + "/3";
        }
    }
}
