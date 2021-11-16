using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float _speed = 3.5f;
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _tripleShot;
    [SerializeField] int _lives = 3;
    [SerializeField] private GameObject _shieldPowerup;
    [SerializeField] private GameObject _gameOverText;
    [SerializeField] private int _score;
    [SerializeField] private AudioClip _laserSoundClip;
    private AudioSource _audioSource;
    [SerializeField]
    private GameObject _rightEngine, _leftEngine;
    private UIManager _uiManager;

    private bool _isTripleShotActive = false;
    private bool _isSpeedPowerupActive = false;
    private bool _isShieldPowerupActive = false;
    private SpawnManager _spawnManager;
    private float _fireRate = 0.15f;
    private float _canFire = -1f;
    private Vector3 offSet = new Vector3(0, 1.05f, 0);

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);

    }
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if (_uiManager == null)
        {
            Debug.LogError("The UI Manager is NULL.");
        }

        if (_audioSource == null)
        {
            Debug.LogError("AudioSource on the player is NULL!");
        }
        else
        {
            _audioSource.clip = _laserSoundClip;
        }

        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL.");
        }
    }

    void Update()
    {

        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }
    }

    void CalculateMovement()
    {

        float HorizontalInput = Input.GetAxis("Horizontal");
        float VerticalInput = Input.GetAxis("Vertical");
            
        Vector3 orient = new Vector3(HorizontalInput, VerticalInput, 0);
        if (_isSpeedPowerupActive)
            transform.Translate(orient * _speed*2 * Time.deltaTime);
        else
            transform.Translate(orient * _speed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);
        if (transform.position.x > 11)
        {
            transform.position = new Vector3(-11, transform.position.y, 0);
        }
        else if (transform.position.x < -11)
        {
            transform.position = new Vector3(11, transform.position.y, 0);
        }
    }

    void FireLaser()
    {
        _canFire = Time.time + _fireRate;
        if (_isTripleShotActive)
            Instantiate(_tripleShot, transform.position, Quaternion.identity);
        else
            Instantiate(_laserPrefab, transform.position + offSet, Quaternion.identity);
        _audioSource.Play();
    }

    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    public void SpeedPowerupActive()
    {
        _isSpeedPowerupActive = true;
        StartCoroutine(SpeedPowerUpDownRoutine());
    }

    public void ShieldPowerupActive()
    {
        _isShieldPowerupActive = true;
        _shieldPowerup.SetActive(true);
       // StartCoroutine(ShieldPowerUpDownRoutine());
    }

    IEnumerator ShieldPowerUpDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isShieldPowerupActive = false;
    }

    IEnumerator SpeedPowerUpDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isSpeedPowerupActive = false;
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }

    public void Damage()
    {
        if (_isShieldPowerupActive)
        {
            _isShieldPowerupActive = false;
            _shieldPowerup.SetActive(false);
            return;
        }
        _lives--;

        if (_lives == 2)
        {
            _leftEngine.SetActive(true);
        }
        else if (_lives == 1)
        {
            _rightEngine.SetActive(true);
        }

        _uiManager.UpdateLives(_lives);
        if (_lives == 0)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }
}
