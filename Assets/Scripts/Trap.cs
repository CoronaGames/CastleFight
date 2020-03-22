using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;

public class Trap : MonoBehaviour
{
    [SerializeField] CircleCollider2D myCircleCollider;
    [SerializeField] float radius;
    [SerializeField] Animator animator;
    [SerializeField] float damage;
    
    // Start is called before the first frame update
    void Start()
    {
        myCircleCollider = GetComponent<CircleCollider2D>();
        animator = GetComponent<Animator>();
        myCircleCollider.enabled = false;
    }


    public void TriggerTrap()
    {
        transform.localScale = new Vector3(radius, radius);
        animator.SetTrigger("Trigger");
        myCircleCollider.enabled = true;
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
       
       if (other.GetComponent<Health>())
       {
           other.GetComponent<Health>().TakeDamage(damage);
       }
        
    }

    public void Destroy()
    {
        if (transform.parent)
        {
            Destroy(transform.parent.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
