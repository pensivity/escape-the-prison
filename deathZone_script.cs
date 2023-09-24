using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deathZone_script : MonoBehaviour
{
    [SerializeField] private GameObject boom;

    [SerializeField] private GameManager gm;

    private bool destroyed;

    // Start is called before the first frame update
    void Start()
    {
        boom = transform.GetChild(0).gameObject;
        
        boom.SetActive(false);

        gm = GameObject.Find("GameManager").GetComponent<GameManager>();

        destroyed = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (destroyed)
        {
            gm.hp = 0;
            destroyed = false;
            StartCoroutine(Kaboom());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            destroyed = true;

        }
    }

    IEnumerator Kaboom()
    {
        boom.SetActive(true);

        yield return new WaitForSeconds(1);

        boom.SetActive(false);
    }
}
