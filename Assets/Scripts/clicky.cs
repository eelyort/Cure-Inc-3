using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clicky : MonoBehaviour
{
        Color[] Data;
        SpriteRenderer SpriteRenderer;
    public Color color;


    public int Width { get { return SpriteRenderer.sprite.texture.width; } }
    public int Height
    {
        get { return SpriteRenderer.sprite.texture.height; }
    }

    void Awake()
    {
        // Get renderer you want to probe
        SpriteRenderer = GetComponent<SpriteRenderer>();

        // extract color data
        Data = SpriteRenderer.sprite.texture.GetPixels();
    }
    void Update()
    {

        if (Input.GetMouseButton(0))
        {

            // Get Mouse position - convert to world position
            Vector3 screenPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            screenPos = new Vector2(screenPos.x, screenPos.y);

            // Check if we clicked on our object
            RaycastHit2D[] ray = Physics2D.RaycastAll(screenPos, Vector2.zero, 0.01f);
            for (int i = 0; i < ray.Length; i++)
            {
                // You will want to tag the image you want to lookup
                if (ray[i].collider.tag == "TAGNAME")
                {
                    // Set click position to the gameobject area
                    screenPos -= ray[i].collider.gameObject.transform.position;
                    int x = (int)(screenPos.x * Width);
                    int y = (int)(screenPos.y * Height) + Height;

                    // Get color data
                    if (x > 0 && x < Width && y > 0 && y < Height)
                    {
                        color = Data[y * Width + x];
                    }
                    break;
                }
            }
            Debug.Log("Color: " + color);
        }
    }
}
