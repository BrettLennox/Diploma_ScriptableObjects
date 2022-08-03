using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PixelMap : MonoBehaviour
{
    public Texture2D mapImage;
    [System.Serializable]
    public struct Mappings
    {
        public GameObject spawnObj;
        public Color spawnColour;
    }
    public Mappings[] mappedElement;
    private Color _pixelColour;

    void GenerateObject(int x, int y)
    {
        //read pixel colours
        _pixelColour = mapImage.GetPixel(x, y);
        if (_pixelColour.a == 0)
        {
            Debug.Log("this pixel is empty, SKIP");
            return;
        }

        foreach (Mappings colourMapping in mappedElement)
        {
            Debug.Log("Check Colour Match: " + _pixelColour + " - " + colourMapping.spawnColour);
            //Scan pixel colour mappings for matching colour
            if (colourMapping.spawnColour.Equals(_pixelColour))
            {
                Debug.Log("Colour Match");
                //turn the pixel x and y into Vector2 position
                Vector2 pos = new Vector2(x, y);
                //Spawn object that matches pixel colour at pixel position
                Instantiate(colourMapping.spawnObj, pos, quaternion.identity, transform);
            }
        }
    }
    
    void GenerateLevel()
    {
        //scan whole texture and get pixel positions
        for (int x = 0; x < mapImage.width; x++)
        {
            for (int y = 0; y < mapImage.height; y++)
            {
                GenerateObject(x,y);
            }
        }
    }

    private void Start()
    {
        GenerateLevel();
    }
}
