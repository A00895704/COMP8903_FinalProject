using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject Door;
    public void ResetDoor()
    {
        PhysicsEngine doorPhysics = Door.GetComponent<PhysicsEngine>();
        doorPhysics.angularAcceleration = 0;
        doorPhysics.angularVelocity = 0;

        Door.transform.rotation = Quaternion.identity;
        Door.transform.position = new Vector3(-1.5f, 0f, 0f);
    }
}
