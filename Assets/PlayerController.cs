using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    //deals with physics (collisions with pens, etc)
    public Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void FixedUpdate()
    {
        //horizontal axis gives -1 if left is pressed, 1 if right is pressed
        //veritcal does same for up and down
        //so the code below forms a vector representing the current movement, then scales it by the move speed
        //it is also multiplied by the time since the last frame (s=vt)
        Vector3 move = new Vector3(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"),0);
        //normalise vector so that length is always 1 (otherwise moving diagonally would be faster)
        move.Normalize();
        move *= moveSpeed * Time.deltaTime;
        rb.MovePosition(move + transform.position);
    }
}
