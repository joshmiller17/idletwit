using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityTracery;

public class BGM : MonoBehaviour
{
    public TraceryGrammar SongGrammar;
    public TextAsset SongGrammarFile;
    public AudioSource bgm;
    public AudioClip[] bgmClips; //C, c1, c7, A7, G, G2, F, F2
    public AudioClip[] goodClips;
    public AudioClip[] badClips;
    private float timeTilChordChange = 30;
    private Map<int, string> chords = new Map<int, string>();
    private string currentChord = "c";
    public bool doingGood = true;

    // Start is called before the first frame update
    void Start()
    {
        bgm = GetComponent<AudioSource>();
        SongGrammar = new TraceryGrammar(SongGrammarFile.text);
        chords.Add(0, "c");
        chords.Add(1, "c1");
        chords.Add(2, "c7");
        chords.Add(3, "a7");
        chords.Add(4, "g");
        chords.Add(5, "g2");
        chords.Add(6, "f");
        chords.Add(7, "f2");
    }

    // Update is called once per frame
    void Update()
    {
        timeTilChordChange -= Time.deltaTime;
        if (timeTilChordChange < 0)
        {
            SetChord();
            timeTilChordChange = 30 + Random.Range(0, 30);
        }
    }

    void SetChord()
    {
        //int whichClip = 0;
        //string newChord = SongGrammar.Parse("#" + currentChord + "#");
        //if (!doingGood && currentChord == "c")
        //{
        //    currentChord = "c1";
        //}
        //else
        //{
        //    currentChord = newChord;
        //}
        //whichClip = chords.Reverse[currentChord];
        //bgm.clip = bgmClips[whichClip];

        if (doingGood)
        {
            bgm.clip = goodClips[Random.Range(0, goodClips.Length)];
        }
        else
        {
            bgm.clip = badClips[Random.Range(0, badClips.Length)];
        }
        bgm.PlayOneShot(bgm.clip);
    }
}

public class Map<T1, T2>
{
    private Dictionary<T1, T2> _forward = new Dictionary<T1, T2>();
    private Dictionary<T2, T1> _reverse = new Dictionary<T2, T1>();

    public Map()
    {
        this.Forward = new Indexer<T1, T2>(_forward);
        this.Reverse = new Indexer<T2, T1>(_reverse);
    }

    public class Indexer<T3, T4>
    {
        private Dictionary<T3, T4> _dictionary;
        public Indexer(Dictionary<T3, T4> dictionary)
        {
            _dictionary = dictionary;
        }
        public T4 this[T3 index]
        {
            get { return _dictionary[index]; }
            set { _dictionary[index] = value; }
        }
    }

    public void Add(T1 t1, T2 t2)
    {
        _forward.Add(t1, t2);
        _reverse.Add(t2, t1);
    }

    public Indexer<T1, T2> Forward { get; private set; }
    public Indexer<T2, T1> Reverse { get; private set; }
}