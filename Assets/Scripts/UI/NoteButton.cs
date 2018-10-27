using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum NOTE_TYPE { TOUCH = 0, DRAG_UP, DRAG_RIGHT, DRAG_LEFT, DRAG_DOWN, NONE }

public class NoteButton : MonoBehaviour {
    public static NOTE_TYPE curType = NOTE_TYPE.NONE;
    public static bool b_touchNoteButton = false;

    public Image[] img;

    public void TOUCH()
    {
        b_touchNoteButton = true;
        ScoreButton.b_touchScoreButton = false;
        EffectSoundManager.b_TouchESD = false;
        curType = NOTE_TYPE.TOUCH;
    }
    public void DRAG_UP()
    {
        b_touchNoteButton = true;
        ScoreButton.b_touchScoreButton = false;
        EffectSoundManager.b_TouchESD = false;
        curType = NOTE_TYPE.DRAG_UP;
    }
    public void DRAG_DOWN()
    {
        b_touchNoteButton = true;
        ScoreButton.b_touchScoreButton = false;
        EffectSoundManager.b_TouchESD = false;
        curType = NOTE_TYPE.DRAG_DOWN;
    }
    public void DRAG_RIGHT()
    {
        b_touchNoteButton = true;
        ScoreButton.b_touchScoreButton = false;
        EffectSoundManager.b_TouchESD = false;
        curType = NOTE_TYPE.DRAG_RIGHT;
    }
    public void DRAG_LEFT()
    {
        b_touchNoteButton = true;
        ScoreButton.b_touchScoreButton = false;
        EffectSoundManager.b_TouchESD = false;
        curType = NOTE_TYPE.DRAG_LEFT;
    }
    /// <summary>
    /// 현재 노트 타입을 string->NoteType 변경
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static NOTE_TYPE GetNoteType(string str)
    {
        switch (str)
        {
            case "DRAG_UP":
                return NOTE_TYPE.DRAG_UP;

            case "DRAG_RIGHT":
                return NOTE_TYPE.DRAG_RIGHT;

            case "DRAG_LEFT":
                return NOTE_TYPE.DRAG_LEFT;

            case "DRAG_DOWN":
                return NOTE_TYPE.DRAG_DOWN;

            case "TOUCH":
                return NOTE_TYPE.TOUCH;

            default:
                return NOTE_TYPE.NONE;
        }
    }
    void Awake()
    {
        curType = NOTE_TYPE.NONE;
    }
    void Update()
    {
        if (curType != NOTE_TYPE.NONE && b_touchNoteButton)
        {
            for (int i = 0; i < img.Length; i++)
            {
                if (i == (int)curType)
                    img[i].color = Color.gray;

                else
                    img[i].color = Color.white;
            }
        }

        else if(!b_touchNoteButton)
        {
            for (int i = 0; i < img.Length; i++)
            {
                img[i].color = Color.white;
            }
        }
    }

}
