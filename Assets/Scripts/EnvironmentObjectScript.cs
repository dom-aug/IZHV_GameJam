using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentObjectScript : MonoBehaviour
{
    public float speed = 10f;

    public Vector3 direction = new Vector3(0, 0, -1);

    private Rigidbody environmentObjectRigidbody;
    
    private BoxCollider environmentObjectBoxCollider;

    // Start is called before the first frame update
    void Start()
    {
        environmentObjectRigidbody = GetComponent<Rigidbody>();
        environmentObjectBoxCollider = GetComponent<BoxCollider>();

       // environmentObjectRigidbody.velocity = direction * speed;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    // despawn on collision with despawn layer
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.layer);
        if (collision.gameObject.layer == LayerMask.NameToLayer("Despawn")) {            
            Destroy(gameObject);
        }
    }
}
