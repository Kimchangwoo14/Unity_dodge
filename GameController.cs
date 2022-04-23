using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance = null;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        //DontDestroyOnLoad(gameObject);
    }
    void OnDestroy() { instance = null; }

    private List<GameObject> bulletObjs = new List<GameObject>();
    // Start is called before the first frame update
    public bool StartGame = false;
    public float bulletSpeedMin = 0.0001f;
    public float bulletSpeedMax = 0.01f;
    private int genCount = 1;
    void Start()
    {
        AddObjectpool(100);
        this.transform.Find("StartButton").gameObject.GetComponent<UIButton>().onClick.Add(new EventDelegate(() =>
        {
            this.transform.Find("StartButton").gameObject.SetActive(false);
            restart();
        }));
    }

    private void restart()
    {
        StartGame = true;
        this.transform.Find("player").transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
        foreach (var obj in bulletObjs)
        {
            if (obj.GetComponent<bullet>().isLife)
                SetBulletPosition(obj);
            obj.GetComponent<bullet>().isLife = false;
            obj.SetActive(false);
            break;
        }
        Score = 0;
        this.transform.Find("score").gameObject.GetComponent<UILabel>().text = Score.ToString();
        genCount = 1;
        bulletSpeedMin = 0.0001f;
        bulletSpeedMax = 0.01f;
        InvokeRepeating("MakeBullet", 0.0f, 1.0f);
        InvokeRepeating("calculateScore", 0.0f, 1.0f);
    }

    private void AddObjectpool(int count = 0)
    {
        for (int i = 0; i < count; ++i)
        {
         
            GameObject bullet = Instantiate(Resources.Load("prefabs/bullet")) as GameObject;
           
            bullet.transform.parent = this.transform.Find("objectpool").transform;
            bullet.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            bullet.gameObject.SetActive(false);
            bulletObjs.Add(bullet);
        }
    }
    private void SetBulletPosition(GameObject bullet)
    {
        float switchValue = Random.value;
        float xValue = Random.Range(-400f, 400f);
        float yValue = Random.Range(-800f, 800f);
        Vector3 position = new Vector3();

        if (switchValue > 0.5f)
        {
            if (Random.value > 0.5f)
            {
                position = new Vector3(-400f, yValue, 0f);

            }
            else
            {
                position = new Vector3(400f, yValue, 0f);
            }
        }
        else
        {
            if (Random.value > 0.5f)
            {
                position = new Vector3(xValue, -800f, 0f);
            }
            else
            {
                position = new Vector3(xValue, 800f, 0f);
            }
        }
        bullet.transform.localPosition = position;
    }
    int Score = 0;
    private void calculateScore()
    {
        if (!StartGame)
            CancelInvoke("calculateScore");
        Score++;
        if (Score > 30)
        {
            bulletSpeedMin = 0.001f;
            bulletSpeedMax = 0.01f;
        }
        if (Score > 50)
        {
            bulletSpeedMin = 0.01f;
            bulletSpeedMax = 0.05f;
        }
        if (Score % 5 == 0)
            genCount += 2;
        this.transform.Find("score").gameObject.GetComponent<UILabel>().text = Score.ToString();
    }

    private void MakeBullet()
    {
        if (!StartGame)
            CancelInvoke("MakeBullet");
        bool bFind = false;
        int findCount = 0;
        for (int i = 0; i < bulletObjs.Count; ++i)
        {
            if (bulletObjs[i].GetComponent<bullet>().isLife)
                continue;
            SetBulletPosition(bulletObjs[i]);
            bulletObjs[i].GetComponent<bullet>().isLife = true;
            bulletObjs[i].gameObject.SetActive(true);
            
            findCount++;
            if (genCount <= findCount)
            {
                bFind = true;
                break;
            }
        }
        if (!bFind)
        {
            AddObjectpool(bulletObjs.Count / 2);
            for (int i = 0; i < bulletObjs.Count; ++i)
            {
                if (bulletObjs[i].GetComponent<bullet>().isLife)
                    continue;
                SetBulletPosition(bulletObjs[i]);
                bulletObjs[i].GetComponent<bullet>().isLife = true;
                bulletObjs[i].gameObject.SetActive(true);

                findCount++;
                if (genCount <= findCount)
                {
                    bFind = true;
                    break;
                }
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void EndGame()
    {
        if (!StartGame)
            return;
        StartGame = false;
        this.transform.Find("StartButton").gameObject.SetActive(true);
        CancelInvoke();
    }
}


