using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Game Trackers")]
    public int spawnHP;
    public int hp;
    public int maxLives;
    public int lives;
    public int spawnAmmo;
    public int ammo;
    public int maxMags;
    public int mags;

    [Header("HUD")]
    [SerializeField] Slider healthBar;
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] public GameObject livesImgs;
    [SerializeField] Slider ammoBar;
    [SerializeField] TextMeshProUGUI ammoText;
    [SerializeField] public GameObject magsImgs;

    [Header("Player-related")]
    [SerializeField] Transform player;
    [SerializeField] public Transform respawnPoint;
    private bool isDead;

    [Header("Pause")]
    [SerializeField] GameObject pauseMenuUI;
    public bool isPaused = false;

    // Start is called before the first frame update
    void Awake()
    {
        spawnHP = 100;
        hp = spawnHP;
        maxLives = 3;
        lives = maxLives;
        spawnAmmo = 25;
        ammo = spawnAmmo;
        maxMags = 2;
        mags = maxMags;

        healthText = GameObject.Find("Health_text").GetComponent<TextMeshProUGUI>();
        ammoText = GameObject.Find("Ammo_text").GetComponent<TextMeshProUGUI>();
        livesImgs.transform.GetChild(1).gameObject.SetActive(true);
        livesImgs.transform.GetChild(2).gameObject.SetActive(true);
        livesImgs.transform.GetChild(3).gameObject.SetActive(true);
        magsImgs.transform.GetChild(1).gameObject.SetActive(true);
        magsImgs.transform.GetChild(2).gameObject.SetActive(true);

        player = GameObject.Find("FirstPerson").GetComponent<Transform>();
        respawnPoint = GameObject.Find("RespawnPoint").GetComponent<Transform>();

        pauseMenuUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.value = hp;
        healthText.text = string.Format("Health: {0}", hp);

        ammoBar.value = ammo;
        ammoText.text = string.Format("Ammo: {0}", ammo);

        // You Died
        if (!isDead && (hp <= 0 || player.position.y <= -20))
        {
            if (lives > 0)
            {
                StartCoroutine(Death());
            } else
            {
                SceneManager.LoadScene("04_YouLose");
            }
        }

        // Pause Menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    void PauseGame()
    {
        Time.timeScale = 0f;
        isPaused = true;
        pauseMenuUI.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
        pauseMenuUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void QuitGame()
    {
        SceneManager.LoadScene("00_StartMenu");
    }

    IEnumerator Death()
    {
        isDead = true;
        yield return new WaitForSeconds(0.5f);

        hp = spawnHP;
        ammo = spawnAmmo;
        player.position = respawnPoint.position;
        livesImgs.transform.GetChild(lives).gameObject.SetActive(false);
        lives--;
        hp = spawnHP;
        ammo = spawnAmmo;
        player.position = respawnPoint.position;
        isDead = false;
    }
}
