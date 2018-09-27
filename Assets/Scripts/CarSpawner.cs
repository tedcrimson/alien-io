using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour {

	public List<GameObject> prefabs;
	public float spawnTime;

	// Use this for initialization
	void Start () {
		InvokeRepeating("SpawnCar", spawnTime, spawnTime);
	}
	
	// Update is called once per frame
	void SpawnCar () {
		var b = Instantiate(
				prefabs[Random.Range(0, prefabs.Count)], 
				this.transform.position, 
				this.transform.rotation, 
				this.transform.parent);
		// b.transform.LookAt(this.transform.forward + b.transform.position);	
		// b.GetComponent<CarBot>().direction = this.transform.forward;
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireCube(this.transform.position, 5 * Vector3.one);
	}
	
}
