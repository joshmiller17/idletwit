using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Particle : MonoBehaviour
{
    public Sprite[] sprites;
    private float ttk = 5;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Image>().sprite = sprites[Random.Range(0, sprites.Length)];
        float x = Random.Range(-1f, 1f);
        float y = Random.Range(-1f, 1f);
        float speed = 300 / (Mathf.Abs(x) + Mathf.Abs(y));
        GetComponent<Rigidbody2D>().velocity = speed * new Vector2(x, y);
    }

    // Update is called once per frame
    void Update()
    {
        ttk -= Time.deltaTime;
        if (ttk < 0)
        {
            Destroy(gameObject);
        }
    }
}
