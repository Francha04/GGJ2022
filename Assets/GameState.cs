using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum State {Menu, Play, Pause, GameOver, Win}

public class GameState : MonoBehaviour
{

    [SerializeField] GameObject gameHub;
    [SerializeField] GameObject mainMenuPanel;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject winPanel;
    [SerializeField] musicManager musicM;

    [SerializeField] GameObject PlayerPrefab;
    [SerializeField] GameObject BushPrefab;
    [SerializeField] GameObject spawnM;
    [SerializeField] GameObject[] lobos;
    [SerializeField] int numberOfBushes = 100;
    [SerializeField] int MapWidth = 50;
    [SerializeField] int MapHeight = 40;
    [SerializeField] float gameTime = 120;
    [SerializeField] int phases = 4;
    private float initialDarknessScale = 1.25f;
    [SerializeField] float endDarknessScale = 1.75f;
    private float initialDarknessOpacity = 0.8f;
    [SerializeField] float endDarknessOpacity = 0.95f;
    public State state = State.Menu;
    private GameObject player;
    private cameraFollow mainCamera;
    
    private GameObject bushes;

    private GameObject laOscuridad;

    private Coroutine timeCoroutine;

    private lake[] lakes;

    bool onLake(float x, float y) {
        foreach (lake l in lakes) {
            if (l.gameObject.GetComponent<PolygonCollider2D>().OverlapPoint(new Vector2(x, y))) {
                return true;
            }
        }
        return false;
    }

    void Start() {
        // player = FindObjectOfType<playerMovement>().gameObject;
        mainCamera = FindObjectOfType<cameraFollow>();
        lakes = FindObjectsOfType<lake>();
        laOscuridad = GameObject.Find("oscuridad");
        initialDarknessScale = laOscuridad.transform.localScale.x;
        initialDarknessOpacity = laOscuridad.GetComponent<SpriteRenderer>().color.a;
        bushes = GameObject.Find("Bushes");
        int bushGroup = 0;                

        // Instanciar arbustos
        int i = 0;
        while (i < numberOfBushes) {
            bushGroup = Random.Range(1, 6) + Random.Range(1, 6);
            float x = Random.Range(-MapWidth/2, MapWidth/2);
            float y = Random.Range(-MapHeight/2, MapHeight/2);

            for (int j = 0; j < bushGroup; j++) {
                float ranAngle = Random.value * (Mathf.PI * 2);
                float distance = 2 + Random.value * 10;
                x += Mathf.Cos(ranAngle) * distance;
                y += Mathf.Sin(ranAngle) * distance;

                if (Mathf.Abs(x) > MapWidth/2 || 
                    Mathf.Abs(y) > MapHeight/2 ) {
                    break;
                }

                if (onLake(x, y)) {
                    break;
                }

                GameObject bush = Instantiate(BushPrefab, new Vector3(x,y,0), Quaternion.identity);
                if (bushes) {
                    bush.transform.parent = bushes.transform;
                }

                i++;
            }
        }

        // Instanciar conejo
        player = Instantiate(PlayerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        mainCamera.Init();
    }

    public void gameOver(string reason) {
        // if (state == State.Play) {
            state = State.GameOver;            
            gameOverPanel.GetComponent<FadeUI>().FadeIn();
            gameHub.GetComponent<FadeUI>().FadeOut();            
            Transform reasonText = gameOverPanel.transform.Find("Razon");
            reasonText.GetComponent<Text>().text = reason;
            musicM.loseGameMusic();
            spawnM.GetComponent<spawnManager>().stopWolfTimer();            
        //Matamos al conejito :(
        Destroy(player);

            StopCoroutine(timeCoroutine);
        // }
    }

    public void restartGame() {
        Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
    }

    public void startGame() {
        if (state != State.Play) {
            
            // UI
            mainMenuPanel.GetComponent<FadeUI>().FadeOut();
            gameHub.GetComponent<FadeUI>().FadeIn();

            // Instanciar lobos

            // Arrancar tiempo
            timeCoroutine = StartCoroutine(startTimer());

            //state = State.Play;
            FindObjectOfType<cameraFollow>().StartFollowing();

            // Inicia lobos
            spawnM.SetActive(true);
            spawnM.GetComponent<spawnManager>().startWolfTimer();
            foreach (GameObject lobo in lobos)
            {
                lobo.SetActive(true);
            }            
        }
    }

    public void pauseGame() {
        if (state == State.Play) {
            state = State.Pause;
            pausePanel.GetComponent<FadeUI>().FadeIn();
            Time.timeScale = 0f;
        }
    }

    public void unPauseGame() {
        if (state == State.Pause) {
            state = State.Play;
            pausePanel.GetComponent<FadeUI>().FadeOut();
            Time.timeScale = 1f;
        } 
        
    }

    void Update() {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Return)) {
            if (state == State.Pause)
            {
                unPauseGame();
            }
            else if (state == State.Play)
            {
                pauseGame();
            }
            }        
    }

    private IEnumerator startTimer()
    {
        float phaseTime = gameTime / phases;
        for (int i = 0; i < phases; i++) {
            float elapsed = ((float)i / phases);
            float scale = initialDarknessScale + (endDarknessScale - initialDarknessScale) * elapsed;
            RectTransform rt = laOscuridad.GetComponent<RectTransform>();
            rt.localScale = new Vector2(1,1) * scale;
            SpriteRenderer sr = laOscuridad.GetComponent<SpriteRenderer>();
            Color color = sr.color;
            color.a = endDarknessOpacity + (initialDarknessOpacity - endDarknessOpacity) * (1 - elapsed);
            sr.color = color;
            FindObjectOfType<MoonClock>().changePhase(i);
            yield return new WaitForSeconds(phaseTime);
        }
        winGame();
    }    

    private void winGame() {
        state = State.Win;
        gameHub.GetComponent<FadeUI>().FadeOut();
        winPanel.GetComponent<FadeUI>().FadeIn();
        Time.timeScale = 1f;
        //Borra los lobos
        spawnM.SetActive(false);
        foreach (wolfAI wolf in FindObjectsOfType<wolfAI>()) {
            Destroy(wolf.gameObject);
        }
    }

}
