using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;
using DG.Tweening;

public class GodMode : MonoBehaviour
{
    public Transform targeted;
    public Transform player;
    public LayerMask enemy;
    public LayerMask ledge;
    private RaycastHit lastRaycastHit;
    public float range;
    public float warpAttackSpeed;
    public float BlinkSpeed;
    public float GodTime = 100f;
    public enum GodModeState
    {
        turnedOFF,
        Aiming,
        Execution
    }
    public PostProcessingProfile processingProfile;
    public PostProcessingBehaviour postProcessing;
   public GodModeState godModeState = GodModeState.turnedOFF;

    private void Start()
    {
    }
    private void Update()
    {
        if (GetLookedAtObject() == null)
        {
            GetComponent<PlayerOneController>().CanMove = true;
        }

        Debug.Log(GodTime);
 
        if (Input.GetAxis("Triggers") == -1)
        {
            if (GodTime > 0)
            {
                GodTime -= Time.deltaTime * 5f;
            }
            else
            {
                GodTime = 0f;
            }


            if (godModeState == GodModeState.turnedOFF && GodTime>0)
            {
                godModeState = GodModeState.Aiming;
                Time.timeScale = 0.4f;
                postProcessing.profile = processingProfile;
            }
        }
        else
        {
            godModeState = GodModeState.turnedOFF;
            postProcessing.profile = null;
            Time.timeScale = 1f;
            GetComponent<PlayerOneController>().CanMove = true;
        }


        if (Input.GetButtonDown("Y"))
        {
            if (godModeState == GodModeState.Aiming && targeted==null && GodTime>0f)
            {
                RaycastHit hit;
                
                if(Physics.Raycast(transform.position, Camera.main.transform.forward, out hit, 1000f,enemy))
                {
                    GameObject Target = new GameObject();
                    Target.transform.position = hit.point;
                    Target.transform.parent = hit.transform;
                    
                    targeted = Target.transform;

                    godModeState = GodModeState.Execution;
                }
            }
        }

        if (Input.GetButtonDown("B"))
        {
            if (godModeState == GodModeState.Aiming && GodTime>0f)
            {
                GetComponent<PlayerOneController>().CanMove = false;
                if (GetLookedAtObject() != null)
                {
                    TeleportToLookAt();
                }

            }
        }

        if (godModeState == GodModeState.Execution)
        {
             Time.timeScale = 1f;
            transform.DOMove(targeted.parent.transform.position, warpAttackSpeed);
            StartCoroutine(waits(warpAttackSpeed));
            // Destroy(targeted.transform.parent.gameObject);
            if (GodTime > 0) { godModeState = GodModeState.Aiming; }
            else { godModeState = GodModeState.turnedOFF; }
         
        }

        if(godModeState == GodModeState.turnedOFF || GodTime<=0f)
        {
            postProcessing.profile =null;
            Time.timeScale = 1f;
            GetComponent<PlayerOneController>().CanMove = true;
        }
        if (godModeState == GodModeState.Aiming)
        {
            postProcessing.profile =processingProfile;
            Time.timeScale = 0.4f;
            GetComponent<PlayerOneController>().CanMove = true;
        }

    }
    private GameObject GetLookedAtObject()
    {
        Vector3 origin = transform.position;
        Vector3 direction = Camera.main.transform.forward;
        if (Physics.Raycast(origin, direction, out lastRaycastHit, range,ledge))
        {
            return lastRaycastHit.collider.gameObject;
        }
        else
        {
            return null;
        }
    }

    private void TeleportToLookAt()
    {
        transform.DOMove(lastRaycastHit.point + lastRaycastHit.normal * 1.5f, BlinkSpeed);
       // StartCoroutine(waits());
    }

    IEnumerator waits(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Camera.main.transform.DOComplete();
        Camera.main.transform.DOShakePosition(.2f, .9f, 25, 90, false, true);
        Destroy(targeted.transform.parent.gameObject);
       
    }
}
