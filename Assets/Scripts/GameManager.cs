using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
public class GameManager : MonoBehaviour
{
    public BackgroundManager _backgroundManager;
    public RocketManager _rocketManager;
    [Header("Pipe Obstacle")]
    [SerializeField] private float _speedPipe = 1.65f;
    [SerializeField] private float _maxTime = 3f;             // Default jarak antar pipa
    [SerializeField] private float _minTime = 1f;            // Batas minimum jarak antar pipa
    [SerializeField] private float _timeDecreaseRate = 0.05f;
    [SerializeField] private float _heightrange = 0.45f;
    [SerializeField] private List<GameObject> _pipe = new List<GameObject>();
    [SerializeField] private GameObject _pipePool;
    [SerializeField] private GameObject _rocketPool;
    [SerializeField] private float _speedIncreaseRate = 0.1f; // Kecepatan bertambah setiap interval waktu
    [SerializeField] private float _increaseInterval = 10f;   // Waktu untuk meningkatkan kecepatan
    private float _timer;
    private float _timeSinceLastIncrease; // Waktu yang telah berlalu sejak terakhir meningkatkan kecepatan
    private float _scoreTimer = 0f; // Timer untuk menambah skor otomatis
    [SerializeField] private float _scoreInterval = 1f; // Interval waktu untuk menambah skor (per detik)
    
    [Header("Rocket Spawn")]
    [SerializeField] private float _rocketSpawnInterval = 5f;  // Interval untuk spawn roket (dalam detik)
    private float _rocketSpawnTimer = 0f;  // Timer untuk melacak waktu spawn roket
    [Space(30)]
    public TMP_Text _scoreText;

    public GameObject _player;
    public Vector3 _posStartPlayer;
    // Static instance untuk Singleton pattern
    public static GameManager Instance { get; private set; }

    // Variabel untuk data game, seperti skor, level, dll.
    public int score;
    public int level;

    [Header("UI")]
    public GameObject _gameOver;
    public GameObject _mainMenu;

    // Memastikan hanya ada satu instance GameManager
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Hancurkan objek jika instance sudah ada
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Jangan hancurkan GameManager saat scene berubah
        }
    }

    void Start(){
        _posStartPlayer = _player.transform.position;
        _gameOver.SetActive(false);
        _scoreText.text = "Score: " + score.ToString();
        SpawnPipe();
        _timeSinceLastIncrease = 0f;
        Time.timeScale = 0f;
    }

    void Update()
    {
        // Spawn pipe sesuai waktu yang ada
        if (_timer > _maxTime)
        {
            SpawnPipe();
            _timer = 0;
        }
        _timer += Time.deltaTime;

        // Timer untuk menambah skor otomatis
        _scoreTimer += Time.deltaTime;
        if (_scoreTimer >= _scoreInterval)
        {
            UpdateScore(1); // Tambahkan skor setiap detik
            _scoreTimer = 0f;
        }

        // Tingkatkan kecepatan seiring waktu
        _timeSinceLastIncrease += Time.deltaTime;
        if (_timeSinceLastIncrease >= _increaseInterval)
        {
            IncreasePipeSpeed();
            DecreaseMaxTime();
            _timeSinceLastIncrease = 0f;
        }

        // Spawn roket setelah interval tertentu
        _rocketSpawnTimer += Time.deltaTime;
        if (_rocketSpawnTimer >= _rocketSpawnInterval)
        {
            SpawnRocket(); // Panggil metode untuk spawn roket
            _rocketSpawnTimer = 0f;  // Reset timer spawn roket
        }
    }

    // Contoh metode untuk memulai game
    public void StartGame()
    {
        _mainMenu.SetActive(false);
        Time.timeScale = 1f;
        score = 0;
        level = 1;
    }

    // Contoh metode untuk mengupdate skor
    public void UpdateScore(int points)
    {
        score += points;
        _scoreText.text = "Score: " + score.ToString();

        // Ganti latar belakang jika skor kelipatan 20
        if (score % 20 == 0)
        {
            _backgroundManager.ChangeBackground();
        }
    }

    // Contoh metode untuk naik level
    public void LevelUp()
    {
        level++;
    }

    public void GameOver(){
        score = 0;
        _gameOver.SetActive(true);
        Time.timeScale = 0f;
        _maxTime = 3f;
        _speedPipe = 1.65f;
        Debug.Log("Game Over!");
    }

    public void RestartGame(){
        _scoreText.text = "Score: " + score.ToString();
        _timeSinceLastIncrease = 0f;
        _gameOver.SetActive(false);
        Time.timeScale = 1f;
        ClearPipePool();
        _player.transform.position = _posStartPlayer;
    }

    void SpawnPipe(){
        Vector2 spawnPos = transform.position + new Vector3(0, Random.Range(-_heightrange, _heightrange));
        GameObject pipe = _pipe[GetWeightedRandomIndex(new float[] { 0.8f, 0.2f })];
        GameObject newPipe = Instantiate(pipe, spawnPos, Quaternion.identity);
        newPipe.transform.parent = _pipePool.transform;
        newPipe.GetComponent<MoveObject>().SetSpeed(_speedPipe); // Pastikan ada komponen Pipe yang mengatur kecepatan
        Destroy(newPipe, 10f);
    }

    void SpawnRocket()
    {
        float screenTop = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)).y;
        float screenBottom = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).y;

        // Tentukan posisi spawn dengan X parent dan Y acak antara batas atas dan bawah layar
        Vector2 spawnPos = new Vector2(_rocketPool.transform.position.x, Random.Range(screenBottom, screenTop));

        // Pilih roket dari pool dan spawn
        GameObject rocket = _rocketManager.GetRocket();  // Ambil prefab roket dari RocketManager
        GameObject newRocket = Instantiate(rocket, spawnPos, Quaternion.Euler(0, 0, 90));

        newRocket.transform.parent = _rocketPool.transform;
        Destroy(newRocket, 10f);
    }

    void IncreasePipeSpeed()
    {
        _speedPipe += _speedIncreaseRate;
    }

    void DecreaseMaxTime()
    {
        if (_maxTime > _minTime)
        {
            _maxTime -= _timeDecreaseRate;
        }
    }

    public void ClearPipePool()
    {
        // Pastikan PipePool tidak null
        if (_pipePool == null)
        {
            Debug.LogWarning("PipePool belum diatur. Pastikan PipePool diset di Inspector.");
            return;
        }

        // Loop melalui semua child PipePool dan hancurkan
        foreach (Transform child in _pipePool.transform)
        {
            Destroy(child.gameObject);
        }

        Debug.Log("Semua pipe dalam PipePool telah dihapus.");
    }

    int GetWeightedRandomIndex(float[] weights)
    {
        float totalWeight = 0f;
        foreach (float weight in weights)
        {
            totalWeight += weight;
        }

        float randomValue = Random.value * totalWeight; // Random float antara 0 dan totalWeight
        float cumulativeWeight = 0f;

        for (int i = 0; i < weights.Length; i++)
        {
            cumulativeWeight += weights[i];
            if (randomValue < cumulativeWeight)
            {
                return i;
            }
        }

        return weights.Length - 1; // Default ke indeks terakhir jika terjadi kesalahan
    }
}
