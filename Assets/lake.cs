using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lake : MonoBehaviour
{
    [SerializeField ]float thirstReduction = 5f;
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) 
        {
            playerNeeds pNeeds = collision.gameObject.GetComponent<playerNeeds>();
            pNeeds.thirst -= thirstReduction;
            if (!pNeeds.isDrinkingPlaying()) 
            {
                print("Drinking not playing. Play sound");
                pNeeds.playDrinkingSound();
            }
        }
    }
}
