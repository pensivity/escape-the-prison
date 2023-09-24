using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBoost_script : MonoBehaviour
{
    [SerializeField] private float jumpBoostValue = 15f;
    private float originalJumpValue;
    [SerializeField] PlayerMovement fpsS;

    private void Start()
    {
        fpsS = GameObject.Find("FirstPerson").GetComponent<PlayerMovement>();
        originalJumpValue = fpsS.jump;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            fpsS.jump += jumpBoostValue;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            fpsS.jump = originalJumpValue;
        }
    }
}
