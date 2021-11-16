using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] float _speed = 3.0f;
    [SerializeField] private AudioClip _clip;
    [SerializeField]
    private int powerupID;

    void Update()
    {
        CalculateMovement();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "Player")
        {
            AudioSource.PlayClipAtPoint(_clip, transform.position);
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                switch (powerupID)
                {
                    case 0: player.TripleShotActive(); break;
                    case 1: player.SpeedPowerupActive(); break;
                    case 2: player.ShieldPowerupActive(); break;
                    default: Debug.Log("Default Value"); break;
                }
            }
            Destroy(this.gameObject);
        }
    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y <= -5.3)
        {
            Destroy(gameObject);
        }
    }
}
