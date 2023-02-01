using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityButton : MonoBehaviour
{
    [SerializeField] private Image abilityImage;
    [SerializeField] private Ability ability;
    private bool isCooldown = false;
    private float castTime;
    private float castCooldown;
    private KeyCode keyPress;
    
    // Start is called before the first frame update
    void Start()
    {
        castTime = ability.castTime;
        castCooldown = ability.castCooldown;
        keyPress = ability.keyPress;
        abilityImage.sprite = ability.image;
        abilityImage.fillAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(keyPress) && !isCooldown)
        {
            isCooldown = true;
            abilityImage.fillAmount = 1;
        }

        if (isCooldown)
        {
            abilityImage.fillAmount -= 1 / (castCooldown + castTime) * Time.deltaTime; 
            
            if(abilityImage.fillAmount <= 0)
            {
                abilityImage.fillAmount = 0;
                isCooldown = false;
            }
        }
    }

    private void OnReset() {
        abilityImage.fillAmount = 0;
        isCooldown = false;
    }

    private void OnEnable() {
        GameManager.OnReset += OnReset;
    }
    private void OnDisable() {
        GameManager.OnReset -= OnReset;
    }
}
