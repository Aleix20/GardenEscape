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

    //Charge bar starts in 0
    private void Start()
    {
        health = 0;
    }
    void Update()
    {
        //Move the chargeBar with the tracker
        ringHealthBar1.rectTransform.position = grabDetect.position;
        ringHealthBar2.rectTransform.position = grabDetect.position;
        if (health > maxHealth) health = maxHealth;

        lerpSpeed = 3f * Time.deltaTime;

        HealthBarFiller();
        ColorChanger();

        //Create a raycast from the grabcheck to the floor
        Physics.Raycast(grabDetect.position, Vector3.down, out grabCheck ,rayDist );

      
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
        //Update chargeBar if not grabbed increasing
        if ((grabCheck.collider.tag == "Bucket") && !bucketGrabbed)
        {

            health = 0;
            Heal(waitTime);
            
        }
        //Update chargeBar if grabbed decreasing
        if ((grabCheck.collider.tag == "Bucket") && bucketGrabbed)
        {
            health = 300;
            Damage(waitTime);

        }

        //Grab the item if we are on top around 3s
        if ((grabCheck.collider.tag == "Bucket") && waitTime >= fixTime && !bucketGrabbed)
        {
            ringHealthBar1.enabled = false;
            ringHealthBar2.enabled = false;
            bucketGrabbed = !bucketGrabbed;
            waitTime = 0.0f;
            SoundManager.Instance.PlayGrabClip();

        }
        //Leave the item if we don't move around 3s
        else if ((grabCheck.collider.tag == "Bucket") && waitTime >= fixTime && bucketGrabbed)
        {
            grabCheck.collider.gameObject.transform.position = new Vector3(boxHolder.position.x, -5f, boxHolder.position.z);
            bucketGrabbed = !bucketGrabbed;
            waitTime = 0.0f;
            SoundManager.Instance.PlayGrabClip();
        }

        //Update position if grabbed
        if (bucketGrabbed)
        {
            grabCheck.collider.gameObject.transform.parent = boxHolder;

            grabCheck.collider.gameObject.transform.position = boxHolder.position;

        }
        else if (!bucketGrabbed)
        {

            grabCheck.collider.gameObject.transform.parent = null;
        }
    }

    private void pressurePlatePlayer()
    {
        //Only active if you are on top of the pressure plate
        //if we don't have any item grabbed, you can activate the pressure plate player
        if (!boxGrabbed && !bucketGrabbed && grabCheck.collider.tag.Equals( "PressurePlatePlayer"))
        {
            door = grabCheck.collider.GetComponent<doorController>();
            door.isactive = false;
            door.door.SetActive(false);
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
        //Update chargeBar if not grabbed increasing
        if ((grabCheck.collider.tag == "Box") && !boxGrabbed)
        {

            health = 0;
            Heal(waitTime);
            
        }
        //Update chargeBar if grabbed decreasing
        if ((grabCheck.collider.tag == "Box") && boxGrabbed)
        {
            health = 300;
            Damage(waitTime);
        }
        //Grab the item if we are on top around 3s

        if ((grabCheck.collider.tag == "Box") && waitTime >= fixTime && !boxGrabbed)
        {
            ringHealthBar1.enabled = false;
            ringHealthBar2.enabled = false;
            Debug.Log("Pick up box");
            boxGrabbed = !boxGrabbed;
            waitTime = 0.0f;
            SoundManager.Instance.PlayGrabClip();
        }
        //Leave the item if we don't move around 3s
        else if ((grabCheck.collider.tag == "Box") && waitTime >= fixTime && boxGrabbed)
        {
            Debug.Log("Drop box");
            grabCheck.collider.gameObject.transform.position = new Vector3(boxHolder.position.x, -5f, boxHolder.position.z);

            boxGrabbed = !boxGrabbed;
            waitTime = 0.0f;
            SoundManager.Instance.PlayGrabClip();
        }

        //Update position if grabbed
        if (boxGrabbed)
        {
            grabCheck.collider.gameObject.transform.parent = boxHolder;

            grabCheck.collider.gameObject.transform.position = boxHolder.position;

        }
        else if (!boxGrabbed)
        {

            grabCheck.collider.gameObject.transform.parent = null;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        //If the tracker has some item grabbed check wall collisions
        if (boxGrabbed || bucketGrabbed)
        {
            if (other.tag.Equals("wall"))
            {
                //If the onject collide with a wall, set the object to the initial position
                boxVariables currentItem = grabCheck.collider.GetComponent<boxVariables>();

                boxGrabbed = false;
                bucketGrabbed = false;
                grabCheck.collider.gameObject.transform.position = currentItem.initPosition;

            }
        }
    }
    
    void LateUpdate()
    {
        //Check the distance with the previous position of the tracker
        float distToLastPos = Vector3.Distance(grabDetect.position, lastPosition);

        //If the tracker is not moving, increase the timer
        if (distToLastPos < 1.5f)
        {
            
            waitTime += Time.deltaTime;
            
        }
        else
        {
            //If the tracker is moving, update the position
            waitTime = 0.0f;
            lastPosition = grabDetect.position;
        }

    }



    void HealthBarFiller()
    {
        //Fill the charge bar gradually
        ringHealthBar1.fillAmount = Mathf.Lerp(ringHealthBar1.fillAmount, (health / maxHealth), lerpSpeed);
        ringHealthBar2.fillAmount = Mathf.Lerp(ringHealthBar2.fillAmount, (health / maxHealth), lerpSpeed);

    }
    void ColorChanger()
    {
        //Changing color from red to green gradually
        Color healthColor = Color.Lerp(Color.red, Color.green, (health / maxHealth));
        ringHealthBar1.color = healthColor;
        ringHealthBar2.color = healthColor;

    }



    public void Damage(float damagePoints)
    {
        //Reduce the chargeBar gradually
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
        //Increase ChargeBar gradually
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
