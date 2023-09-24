using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthItem : MonoBehaviour
{
    private float speed = 1f;
    private float xRotation;
    private float yRotation;
    private float zRotation;
    private float yCenter;

    private bool used;

    [SerializeField] GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        yCenter = transform.position.y;
        used = false;
    }

    private void Update()
    {
        transform.position = new Vector3(transform.position.x, yCenter + Mathf.PingPong(Time.time * speed, 1f), transform.position.z);
        transform.Rotate(new Vector3(xRotation, yRotation, zRotation) * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && gm.lives < gm.maxLives && !used)
        {
            gm.lives++;
            gm.livesImgs.transform.GetChild(gm.lives).gameObject.SetActive(true);

            StartCoroutine(UseUp());
        }
    }

    IEnumerator UseUp()
    {
        used = true;
        GetComponent<SphereCollider>().enabled = false;
        GetComponent<MeshRenderer>().enabled = false;
        yield return new WaitForSeconds(10);

        GetComponent<SphereCollider>().enabled = true;
        GetComponent<MeshRenderer>().enabled = true;
        used = false;
    }
}
