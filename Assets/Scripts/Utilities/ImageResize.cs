using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageResize : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ImageResizer(RawImage image, Texture texture)
    {
        image.texture = texture;
        float reduction = 2;
        //check size and normalize
        if (image.rectTransform.sizeDelta.y > 2000)
        {
            reduction = 2;
            image.rectTransform.sizeDelta = new Vector2(image.rectTransform.sizeDelta.x, image.rectTransform.sizeDelta.y + 300);
            image.rectTransform.sizeDelta = new Vector3(image.rectTransform.sizeDelta.x / reduction, image.rectTransform.sizeDelta.y / reduction);
        }
        if (image.rectTransform.sizeDelta.x > 1000)
        {
            reduction = 2;
            image.rectTransform.sizeDelta = new Vector2(image.rectTransform.sizeDelta.x, image.rectTransform.sizeDelta.y + 300);
            image.rectTransform.sizeDelta = new Vector3(image.rectTransform.sizeDelta.x / reduction, image.rectTransform.sizeDelta.y / reduction);
        }
        if (image.rectTransform.sizeDelta.y > 1000 & image.rectTransform.sizeDelta.x > 300)
        {
            reduction = 2;
            image.rectTransform.sizeDelta = new Vector2(image.rectTransform.sizeDelta.x, image.rectTransform.sizeDelta.y + 300);
        }
        if(image.rectTransform.sizeDelta.x > 800 & image.rectTransform.sizeDelta.y > 300)
        {
            reduction = 2;
        }
        if(image.rectTransform.sizeDelta.y < 250 & image.rectTransform.sizeDelta.x < 400)
        {
            reduction = .2f;
        }
        if(image.rectTransform.sizeDelta.x < 250 & image.rectTransform.sizeDelta.y < 300)
        {
            reduction = .2f;
        }
        image.rectTransform.sizeDelta = new Vector3(image.rectTransform.sizeDelta.x / reduction, image.rectTransform.sizeDelta.y / reduction);
    }
    
}
