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
    public int Echoes = 0;
    public int Graffitis = 0;
    public int Eggplants = 0;
    public GameObject EchoStat;
    public GameObject GraffitiStat;
    public GameObject EggplantStat;
    public AudioSource pop;
    public AudioClip[] popClips;

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
        if (transform.position.y < 0)
        {
            Destroy(gameObject);
        }
    }

    public void UpdateStats()
    {
        EchoStat.GetComponent<Text>().text = StatToString(Echoes);
        GraffitiStat.GetComponent<Text>().text = StatToString(Graffitis);
        EggplantStat.GetComponent<Text>().text = StatToString(Eggplants);
    }

    void PlayPop()
    {
        pop.clip = popClips[Random.Range(0, popClips.Length)];
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
            return k.ToString("F1") + "k";
        }
    }

    public void Echo()
    {

        if (!isEchoed)
        {
            Nest.GetComponent<Nest>().PostTwit("You", transform.Find("Content").GetComponent<Text>().text, Echoes, Graffitis, Eggplants);

            EchoButton.GetComponent<Image>().color = new Color32(0, 20, 255, 100);
            Echoes += 1;
            EchoStat.GetComponent<Text>().text = StatToString(Echoes);
            isEchoed = true;
            PlayPop();
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
        }
        else
        {
            EggplantButton.GetComponent<Image>().color = new Color(Eggplants % 255 / 255f, 20 / 255f, 1f, 1f);
            Eggplants += 1;
            EggplantStat.GetComponent<Text>().text = Eggplants.ToString();
            PlayPop();
        }
    }

    void OnBecameInvisible()
    {
        Debug.Log("Destroy");
        Destroy(gameObject);
    }
}
