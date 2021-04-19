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
    Coroutine curDeathDelay;
    private void Start()
    {
        curDeathDelay = StartCoroutine(DeathDelay(10.0f));
    }
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
        Debug.Log("<color=yellow>>>>>>BulletHit</color>");
        GetComponent<SphereCollider>().enabled = false;
        Vector3 targetDir = transform.InverseTransformPoint(other.transform.position);
        float angle = Vector3.Angle(other.transform.right, transform.forward);
        Debug.Log("Angle: " + angle);
        angle = angle * Mathf.Deg2Rad;
        float radius = Vector3.Distance(other.gameObject.transform.Find("DoorHinge").transform.position, transform.position);
        Debug.Log("Radius: " + radius);
        Debug.Log("Force: " + transform.forward * Mass * Velocity);
        TorqueComponents bulletTorque = new TorqueComponents(transform.forward * Mass * Velocity, angle, radius, gameObject);
        PhysicsEngine otherBody = other.gameObject.GetComponent<PhysicsEngine>();
        otherBody.AddTorque(bulletTorque);
        Velocity = 0;
        StopCoroutine(curDeathDelay);
        StartCoroutine(DeathDelay(1.0f));
    }

    public IEnumerator DeathDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
