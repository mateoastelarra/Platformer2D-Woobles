using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float enemySpeed = 1f;
    [SerializeField] float dyingTime = 1f;
    [SerializeField] float dyingDissapearingSpeed = 2f;
    [SerializeField] AudioClip dyingSound;


    [Header("Dissappear Property")]
    [SerializeField] bool hasDissapearAndReappear = true;
    [SerializeField] float dissapearSpeed = 0.5f;
    [SerializeField] float dissappearingLimit = 0.3f;
    Rigidbody2D enemyRB2D;
    SpriteRenderer enemySpriteRenderer;
    bool isDissappearing;
    bool isDying = false;
    void Start()
    {
        enemyRB2D = GetComponent<Rigidbody2D>();
        enemySpriteRenderer = gameObject.GetComponent<SpriteRenderer>();;
    }

    
    void Update()
    {
        enemyRB2D.velocity = new Vector2 (enemySpeed, enemyRB2D.velocity.y);
        if (hasDissapearAndReappear)
        {
            if (enemySpriteRenderer.color.a >= 1f - Mathf.Epsilon)
            {
                isDissappearing = true;
            }
            else if (enemySpriteRenderer.color.a <= dissappearingLimit + Mathf.Epsilon)
            {
                isDissappearing = false;
            }
            DissapearAndReappear(isDissappearing, dissapearSpeed);
        }
        if (isDying)
        {
            Dissapear(dyingDissapearingSpeed);
            enemyRB2D.velocity = new Vector2(0f, 0f);
        }
        
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Platform")
        {
            FlipEnemyFacing();
            enemySpeed = -enemySpeed;
        }
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.tag == "PlayerFeet" && FindObjectOfType<PlayerMovement>().playerIsAlive)
        {
            gameObject.GetComponent<CircleCollider2D>().enabled = false;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
            Vector3 position = transform.position;
            position.z = -10;
            AudioSource.PlayClipAtPoint(dyingSound, position);
            isDying = true;
            StartCoroutine(Die());
        }
    }

    void FlipEnemyFacing()
    {
        transform.localScale = new Vector2 (-Mathf.Sign(enemyRB2D.velocity.x) * Mathf.Abs(transform.localScale.x), transform.localScale.y);
    }

    void Dissapear(float speed)
    {
        Vector4 actualColor = enemySpriteRenderer.color;
        enemySpriteRenderer.color = actualColor + new Vector4 (0, 0, 0, - speed * Time.deltaTime);
    }

    void Reappear()
    {
        Vector4 actualColor = enemySpriteRenderer.color;
        enemySpriteRenderer.color = actualColor + new Vector4 (0, 0, 0, dissapearSpeed * Time.deltaTime);
    }

    void Rotate()
    {
        Quaternion actualRotation = transform.localRotation;
        actualRotation.z = 100 * Time.deltaTime;
        transform.localRotation = actualRotation;
    }

    void DissapearAndReappear(bool isDissappearing, float speed)
    {
        if (isDissappearing)
        {
            Dissapear(speed);
        }
        else
        {
            Reappear();
        }
    }

    IEnumerator Die()
    {
        yield return new WaitForSecondsRealtime(dyingTime);
        Destroy(gameObject);
    }
}
