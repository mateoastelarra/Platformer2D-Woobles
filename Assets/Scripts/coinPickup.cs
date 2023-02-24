using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coinPickup : MonoBehaviour
{
    [SerializeField] AudioClip coinSound;
    [SerializeField] float coinSoundVolume = 0.5f;
    [SerializeField] int pointsForCoinPickUp = 100;

    bool wasPickedUp = false;

    void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.tag == "Player" && !wasPickedUp)
        {
            wasPickedUp = true;
            Vector3 position = transform.position;
            position.z = -10;
            AudioSource.PlayClipAtPoint(coinSound, position, coinSoundVolume);
            Destroy(gameObject);
            FindObjectOfType<GameSession>().AddToScore(pointsForCoinPickUp);
        }    
    }
}
