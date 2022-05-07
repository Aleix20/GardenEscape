using UnityEngine;
using UnityEngine.UI;

public class grabController : MonoBehaviour
{

    public Transform grabDetect;
    public Transform boxHolder;
    public float rayDist;

    private Vector3  lastPosition;
    private float waitTime;

    private float fixTime = 3.0f;

    bool itemGrabbed = false;

    public Image ringHealthBar;


    float health, maxHealth = 300;
    float lerpSpeed;
    private void Start()
    {
        health = 0;
    }
    // Update is called once per frame
    void Update()
    {
        Debug.Log("GrabDetect pos is: " + grabDetect.position);
        ringHealthBar.rectTransform.position = grabDetect.position;
        Debug.Log("ringHealthBar pos is: " + ringHealthBar.rectTransform.position);
        if (health > maxHealth) health = maxHealth;

        lerpSpeed = 3f * Time.deltaTime;

        HealthBarFiller();
        ColorChanger();

        RaycastHit grabCheck;
        Physics.Raycast(grabDetect.position, Vector3.down, out grabCheck ,rayDist );
        Debug.Log("WaitTime is: " + waitTime);
        Debug.Log(grabCheck.collider.tag);
        if (grabCheck.collider != null  )
        {
            if (grabCheck.collider.tag == "Box" && !itemGrabbed)
            {
                
                health = 0;
                Heal(waitTime);
            }

            float distToLastPos = Vector3.Distance(grabDetect.position, lastPosition);
            if (grabCheck.collider.tag == "Box" && itemGrabbed )
            {
                health = 300;
                Damage(waitTime);
            }
         
            if (grabCheck.collider.tag == "Box" && waitTime >= fixTime && !itemGrabbed)
            {
                ringHealthBar.enabled = false;
                Debug.Log("Pick up box");
                itemGrabbed = !itemGrabbed;
                waitTime = 0.0f;

            }
            else if (grabCheck.collider.tag == "Box" && waitTime >= fixTime && itemGrabbed)
            {
                Debug.Log("Drop box");
                itemGrabbed = !itemGrabbed;
                waitTime = 0.0f;
            }

            
            if (itemGrabbed )
            {
                grabCheck.collider.gameObject.transform.parent = boxHolder;
                grabCheck.collider.gameObject.transform.position = boxHolder.position;
                grabCheck.collider.gameObject.GetComponent<Rigidbody>().isKinematic = true;

            }
            else if(!itemGrabbed) 
            {
                
                grabCheck.collider.gameObject.transform.parent = null;
                grabCheck.collider.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            }
        }
    }
    void LateUpdate()
    {

        float distToLastPos = Vector3.Distance(grabDetect.position, lastPosition);

        if (distToLastPos < 0.3f)
        {
            
            waitTime += Time.deltaTime;
            
        }
        else
        {
            waitTime = 0.0f;
            lastPosition = grabDetect.position;
        }

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
        {
            ringHealthBar.enabled = true;
            health -= (damagePoints * 100);
        }
        else if (health <=0)
        {
            ringHealthBar.enabled = false;
        }

    }
    public void Heal(float healingPoints)
    {

        if (health < maxHealth)
        {
            ringHealthBar.enabled = true;
            health += (healingPoints * 100);

        }else if (health >= maxHealth)
        {
            ringHealthBar.enabled = false;
        }
    }
}
