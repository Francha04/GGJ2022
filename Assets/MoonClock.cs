using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoonClock : MonoBehaviour
{

    [SerializeField] Sprite[] fases;

    void Start()
    {
        GetComponent<Image>().sprite = fases[0];
    }

    public void changePhase(int p) {
        GetComponent<Image>().sprite = fases[p];
    }
}
