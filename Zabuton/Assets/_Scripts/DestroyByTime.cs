﻿using UnityEngine;
using System.Collections;

public class DestroyByTime : MonoBehaviour
{
    public float destroyTimer;
	void Start ()
    {
        Destroy(gameObject, destroyTimer);
	}

}
