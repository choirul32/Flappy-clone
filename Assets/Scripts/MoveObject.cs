using UnityEngine;

public class MoveObject : MonoBehaviour
{
    [SerializeField] private float _speed = 0.65f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void SetSpeed(float speed){
        _speed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.left * _speed * Time.deltaTime;
    }
}
