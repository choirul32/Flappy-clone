using UnityEngine;
using System.Collections;

public class Shield : PowerUpBase
{
    // Metode untuk mengaktifkan power-up
    public override void Activate(GameObject player){

    }

    // Metode untuk menonaktifkan power-up
    protected override void Deactivate(GameObject player){

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // Pastikan power-up hanya aktif untuk player
        {
            collision.gameObject.GetComponent<FlappyController>().ActivatedShield();
            Destroy(gameObject);
            Debug.LogError("shield");
        }
    }
}
