using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFollow : MonoBehaviour
{
    public GameObject player;
    private GameState gameState;
    private RectTransform oscuridad;

    public float introDelay;
    public float introYOffset;
    public float introZoom;
    public float introDarkness;

    private float inGameZoom;
    private Vector2 inGameDarkness;
    private Vector3 initialPosition;

    int state = 0; //0 posicion de menu; 1 posicion in game
    private void Start()
    {
        gameState = FindObjectOfType<GameState>();
    }

    public void Init() {
        gameState = FindObjectOfType<GameState>();
        oscuridad = GameObject.Find("oscuridad").GetComponent<RectTransform>();

        // centrar oscuridad en conejo
        if (!player) {
            player = FindObjectOfType<playerMovement>().gameObject;
        }
        oscuridad.position = - new Vector3(0, introYOffset, 0);

        inGameZoom = GetComponent<Camera>().orthographicSize;
        inGameDarkness = oscuridad.localScale;

        GetComponent<Camera>().orthographicSize = introZoom;
        oscuridad.localScale = new Vector2(1,1) * introDarkness;

        initialPosition = transform.position + new Vector3(0, introYOffset, 0);
        transform.position = initialPosition;
    }

    public void StartFollowing() {
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        for (float t = 0; t < introDelay; t += 0.1f) {
            Vector3 targetPos = new Vector3(player.transform.position.x, player.transform.position.y, -10);
            this.transform.position = Vector3.Lerp(initialPosition, targetPos, t);
            oscuridad.localScale = Vector2.Lerp(new Vector2(1,1) * introDarkness, inGameDarkness, t);
            oscuridad.position = Vector2.Lerp(new Vector3(0, introYOffset, 0), new Vector3(0, 0, 0), t);
            GetComponent<Camera>().orthographicSize = Mathf.Lerp(introZoom, inGameZoom, t);
            yield return new WaitForSeconds(0.05f);
        }
        state = 1;
        gameState.state = State.Play;
    }

    public void _Update()
    {
        if (player && gameState.state == State.Play && state == 1) {
            this.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);
        }
    }
}
