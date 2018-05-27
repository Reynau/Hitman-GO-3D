using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class CollectibleLabel : MonoBehaviour
{
    public CollectibleType collectibleType = CollectibleType.FastFood;

    Text _textLabel;
    PlayerManager _player;

    void Awake()
    {
        _textLabel = gameObject.GetComponent<Text>();
        _player = Object.FindObjectOfType<PlayerManager>().GetComponent<PlayerManager>();
    }

    void Start()
    {
        if (_textLabel != null && _player != null)
        {
            switch (collectibleType)
            {
                case CollectibleType.FastFood:
                    _textLabel.text = "Fast Food: " + _player.fastFoodCount.ToString() + "/3";
                    break;
                case CollectibleType.HealthyFood:
                    _textLabel.text = "Healthy Food: " + _player.healthyCount.ToString() + "/3";
                    break;
            }
            
        }
    }
}
