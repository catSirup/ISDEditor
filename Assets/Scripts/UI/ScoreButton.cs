using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreButton : MonoBehaviour
{
    public static int curScore = 0;
    public static bool b_touchScoreButton = false;

    public Image[] img;

    public void Button_100()
    {
        b_touchScoreButton = true;
        NoteButton.b_touchNoteButton = false;
        EffectSoundManager.b_TouchESD = false;
        curScore = 100;
    }

    public void Button_200()
    {
        b_touchScoreButton = true;
        NoteButton.b_touchNoteButton = false;
        EffectSoundManager.b_TouchESD = false;
        curScore = 200;
    }

    public void Button_400()
    {
        b_touchScoreButton = true;
        NoteButton.b_touchNoteButton = false;
        EffectSoundManager.b_TouchESD = false;
        curScore = 400;
    }

    public void Button_800()
    {
        b_touchScoreButton = true;
        NoteButton.b_touchNoteButton = false;
        EffectSoundManager.b_TouchESD = false;
        curScore = 800;
    }

    void Update()
    {
        if (b_touchScoreButton)
        {
            switch (curScore)
            {
                case 100:
                    img[0].color = Color.gray;
                    img[1].color = Color.white;
                    img[2].color = Color.white;
                    img[3].color = Color.white;
                    break;

                case 200:
                    img[0].color = Color.white;
                    img[1].color = Color.gray;
                    img[2].color = Color.white;
                    img[3].color = Color.white;
                    break;

                case 400:
                    img[0].color = Color.white;
                    img[1].color = Color.white;
                    img[2].color = Color.gray;
                    img[3].color = Color.white;
                    break;

                case 800:
                    img[0].color = Color.white;
                    img[1].color = Color.white;
                    img[2].color = Color.white;
                    img[3].color = Color.gray;
                    break;

                default:
                    img[0].color = Color.white;
                    img[1].color = Color.white;
                    img[2].color = Color.white;
                    img[3].color = Color.white;
                    break;
            }
        }

        else
        {
            for (int i = 0; i < img.Length; i++)
                img[i].color = Color.white;
        }

    }
}
