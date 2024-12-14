using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RocketManager : MonoBehaviour
{
    [SerializeField] private List<Sprite> _rocket = new List<Sprite>();
    [SerializeField] private GameObject _prefabRocket;
    SpriteRenderer _renderer;
    void Start()
    {
        _renderer = _prefabRocket.GetComponent<SpriteRenderer>();
    }
    public GameObject GetRocket(){
        _renderer.sprite = _rocket[Random.Range(0, _rocket.Count - 1)];
        return _prefabRocket;
    }
}
