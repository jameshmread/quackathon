﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour {

	public Transform generationPoint;

	public float objectWidth;

	public ObjectPooler objectPools;

	// Use this for initialization
	void Start() {
		this.objectWidth = objectPools.pooledObject.GetComponent<BoxCollider2D>().size.x;
	}

	// Update is called once per frame
	void Update () {
		if (this.transform.position.x < generationPoint.position.x)
		{
			transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);

			GameObject newPlatform = objectPools.getPooledObject();
			newPlatform.transform.position = transform.position;
			newPlatform.transform.rotation = transform.rotation;
			newPlatform.SetActive(true);

			transform.position = new Vector3(transform.position.x + this.objectWidth, transform.position.y, transform.position.z);
		}
	}    
}