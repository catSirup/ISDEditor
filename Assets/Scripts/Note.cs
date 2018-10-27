using UnityEngine;
using System.Collections;

public class Note : MonoBehaviour
{
    public AudioClip audioClip;
    public SpriteRenderer notice_Score;
    public Sprite[] scoreImg;
    public GameObject notice_Sound;
    public SpriteRenderer spriteRenderer;
    public NOTE_TYPE note_type;
    public int score;
    [SerializeField]
    private GameObject notePos_prefab;
    private GameObject notePos;
    private GameObject notePosParentObj;

    public void Init()
    {
        notePosParentObj = GameObject.Find("CurBGMStateBar").transform.Find("Gauge").gameObject;
        notice_Sound.SetActive(false);
        note_type = NOTE_TYPE.NONE;
        score = 0;
        spriteRenderer.color = Color.white;
        notice_Score.sprite = scoreImg[0];

        CreateNotePos();
    }
    private void CreateNotePos()
    {
        GameObject clone = GameObject.Instantiate(notePos_prefab);
        clone.name = "notePos";
        clone.transform.parent = notePosParentObj.transform;


        float ratio = (transform.localPosition.x - LineManager.startPos.x) / (LineManager.endPos.x - 0.6f - LineManager.startPos.x);
        float posX = StatusBar.startPos.x + (StatusBar.endPos.x - StatusBar.startPos.x) * ratio;

        clone.transform.position = new Vector2(posX, 1.4f);
        clone.SetActive(true);
        notePos = clone;
    }
    public void SetNoteType ()
    {
        note_type = NoteButton.curType;

        switch(note_type)
        {
            case NOTE_TYPE.TOUCH:
                spriteRenderer.color = new Color(255, 0, 255);
                break;

            case NOTE_TYPE.DRAG_UP:
                spriteRenderer.color = Color.red;
                break;

            case NOTE_TYPE.DRAG_RIGHT:
                spriteRenderer.color = Color.blue;
                break;

            case NOTE_TYPE.DRAG_LEFT:
                spriteRenderer.color = Color.green;
                break;

            case NOTE_TYPE.DRAG_DOWN:
                spriteRenderer.color = Color.yellow;
                break;

            default:
                spriteRenderer.color = Color.white;
                break;
        }
    }
    public void SetScoreType ()
    {
        score = ScoreButton.curScore;

        switch(score)
        {
            case 100:
                notice_Score.sprite = scoreImg[1];
                break;

            case 200:
                notice_Score.sprite = scoreImg[2];
                break;

            case 400:
                notice_Score.sprite = scoreImg[3];
                break;

            case 800:
                notice_Score.sprite = scoreImg[4];
                break;

            default:
                notice_Score.sprite = scoreImg[0];
                break;
        }
    }
    public void SetEffect(AudioClip clip)
    {
        audioClip = clip;

        if (audioClip == null)
            notice_Sound.SetActive(false);

        else if (audioClip != null)
            notice_Sound.SetActive(true);
    }
    /// <summary>
    /// 노트 눌렀을 때 음악 플레이
    /// </summary>
    public void PlaySound()
    {
        if(audioClip != null)
            AudioSource.PlayClipAtPoint(audioClip, Camera.main.transform.position);
    }
    /// <summary>
    /// 오디오 클립의 이름을 가져감.
    /// NULL 예외처리를 해주기 위해서 함수화 시킴.
    /// </summary>
    /// <returns></returns>
    public string GetAudioClipName()
    {
        if (audioClip == null)
            return "NONE";

        else
            return audioClip.name;
    }
    /// <summary>
    /// 노트 생성 시간을 비율에 맞춰서 변환시켜줌.
    /// </summary>
    /// <returns></returns> 
    public float AppearTime()
    {
        return (transform.position.x - LineManager.startPos.x) / 
            (LineManager.endPos.x - LineManager.startPos.x) * EditorManager.editorMgr.audioSource.clip.length;
    }
    /// <summary>
    /// 노트 삭제
    /// </summary>
    public void DeleteNote()
    {
        Destroy(notePos);
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name == "CurBGMPosition" && audioClip != null)
            PlaySound();
    }
}
