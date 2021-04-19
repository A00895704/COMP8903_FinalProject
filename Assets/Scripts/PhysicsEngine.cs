using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public struct TorqueComponents
{
	public Vector3 force;
    public float radians; //angle in radians between the force and radius
    public float radius;
	public GameObject objRef;
	public TorqueComponents(Vector3 force, float radians, float radius, GameObject objRef)
    {
		this.force = force;
		this.radians = radians;
		this.radius = radius;
		this.objRef = objRef;
    }
}

public class ReadOnlyAttribute : PropertyAttribute
{

}

[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
	public override float GetPropertyHeight(SerializedProperty property,
											GUIContent label)
	{
		return EditorGUI.GetPropertyHeight(property, label, true);
	}

	public override void OnGUI(Rect position,
							   SerializedProperty property,
							   GUIContent label)
	{
		GUI.enabled = false;
		EditorGUI.PropertyField(position, property, label, true);
		GUI.enabled = true;
	}
}

public class PhysicsEngine : MonoBehaviour
{
	private List<TorqueComponents> addedTorqueList = new List<TorqueComponents>();

	public float mass = 1;              // [kg]
	public float netTorque;				// [kg*m^2/s^2]
	public float length;				//length of the plank
	
	public float inertia;

	public Transform doorHinge;
	[ReadOnly] public float angularVelocity;
	[ReadOnly] public float angularAcceleration;
	[ReadOnly] public float angularMomentum;


	// Use this for initialization
	void Start()
	{
		inertia = mass * Mathf.Pow(length, 2) / 3;
		SetupThrustTrails();
	}

	void FixedUpdate()
	{
		RenderTrails();
		UpdatePosition();
	}

	public void AddTorque(TorqueComponents tc)
	{
		addedTorqueList.Add(tc);
	}

	void UpdatePosition()
	{
		// Sum the added torques and clear the list
		foreach (TorqueComponents torqueForce in addedTorqueList)
		{
			float force = torqueForce.force.magnitude;
			float forceDirMod = Vector3.Angle(torqueForce.objRef.transform.forward, transform.forward) * Mathf.Deg2Rad;
			forceDirMod = forceDirMod <= 90 * Mathf.Deg2Rad ? 1f : -1f;
			netTorque += torqueForce.radius * force * forceDirMod * Mathf.Sin(torqueForce.radians);
		}
		
		addedTorqueList = new List<TorqueComponents>();

		//// Calculate position change due to net force
		angularAcceleration = netTorque / inertia;
		angularVelocity += angularAcceleration * Time.deltaTime;
		angularMomentum = inertia * angularVelocity;
		transform.RotateAround(doorHinge.position, Vector3.up, angularVelocity);
		if(netTorque != 0)
			Debug.Log("Net torque: " + netTorque);
		netTorque = 0;
		//netTorque = 0;
	}




	/// Code for drawing thrust tails
	public bool showTrails = true;

	private LineRenderer lineRenderer;
	private int numberOfForces;

	// Use this for initialization
	void SetupThrustTrails()
	{
		lineRenderer = gameObject.AddComponent<LineRenderer>();
		lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
		lineRenderer.SetColors(Color.yellow, Color.yellow);
		lineRenderer.SetWidth(0.2F, 0.2F);
		lineRenderer.useWorldSpace = false;
	}

	// Update is called once per frame
	void RenderTrails()
	{
		if (showTrails)
		{
			lineRenderer.enabled = true;
			numberOfForces = addedTorqueList.Count;
			lineRenderer.SetVertexCount(numberOfForces * 2);
			int i = 0;
            foreach (TorqueComponents addedTorque in addedTorqueList)
            {
                lineRenderer.SetPosition(i, Vector3.zero);
                lineRenderer.SetPosition(i + 1, -addedTorque.force);
                i = i + 2;
            }
        }
		else
		{
			lineRenderer.enabled = false;
		}
	}
}