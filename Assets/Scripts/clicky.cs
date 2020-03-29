using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class clicky : MonoBehaviour
{
        // Color[] data;
        //SpriteRenderer spriteRenderer;
    public Color color;
    public GameObject colorMap;
    public GameObject map;
    public GameObject canvas;
    private GraphicRaycaster raycaster;
    private RectTransform colorMapTransform;
    protected MainGame mainGame;

    /*
    public int Width { get { return spriteRenderer.sprite.texture.width; } }
    public int Height
    {
        get { return spriteRenderer.sprite.texture.height; }
    }
    */

    void Awake()
    {
        // Get renderer you want to probe
        // spriteRenderer = colorMap.GetComponent<SpriteRenderer>();

        // colorMap.GetComponent<Renderer>().enabled = false;
        // map.GetComponent<Renderer>().enabled = false;

        mainGame = gameObject.GetComponent<MainGame>();

        // set scales equal
        colorMapTransform = colorMap.GetComponent<RectTransform>();
        RectTransform lungTransform = map.GetComponent<RectTransform>();
        colorMapTransform.anchoredPosition = lungTransform.anchoredPosition;
        colorMapTransform.rotation = lungTransform.rotation;
        colorMapTransform.localScale = lungTransform.localScale;

        // get raycaster
        // this.raycaster = canvas.GetComponent<GraphicRaycaster>();
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
            /*
            Color color;
            bool success = GetSpritePixelColorUnderMousePointer(spriteRenderer, out color);
            Debug.Log("Success? " + success + ", color: " + color);
            Debug.Log("G: " + color.g);
            */

            /*
            // new code for canvas
            // set up pointer event
            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            List<RaycastResult> results = new List<RaycastResult>();

            // raycast mouse click positon
            pointerData.position = Input.mousePosition;
            this.raycaster.Raycast(pointerData, results);

            // for every result, output name of game object on canvas hit by ray
            foreach(RaycastResult result in results) {
                if(result.isValid && result.gameObject.name == "ColorMap2") {
                    Debug.Log("Hit GameObject: " + result.gameObject.name);
                    Debug.Log("SortingLayer: " + result.sortingLayer);
                    Debug.Log("SortingOrder: " + result.sortingOrder);
                }
            }
            */

            /*
            Color color;

            Texture2D img = colorMap.GetComponent<Image>().sprite.texture;
            Sprite sprite = colorMap.GetComponent<Image>().sprite;

            bool success = GetSpritePixelColor(img, out color, sprite);
            Debug.Log("Success? " + success + ", color: " + color);
            Debug.Log("G: " + color.g);
            */

            // attempt 3
            Image img = colorMap.GetComponent<Image>();
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = Input.mousePosition;

            // Debug.Log("Name: " + colorMap.name);

            Vector2 localCursor;
            if(RectTransformUtility.ScreenPointToLocalPointInRectangle(colorMapTransform, eventData.position, eventData.pressEventCamera, out localCursor)) {
                // click inside sprite
                Debug.Log("Succes? localCursor: " + localCursor);

                Sprite sprite = img.sprite;

                // Debug.Log("Sprite Name: " + sprite.name);

                Rect textureRect = sprite.rect;
                // float pixelsPerUnit = sprite.pixelsPerUnit;
                float pixelsPerUnit = 1;
                float halfRealTexWidth = sprite.texture.width * pixelsPerUnit * 0.5f; // use the real texture width here because center is based on this -- probably won't work right for atlases
                float halfRealTexHeight = sprite.texture.height * pixelsPerUnit * 0.5f;
                // Convert to pixel position, offsetting so 0,0 is in lower left instead of center
                int texPosX = (int)(localCursor.x * colorMapTransform.localScale.x * pixelsPerUnit + halfRealTexWidth);
                int texPosY = (int)(localCursor.y * colorMapTransform.localScale.y * pixelsPerUnit + halfRealTexHeight);

                Color color = sprite.texture.GetPixel(texPosX, texPosY);

                sprite.texture.SetPixel(texPosX, texPosY, new Color(255, 255, 255));

                Debug.Log("texture.width: " + sprite.texture.width + "\ntextureRect: " + textureRect);
                Debug.Log("maxx: " + textureRect.xMax);
                Debug.Log("texPosX: " + texPosX + ", texPosY: " + texPosY + "\nSuccess2? color: " + color);
                // Debug.Log("2drect: " + colorMapTransform.localScal);
            }
        }
    }
    public bool GetSpritePixelColor(Texture2D img, out Color color, Sprite sprite) {
        color = new Color();
        Camera cam = Camera.main;
        Vector2 mousePos = Input.mousePosition;
        Vector2 viewportPos = cam.ScreenToViewportPoint(mousePos);
        if (viewportPos.x < 0.0f || viewportPos.x > 1.0f || viewportPos.y < 0.0f || viewportPos.y > 1.0f) return false; // out of viewport bounds

        // Cast a ray from viewport point into world
        Ray ray = cam.ViewportPointToRay(viewportPos);

        Debug.Log("Ray: " + ray);

        return IntersectsTexture(img, ray, out color, sprite);
    }
    
    private bool IntersectsTexture(Texture2D texture, Ray ray, out Color color, Sprite sprite) {
        color = new Color();
        if (texture == null)
            return false;
        /*
        // Check atlas packing mode
        if (sprite.packed && sprite.packingMode == SpritePackingMode.Tight) {
            // Cannot use textureRect on tightly packed sprites
            Debug.LogError("SpritePackingMode.Tight atlas packing is not supported!");
            // TODO: support tightly packed sprites
            return false;
        }
        */
        // Craete a plane so it has the same orientation as the sprite transform
        Plane plane = new Plane(canvas.transform.forward, canvas.transform.position);
        Debug.Log("plane: " + plane);
        // Intersect the ray and the plane
        float rayIntersectDist; // the distance from the ray origin to the intersection point
        if (!plane.Raycast(ray, out rayIntersectDist))
            return false; // no intersection
                          // Convert world position to sprite position
                          // worldToLocalMatrix.MultiplyPoint3x4 returns a value from based on the texture dimensions (+/- half texDimension / pixelsPerUnit) )
                          // 0, 0 corresponds to the center of the TEXTURE ITSELF, not the center of the trimmed sprite textureRect
        Debug.Log("rayIntersectDist: " + rayIntersectDist);

        // intersect point
        Vector3 intersect = ray.origin + (ray.direction * rayIntersectDist);
        Debug.Log("intersect: " + intersect);
        Vector3 spritePos = colorMap.transform.worldToLocalMatrix.MultiplyPoint3x4(intersect);
        // Debug.Log("spritePos: " + spritePos);
        // Vector3 spritePos = spriteRenderer.worldToLocalMatrix.MultiplyPoint3x4(ray.origin + (ray.direction * rayIntersectDist));
        Rect textureRect = sprite.rect;
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
        Plane plane = new Plane(colorMap.transform.forward, colorMap.transform.position);
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
