using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SetLevels : MonoBehaviour {

	public Collider m_Collider;

	public PlayerController player;

	public List<GameObject> uniques;
	public List<string> meshos;
	public Transform rootObj;
	public Transform uniqueRoot;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	/// <summary>
	/// OnGUI is called for rendering and handling GUI events.
	/// This function can be called multiple times per frame (one call per event).
	/// </summary>
	void OnGUI()
	{
		if(GUI.Button(new Rect(200, 200, 200, 200), "RATY"))
		{
			var hitos = Physics.BoxCastAll(m_Collider.bounds.center, m_Collider.bounds.size, Vector3.forward, Quaternion.identity, 10000, 1<<9);
			// var hitos = Physics.BoxCastAll(m_Collider.bounds.center, transform.localScale,;
			foreach (var item in hitos)
			{
            	Debug.Log("Hit : " + item.collider.name);
				var t = item.transform.GetComponent<TargetObject>();
				if(t != null)
					t.Level = player.Level;
				
			}
		}
		if(GUI.Button(new Rect(400, 400, 200, 200), "RATY"))
		{
			Vector3 pos = Vector3.zero;
			meshos = new List<string>();

			// var hitos = Physics.BoxCastAll(m_Collider.bounds.center, transform.localScale,;
			foreach (Transform item in rootObj)
			{
				var t = item.transform.GetComponent<TargetObject>();
				var m = item.transform.GetComponent<MeshFilter>();
				if(t != null && m != null){
					if(!meshos.Contains(m.mesh.name))
					{
						meshos.Add(m.mesh.name);
						Instantiate(t.gameObject, new Vector3(pos.x, t.transform.position.y, pos.z), t.transform.rotation, uniqueRoot).transform.localScale = t.transform.lossyScale;
						pos += Vector3.right * 3;
					}
					// t.Level = player.Level;
				}
				
			}
		}


	}
}
