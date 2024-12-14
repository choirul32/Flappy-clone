using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlappyController : MonoBehaviour
{
    [Header("Player Settings")]
    public bool hasShield;
    public GameObject shieldObject;
    public float flapForce = 5f; // Kekuatan saat melompat/flap
    public float tiltSmooth = 5f; // Kecepatan rotasi karakter
    public float maxDownwardRotation = -90f; // Rotasi maksimum ke bawah

    [Header("Dust Effect")]
    public GameObject dustPrefab; // Prefab atau GameObject animasi debu
    public Transform dustSpawnPoint; // Posisi spawn efek debu
    private Rigidbody2D rb; // Referensi Rigidbody2D untuk karakter

    void Start()
    {
        // Mengambil komponen Rigidbody2D dari karakter
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Deteksi input untuk melompat
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0) || Input.touchCount > 0)
        {
            // Jika ada tap di layar (Android) atau klik mouse (PC)
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                Flap();
            }
            else if (Input.GetMouseButtonDown(0))
            {
                Flap();
            }
            else if (Input.GetKeyDown(KeyCode.Space)) // Tambahan untuk keyboard (PC)
            {
                Flap();
            }
        }

        HandleRotation();
    }

    void Flap()
    {
        // Reset kecepatan vertikal sebelum melompat agar responsif
        rb.linearVelocity = Vector2.zero;

        // Tambahkan gaya ke atas untuk lonjakan/flap
        rb.AddForce(Vector2.up * flapForce, ForceMode2D.Impulse);
        PlayDustEffect();
    }

    void HandleRotation()
    {
        // Ambil kecepatan vertikal dari Rigidbody2D
        float velocityY = rb.linearVelocity.y;

        // Hitung rotasi berdasarkan kecepatan vertikal
        float targetRotation = Mathf.Lerp(maxDownwardRotation, 30f, (velocityY + 5f) / 10f);

        // Terapkan rotasi secara halus
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, targetRotation), Time.deltaTime * tiltSmooth);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Jika karakter menyentuh rintangan atau tanah, game over
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            if (hasShield)
            {
                hasShield = false;
                shieldObject.SetActive(false);
            }else{
                GameManager.Instance.GameOver();
            }
        }
        else if (collision.gameObject.CompareTag("Ground"))
        {
            GameManager.Instance.GameOver();
        }
    }

    void PlayDustEffect()
    {
        if (dustPrefab != null && dustSpawnPoint != null)
        {
            // Spawn GameObject dust pada posisi spawn point
            GameObject dust = Instantiate(dustPrefab, dustSpawnPoint.position, Quaternion.identity);

            // Hancurkan efek setelah animasi selesai (misalnya 1 detik)
            Destroy(dust, 1f);
        }
    }

    public void ActivatedShield(){
        hasShield = true;
        shieldObject.SetActive(true);
    }
}
