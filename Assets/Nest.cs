using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityTracery;

public class Nest : MonoBehaviour
{
    public GameObject TwitPostPrefab;
    public TextAsset GraffitiGrammarFile;
    public GameObject NB;
    public GameObject SMBox;
    public GameObject ScratchBox;
    public GameObject Placeholder;
    public TextAsset UsernameGrammarFile;
    public TextAsset TwitGrammarFile;
    public TraceryGrammar UsernameGrammar;
    public TraceryGrammar TwitGrammar;
    public bool isSM = false;
    private List<GameObject> Twits = new List<GameObject>();
    private float timeTilNextUserPost = 0;
    public AudioSource clack;
    public AudioClip[] clackClips;
    public float twitIntensity = 5f; //less is more
    public float timeSinceLastTwit = 0f;
    private float initialDelay = 54f;
    private bool tested = false;

    // Start is called before the first frame update
    void Start()
    {
        UsernameGrammar = new TraceryGrammar(UsernameGrammarFile.text);
        TwitGrammar = new TraceryGrammar(TwitGrammarFile.text);
        ScratchBox.GetComponent<InputField>().interactable = false;
        if (!isSM)
        {
            Placeholder.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (initialDelay > 0)
        {
            initialDelay -= Time.deltaTime;
            if (initialDelay <= 0 && !isSM && !tested)
            {
                ScratchBox.GetComponent<InputField>().interactable = true;
                Placeholder.SetActive(true);
                PostTwit("You", "Just joined Twit Idol! #twit #idol #beginner", -1, 0, 0, 0);
                initialDelay = 25f;
                tested = true;
            }
            return;
        }
        timeSinceLastTwit += Time.deltaTime;
        if (isSM && timeSinceLastTwit > Random.Range(0f, Mathf.Max(1, 1000000 - 3 * NB.GetComponent<NumbersBoard>().peeps)))
        {
            LosePeeps();
            if (isSM && NB.GetComponent<NumbersBoard>().peeps > 3 && !isTutorialGoing() && Random.Range(0f, 1f) < 0.3f)
            {
                PostTwit(GenerateUsername(), TwitGrammar.Parse("#notwit#"));
                LosePeeps();
            }
            else if (isSM && timeSinceLastTwit > 30 && NB.GetComponent<NumbersBoard>().peeps > 5)
            {
                string twit = "Sheesh, you haven't posted in " + timeSinceLastTwit.ToString("F1") + " seconds, what's the deal?";
                PostTwit(GenerateUsername(), twit);
                LosePeeps();
            }
        }

        if (!isSM && timeTilNextUserPost < 0)
        {
            GenerateUserPost();
            if (twitIntensity < 0.2f)
            {
                twitIntensity = 3f;
            }
            else if (twitIntensity > 15)
            {
                twitIntensity -= 10;
            }
            timeTilNextUserPost = Mathf.Pow(Random.Range(0.2f, twitIntensity), 1.4f);
            twitIntensity += Random.Range(-1f, 1f);
        }
        else
        {
            timeTilNextUserPost -= Time.deltaTime;
        }
    }

    public void LosePeeps()
    {
        for (int i = 0; i < Random.Range(0, NB.GetComponent<NumbersBoard>().peeps / 5); i++)
        {
            NB.GetComponent<NumbersBoard>().RemovePeep();
        }
        if (0.5 > Random.Range(0f, Mathf.Max(1, 10000 - NB.GetComponent<NumbersBoard>().peeps)))
        {
            LosePeeps();
        }
    }

    public bool isTutorialGoing()
    {
        return ScratchBox.GetComponent<ScratchBox>().tutorialTwits.Count > 0;
    }

    void PlayClack()
    {
        clack.clip = clackClips[Random.Range(0, clackClips.Length)];
        clack.PlayOneShot(clack.clip);
    }

    void GenerateUserPost()
    {
        float success = Random.Range(-1.0f, 2.0f) * (NB.GetComponent<NumbersBoard>().peeps + 1);
        success = Mathf.Max(Mathf.Pow(success, Random.Range(0f, 3f)), 0);

        PostTwit(GenerateUsername(), GenerateTwit(), success / 1000000f);
    }

    public string GenerateUsername()
    {
        if (UsernameGrammar == null)
        {
            UsernameGrammar = new TraceryGrammar(UsernameGrammarFile.text);
        }
        if (Random.Range(0, 1f) < 0.7f)
        {
            return UsernameGrammar.Generate();
        }
        else if (Random.Range(0, 1f) < 0.3f)
        {
            return UsernameGrammar.Generate() + Random.Range(0, 10000).ToString();
        }
        else
        {
            return UsernameGrammar.Generate() + Random.Range(0, 100).ToString();
        }
    }

    string GenerateTwit()
    {
        if (TwitGrammar == null)
        {
            TwitGrammar = new TraceryGrammar(TwitGrammarFile.text);
        }
        return TwitGrammar.Generate();
    }

    public string WriteTwit()
    {
        return TwitGrammar.Parse("#twit#");
    }

    private string AuthorToUsername(string author)
    {
        string username = author;
        if (username != "You")
        {
            username = "@" + username;
        }
        return username;
    }


    public GameObject PostTwit(string author, string content, float popularity, int Echoes, int Graffitis, int Eggplants)
    {
        GameObject NewPost = Instantiate(TwitPostPrefab, transform.position + new Vector3(0, 170, 0), Quaternion.identity);
        NewPost.GetComponent<TwitPost>().Nest = this.gameObject;
        NewPost.GetComponent<TwitPost>().GraffitiGrammarFile = GraffitiGrammarFile;
        NewPost.GetComponent<TwitPost>().SMBox = SMBox;
        NewPost.GetComponent<TwitPost>().NB = NB.GetComponent<NumbersBoard>();
        NewPost.GetComponent<TwitPost>().popularity = popularity;
        NewPost.transform.SetParent(gameObject.transform);

        NewPost.transform.Find("Author").GetComponent<Text>().text = AuthorToUsername(author);
        NewPost.transform.Find("Content").GetComponent<Text>().text = content;
        if (!isSM)
        {
            NewPost.GetComponent<TwitPost>().Echoes = Echoes;
            NewPost.GetComponent<TwitPost>().Graffitis = Graffitis;
            NewPost.GetComponent<TwitPost>().Eggplants = Eggplants;
            NewPost.GetComponent<TwitPost>().author = author;
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
                    Twit.transform.position += new Vector3(0, -100, 0);
                }
            }
        }

        Twits.Add(NewPost);
        PlayClack();
        return NewPost;
    }


    public GameObject PostTwit(string author, string content, float popularity = 0)
    {
        //if I had more time, this should get moved to TwitPost
        if (popularity == -1)
        {
            return PostTwit(author, content, popularity, 0, 0, 0);
        }
        float success = popularity * 1000000;
        float variance = success / 10f + Random.Range(0f, 2f);
        int Echoes = Mathf.RoundToInt(Mathf.Max(0, Mathf.Floor(success + variance * Random.Range(-5f, 3f))));
        if (Echoes < 5)
        {
            Echoes = Mathf.Max(0, Echoes - Random.Range(0, 5));
        }
        int Graffitis = Mathf.RoundToInt(Mathf.Max(0, Mathf.Floor(success * Random.Range(0.3f, 0.8f))));
        if (Graffitis < 5)
        {
            Graffitis = Mathf.Max(0, Graffitis - Random.Range(0, 5));
        }
        int Eggplants = Mathf.RoundToInt(Mathf.Max(0, Mathf.Floor(success + variance * Random.Range(-1f, 10f))));

        return PostTwit(author, content, popularity, Echoes, Graffitis, Eggplants);
    }
}
