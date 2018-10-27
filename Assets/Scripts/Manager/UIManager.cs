using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    [SerializeField]
    private Text curBGMStateText;
    [SerializeField]
    private Text curBGMText;

    public void init()
    {
        curBGMStateText.text = "현재 위치 00 : 00 : 00 (00.00%)";
        curBGMText.text = "선택된 BGM이 없습니다.";
    }

    public void SetCurBGMStateText(string str)  { curBGMStateText.text = str; }
    public Text GetCurBGMStateText()            { return curBGMStateText; }
    public void SetCurBGMText(string str)       { curBGMText.text = str; }
    public Text GetCurBGMText()                 { return curBGMText; }

    /// <summary>
    /// 현재 음악의 진행 시간을 문자열로 리턴함.
    /// </summary>
    /// <param name="time"> 현재 음악의 진행 시간 </param>
    /// <returns></returns>
    public string ShowCurretAudioPlayTime(float time)
    {
        int minutes = (int)(time / 60);
        int second = (int)(time - minutes * 60);
        int milisec = (int)((time - second - minutes * 60) * 100);

        if (second >= 10)
        {
            if (milisec >= 10)
                return "현재 위치 " + "0" + minutes.ToString() + " : " + second.ToString() + " : " + milisec.ToString();
            else
                return "현재 위치 " + "0" + minutes.ToString() + " : " + second.ToString() + " : 0" + milisec.ToString();
        }
        else
        {
            if (milisec >= 10)
                return "현재 위치 " + "0" + minutes.ToString() + " : 0" + second.ToString() + " : " + milisec.ToString();
            else
                return "현재 위치 " + "0" + minutes.ToString() + " : 0" + second.ToString() + " : 0" + milisec.ToString();
        }
    }

    /// <summary>
    /// 현재 음악의 진행 시간을 퍼센트화 시켜서 문자열로 리턴함.
    /// </summary>
    /// <param name="length"> 음악의 총 길이 </param>
    /// <param name="time"> 현재 진행 시간</param>
    /// <returns></returns>
    public string ShowCurrentAudioPlayPersent(float length, float time)
    {
        int onesUnit = (int)((time / length) * 100);
        int miliUnit = (int)(((time / length) * 100 - onesUnit) * 100);

        if (onesUnit < 10)
        {
            if (miliUnit >= 10)
                return " (0" + onesUnit.ToString() + "." + miliUnit.ToString() + "%)";
            else
                return " (0" + onesUnit.ToString() + ".0" + miliUnit.ToString() + "%)";
        }

        else if (onesUnit >= 10 && onesUnit < 100)
        {
            if (miliUnit >= 10)
                return " (" + onesUnit.ToString() + "." + miliUnit.ToString() + "%)";
            else
                return " (" + onesUnit.ToString() + ".0" + miliUnit.ToString() + "%)";
        }

        else
            return " (100.00%)";
    }
}
