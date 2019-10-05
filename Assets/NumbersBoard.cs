using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumbersBoard : MonoBehaviour
{

    public GameObject PeepCount;
    public GameObject EggplantCount;
    public int peeps;
    public long eggplants;
    private HashSet<string> Peepers = new HashSet<string>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RemovePeep()
    {
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
        if (Peepers.Contains(peep))
        {
            return false;
        }
        else
        {
            Peepers.Add(peep);
            peeps += 1;
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
