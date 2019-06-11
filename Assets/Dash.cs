using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash :Ability
{

    public float dashPower;
    public float dashDuration;
    private PlayerMovement playerMovement;
    float timebtwdash;
    float dashRate=5f;
    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("L1") && timebtwdash<=0)
        {
            StartCoroutine(ability());
            timebtwdash = dashRate;
        }
        else
        {
            timebtwdash -= Time.deltaTime;
        }
    }
    public override IEnumerator ability()
    {
        playerMovement.addforce(Camera.main.transform.forward* -Input.GetAxisRaw("MoveY"), dashPower);
        yield return new WaitForSeconds(dashDuration);
        playerMovement.ResetImpact();
    }
}
