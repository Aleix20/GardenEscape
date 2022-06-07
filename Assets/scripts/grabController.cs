using System;
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

    bool boxGrabbed = false;
    bool bucketGrabbed = false;
    
    public Image ringHealthBar1, ringHealthBar2;
    private doorController door = null;
    private RaycastHit grabCheck;

    float health, maxHealth = 300;
    float lerpSpeed;
    private void Start()
    {
        health = 0;
    }
    // Update is called once per frame
    void Update()
    {
        //Debug.Log("GrabDetect pos is: " + grabDetect.position);
        ringHealthBar1.rectTransform.position = grabDetect.position;
        ringHealthBar2.rectTransform.position = grabDetect.position;
        //Debug.Log("ringHealthBar pos is: " + ringHealthBar.rectTransform.position);
        if (health > maxHealth) health = maxHealth;

        lerpSpeed = 3f * Time.deltaTime;

        HealthBarFiller();
        ColorChanger();

        
        Physics.Raycast(grabDetect.position, Vector3.down, out grabCheck ,rayDist );
        //Debug.Log("WaitTime is: " + waitTime);
        //Debug.Log(grabCheck.collider.tag);
      
        if (grabCheck.collider != null  )
        {
            pressurePlatePlayer();
            if (!bucketGrabbed)
            {
                boxGrab();
            }
            if (!boxGrabbed)
            {

                bucketGrab();
            }
        }
    }

    private void bucketGrab()
    {
        if ((grabCheck.collider.tag == "Bucket") && !bucketGrabbed)
        {

            health = 0;
            Heal(waitTime);
            
        }

        float distToLastPos = Vector3.Distance(grabDetect.position, lastPosition);
        if ((grabCheck.collider.tag == "Bucket") && bucketGrabbed)
        {
            health = 300;
            Damage(waitTime);

        }

        if ((grabCheck.collider.tag == "Bucket") && waitTime >= fixTime && !bucketGrabbed)
        {
            ringHealthBar1.enabled = false;
            ringHealthBar2.enabled = false;
            //Debug.Log("Pick up box");
            bucketGrabbed = !bucketGrabbed;
            waitTime = 0.0f;
            SoundManager.Instance.PlayGrabClip();

        }
        else if ((grabCheck.collider.tag == "Bucket") && waitTime >= fixTime && bucketGrabbed)
        {
            //Debug.Log("Drop box");
            grabCheck.collider.gameObject.transform.position = new Vector3(boxHolder.position.x, -5f, boxHolder.position.z);

            bucketGrabbed = !bucketGrabbed;
            waitTime = 0.0f;
            SoundManager.Instance.PlayGrabClip();
        }


        if (bucketGrabbed)
        {
            grabCheck.collider.gameObject.transform.parent = boxHolder;

            grabCheck.collider.gameObject.transform.position = boxHolder.position;
            //grabCheck.collider.gameObject.GetComponent<Rigidbody>().isKinematic = true;

        }
        else if (!bucketGrabbed)
        {

            grabCheck.collider.gameObject.transform.parent = null;
            //grabCheck.collider.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        }
    }

    private void pressurePlatePlayer()
    {
        Debug.Log(grabCheck.collider.tag);
        if (!boxGrabbed && !bucketGrabbed && grabCheck.collider.tag.Equals( "PressurePlatePlayer"))
        {
            door = grabCheck.collider.GetComponent<doorController>();
            door.isactive = false;
            door.door.SetActive(false);
            //Debug.Log("pressure plate player");
        }
        else
        {
            if (door != null)
            {
                door.door.SetActive(true);
            }

        }

    }

    private void boxGrab()
    {
        
        if ((grabCheck.collider.tag == "Box") && !boxGrabbed)
        {

            health = 0;
            Heal(waitTime);
            
        }

        float distToLastPos = Vector3.Distance(grabDetect.position, lastPosition);
        if ((grabCheck.collider.tag == "Box") && boxGrabbed)
        {
            health = 300;
            Damage(waitTime);
        }

        if ((grabCheck.collider.tag == "Box") && waitTime >= fixTime && !boxGrabbed)
        {
            ringHealthBar1.enabled = false;
            ringHealthBar2.enabled = false;
            Debug.Log("Pick up box");
            boxGrabbed = !boxGrabbed;
            waitTime = 0.0f;
            SoundManager.Instance.PlayGrabClip();
        }
        else if ((grabCheck.collider.tag == "Box") && waitTime >= fixTime && boxGrabbed)
        {
            Debug.Log("Drop box");
            grabCheck.collider.gameObject.transform.position = new Vector3(boxHolder.position.x, -5f, boxHolder.position.z);

            boxGrabbed = !boxGrabbed;
            waitTime = 0.0f;
            SoundManager.Instance.PlayGrabClip();
        }


        if (boxGrabbed)
        {
            grabCheck.collider.gameObject.transform.parent = boxHolder;

            grabCheck.collider.gameObject.transform.position = boxHolder.position;
            //grabCheck.collider.gameObject.GetComponent<Rigidbody>().isKinematic = true;

        }
        else if (!boxGrabbed)
        {

            grabCheck.collider.gameObject.transform.parent = null;
            //grabCheck.collider.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (boxGrabbed || bucketGrabbed)
        {
            if (other.tag.Equals("wall"))
            {
                boxVariables currentItem = grabCheck.collider.GetComponent<boxVariables>();

                boxGrabbed = false;
                bucketGrabbed = false;
                grabCheck.collider.gameObject.transform.position = currentItem.initPosition;

            }
        }
    }
    
    void LateUpdate()
    {

        float distToLastPos = Vector3.Distance(grabDetect.position, lastPosition);

        if (distToLastPos < 1.5f)
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
        {
            ringHealthBar1.enabled = true;
            ringHealthBar2.enabled = true;

            health -= (damagePoints * 100);
        }
        else if (health <=0)
        {
            ringHealthBar1.enabled = false;
            ringHealthBar2.enabled = false;

        }

    }
    public void Heal(float healingPoints)
    {

        if (health < maxHealth)
        {
            ringHealthBar1.enabled = true;
            ringHealthBar2.enabled = true;

            health += (healingPoints * 100);

        }else if (health >= maxHealth)
        {
            ringHealthBar2.enabled = false;
            ringHealthBar1.enabled = false;

        }
    }
}
