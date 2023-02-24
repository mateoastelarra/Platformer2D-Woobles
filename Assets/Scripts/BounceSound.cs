using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceSound : MonoBehaviour
{
    [SerializeField] AudioClip bouncySound;
    [SerializeField] float bouncySoundVolume = 1f;
    AudioSource audioSource;
    Rigidbody2D bouncingTilemapRB2D;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        bouncingTilemapRB2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        PlayBouncyJump();
    }

    private void PlayBouncyJump()
    {
        if (bouncingTilemapRB2D.IsTouchingLayers(LayerMask.GetMask("Player")))
        {
            Vector3 position = transform.position;
            position.z = -10;
            AudioSource.PlayClipAtPoint(bouncySound, position, bouncySoundVolume);
        }
    }
}
