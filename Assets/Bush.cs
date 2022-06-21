using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bush : MonoBehaviour
{

    [SerializeField] float food = 100;
    [SerializeField] GameObject sound;
    [SerializeField] Sprite newSprite;
    byte state = 0; //State 0 significa que esta sin comer. 1 significa que ya esta  comido.

    SpriteRenderer mySpriteRenderer;
    

    void Awake() {
        mySpriteRenderer = this.GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D collition) {
        if (collition.gameObject.CompareTag("Player") && state == 0) {
            mySpriteRenderer.sprite = newSprite;
            playerNeeds pNeeds = collition.gameObject.GetComponent<playerNeeds>();
            pNeeds.hunger -= food;            
            pNeeds.playEatSound();
            state = 1;             
        }
    }

}
