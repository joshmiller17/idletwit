using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cloud : MonoBehaviour
{
    public Sprite[] sprites;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Image>().sprite = sprites[Random.Range(0, sprites.Length)];
        float scale = Random.Range(0.5f, 4f);
        if (scale > 1.6f)
        {
            transform.SetAsFirstSibling();
        }
        else
        {
            transform.SetAsLastSibling();
        }
        GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(5f, 100f) / scale, 0);
        transform.localScale = new Vector3(scale, scale, scale);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y > 3000)
        {
            Destroy(gameObject);
        }
    }
}
