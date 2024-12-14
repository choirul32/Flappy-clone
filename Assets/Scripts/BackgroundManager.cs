using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BackgroundManager : MonoBehaviour
{
    public List<Sprite> _background = new List<Sprite>();
    SpriteRenderer _renderer;
    int _currentIndexBackground;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _currentIndexBackground = 0;
        _renderer = gameObject.GetComponent<SpriteRenderer>();
        _renderer.sprite = _background[_currentIndexBackground];
    }

    public void ChangeBackground(){
        _currentIndexBackground = _currentIndexBackground == 0 ? 1 : 0;
        _renderer.sprite = _background[_currentIndexBackground];
    }
}
