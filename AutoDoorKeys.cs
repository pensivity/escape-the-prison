using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDoorKeys : MonoBehaviour
{
    [SerializeField] Animator anim;
    private float startHeight;
    [SerializeField] PlayerMovement player;

    // Start is called before the first frame update
    void Start()
    {
        startHeight = transform.position.y;
        anim = GetComponent<Animator>();
        anim.SetBool("approaching", false);
        anim.SetBool("unlocked", false);

        player = GameObject.Find("FirstPerson").GetComponent<PlayerMovement>();
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

            if (player.hasKey)
            {
                anim.SetBool("unlocked", true);

                // Destroy the key
                Destroy(GameObject.Find("Key"));
                player.hasKey = false;
            } else
            {
                player.popUpTextCanvas.SetActive(true);
                player.popUpText.text = "You need a key!";
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.popUpTextCanvas.SetActive(false);
        }
    }
}
