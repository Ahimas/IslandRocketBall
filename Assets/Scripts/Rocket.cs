using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    private Transform target;

    public float speed = 7f;
    public float rockectStrenght = 12f;
    public float timeOfLife = 5f;

  
    // Update is called once per frame
    void Update()
    {
        if ( target != null )
        {
            Vector3 moveDirection = (target.transform.position - transform.position).normalized;
            transform.position += moveDirection * speed * Time.deltaTime;
            transform.LookAt(target);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ( collision.gameObject.CompareTag(target.tag) )
        {
            Vector3 away = -collision.GetContact(0).normal;
            collision.collider.attachedRigidbody.AddForce(away * rockectStrenght, ForceMode.Impulse);
            Destroy(gameObject);
        }
    }

    public void Fire(Transform target) {
        this.target = target;
        Destroy(gameObject, timeOfLife);
    }
}
