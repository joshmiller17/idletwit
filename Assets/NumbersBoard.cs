using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumbersBoard : MonoBehaviour
{

    public GameObject PeepCount;
    public GameObject EggplantCount;
    public GameObject DeleteButton;
    public GameObject Cloud;
    public GameObject PeepCloud;
    public GameObject bgm;
    public int peeps;
    public long eggplants;
    public int maxPeepsEver = 0;
    private HashSet<string> Peepers = new HashSet<string>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (maxPeepsEver > 10 && peeps < 0.8 * maxPeepsEver)
        {
            DeleteButton.SetActive(true);
        }

        // Cloud spawner
        if (Random.Range(0f, 100f) < .006)
        {
            GameObject spawn = Instantiate(Cloud, new Vector3(-1000, Random.Range(-500, 1000), 0), Quaternion.identity);
            spawn.transform.SetParent(gameObject.transform.parent);
        }
    }

    public void RemovePeep()
    {
        bgm.GetComponent<BGM>().doingGood = false;
        if (peeps < 3)
        {
            return;
        }
        HashSet<string>.Enumerator e = Peepers.GetEnumerator();
        string lostPeep = e.Current;
        Peepers.Remove(lostPeep);
        peeps -= 1;
    }

    public bool AddPeep(string peep)
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject spawn = Instantiate(PeepCloud, new Vector3(-1000, Random.Range(-500, 1000), 0), Quaternion.identity);
            spawn.transform.SetParent(gameObject.transform.parent);
        }
        if (Peepers.Contains(peep) || eggplants < 100 * peeps)
        {
            return false;
        }
        else
        {
            if (Random.Range(0f, 1f) > 0.995f)
            {
                bgm.GetComponent<BGM>().doingGood = true;
            }
            Peepers.Add(peep);
            peeps += 1;
            if (peeps > maxPeepsEver)
            {
                maxPeepsEver = peeps;
            }
            PeepCount.GetComponent<Text>().text = peeps.ToString();
            return true;
        }
    }

    public void AddEggplants(int Eggplants)
    {
       
        eggplants += Eggplants;
        if (eggplants < 1000000)
        {
            EggplantCount.GetComponent<Text>().text = eggplants.ToString();
        }
        else
        {
            EggplantCount.GetComponent<Text>().text = (eggplants / 1000000f).ToString("F1") + "m";
        }
    }
}
