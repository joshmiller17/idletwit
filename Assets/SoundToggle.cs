using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundToggle : MonoBehaviour
{
    public bool music;
    //public bool sfx;
    public Button button;
    public Sprite musicImage;
    //public Sprite sfxOnlyImage;
    public Sprite noMusicImage;

    // Start is called before the first frame update
    void Start()
    {
        music = true;
        //sfx = true;
        button = GetComponent<Button>();
        button.image.sprite = musicImage;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Toggle()
    {
        AudioListener.volume = 1 - AudioListener.volume;
        if (!music)
        {
            music = true;
            //sfx = true;
            button.image.sprite = musicImage;
        }
        else
        {
            music = false;
            button.image.sprite = noMusicImage;
        }
    }
}
