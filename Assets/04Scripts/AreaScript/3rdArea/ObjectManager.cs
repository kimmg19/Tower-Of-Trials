using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public float rotateX;
    public float rotateY;
    public float rotateZ;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // ť�� ȸ��
        this.transform.Rotate(this.rotateX, this.rotateY, this.rotateZ);
    }
}
