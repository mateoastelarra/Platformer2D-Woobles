using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class melonPickup : MonoBehaviour
{
    [SerializeField] AudioClip melonSound;
    [SerializeField] float melonSoundVolume = 1f;
    [SerializeField] int pointsForMelonPickUp = 1000;
    

    [SerializeField] bool wasPickedUp = false;

    void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.tag == "Player" && !wasPickedUp)
        {
            wasPickedUp = true;
            Vector3 position = transform.position;
            position.z = -10;
            AudioSource.PlayClipAtPoint(melonSound, position, melonSoundVolume);
            FindObjectOfType<LevelFinish>().GetWaterMelon();
            Destroy(gameObject);
            FindObjectOfType<GameSession>().AddToScore(pointsForMelonPickUp);
        }    
    }
}
