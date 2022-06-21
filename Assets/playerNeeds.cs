using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FMODUnity;

public class playerNeeds : MonoBehaviour
{

    GameState gameState;
    //Sound stuff
    [SerializeField] StudioEventEmitter eatSFXEmitter;
    [SerializeField] StudioEventEmitter drinkSFXEmitter;
    //End  sound stuff
    public float hunger = 0;
    public float thirst = 0;

    [SerializeField] int maxHunger = 1200;
    [SerializeField] int maxThirst = 1200;

    [SerializeField] float hungerInc = 0.3f;
    [SerializeField] float thirstInc = 0.3f;

    [SerializeField] float sprintIncMultiplier = 3f;

    [SerializeField] Slider hungerSlider;
    [SerializeField] Slider thirstSlider;

    Image hungerFillArea;
    Image thirstFillArea;

    void Start() {
        gameState = FindObjectOfType<GameState>();        
    }

    void Update()
    {
        if (gameState.state == State.Play || gameState.state == State.Pause) {

            if (!hungerSlider){
                hungerSlider = GameObject.Find("HungerSlider").GetComponent<Slider>();
                hungerFillArea = hungerSlider.gameObject.transform
                    .Find("Fill Area").Find("Fill").GetComponent<Image>();
            }
            if (!thirstSlider){
                thirstSlider = GameObject.Find("ThirstSlider").GetComponent<Slider>();
                thirstFillArea = thirstSlider.gameObject.transform
                    .Find("Fill Area").Find("Fill").GetComponent<Image>();
            }

            if (hunger < 0) hunger = 0;
            if (thirst < 0) thirst = 0;

            bool isSprinting = this.GetComponent<playerMovement>().isSprinting;

            hunger += hungerInc * (isSprinting ? sprintIncMultiplier : 1);
            thirst += thirstInc * (isSprinting ? sprintIncMultiplier : 1);

            hungerSlider.value = 1 - hunger / maxHunger;
            thirstSlider.value = 1 - thirst / maxThirst;

            if (hunger >= maxHunger)
            {
                gameState.gameOver("Has muerto de hambre :(");
            }
            if (thirst >= maxThirst)
            {
                gameState.gameOver("Has muerto de sed :(");
            }

            if (hunger / maxHunger > 0.75f) {
                hungerFillArea.color = Color.red;
            } else {
                hungerFillArea.color = new Color32(0xEA, 0x59, 0x24, 0xFF);
            }

            if (thirst / maxThirst > 0.75f) {
                thirstFillArea.color = Color.red;
            } else {
                thirstFillArea.color = new Color32(0x0E, 0x7F, 0xAA, 0xFF);
            }

        }

    }

    public void alert() {
        print("ALERTA!");
    }
    public void playEatSound() 
    {
        eatSFXEmitter.Play();
    }
    public void playDrinkingSound() 
    {
        drinkSFXEmitter.Play();        
        drinkSFXEmitter.OverrideAttenuation = true;        
    }
    public bool isDrinkingPlaying() 
    {
        return drinkSFXEmitter.IsPlaying();
    }

}

