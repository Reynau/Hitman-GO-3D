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
                    _textLabel.text = "Total Fast Food: " + PlayerManager.totalfastFoodCount.ToString() + "/3";
                    if (PlayerManager.totalfastFoodCount >= 3)
                    {
                        _textLabel.text = _textLabel.text + "\n" + "Some snacks after" + "\n" + "great assassinations";
                    }
                    break;
                case CollectibleType.HealthyFood:
                    _textLabel.text = "Total Healthy Food: " + PlayerManager.totalhealthyCount.ToString() + "/3";
                    if (PlayerManager.totalhealthyCount >= 3)
                    {
                        _textLabel.text = _textLabel.text + "\n" + "A healthy assassin needs" + "\n" + "healthy food";
                    }
                    break;
            }
            
        }
    }
}
