using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public PhysicsEngine DoorBody;

    public float Mass = 1;
    public float Velocity = 1;

    /*
    TorqueComponents bulletTorque
    {
        get
        {
            return new TorqueComponents(transform.forward * Mass * Velocity, 1, 1);
        }
    }
    */
    private void FixedUpdate()
    {
        float step = 1f / 60f;
        transform.position += Velocity * transform.forward * step;
        if(Velocity <= 0)
        {
            gameObject.GetComponent<Rigidbody>().useGravity = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Bullet>() != null)
            return;
        GetComponent<SphereCollider>().enabled = false;
        Vector3 targetDir = transform.InverseTransformPoint(other.transform.position);
        float angle = Vector3.Angle(other.transform.right, transform.forward);
        angle = angle * Mathf.Deg2Rad;
        float radius = Vector3.Distance(other.gameObject.transform.Find("DoorHinge").transform.position, transform.position);
        /*
        Debug.Log("BulletHit");
        Debug.Log("Angle: " + angle);
        Debug.Log("Radius: " + radius);
        Debug.Log("Force: " + transform.forward * Mass * Velocity);
        */
        TorqueComponents bulletTorque = new TorqueComponents(transform.forward * Mass * Velocity, angle, radius, gameObject);
        PhysicsEngine otherBody = other.gameObject.GetComponent<PhysicsEngine>();
        otherBody.AddTorque(bulletTorque);
        Velocity = 0;
        StartCoroutine(DeathDelay());
    }

    public IEnumerator DeathDelay()
    {
        yield return new WaitForSeconds(1.0f);
        Destroy(gameObject);
    }
}
