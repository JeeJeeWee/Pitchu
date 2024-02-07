using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemieScript : MonoBehaviour
{
    public float Hp;
    public float knockbackTime;
    private float currentHp;
    private float currentKnockbackTime; 

    private bool isHit;

    public ParticleSystem enemieHitParticle;
    public ParticleSystem enemieDeathParticle;

   

    private Rigidbody2D rbEnemie;

    // Start is called before the first frame update
    void Start()
    {
        rbEnemie = GetComponent<Rigidbody2D>();
        currentHp = Hp;

        isHit = false;
    }

    // Update is called once per frame
    void Update()
    {
        StopForce();
    }

    ///public void GetHit(int damage, float knockbackX, float knockbackY)
    public void GetHit(int damage, float knockbackX, float knockbackY)
    {
        isHit = true;
        currentHp -= damage;

        if(currentHp <= 0)
        {
            Die();
        }

        rbEnemie.velocity = new Vector2(rbEnemie.velocity.x + knockbackX, rbEnemie.velocity.y + knockbackY);
        enemieHitParticle.Play();

        currentKnockbackTime = knockbackTime;
    }

    private void StopForce()
    {
        if(currentKnockbackTime > 0)
        {
            currentKnockbackTime -= Time.deltaTime;
        }

        if(currentKnockbackTime <= 0 && isHit == true)
        {
            isHit = false;
            rbEnemie.velocity = new Vector2(0,0);
            return;
        }
    }

    private void Die()
    {
        enemieDeathParticle.Play();        
    }
}
