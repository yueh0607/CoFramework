using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KKK : MonoBehaviour
{
    [SerializeField] string seed = string.Empty;
    void Start()
    {
        Texture2D texture2D = new Texture2D(100,100);
        int hash   = seed.GetHashCode();
        for(int i=0;i<100;i++)
        {
            for(int j=0; j<100; j++)
            {
                float v = (i) / 100f;
                float v2 = (j) / 100f;   
                float t = Mathf.PerlinNoise(v, v2);
                Debug.Log(t);
                texture2D.SetPixel(i, j, new Color(t, t, t));
                
            }
        }
        texture2D.Apply();
        GetComponent<SpriteRenderer>().sprite = Sprite.Create(texture2D,
            new Rect(0, 0, 100, 100), Vector2.zero);
    }

    void Update()
    {
        
    }
}
