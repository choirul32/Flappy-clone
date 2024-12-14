using UnityEngine;
using System.Collections;

public abstract class PowerUpBase : MonoBehaviour
{
    // Durasi efek power-up
    [SerializeField] protected float duration = 5f;

    // Metode untuk mengaktifkan power-up
    public abstract void Activate(GameObject player);

    // Metode untuk menonaktifkan power-up
    protected abstract void Deactivate(GameObject player);

    // Coroutine untuk menangani durasi power-up
    
}
