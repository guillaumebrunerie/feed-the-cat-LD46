using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private SpriteRenderer sr;
    public Sprite[] sprites;
    public Sprite[] consumedSprites;
    private float veloc;
    private float rot;

    private float initialContents = 0.25f;
    private float contents;

    static int Mod(int a, int b) {
        return (((a % b) + b) % b);
    }

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        contents = initialContents;

        float minY = 2.0f;
        float maxY = 4.0f;
        float minSpeedX = 20f;
        float maxSpeedX = 25f;
        float minSpeedY = 1f;
        float maxSpeedY = 2f;

        int side = (2 * Random.Range(0, 2)) - 1;
        rb2d.position = new Vector3(11f * side, Random.Range(minY, maxY), 0);
        rb2d.velocity = new Vector2(-Random.Range(minSpeedX, maxSpeedX) * side, Random.Range(minSpeedY, maxSpeedY));

        rb2d.AddTorque(Random.Range(-100f, 100f));
        
        GameManager.instance.AddObjectToList(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (rb2d.position.y < -10)
            Destroy(gameObject);

        if (contents >= 0.2499)
            sr.sprite = sprites[Mod((int) Mathf.Round(rb2d.rotation / 45), 8)];
        else if (contents >= initialContents / 2)
            sr.sprite = consumedSprites[0];
        else if (contents >= 0.001)
            sr.sprite = consumedSprites[1];
        else
            sr.sprite = consumedSprites[2];
    }

    public void GetForce(Vector2 to, float strength)
    {
        Vector2 force = -strength * to / to.sqrMagnitude;
        if (to.sqrMagnitude < 25)
            rb2d.AddForce(force, ForceMode2D.Impulse);
    }

    public bool IsReady()
    {
        return (rb2d.velocity.sqrMagnitude < 1
                && !IsEmpty()
                && (Mathf.Abs(rb2d.rotation % 360) < 5 || Mathf.Abs(rb2d.rotation % 360) > 355));
    }
    
    public float Consume(float max)
    {
        if (contents >= max)
        {
            contents -= max;
            return max;
        }
        else
        {
            contents = 0;
            return contents;
        }
    }

    public bool IsEmpty()
    {
        return (contents <= 0.001);
    }
    
    void OnDestroy()
    {
        GameManager.instance.RemoveObjectFromList(gameObject);
    }

    public Vector2 Position()
    {
        return rb2d.position;
    }
}
