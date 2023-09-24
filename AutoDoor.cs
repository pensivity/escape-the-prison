using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDoor : MonoBehaviour
{
    [SerializeField] Animator anim;
    private float startHeight;

    // Start is called before the first frame update
    void Start()
    {
        startHeight = transform.position.y;
        anim = GetComponent<Animator>();
        anim.SetBool("approaching", false);
    }

    private void Update()
    {
        if (transform.position.y >= startHeight + 6)
        {
            gameObject.SetActive(false);
        } else
        {
            gameObject.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Raise the door up.
            anim.SetBool("approaching", true);
        }
    }
}
