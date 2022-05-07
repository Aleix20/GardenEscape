using UnityEngine;
using UnityEngine.UI;
/* http://www.Mousawi.Dev By @AbdullaMousawi*/
public class Health : MonoBehaviour
{
   
    public Image  ringHealthBar;
    

    float health, maxHealth = 100;
    float lerpSpeed;

    private void Start()
    {
        health = 0;
    }

    private void Update()
    {
      
        if (health > maxHealth) health = maxHealth;

        lerpSpeed = 3f * Time.deltaTime;

        HealthBarFiller();
        ColorChanger();
    }

    void HealthBarFiller()
    {
        
        ringHealthBar.fillAmount = Mathf.Lerp(ringHealthBar.fillAmount, (health / maxHealth), lerpSpeed);

    }
    void ColorChanger()
    {
        Color healthColor = Color.Lerp(Color.red, Color.green, (health / maxHealth));
        ringHealthBar.color = healthColor;
    }



    public void Damage(float damagePoints)
    {
        if (health > 0)
            health -= damagePoints;
    }
    public void Heal(float healingPoints)
    {
        if (health < maxHealth)
            health += healingPoints;
    }
}
