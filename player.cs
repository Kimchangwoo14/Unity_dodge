using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    float speed = 5.0f;
    void Update()
    {
        if (!GameController.instance.StartGame)
            return;

        Vector3 movePos = new Vector3();
        bool isInput = false;
        if (Input.GetMouseButton(0))
        {
            movePos = Input.mousePosition;
            isInput = true;
        }

        if (Input.touchCount > 0)
        {
            movePos = Input.GetTouch(0).position;
            isInput = true;
        }
        if (isInput)
        {

            Vector3 transPos = Camera.main.ScreenToWorldPoint(movePos);
            Vector3 targetPos = new Vector3(transPos.x, transPos.y, 0);
            transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * speed);

            Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);

            if (pos.x < 0f) pos.x = 0f;
            if (pos.x > 1f) pos.x = 1f;
            if (pos.y < 0f) pos.y = 0f;
            if (pos.y > 1f) pos.y = 1f;

            transform.position = Camera.main.ViewportToWorldPoint(pos);
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "bullet")
        {
            GameController.instance.EndGame();
            Debug.Log("End");
        }
    }

}
