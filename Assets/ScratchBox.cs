using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScratchBox : MonoBehaviour
{

    public GameObject UserText;
    public GameObject RealText;
    public GameObject TwitPost;
    public GameObject Nest;
    public GameObject SMBox;


    private string nextTwit = "";

    private string[] testTwits = { "Blah blah", "test only", "what a twit" };
    private List<int> tutorialTimers = new List<int>{ 4, 5, 7, 4 };
    private float timeToNextTutorial = 0;
    private List<string> tutorialTwits = new List<string>
    {
        "Welcome to Twit Idol!",
        "Type some Twits to make friends!",
        "See a post you like? Make sure to SMASH that Eggplant button!",
        "Now get twitposting!!1!"
    };

    // Start is called before the first frame update
    void Start()
    {
        GenerateNewTwit();

    }


    //TODO
    void GenerateNewTwit()
    {
        nextTwit = testTwits[Random.Range(0, 3)];
    }

    // Update is called once per frame
    void Update()
    {
        if (tutorialTwits.Count > 0)
        {
            if (timeToNextTutorial < 0)
            {
                SMBox.GetComponent<Nest>().PostTwit("Teh Twit Birb (TM)", tutorialTwits[0]);
                tutorialTwits.RemoveAt(0);
                timeToNextTutorial = tutorialTimers[0];
                tutorialTimers.RemoveAt(0);
            }
            else
            {
                timeToNextTutorial -= Time.deltaTime;
            }
        }
    }

    //TODO
    void SendTwit()
    {
        Nest.GetComponent<Nest>().PostTwit("You", RealText.GetComponent<Text>().text);
        RealText.GetComponent<Text>().text = ""; //clear field
    }

    void PostNextTwitBit()
    {
        int charsLeft = nextTwit.Length;
        if (charsLeft == 0)
        {
            SendTwit();
            GenerateNewTwit();
            charsLeft = nextTwit.Length;
        }
        int chars = Random.Range(1, Mathf.Min(charsLeft, 4));
        RealText.GetComponent<Text>().text += nextTwit.Substring(0, chars);
        nextTwit = nextTwit.Substring(chars);
    }

	public void ConvertInputToScratch(string userInput){
        UserText.GetComponent<Text>().text = "";
        PostNextTwitBit();
	}
}
