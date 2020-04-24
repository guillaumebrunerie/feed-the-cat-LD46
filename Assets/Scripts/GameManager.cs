using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private float timeWaited;
    public float timeBetweenObjects;
    public GameObject food;
    public GameObject explosion;
    public static GameManager instance;
    public List<GameObject> objects;
    public float score;
    public float highScore;
    public GameObject catPrefab;
    private GameObject cat;
    public Text scoreUI;

    private bool playing = false;
    
    public GameObject easyButton;
    public GameObject hardButton;
    
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
        
    public void StartGame(bool hard)
    {
        if (cat != null)
            Destroy(cat);
        for (int i = 0; i < objects.Count; i++)
        {
            Destroy(objects[i]);
        }

        score = 0;
        objects = new List<GameObject>();
        timeWaited = timeBetweenObjects;
        easyButton.SetActive(false);
        hardButton.SetActive(false);
        cat = Instantiate(catPrefab);
        cat.GetComponent<Cat>().SetTimeToNoFood(hard ? 20 : 40);
        playing = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (playing)
        {
            timeWaited += Time.deltaTime;
            if (timeWaited > timeBetweenObjects)
            {
                timeWaited -= timeBetweenObjects;
                SpawnObject();
            }
            score += Time.deltaTime * (cat.GetComponent<Cat>().IsSleeping() ? 3 : 1);
            scoreUI.text = "Score: " + ((int) score);

            if (Input.GetMouseButtonDown(0))
            {
                Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                pos.z = 0;
                Instantiate(explosion, pos, Quaternion.identity);
            }
        }
    }

    public void AddObjectToList(GameObject obj)
    {
        objects.Add(obj);
    }

    public void RemoveObjectFromList(GameObject obj)
    {
        objects.Remove(obj);
    }
    
    void SpawnObject()
    {
        Instantiate(food);
    }

    public void GameOver()
    {
        playing = false;
        easyButton.SetActive(true);
        hardButton.SetActive(true);
        highScore = Mathf.Max(score, highScore);
        scoreUI.text = "Score: " + ((int) score) + " (High: " + ((int) highScore) + ")";
    }
}
