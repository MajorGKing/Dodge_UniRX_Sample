using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class Rotator : MonoBehaviour 
{
    public float rotationSpeed = 60f;

    void Start()
    {
        this.UpdateAsObservable().TakeUntilDestroy(this).Subscribe(_ =>
        {
            transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);    
        });
    }
}