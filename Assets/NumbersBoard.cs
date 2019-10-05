using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumbersBoard : MonoBehaviour
{

    public GameObject PeepCount;
    public GameObject EggplantCount;
    public GameObject DeleteButton;
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
        if (maxPeepsEver > 100 && peeps < maxPeepsEver / 2)
        {
            DeleteButton.SetActive(true);
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
        bgm.GetComponent<BGM>().doingGood = true;
        if (Peepers.Contains(peep) || eggplants < 100 * peeps)
        {
            return false;
        }
        else
        {
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
