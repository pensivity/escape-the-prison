using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] Transform target;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        target = GameObject.Find("FirstPerson").transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0);

        rb.AddForce(-target.right * 4000, ForceMode.Force);
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.CheckSphere(transform.position, 1f, LayerMask.GetMask("Ground", "Wall", "Ramp")))
        {
            Destroy(this.gameObject);
        }
        Destroy(this.gameObject, 3f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            // Decrease enemy health, explode bullet
            collision.gameObject.GetComponent<AI_script>().enemyHp -= 20;
            Destroy(this.gameObject);
        }
    }
}
