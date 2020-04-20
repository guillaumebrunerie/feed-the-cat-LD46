using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.SceneManagement;

public class Cat : MonoBehaviour
{
    private float timeToNoFood;
    private float catSpeed = 3.6f;
    private float eatSpeed = 0.2f;

    private float food;
    private bool facingLeft = true;

    private Food pieceOfFood;

    private Vector2 deltal = new Vector2(1.6f , 0.5f);
    private Vector2 deltar = new Vector2(-1.6f , 0.5f);
    
    private Animator animator;
    private Rigidbody2D rb2d;

    public Sprite wokeupSprite;

    public GameObject foodBar;

    enum State
    {
        Sleeping,
        Waiting,
        Walking,
        Eating
    }

    private State state;
    private string[] triggers = {"Sleep", "Wait", "Walk", "Eat"};
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();

        food = 1f;
        state = State.Sleeping;
        timeToNoFood = GameManager.instance.timeToNoFood;

        foodBar = GameObject.Find("FoodBar");
    }

    // Update is called once per frame
    void Update()
    {
        if (state != State.Eating)
            food -= Time.deltaTime/timeToNoFood;

        GameManager.instance.foodBar.fillAmount = food * 0.85f;
        foodBar.transform.position = new Vector3 (transform.position.x, transform.position.y + 1.75f, 0);

        State newstate;

        if (state == State.Sleeping)
        {
            if (food < 0.75)
                newstate = State.Waiting;
            else
                newstate = State.Sleeping;
        }
        else
        {
            if (food > 0.9 && state != State.Eating)
                newstate = State.Sleeping;
            else
            {
                pieceOfFood = FindFood();
                if (pieceOfFood != null)
                {
                    if (ReachedFood())
                    {
                        newstate = State.Eating;
                        food += pieceOfFood.Consume(eatSpeed * Time.deltaTime);
                    }
                    else
                    {
                        newstate = State.Walking;
                        rb2d.MovePosition(Vector2.MoveTowards(rb2d.position, pieceOfFood.Position() + (facingLeft ? deltal : deltar), catSpeed * Time.deltaTime));
                        facingLeft = (rb2d.position.x > pieceOfFood.Position().x);
                        transform.localScale = new Vector3(facingLeft ? .5f : -.5f, .5f, 1);
                    }
                }
                else
                {
                    newstate = State.Waiting;
                }
            }
        }

        if (food < 0)
        {
            foodBar.transform.position = new Vector3 (-11, 0, 0);
            GameManager.instance.GameOver();
            newstate = State.Sleeping;
        }

        if (newstate != state)
        {
            state = newstate;
            animator.SetTrigger(triggers[(int) state]);
        }
    }
    
    private bool ReachedFood()
    {
        return ((rb2d.position - pieceOfFood.Position() - (facingLeft ? deltal : deltar)).sqrMagnitude < 0.1);
    }

    public float GetFood()
    {
        return food;
    }

    public bool IsSleeping()
    {
        return (state == State.Sleeping);
    }
    
    public Food FindFood()
    {
        List<Food> objs = GameManager.instance.objects;
        float catX = rb2d.position.x;

        float bestDistance = 1000;
        Food bestFood = null;
        float secondBestDistance = 1000;
        Food secondBestFood = null;
        
        for (int i = 0; i < objs.Count; i++)
        {
            if (objs[i].IsReady())
            {
                float dleft = catX - objs[i].GetX(); // - deltal.x;
                float dright = objs[i].GetX() - catX; // - deltal.x;
                if (facingLeft)
                {
                    if (dleft > 0 && dleft < bestDistance)
                    {
                        bestDistance = dleft;
                        bestFood = objs[i];
                    }
                    if (dright > 0 && dright < secondBestDistance)
                    {
                        secondBestDistance = dright;
                        secondBestFood = objs[i];
                    }
                }
                else
                {
                    if (dright > 0 && dright < bestDistance)
                    {
                        bestDistance = dright;
                        bestFood = objs[i];
                    }
                    if (dleft > 0 && dleft < secondBestDistance)
                    {
                        secondBestDistance = dleft;
                        secondBestFood = objs[i];
                    }
                }
            }
        }
        if (bestFood == null)
            bestFood = secondBestFood;
        return bestFood;
    }
}
