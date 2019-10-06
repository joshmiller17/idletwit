using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityTracery;
using UnityEngine.UI;

public class TwitPost : MonoBehaviour
{
    public TextAsset GraffitiGrammarFile;
    public TraceryGrammar GraffitiGrammar;
    public GameObject Spray;
    private bool isEchoed = false;
    private bool isGraffitied = false;
    private bool isEggplanted = false;
    public GameObject Nest;
    public GameObject SMBox;
    public GameObject EchoButton;
    public GameObject GraffitiButton;
    public GameObject EggplantButton;
    public NumbersBoard NB;
    public int Echoes = 0;
    public int Graffitis = 0;
    public int Eggplants = 0;
    public GameObject EchoStat;
    public GameObject GraffitiStat;
    public GameObject EggplantStat;
    public AudioSource pop;
    public AudioClip[] popClips;
    public AudioSource grafSource;
    public AudioClip[] grafClips;
    public float popularity; // success measure between 0 and 1, typically; = success / 1m ... set on Post
    public string author = "";
    private int smash = 0;
    private string[] LotsaEggplants =
    {
        "A LOT",
        "TONS",
        "HECKIN'",
        "WOWEE",
        "SO MANY",
        "RIP",
        "YIKES",
        "TOO HIGH"
    };

    void Awake()
    {
        pop = GetComponent<AudioSource>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        GraffitiGrammar = new TraceryGrammar(GraffitiGrammarFile.text);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -1000) //don't delete until a while off screen
        {
            Destroy(gameObject);
        }

