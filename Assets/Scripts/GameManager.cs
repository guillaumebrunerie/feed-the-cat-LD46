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
    public List<Food> objects;
    public Image foodBar;
    public float score;
    public float highScore;
    public Cat cat;
    public Cat thisCat;
    public Text scoreUI;

    private bool playing = false;
    public float timeToNoFood = 20;
    
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
        timeToNoFood = hard ? 10 : 40;
        if (thisCat != null)
            Destroy(thisCat.gameObject);
        for (int i = 0; i < objects.Count; i++)
        {
            Destroy(objects[i].gameObject);
        }

        score = 0;
        objects = new List<Food>();
        timeWaited = timeBetweenObjects;
        easyButton.SetActive(false);
        hardButton.SetActive(false);
        thisCat = Instantiate(cat);
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
            score += Time.deltaTime * (cat.IsSleeping() ? 3 : 1);
            scoreUI.text = "Score: " + ((int) score);

            if (Input.GetMouseButtonDown(0))
            {
                Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                pos.z = 0;
                Instantiate(explosion, pos, Quaternion.identity);
            }
        }
    }

    public void AddObjectToList(Food obj)
    {
        objects.Add(obj);
    }

    public void RemoveObjectFromList(Food obj)
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
