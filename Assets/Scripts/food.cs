﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class food : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveFoodDown()
    {
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - 8f, transform.localPosition.z);
    }
}
