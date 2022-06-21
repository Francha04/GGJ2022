using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSensor : MonoBehaviour
{
    [SerializeField] string tagToCompare;
    // private UnityEvent<GameObject> onSoundEnter;
    private string soundTag = "PlayerSound";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(soundTag)) {
            //FindObjectOfType<wolfAI>().onSoundEnter(collision.gameObject);
        }
    }
}