        if ((popularity > 0 || author == "You" || (Random.Range(0, 10) < 1 && Echoes + Graffitis + Eggplants > 0)) && Random.Range(0, 5) < 1)
        {
            if (author == "You")
            {
                popularity += (Nest.GetComponent<Nest>().twitIntensity * NB.peeps) / 100000000;
            }
            popularity = Mathf.Min(0.001f, popularity);
            //PlayQuietPop();
            int NewGraffitis = Mathf.RoundToInt(Mathf.Max(0,popularity) * Random.Range(0, NB.peeps));
            int NewEchoes = Mathf.RoundToInt(Mathf.Max(0, popularity) * Random.Range(0, NB.peeps));
            int NewEggplants = Mathf.RoundToInt(Mathf.Max(0, popularity) * Random.Range(0, NB.peeps));
            if (Random.Range(0, 100) < 1)
            {
                NewEggplants += 1;
            }
            Echoes += NewEchoes;
            Graffitis += NewGraffitis;
            Eggplants += NewEggplants;
            if (author == "You")
            {
                popularity += NB.peeps / 100000f;
                NB.AddEggplants(NewEggplants);
                if (Random.Range(0f, 1f) < Mathf.Min(0.5f, popularity * 100 + 0.00002f + NB.peeps * .00000001f))
                {
                    if (NB.eggplants > 100 * NB.peeps)
                    {
                        NB.AddPeep(Nest.GetComponent<Nest>().GenerateUsername());
                    }
                    else
                    {
                        NB.AddEggplants(1);
                    }
                }
            }
            UpdateStats();
            SpawnEggplant();
        }
    }

    private void SpawnEggplant()
    {
        //TODO
    }

    private void SpawnPeep()
    {
        //TODO
    }

    public void UpdateStats()
    {
        EchoStat.GetComponent<Text>().text = StatToString(Echoes);
        GraffitiStat.GetComponent<Text>().text = StatToString(Graffitis);
        if (!isEggplanted)
        {
            EggplantStat.GetComponent<Text>().text = StatToString(Eggplants);
        }
        else
        {
            if (Eggplants < 1000000)
            {
                EggplantStat.GetComponent<Text>().text = Eggplants.ToString();
            }
            else
            {
            }
        }
    }

    void PlayQuietPop()
    {
        Debug.Log(popClips.Length);
        pop.clip = popClips[Random.Range(0, popClips.Length)];
        pop.volume = 0.25f;
        pop.PlayOneShot(pop.clip);
    }

    void PlayPop()
    {
        pop.clip = popClips[Random.Range(0, popClips.Length)];
        pop.volume = 1f;
        pop.PlayOneShot(pop.clip);
    }

    void PlayGraffiti()
    {
        grafSource.clip = grafClips[Random.Range(0, grafClips.Length)];
        grafSource.volume = 1f;
        grafSource.PlayOneShot(grafSource.clip);
    }

    string StatToString(int stat)
    {
        if (stat < 999)
        {
            return stat.ToString();
        }
        else
        {
            float k = stat / 1000f;
            if (k < 1000)
            {
                return k.ToString("F1") + "k";
            }
            else
            {
                float m = k / 1000f;
                return m.ToString("F1") + "m";
            }
        }
    }

    public void Echo()
    {

        if (!isEchoed)
        {
            GameObject NewPost = Nest.GetComponent<Nest>().PostTwit("You", transform.Find("Content").GetComponent<Text>().text, popularity, Echoes, Graffitis, Eggplants);
            NewPost.GetComponent<TwitPost>().isEchoed = true;
            NewPost.GetComponent<TwitPost>().Echoes += 1;
            NewPost.GetComponent<TwitPost>().EchoButton.GetComponent<Image>().color = new Color32(0, 20, 255, 100);

            EchoButton.GetComponent<Image>().color = new Color32(0, 20, 255, 100);
            Echoes += 1;
            EchoStat.GetComponent<Text>().text = StatToString(Echoes);
            isEchoed = true;
            PlayPop();

            if (author == "You" && !Nest.GetComponent<Nest>().isTutorialGoing())
            {
                Nest.GetComponent<Nest>().LosePeeps();
                SMBox.GetComponent<Nest>().PostTwit(Nest.GetComponent<Nest>().GenerateUsername(), "Echoing your own twit? Laaame!");
            }
            else if (Random.Range(0f, 1f) < Mathf.Min(0.5f, +0.0002f + popularity * 10 * NB.peeps * .0000001f))
            {
                if (NB.eggplants > 100 * NB.peeps)
                {
                    NB.AddPeep(author);
                }
            }
        }
        else
        {
            return;
        }
    }

    public void Graffiti()
    {
        if (!isGraffitied)
        {
            GraffitiButton.GetComponent<Image>().color = new Color32(0, 20, 255, 100);
            Graffitis += 1;
            GraffitiStat.GetComponent<Text>().text = StatToString(Graffitis);
            isGraffitied = true;
            PlayGraffiti();
            Spray.GetComponent<Text>().text = GraffitiGrammar.Generate();

            if (author == "You" && !Nest.GetComponent<Nest>().isTutorialGoing())
            {
                Nest.GetComponent<Nest>().LosePeeps();
                SMBox.GetComponent<Nest>().PostTwit(Nest.GetComponent<Nest>().GenerateUsername(), "Graffiti on your own twit? You must be so lonely!");
            }
            else if (Graffitis == 1)
            {
                if (Random.Range(0f, 1f) < 0.3f) {
                    NB.AddPeep(author);
                }
                else if(Random.Range(0f, 1f) < 0.7f)
                {
                    NB.AddPeep(Nest.GetComponent<Nest>().GenerateUsername());
                }
            }
            else if (Random.Range(0f, 1f) < Mathf.Min(0.5f, + 0.0002f + popularity * 10 * NB.peeps * .0000001f))
            {
                if (NB.eggplants > 100 * NB.peeps)
                {
                    NB.AddPeep(author);
                }
            }
        }
        else
        {
            return;
        }
    }

    public void Eggplant()
    {
        if (!isEggplanted)
        {
            EggplantButton.GetComponent<Image>().color = new Color(0, 20/255f, 1f, 1f);
            Eggplants += 1;
            EggplantStat.GetComponent<Text>().text = StatToString(Eggplants);
            isEggplanted = true;
            PlayPop();

            if (Random.Range(0f, 1f) < Mathf.Min(0.5f, + 0.000002f + popularity * NB.peeps * .000000001f))
            {
                if (NB.eggplants > 100 * NB.peeps)
                {
                    NB.AddPeep(author);
                }
            }
        }
        else
        {
            EggplantButton.GetComponent<Image>().color = new Color(Mathf.Min(1, smash / 50f), 20 / 255f, 1f, 1f);
            Eggplants += 1;
            smash += 1;
            if (Eggplants < 1000000)
            {
                EggplantStat.GetComponent<Text>().text = Eggplants.ToString();
            }
            else
            {
                EggplantStat.GetComponent<Text>().text = LotsaEggplants[Random.Range(0, LotsaEggplants.Length)];
            }
            PlayPop();
        }
    }

}
