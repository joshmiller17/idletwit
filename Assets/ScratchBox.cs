using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScratchBox : MonoBehaviour
{

    public GameObject UserText;
    public GameObject RealText;
    public GameObject TwitPost;
    public GameObject Placeholder;
    public GameObject Nest;
    public GameObject SMBox;


    private string nextTwit = "";

    private string[] testTwits = { "Blah blah", "test only", "what a twit", "this twit is really long and is only a test just a test to test testing" };
    private List<float> tutorialTimers = new List<float> {  3, 2,   1, 2, 3,   2, 3, 6,
                                                            2,5,3,   .8f,.7f,.9f,  8,3,10,    8, 3, 4,
                                                            3.6f, 4.2f, 5,  3 };
    private float timeToNextTutorial = 0;
    private List<string> tutorialTwits = new List<string>
    {
        "Welcome to Twit Idol!", //no timer

        "You must be here because you have no friends.",
        "No one to care about you",

        "No one who loves you",
        "NOTHING",
        "That's okay.",

        "We love you!",
        "Even nobodies like you can gain the famous VALIDATION MARK eventually!",
        "Don't you want to become a CERTIFIED TWITPOSTER?",

        "I know you do.",
        "Everyone starts from nothing on Twit Idol.",
        "But eventually, you might",

        "maybe",
        "if you're lucky",
        "become someone!!",
        
        "like",
        "I mean someone worth caring about",
        "...",


        "Type some Twits to make friends!",
        "See a post you like? Make sure to SMASH that Eggplant button!",
        "Click Echo to claim a Twit as your own!",

        "Feel like a Twit isn't enough about you? Click that Graffiti button to fix that!",
        "Eventually, you'll start to earn some Peeps who care about you!",
        "Reach MAXIMUM PEEPAGE to become a true TWIT IDOL!",

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
        nextTwit = testTwits[Random.Range(0, 4)];
    }

    public void ClearTwit()
    {
        RealText.GetComponent<Text>().text = "";
        UserText.GetComponent<Text>().text = "";
        GetComponent<InputField>().text = "";
        Placeholder.GetComponent<Text>().text = "What's scratchin' you?";
        GenerateNewTwit();
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
                if (tutorialTimers.Count > 0)
                {
                    timeToNextTutorial = tutorialTimers[0];
                    tutorialTimers.RemoveAt(0);
                }
            }
            else
            {
                timeToNextTutorial -= Time.deltaTime;
            }
        }

    }

    bool isTutorialGoing()
    {
        return tutorialTwits.Count > 0;
    }

    void SendTwit()
    {
        Nest.GetComponent<Nest>().PostTwit("You", RealText.GetComponent<Text>().text);
        ClearTwit();
        Nest.GetComponent<Nest>().twitIntensity -= 0.15f;
        Nest.GetComponent<Nest>().timeSinceLastTwit = 0f;
    }

    void PostNextTwitBit()
    {
        int charsLeft = nextTwit.Length;
        if (charsLeft == 0)
        {
            SendTwit();
            GenerateNewTwit();
            charsLeft = nextTwit.Length;
            Placeholder.GetComponent<Text>().text = "What's scratchin' you?";
        }
        else
        {
            int chars = Random.Range(1, Mathf.Min(charsLeft, 4));
            RealText.GetComponent<Text>().text += nextTwit.Substring(0, chars);
            nextTwit = nextTwit.Substring(chars);
            Placeholder.GetComponent<Text>().text = "";
        }
    }

	public void ConvertInputToScratch(string userInput){
        UserText.GetComponent<Text>().text = "";
        if (userInput != "")
        {
            PostNextTwitBit();
        }
	}
}
