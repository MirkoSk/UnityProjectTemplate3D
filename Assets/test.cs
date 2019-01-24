using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    [SerializeField] GameEvent fadeInEvent = null;
    [SerializeField] GameEvent fadeOutEvent = null;
    [SerializeField] float time = 2f;

    float timer;
    float counter = 1.3f;



    // Start is called before the first frame update
    void Start()
    {
        RaiseFadeOut(1f);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= time)
        {
            RaiseFadeIn(1f);
            timer = 0f;
            counter -= 0.1f;
        }
    }

    void RaiseFadeOut(float value)
    {
        fadeOutEvent.Raise(this, value);
    }

    void RaiseFadeIn(float value)
    {
        fadeInEvent.Raise(this, value);
    }
}
