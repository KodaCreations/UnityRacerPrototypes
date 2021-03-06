﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Ship : MonoBehaviour
{

    public float speed = 90f;
    public float turnSpeed = 5f;
    public float hoverForce = 65f;
    public float hoverHeight = 3.5f;
    public bool flying;
    private float powerInput;
    private float turnInput;
    private float vTurnInput;
    private Rigidbody rb;
    
    private Transform meshTransform;

    float cachedTurnInput;

    public float stability = 2f;
    public float stablitySpeed = 0.3f;
    public string vertical = "Vertical";
    public string horizontal = "Horizontal";

    // Variables for displaying current speed on the HUD
    //private float kmh = 0.0f;
    //public Text speedText;

    void Start()
    {
        flying = false;
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.inertiaTensor = new Vector3(1, 1, 1);
        meshTransform = transform.FindChild("Mesh");        
    }

    void Update()
    {
<<<<<<< HEAD
        if (flying)
        {
            if (Input.GetKey(KeyCode.Space))
                powerInput = 1;
            else
                powerInput = 0;
            vTurnInput = Input.GetAxis("Vertical");
        }
        else
        {
            powerInput = Input.GetAxis("Vertical");
            vTurnInput = 0;
        }
=======
<<<<<<< HEAD
        powerInput = Input.GetAxis(vertical);
        turnInput = Input.GetAxis(horizontal);
=======
        powerInput = Input.GetAxis("Vertical");
>>>>>>> master
        turnInput = Input.GetAxis("Horizontal");

        // Calculates speed in km/h and updates HUD text with 0 decimal points.
        //kmh = rb.velocity.magnitude * 3.6f;
        //speedText.text = kmh.ToString("N0") + " km/h";
>>>>>>> master
    }

    void FixedUpdate()
    {
        rb.AddRelativeForce(0f, 0f, powerInput * speed, ForceMode.Acceleration);
        rb.AddRelativeTorque(0f, turnInput * turnSpeed, 0f, ForceMode.Acceleration);
        if ((vTurnInput > 0 && transform.eulerAngles.x >= 75 && transform.eulerAngles.x < 180) || (vTurnInput < 0 && transform.eulerAngles.x <= 285 && transform.eulerAngles.x > 180))
            vTurnInput = 0;
        rb.AddRelativeTorque(vTurnInput * turnSpeed, 0f, 0f, ForceMode.Acceleration);
        if (transform.eulerAngles.x > 75 && transform.eulerAngles.x < 180)
            transform.eulerAngles = new Vector3(75, transform.eulerAngles.y, transform.eulerAngles.z);
        Debug.Log(transform.eulerAngles.x);
        if (transform.eulerAngles.x < 285 && transform.eulerAngles.x > 180)
            transform.eulerAngles = new Vector3(285, transform.eulerAngles.y, transform.eulerAngles.z);


        if (flying)
            UpdateAir();
        else
            UpdateGround();

        //Vector3 predictedUp = Quaternion.AngleAxis(
        //    rb.angularVelocity.magnitude * Mathf.Rad2Deg * stability / stablitySpeed,
        //    rb.angularVelocity
        //) * transform.up;
        //Vector3 torqueVector = Vector3.Cross(predictedUp, Vector3.up);
        ////torqueVector = Vector3.Project(torqueVector, transform.forward);
        //rb.AddTorque(torqueVector * stablitySpeed * stablitySpeed);
    }

    void UpdateAir()
    {
        
    }

    void UpdateGround()
    {
        //rb.AddRelativeForce(0f, 0f, powerInput * speed, ForceMode.Acceleration);
        //rb.AddRelativeTorque(0f, turnInput * turnSpeed, 0f, ForceMode.Acceleration);

        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit hit;

        //rb.velocity.Set(rb.velocity.x, 0, rb.velocity.z);

        if (Physics.Raycast(ray, out hit, hoverHeight * 20))
        {
            //hit.distance = Mathf.Clamp(hit.distance, 0, hoverHeight * 2);
            float proportionalHeight = (hoverHeight - hit.distance * 0.2f) / hoverHeight;


            Vector3 appliedHoverForce = transform.up * proportionalHeight * hoverForce;
            rb.AddForce(appliedHoverForce, ForceMode.Acceleration);

            //Debug.Log(proportionalHeight + "  " + appliedHoverForce + "  " + hit.distance + "  " + rb.velocity);

            //if (hit.distance > hoverHeight / 2f)
            //    rb.AddForce(-transform.up * ((hit.distance - hoverHeight) / hoverHeight) * hoverForce, ForceMode.Acceleration);


            //if (hit.distance < hoverHeight)
            //{
            //    rb.AddForce(-transform.up * ((hit.distance - hoverHeight) / hoverHeight) * hoverForce, ForceMode.Acceleration);
            //}



            //if (hit.distance > hoverHeight)
            //rb.velocity.Set(rb.velocity.x, 0, rb.velocity.z);

            //Vector3 temp = rb.position;
            //temp.y = Mathf.Lerp(temp.y, hit.point.y + hoverHeight, Time.fixedDeltaTime * 2);
            //if (rb.position.y > hit.point.y + hoverHeight + 2)
            //{
            //    //rb.MovePosition(temp);
            //    //rb.position = temp;

            //    rb.AddTorque(-transform.right * hoverForce * proportionalHeight * 5, ForceMode.Acceleration);

            //    Debug.Log("hej");
            //}
            //else if (rb.position.y < hit.point.y + hoverHeight)
            //{
            //    rb.AddTorque(transform.right * hoverForce * proportionalHeight, ForceMode.Acceleration);
            //    Debug.Log("tja");
            //}

            Debug.DrawRay(hit.point, hit.normal, Color.red, 10);

            Vector3 x = Vector3.Cross(transform.up.normalized, hit.normal.normalized);
            float theta = Mathf.Asin(x.magnitude);
            Vector3 w = x.normalized * theta / Time.fixedDeltaTime;

            Quaternion q = transform.rotation * rb.inertiaTensorRotation;
            Vector3 T = q * Vector3.Scale(rb.inertiaTensor, (Quaternion.Inverse(q) * w));

            Debug.DrawRay(transform.position, transform.up * 20, Color.green, 10);

            T -= transform.right;

            rb.AddTorque(T * 0.2f, ForceMode.Acceleration);
        }



        //Vector3 predictedUp = Quaternion.AngleAxis(
        //    rb.angularVelocity.magnitude * Mathf.Rad2Deg * stability / stablitySpeed,
        //    rb.angularVelocity
        //) * transform.up;
        //Vector3 torqueVector = Vector3.Cross(predictedUp, Vector3.up);
        ////torqueVector = Vector3.Project(torqueVector, transform.forward);
        //rb.AddTorque(torqueVector * stablitySpeed * stablitySpeed);






        if (turnInput < 0 && turnInput < cachedTurnInput || turnInput == -1)
        {
            meshTransform.localRotation = Quaternion.Slerp(meshTransform.localRotation, Quaternion.Euler(0, 0, 60), turnSpeed * -turnInput * Time.deltaTime);
        }
        else if (turnInput > 0 && turnInput > cachedTurnInput || turnInput == 1)
        {
            meshTransform.localRotation = Quaternion.Slerp(meshTransform.localRotation, Quaternion.Euler(0, 0, -60), turnSpeed * turnInput * Time.deltaTime);
        }
        else
        {
            meshTransform.localRotation = Quaternion.Slerp(meshTransform.localRotation, Quaternion.Euler(0, 0, 0), turnSpeed * 0.8f * Time.deltaTime);
        }

        cachedTurnInput = turnInput;
    }
}