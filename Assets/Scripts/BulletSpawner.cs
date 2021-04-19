using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletSpawner : MonoBehaviour
{
    
    public GameObject bulletGO;

    public GameObject location1;
    public GameObject location2;
    public GameObject location3;
    public GameObject location4;

    public InputField MassField;
    public InputField VelocityField;

    Vector3 mouseDownLoc;
    Vector3 mouseCurLoc;

    GameObject curBullet;
    public void SpawnBullet(int location)
    {
        Transform locTrans;

        switch (location)
        {
            case 0:
                locTrans = location1.transform;
                break;
            case 1:
                locTrans = location2.transform;
                break;
            case 2:
                locTrans = location3.transform;
                break;
            default:
            case 3:
                locTrans = location4.transform;
                break;
        }


        GameObject newBullet = Instantiate(bulletGO, locTrans.position, locTrans.rotation);
        newBullet.GetComponent<Bullet>().Mass = float.Parse(MassField.text);
        newBullet.GetComponent<Bullet>().Velocity = float.Parse(VelocityField.text);
    }

    private void FixedUpdate()
    {
        /*
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("MouseDown");
            mouseDownLoc = Input.mousePosition;
            
            Debug.Log(mouseDownLoc);
            Vector3 point = Camera.main.ScreenToWorldPoint(mouseDownLoc);
            Debug.Log(point);
            curBullet = Instantiate(bulletGO, new Vector3(point.x, 0.5f, point.y), Quaternion.identity);
        }
        else if (Input.GetMouseButton(0) && curBullet != null)
        {
            mouseCurLoc = Input.mousePosition;
            Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(mouseDownLoc.x, 0.5f, mouseDownLoc.z));
            curBullet.transform.LookAt(point);
        }
        else if (Input.GetMouseButtonUp(0) && curBullet != null)
        {
            mouseCurLoc = Input.mousePosition;
            Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(mouseDownLoc.x, 0.5f, mouseDownLoc.z));
            curBullet.transform.LookAt(point);

            curBullet.GetComponent<Bullet>().Mass = float.Parse(MassField.text);
            curBullet.GetComponent<Bullet>().Velocity = float.Parse(VelocityField.text);
            curBullet = null;
        }
        */
    }
}
