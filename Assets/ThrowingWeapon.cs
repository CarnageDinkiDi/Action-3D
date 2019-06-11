using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class ThrowingWeapon : MonoBehaviour
{

	public GameObject axe;
    public Rigidbody axeRb;
    public GameObject axeTempHolder;
    public GameObject bullet;
    public Transform GunNozzle;
    public float BulletSpeed;

    public float axeFlightSpeed = 10f;
    public float axeThrowPower = 10f;
    public float axeRotationSpeed = 10f;

    public ThrowingWeaponCollision axeCollsions;

    public enum AxeState { Static, Thrown, Travelling, Returning }
    public AxeState axeState;

    private float startTime;
    private float journeyLength;
    private Vector3 startPos;
    private Vector3 endPos;

    public float FireRate;
    private float timebtwShooting;
    public Camera cam;
    public float cameraShakeMagnitude;

    public bool isCameraShaking;
    public float ShakeSeconds;
    Vector3 camPos;
    public Transform player;
    public GodMode godMode;
    // Use this for initialization
    void Start()
    {
        axeRb = axe.GetComponent<Rigidbody>();
        axeRb.isKinematic = true;
        axeRb.useGravity = false;
        isCameraShaking = false;

    }

    void FixedUpdate()
    {
        if (Input.GetAxisRaw("Triggers") == 1 && godMode.godModeState == GodMode.GodModeState.turnedOFF && timebtwShooting <= 0)
        {
            GameObject _bullet = Instantiate(bullet, GunNozzle.position, Quaternion.identity);
            _bullet.GetComponent<Rigidbody>().AddForce(transform.TransformDirection(Vector3.forward) * BulletSpeed, ForceMode.Impulse);
            Destroy(_bullet, 5f);
            timebtwShooting = FireRate;
        }
        else
        {
            timebtwShooting -= Time.deltaTime;
        }


        if (godMode.godModeState == GodMode.GodModeState.turnedOFF)
        {
            endPos = axeTempHolder.transform.position;
            camPos = player.position;
            if (Input.GetButtonDown("R1") && axe.transform.position == axeTempHolder.transform.position)
            {
                axeState = AxeState.Thrown;
            }

            if (Input.GetButtonDown("Y") && axe.transform.position != axeTempHolder.transform.position)
            {
                startPos = axe.transform.position;
                startTime = Time.time;
                journeyLength = Vector3.Distance(startPos, endPos);
                axeState = AxeState.Returning;
            }

            if (axeState == AxeState.Thrown)
            {
                ThrownAxeWithPhysics();
            }

            if (axeState == AxeState.Travelling || axeState == AxeState.Returning)
            {
                axe.transform.Rotate(6.0f * axeRotationSpeed * Time.deltaTime, 0, 0);
            }

            if (axeState == AxeState.Returning)
            {
                axeCollsions.NoCollision(1);
                RecallAxe();

            }
            else if (axeState != AxeState.Returning)
            {
                axeCollsions.NoCollision(0);
            }
        }
        

    }

    private void LateUpdate()
    {
       
    }
    void ThrownAxeWithPhysics()
    {
        axe.transform.parent = null;
        axeState = AxeState.Travelling;
        axeRb.isKinematic = false;
        axeRb.useGravity = true;
        axeRb.AddForce(axe.transform.forward * axeThrowPower);
    }

    void RecallAxe()
    {
        float distCovered = (Time.time - startTime) * axeFlightSpeed;
        float fracJourney = distCovered / journeyLength;
        axe.transform.position = Vector3.Lerp(startPos, endPos, fracJourney);

        if (fracJourney >= 1.0f)
        {
            RecalledAxe();
        }
    }

    void RecalledAxe()
    {
        axeState = AxeState.Static;
        axeCollsions.RemoveConstraints();
        axe.transform.position = axeTempHolder.transform.position;
        axe.transform.rotation = axeTempHolder.transform.rotation;
        axeRb.isKinematic = true;
        axeRb.useGravity = false;
        axe.transform.parent = this.transform;
        isCameraShaking = true;
        Invoke("CameraShake",ShakeSeconds);
        Camera.main.transform.DOComplete();
        Camera.main.transform.DOShakePosition(.2f, .9f, 25, 90, false, true);
    }

    public void CollisionOccured()
    {
        axeState = AxeState.Static;
    }

    void CameraShake()
    {
        isCameraShaking = false;
    }
}
