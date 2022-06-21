using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class wolfKill : MonoBehaviour
{
    private GameState gameStateManager;    
    private void Awake()
    {
        gameStateManager = FindObjectOfType<GameState>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {            
            gameStateManager.gameOver("Te ha comido un lobo.");
        }
    }
}
