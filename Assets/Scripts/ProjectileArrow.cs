using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;

public class ProjectileArrow : MonoBehaviour
{

    public Rigidbody2D myRigidbody;
    Transform startPosition;
    public Health target;
    public float projectileDamage = 10;
    private float projectileSpeed = 10f;
    private Vector3 targetPosition;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        if(target != null)
        {
            transform.position = Vector2.MoveTowards(gameObject.transform.position, target.transform.position, projectileSpeed * Time.deltaTime);
            targetPosition = new Vector3(target.transform.position.x, target.transform.position.y, target.transform.position.z);
        }
        else
        {
            // transform.position = Vector2.MoveTowards(gameObject.transform.position, targetPosition, projectileSpeed * Time.deltaTime);
            DestroyObject();
        }
    }
        


    public void SetTarget(Health target)
    {
        this.target = target;
    }

    public void SetProjectileDamage(float projectileDamage)
    {
        this.projectileDamage = projectileDamage;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((other != null) && (target != null) && (gameObject != null))
        {
            if (other.gameObject == target.gameObject)
            {
                target.TakeDamage(projectileDamage);
                targetPosition = new Vector3(other.gameObject.transform.position.x, other.gameObject.transform.position.y, other.gameObject.transform.position.z);
                DestroyObject();

            }
        }
        else
        {
            DestroyObject();
        }
    }

 

    public void SetRotation()
    {
        var vector = transform.position - target.gameObject.transform.position;
        var angle = Mathf.Atan2(vector.y, vector.x);
        angle *= 57.2957795f;
        gameObject.transform.rotation = Quaternion.Euler(0f,0f,Mathf.Abs(angle));
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }


}
