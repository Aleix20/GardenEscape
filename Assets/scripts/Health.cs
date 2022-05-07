using UnityEngine;
using UnityEngine.UI;
/* http://www.Mousawi.Dev By @AbdullaMousawi*/
public class Health : MonoBehaviour
{
   
    public Image  ringHealthBar1, ringHealthBar2;
    

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

        ringHealthBar1.fillAmount = Mathf.Lerp(ringHealthBar1.fillAmount, (health / maxHealth), lerpSpeed);
        ringHealthBar2.fillAmount = Mathf.Lerp(ringHealthBar2.fillAmount, (health / maxHealth), lerpSpeed);

    }
    void ColorChanger()
    {
        Color healthColor = Color.Lerp(Color.red, Color.green, (health / maxHealth));
        ringHealthBar1.color = healthColor;
        ringHealthBar2.color = healthColor;
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
