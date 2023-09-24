using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyBulletScript : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] Transform target; 

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        target = GameObject.Find("Enemy").transform.GetChild(0).GetChild(0).GetChild(0).transform;

        rb.AddForce(-target.right * 2000, ForceMode.Force);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Physics.CheckSphere(transform.position, 0.4f, LayerMask.GetMask("Ground", "Wall")))
        {
            Destroy(gameObject);
        }
        Destroy(gameObject, 5f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // Decrease enemy health, explode bullet
            GameObject.Find("GameManager").GetComponent<GameManager>().hp -= 25;
            Destroy(gameObject);
        }
    }
}
