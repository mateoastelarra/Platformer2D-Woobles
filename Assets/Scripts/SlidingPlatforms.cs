using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingPlatforms : MonoBehaviour
{
    [SerializeField] float slidingSpeed = 2f;
    bool slidingLeft = false; 

    Rigidbody2D myRB2D;

    void Start() 
    {
        myRB2D = GetComponent<Rigidbody2D>();
        myRB2D.velocity = new Vector2(slidingSpeed,0);
    }
    void Update()
    {
         
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        if (other.gameObject.tag == "Platform")
        {
            if (!slidingLeft)
            {
                myRB2D.velocity = new Vector2 (-slidingSpeed, 0);
                slidingLeft = true;
            }
            else
            {
                myRB2D.velocity = new Vector2 (slidingSpeed, 0);
                slidingLeft = false;
            }
            
        }
        else
        {
            if (slidingLeft)
            {
                myRB2D.velocity = new Vector2 (-slidingSpeed, 0);
            }
            else
            {
                myRB2D.velocity = new Vector2 (slidingSpeed, 0);
            }
        }    
    }
}
