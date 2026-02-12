using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class HandPresencePhysics : MonoBehaviour
{

    public Transform target;
    private Rigidbody rb;
    public Renderer nonPhysicalHand;
    public float showNonPhysicalHandDistance = 0.05f;
    private Collider[] handColliders;
    public GameObject testCube;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        handColliders = GetComponentsInChildren<Collider>();

        Debug.Log(handColliders.Count());
    }

    // https://youtu.be/CfzO6jvLY-w?t=584
    public void EnableHandCollider()
    {
        foreach(var c in handColliders)
        {
            c.enabled = true;
        }

        testCube.GetComponent<Renderer>().material.color = new Color(0,255,0);
    }

    public void EnableHandCollider_Delay(float delay)
    {
        testCube.GetComponent<Renderer>().material.color = new Color(0,0,255);
        Invoke(nameof(EnableHandCollider), delay);
    }

    public void DisableHandCollider()
    {
        foreach(var c in handColliders)
        {
            c.enabled = false;
        }
        testCube.GetComponent<Renderer>().material.color = new Color(255,0,0);
    }

    public void HoverTest()
    {
        testCube.GetComponent<Renderer>().material.color = new Color(238, 0, 255);
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, target.position);

        if(distance > showNonPhysicalHandDistance)
        {
            nonPhysicalHand.enabled = true;
        }
        else
        {
            nonPhysicalHand.enabled = false;
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = (target.position - transform.position) / Time.fixedDeltaTime;

        Quaternion rotationDifference = target.rotation * Quaternion.Inverse(transform.rotation);
        rotationDifference.ToAngleAxis(out float angleInDegree, out Vector3 rotationAxis);

        Vector3 rotationDifferenceInDegree = angleInDegree * rotationAxis;
        rb.angularVelocity = rotationDifferenceInDegree * Mathf.Deg2Rad / Time.fixedDeltaTime;
    }
}
