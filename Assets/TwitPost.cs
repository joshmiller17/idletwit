using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TwitPost : MonoBehaviour
{

    private bool isEchoed = false;
    private bool isGraffitied = false;
    private bool isEggplanted = false;
    public GameObject Nest;
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
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -2000) //don't delete until a while off screen
        {
            Destroy(gameObject);
        }

        if (popularity > 0 /*shortcut for SM, maybe fix*/ && Random.Range(0, 10) < 1)
        {
            //PlayQuietPop();
            int NewGraffitis = Mathf.RoundToInt(popularity * 100 * Random.Range(0, 20));
            int NewEchoes = Mathf.RoundToInt(popularity * 100 * Random.Range(0, 40));
            int NewEggplants = Mathf.RoundToInt(popularity * 100 * Random.Range(0, 100));
            Echoes += NewEchoes;
            Graffitis += NewGraffitis;
            Eggplants += NewEggplants;
            if (author == "You")
            {
                NB.AddEggplants(Mathf.RoundToInt((NewEggplants * NB.peeps) / 1000000f));
                if (Random.Range(0f, 1f) < Mathf.Min(0.5f, popularity * 100 + 0.00002f + NB.peeps * .00000001f))
                {
                    NB.AddPeep(Nest.GetComponent<Nest>().GenerateUsername());
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
                EggplantStat.GetComponent<Text>().text = LotsaEggplants[Random.Range(0, LotsaEggplants.Length)];
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

            if (author == "You")
            {
                Nest.GetComponent<Nest>().LosePeeps();
            }
            else if (Random.Range(0f,1f) < Mathf.Min(0.5f, + 0.0002f + popularity * 10 * NB.peeps * .0000001f))
            {
                NB.AddPeep(author);
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
            PlayPop();

            if (author == "You")
            {
                Nest.GetComponent<Nest>().LosePeeps();
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
                NB.AddPeep(author);
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
                NB.AddPeep(author);
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

    void OnBecameInvisible()
    {
        Debug.Log("Destroy");
        Destroy(gameObject);
    }
}
