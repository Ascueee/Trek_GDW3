using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapple : MonoBehaviour
{
    [Header("Refremces")]
    private PlayerMovement pm;
    public Transform cam;
    public Transform gunTip;
    public LayerMask whatIsGrappleable;
    public LineRenderer lr;

    [Header("Grappling vars")]
    public float maxGrappleDistance;
    public float grappleDelayTime;
    public float grappleAmmo;

    private Vector3 grapplePoint;

    [Header("CoolDown")]
    public float grappleCoolDown;
    private float grapplingCoolDownTimer;
    public float overShootYAxis;


    [Header("Input")]
    public KeyCode grappleKey = KeyCode.Mouse0;

    [Header("Grapple Sounds")]
    [SerializeField] AudioSource gs;
    [SerializeField] AudioClip grappleShootSound, grappleHitSound;

    private bool grappling;

    private void Start()
    {
        lr.enabled = false;
        pm = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(grappleKey))
        {
            StartGrapple();

        }
            

        if (grapplingCoolDownTimer > 0)
            grapplingCoolDownTimer -= Time.deltaTime;
    }

    private void LateUpdate()
    {
        if(grappling)
            lr.SetPosition(0, gunTip.position);
    }

    void StartGrapple()
    {
        gs.clip = grappleShootSound;
        gs.Play();

        if (grapplingCoolDownTimer > 0)
            return;

        grappling = true;

        pm.freeze = true;

        RaycastHit hit;

        if(Physics.Raycast(cam.position, cam.forward, out hit, maxGrappleDistance, whatIsGrappleable))
        {
            grapplePoint = hit.point;
            Invoke(nameof(ExecuteGrapple), grappleDelayTime);
        }
        else
        {
            grapplePoint = cam.position + cam.forward * maxGrappleDistance;
            Invoke(nameof(StopGrapple), grappleDelayTime);
        }

        lr.enabled = true;
        lr.SetPosition(1, grapplePoint);
    }

    void ExecuteGrapple()
    {
        pm.freeze = false;

        Vector3 lowestPoint = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);
        float grapplePointRelativeY = grapplePoint.y - lowestPoint.y;
        float highestPointOnArc = grapplePointRelativeY + overShootYAxis;

        if (grapplePointRelativeY < 0)
            highestPointOnArc = overShootYAxis;

        pm.JumpToPosition(grapplePoint, highestPointOnArc);

        Invoke(nameof(StopGrapple), 1f);
    }

    public void StopGrapple()
    {
        grappling = false;

        grapplingCoolDownTimer = grappleCoolDown;

        lr.enabled = false;
    }
}
