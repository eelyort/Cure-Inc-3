using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clicky : MonoBehaviour
{
        Color[] data;
        SpriteRenderer spriteRenderer;
    public Color color;
    public GameObject colorMap;

    public int Width { get { return spriteRenderer.sprite.texture.width; } }
    public int Height
    {
        get { return spriteRenderer.sprite.texture.height; }
    }

    void Awake()
    {
        // Get renderer you want to probe
        spriteRenderer = colorMap.GetComponent<SpriteRenderer>();

        // extract color data
        data = spriteRenderer.sprite.texture.GetPixels();

        colorMap.GetComponent<Renderer>().enabled = false;
    }
    void Update()
    {

        if (Input.GetMouseButton(0))
        {
            /*
            Debug.Log("Mouse Pos: " + Input.mousePosition);
            Debug.Log("nearClipPlane: " + Camera.main.nearClipPlane);
            // Get Mouse position - convert to world position
            Vector3 screenPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
            Debug.Log("V3: " + screenPos);
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
            */
            Color color;
            bool success = GetSpritePixelColorUnderMousePointer(spriteRenderer, out color);
            Debug.Log("Success? " + success + ", color: " + color);
            Debug.Log("G: " + color.g);
        }
    }
    public bool GetSpritePixelColorUnderMousePointer(SpriteRenderer spriteRenderer, out Color color) {
        color = new Color();
        Camera cam = Camera.main;
        Vector2 mousePos = Input.mousePosition;
        Vector2 viewportPos = cam.ScreenToViewportPoint(mousePos);
        if (viewportPos.x < 0.0f || viewportPos.x > 1.0f || viewportPos.y < 0.0f || viewportPos.y > 1.0f) return false; // out of viewport bounds
                                                                                                                        // Cast a ray from viewport point into world
        Ray ray = cam.ViewportPointToRay(viewportPos);

        // Check for intersection with sprite and get the color
        return IntersectsSprite(spriteRenderer, ray, out color);
    }
    private bool IntersectsSprite(SpriteRenderer spriteRenderer, Ray ray, out Color color) {
        color = new Color();
        if (spriteRenderer == null) return false;
        Sprite sprite = spriteRenderer.sprite;
        if (sprite == null) return false;
        Texture2D texture = sprite.texture;
        if (texture == null) return false;
        // Check atlas packing mode
        if (sprite.packed && sprite.packingMode == SpritePackingMode.Tight) {
            // Cannot use textureRect on tightly packed sprites
            Debug.LogError("SpritePackingMode.Tight atlas packing is not supported!");
            // TODO: support tightly packed sprites
            return false;
        }
        // Craete a plane so it has the same orientation as the sprite transform
        Plane plane = new Plane(transform.forward, transform.position);
        // Intersect the ray and the plane
        float rayIntersectDist; // the distance from the ray origin to the intersection point
        if (!plane.Raycast(ray, out rayIntersectDist)) return false; // no intersection
                                                                     // Convert world position to sprite position
                                                                     // worldToLocalMatrix.MultiplyPoint3x4 returns a value from based on the texture dimensions (+/- half texDimension / pixelsPerUnit) )
                                                                     // 0, 0 corresponds to the center of the TEXTURE ITSELF, not the center of the trimmed sprite textureRect
        Vector3 spritePos = spriteRenderer.worldToLocalMatrix.MultiplyPoint3x4(ray.origin + (ray.direction * rayIntersectDist));
        Rect textureRect = sprite.textureRect;
        float pixelsPerUnit = sprite.pixelsPerUnit;
        float halfRealTexWidth = texture.width * 0.5f; // use the real texture width here because center is based on this -- probably won't work right for atlases
        float halfRealTexHeight = texture.height * 0.5f;
        // Convert to pixel position, offsetting so 0,0 is in lower left instead of center
        int texPosX = (int)(spritePos.x * pixelsPerUnit + halfRealTexWidth);
        int texPosY = (int)(spritePos.y * pixelsPerUnit + halfRealTexHeight);
        // Check if pixel is within texture
        if (texPosX < 0 || texPosX < textureRect.x || texPosX >= Mathf.FloorToInt(textureRect.xMax)) return false; // out of bounds
        if (texPosY < 0 || texPosY < textureRect.y || texPosY >= Mathf.FloorToInt(textureRect.yMax)) return false; // out of bounds
                                                                                                                   // Get pixel color
        color = texture.GetPixel(texPosX, texPosY);
        return true;
    }
}
