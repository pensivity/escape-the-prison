using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trapdoor_script : MonoBehaviour
{
    private float rotationSpeed = 10f;
    private float maxAngle = 90f;
    private bool isOpen = false;
    private bool opening = false;

    [SerializeField] private Transform startPos;

    private void Start()
    {
        startPos = transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            opening = true;
        }
    }

    // Do on trigger exit to flip it back into place

    private void Update()
    {
        if (opening)
        {
            float angle = Mathf.LerpAngle(transform.rotation.eulerAngles.z, maxAngle, Time.deltaTime * rotationSpeed);
            transform.rotation = Quaternion.Euler(0f, 0f, angle);

            if (!isOpen)
            {
                // close trap after 3 seconds
                StartCoroutine(OpenTrap());
            }
        }
    }

    IEnumerator OpenTrap()
    {
        isOpen = true;

        yield return new WaitForSeconds(3);

        isOpen = false;
        opening = false;
        transform.position = startPos.position;
        transform.rotation = Quaternion.Euler(0f, 0f, 180f);
    }
}
