using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMouse : MonoBehaviour
{
    private Vector3 direction;
    public float rotationSpeed;
    public float speed;
    private Rigidbody2D rb2d;
    
    // Start is called before the first frame update
    void Start()
    {
        direction = Vector3.right;
        rb2d = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Vector2 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 newDirection = cursorPosition - rb2d.position;
        float localSpeed = Mathf.Clamp(newDirection.magnitude, 0, 2) * speed / 2;
        newDirection.Normalize();

        rb2d.AddForce(newDirection * localSpeed);
        rb2d.rotation = Mathf.Atan2(newDirection.y, newDirection.x) * Mathf.Rad2Deg - 90;
        
        // Vector3 newDirection = Vector3.Normalize(cursorPosition - transform.position);

        // direction = Vector3.RotateTowards(direction, newDirection, rotationSpeed, 1);
        // rb2d.rotation = Quaternion.LookRotation(Vector3.forward, direction);

        // float distance = (cursorPosition - rb2d.position).sqrMagnitude;
        // rb2d.position = rb2d.position + rb2d.up * Mathf.Clamp(speed * Time.deltaTime, 0, distance);
    }
}
