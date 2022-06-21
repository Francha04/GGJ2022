using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfSound : MonoBehaviour
{
    [SerializeField] GameObject wolfSound;
    [SerializeField] GameObject wolfGrowl;

    public void growl() {
        Instantiate(wolfGrowl, transform);
    }

    private IEnumerator snarl()
    {

        int timeToWait = Random.Range(7, 40);
        yield return new WaitForSeconds(timeToWait);
        Instantiate(wolfSound, transform);
        StartCoroutine(snarl());
    }

   void Start()
    {
        StartCoroutine(snarl());
    }
}
