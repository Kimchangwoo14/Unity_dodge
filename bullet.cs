using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{

    Vector3 targetPos;
    Vector3 myPos;

    Vector3 newPos;
    public bool isLife = false;
    
    // Start is called before the first frame update
    void Start()
    {
        targetPos = GameObject.Find("player").transform.position;
        myPos = transform.position;
        float speed = Random.Range(0.0001f, 0.01f);
        newPos = (targetPos - myPos) * speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameController.instance.StartGame)
        {
            if (isLife)
            {
                isLife = false;
                transform.gameObject.SetActive(false);
            }
        }
        bool isDie = false;
        if (isLife)
        {
            transform.position = transform.position + newPos;

            if (transform.localPosition.y > 820.0f || transform.localPosition.y < -820.0f)
                isDie = true;
            if (transform.localPosition.x > 420.0f || transform.localPosition.x < -420.0f)
                isDie = true;

        }
        if (isDie)
        {
            isLife = false;
            transform.gameObject.SetActive(false);
        }
    }

}
