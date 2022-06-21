using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerSensor : MonoBehaviour
{
    public UnityEvent<GameObject> onPlayerEnter;
    public UnityEvent<GameObject> onPlayerExit;
    [SerializeField] string tagToCompare;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(tagToCompare))
        {
            onPlayerEnter.Invoke(collision.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(tagToCompare))
        {
            onPlayerExit.Invoke(collision.gameObject);
        }
    }
}
