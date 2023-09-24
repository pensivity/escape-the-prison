using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunScript : MonoBehaviour
{
    // NOTE: The gun can't have a collider (or the spawn point needs to be outside the collider) otherwise bullet will impact it immediately.
    [SerializeField] GameObject bullet;
    [SerializeField] Transform bulletTransform;
    private bool isReloading;
    private bool promptingReload;

    [SerializeField] GameManager gm;
    [SerializeField] PlayerMovement player;

    [SerializeField] Transform gunPos;

    // Start is called before the first frame update
    void Start()
    {
        bullet = Resources.Load("Prefabs/bullet") as GameObject;
        bulletTransform = transform.GetChild(0).transform;

        isReloading = false;
        promptingReload = false;

        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        player = GameObject.Find("FirstPerson").GetComponent<PlayerMovement>();
        gunPos = GameObject.Find("FirstPerson").transform.GetChild(0).GetChild(0).transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gm.isPaused && !isReloading) {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (gm.ammo > 0)
                {
                    Instantiate(bullet, bulletTransform.position, Quaternion.identity);
                    gm.ammo--;
                } else if (gm.mags > 0 && !promptingReload)
                {
                    StartCoroutine(reloadPrompt("Press R to reload!"));
                } else if (!promptingReload)
                {
                    StartCoroutine(reloadPrompt("Need more ammo!"));
                }
            }

            if (Input.GetKeyDown(KeyCode.R) && gm.mags > 0 && gm.ammo < gm.spawnAmmo)
            {
                StartCoroutine(reloading());
            }
        }
    }

    IEnumerator reloading()
    {
        isReloading = true;
        gm.magsImgs.transform.GetChild(gm.mags).gameObject.SetActive(false);
        gm.mags--;
        gunPos.localPosition = new Vector3(gunPos.localPosition.x, gunPos.localPosition.y - 0.2f, gunPos.localPosition.z - 0.2f);

        yield return new WaitForSeconds(1f);

        gunPos.localPosition = new Vector3(gunPos.localPosition.x, gunPos.localPosition.y + 0.2f, gunPos.localPosition.z + 0.2f);
        gm.ammo = gm.spawnAmmo;
        isReloading = false;
    }

    IEnumerator reloadPrompt(string promptText)
    {
        promptingReload = true;
        player.popUpTextCanvas.SetActive(true);
        player.popUpText.text = promptText;
        yield return new WaitForSeconds(0.8f);

        player.popUpTextCanvas.SetActive(false);
        promptingReload = false;
    }
}
