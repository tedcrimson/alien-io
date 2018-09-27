using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Culler : MonoBehaviour {

	public Material CullMaterial;
	private Material[] lastMaterials;

	/// <summary>
	/// OnTriggerEnter is called when the Collider other enters the trigger.
	/// </summary>
	/// <param name="other">The other Collider involved in this collision.</param>
	void OnTriggerEnter(Collider other)
	{
		var mesh = other.GetComponent<MeshRenderer>();
		var ufo = other.GetComponentInParent<UFOController>();
		if(!mesh || ufo) return;

		lastMaterials = mesh.sharedMaterials;

		// Debug.LogError(lastMaterials.Length);

		var culls = new Material[lastMaterials.Length];

		for (int i = 0; i < lastMaterials.Length; i++)
		{
			culls[i] = CullMaterial;
		}
		mesh.materials = culls;
	}


	/// <summary>
	/// OnTriggerExit is called when the Collider other has stopped touching the trigger.
	/// </summary>
	/// <param name="other">The other Collider involved in this collision.</param>
	void OnTriggerExit(Collider other)
	{
		var mesh = other.GetComponent<MeshRenderer>();
		var ufo = other.GetComponentInParent<UFOController>();

		if(!mesh || ufo) return;
		mesh.sharedMaterials = lastMaterials;

	}
}
