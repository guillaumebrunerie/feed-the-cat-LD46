using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float strength;
    private bool isExploding;
    private float time;
    private float length = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        Explode();
        isExploding = false;
        time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isExploding)
        {
            isExploding = true;
            StartCoroutine(ExplodeSprite());
        }
    }

    IEnumerator ExplodeSprite()
    {
        while (time < length)
        {
            time += Time.deltaTime;
            float scale = Mathf.Max(Mathf.Pow(1f - time / length, 5) * 2 - 1, 0);
            transform.localScale = new Vector3(scale, scale, scale);
            GetComponent<Rigidbody2D>().rotation = Random.Range(0f, 360f);
            yield return null;
        }
        Destroy(gameObject);
    }

    void Explode()
    {
        List<GameObject> objects = GameManager.instance.objects;
        for (int i = 0; i < objects.Count; i++)
        {
            objects[i].GetComponent<Food>().GetForce(transform.position - objects[i].transform.position, strength);
        }
    }
}
