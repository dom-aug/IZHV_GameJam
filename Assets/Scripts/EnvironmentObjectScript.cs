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
        if (!GameManagerScript.Instance.gameStarted) {
            return;
        }
        
        if (transform.position.z < -300) {
            Destroy(gameObject);
        }

        speed = GameManagerScript.Instance.gameSpeed;
        transform.position += direction * speed * Time.deltaTime;
    }

    void OnBecameInvisible() {
        //Destroy(gameObject);
    }

     // despawn on collision with despawn layer
    /* void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Despawn")) {            
            Destroy(gameObject);
        }
    } */

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("GroundTexture")) {
            Destroy(gameObject);
        }
    }
}
