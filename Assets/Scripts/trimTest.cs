using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trimTest : MonoBehaviour
{
    public Sprite spriteToTrim;
    [Range(0.1f, 1f)] public float scalePercentage = 0.5f; // Set this between 0.1 and 1

    void Start()
    {
        //Sprite trimmedSprite = TrimSprite(spriteToTrim, scalePercentage);
        // Assign the trimmed sprite to a SpriteRenderer or use it as needed
        //GetComponent<SpriteRenderer>().sprite = trimmedSprite;
        StartCoroutine("makeSmaller");
    }

    IEnumerator makeSmaller()
    {
        Texture2D originalTexture = spriteToTrim.texture;
        Rect originalRect = spriteToTrim.textureRect;
        for(int i = 0; i < 2000; i ++)
        {
            // Calculate the new dimensions
            int newWidth = Mathf.RoundToInt(originalRect.width * Mathf.Lerp(1, scalePercentage, i/2000));
            int newHeight = Mathf.RoundToInt(originalRect.height * Mathf.Lerp(1, scalePercentage, i / 2000));

            // Calculate the center point
            int centerX = Mathf.RoundToInt(originalRect.x + originalRect.width / 2);
            int centerY = Mathf.RoundToInt(originalRect.y + originalRect.height / 2);

            // Calculate the new rectangle bounds
            int startX = Mathf.Max(0, centerX - newWidth / 2);
            int startY = Mathf.Max(0, centerY - newHeight / 2);

            // Adjust for edges
            if (startX + newWidth > originalTexture.width)
            {
                startX = originalTexture.width - newWidth;
            }
            if (startY + newHeight > originalTexture.height)
            {
                startY = originalTexture.height - newHeight;
            }

            // Get the pixels of the new rectangle
            Color[] newPixels = originalTexture.GetPixels(startX, startY, newWidth, newHeight);

            // Create a new texture
            Texture2D newTexture = new Texture2D(newWidth, newHeight);
            newTexture.SetPixels(newPixels);
            newTexture.Apply();

            // Create a new sprite from the trimmed texture
            Sprite trimmedSprite = Sprite.Create(newTexture, new Rect(0, 0, newWidth, newHeight), new Vector2(0.5f, 0.5f));
            GetComponent<SpriteRenderer>().sprite = trimmedSprite;
            yield return null;
        }
    }

    Sprite TrimSprite(Sprite originalSprite, float scale)
    {
        Texture2D originalTexture = originalSprite.texture;
        Rect originalRect = originalSprite.textureRect;

        // Calculate the new dimensions
        int newWidth = Mathf.RoundToInt(originalRect.width * scale);
        int newHeight = Mathf.RoundToInt(originalRect.height * scale);

        // Calculate the center point
        int centerX = Mathf.RoundToInt(originalRect.x + originalRect.width / 2);
        int centerY = Mathf.RoundToInt(originalRect.y + originalRect.height / 2);

        // Calculate the new rectangle bounds
        int startX = Mathf.Max(0, centerX - newWidth / 2);
        int startY = Mathf.Max(0, centerY - newHeight / 2);

        // Adjust for edges
        if (startX + newWidth > originalTexture.width)
        {
            startX = originalTexture.width - newWidth;
        }
        if (startY + newHeight > originalTexture.height)
        {
            startY = originalTexture.height - newHeight;
        }

        // Get the pixels of the new rectangle
        Color[] newPixels = originalTexture.GetPixels(startX, startY, newWidth, newHeight);

        // Create a new texture
        Texture2D newTexture = new Texture2D(newWidth, newHeight);
        newTexture.SetPixels(newPixels);
        newTexture.Apply();

        // Create a new sprite from the trimmed texture
        Sprite trimmedSprite = Sprite.Create(newTexture, new Rect(0, 0, newWidth, newHeight), new Vector2(0.5f, 0.5f));

        return trimmedSprite;
    }
}