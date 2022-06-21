using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    [SerializeField] int loudness = 16;
    [SerializeField] float soundSpeed = 2f;
    [SerializeField] float initialSize = 4f;
    public float currentSize;
    private SpriteRenderer sprite;
    


    void Awake() {
        sprite = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentSize = initialSize;
        AudioSource audio = GetComponent<AudioSource>();
        audio.spatialBlend = 1;
        audio.rolloffMode = AudioRolloffMode.Linear;
        audio.maxDistance = loudness / 2;
        audio.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentSize < loudness)
        {
            currentSize += soundSpeed;
            
            transform.localScale = new Vector2(currentSize, currentSize);
            // this.GetComponent<CircleCollider2D>().radius = currentSize;

            float r = Mathf.Clamp01(currentSize/loudness);
            if (r > 0.75) {
                //Empieza a desvanecerse
                Color color = sprite.material.color;
                color.a = 1 - (r - 0.75f) * 4;
                sprite.material.color = color;
            }
        } else {
            Destroy(this.gameObject, 1f);
        }
    }
}
