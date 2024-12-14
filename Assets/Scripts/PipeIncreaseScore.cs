using UnityEngine;

public class PipeIncreaseScore : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.UpdateScore(1);
        }
    }
}
