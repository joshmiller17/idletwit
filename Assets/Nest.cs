using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Nest : MonoBehaviour
{
    public GameObject TwitPostPrefab;
    public bool isSM = false;
    private List<GameObject> Twits = new List<GameObject>();
    private float timeTilNextUserPost = 0;
    public AudioSource clack;
    public AudioClip[] clackClips;
    private float twitIntensity = 5f; //less is more

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isSM && timeTilNextUserPost < 0)
        {
            GenerateUserPost();
            timeTilNextUserPost = Mathf.Pow(Random.Range(0.2f, twitIntensity), 1.4f);
            twitIntensity += Random.Range(-1f, 1f);
            if (twitIntensity < 0.2f)
            {
                twitIntensity += 3;
            }
            else if (twitIntensity > 15)
            {
                twitIntensity -= 10;
            }
        }
        else
        {
            timeTilNextUserPost -= Time.deltaTime;
        }
    }


    void PlayClack()
    {
        clack.clip = clackClips[Random.Range(0, clackClips.Length)];
        clack.PlayOneShot(clack.clip);
    }

    void GenerateUserPost()
    {
        float success = Random.Range(-1.0f, 10.0f);
        success = Mathf.Max(Mathf.Pow(success, Random.Range(0f, 4f)), 0);
        float variance = success / 10f + Random.Range(0f, 2f);
        int echoes = Mathf.RoundToInt(Mathf.Max(0, Mathf.Floor(success + variance * Random.Range(-5f, 3f))));
        int graffitis = Mathf.RoundToInt(Mathf.Max(0, Mathf.Floor(success * Random.Range(0.5f, 1.2f))));
        int eggplants = Mathf.RoundToInt(Mathf.Max(0, Mathf.Floor(success + variance * Random.Range(-1f, 10f))));
        PostTwit(GenerateUsername(), GenerateRandomTwit(), echoes, graffitis, eggplants);
    }

    //TODO Tracery
    string GenerateUsername()
    {
        return "User" + Random.Range(0, 10000).ToString();
    }

    //TODO Tracery
    string GenerateRandomTwit()
    {
        return "blah, my favorite number is " + Random.Range(0, 10000).ToString();
    }

    public void PostTwit(string author, string content, int Echoes=0, int Graffitis=0, int Eggplants=0)
    {
        GameObject NewPost = Instantiate(TwitPostPrefab, transform.position + new Vector3(0, 170, 0), Quaternion.identity);
        NewPost.GetComponent<TwitPost>().Nest = this.gameObject;
        NewPost.transform.SetParent(gameObject.transform);
        NewPost.transform.Find("Author").GetComponent<Text>().text = author;
        NewPost.transform.Find("Content").GetComponent<Text>().text = content;
        if (!isSM)
        {
            NewPost.GetComponent<TwitPost>().Echoes = Echoes;
            NewPost.GetComponent<TwitPost>().Graffitis = Graffitis;
            NewPost.GetComponent<TwitPost>().Eggplants = Eggplants;
            NewPost.GetComponent<TwitPost>().UpdateStats();
        }

        //slide Twits down

        List<GameObject> KnownTwits = new List<GameObject>(Twits);
        foreach (GameObject Twit in KnownTwits)
        {
            if (Twit == null)
            {
                Twits.Remove(Twit);
            }
            else
            {
                if (isSM)
                {
                    Twit.transform.position += new Vector3(0, -75, 0);
                }
                else
                {
                    Twit.transform.position += new Vector3(0, -125, 0);
                }
            }
        }

        Twits.Add(NewPost);
        PlayClack();
    }
}
