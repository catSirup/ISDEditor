using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EditorManager : MonoBehaviour {
    public static EditorManager editorMgr = null;   // 에디터 매니저 싱글톤 처리.
    [SerializeField]
    private UIManager uiMgr;                        // UIManager.
    [SerializeField]
    private StatusBar stateBar;                     // BGM Status Bar;
    [SerializeField]
    private LineManager lineMgr;                    // Line 생성 매니저.
    [SerializeField]
    private XmlTools xmlTool;                       // 데이터 세이브 클래스.

    public AudioSource audioSource;                 // 배경음악 재생될 컴포넌트.
    public float tempTime;                          // 임시로 AudioTime.time을 저장할 변수.
    [SerializeField]
    private Note note_prefab;                       // 노트 생성할 프리팹
    public NoteManager noteMgr;                     // 노트 매니저
    [SerializeField]
    private Scrollbar scrollbar;                    // 효과음 스크롤 하기 위한 UI
    [SerializeField]
    private EffectSoundManager effectSoundMgr;      // 이펙트 사운드 매니저
    [SerializeField]
    private LineScrollbar lineScrollbar;            // 라인 스크롤 클래스
    // 구간반복 ///////////
    [SerializeField]
    private GameObject repeatRangePrf;                 // 반복구간 보여주는 오브젝트
    private GameObject repeatRange;
    public bool b_startRepeat;
    public bool b_readyRepeat;
    public Vector2 startPos;        // 구간반복의 시작
    public Vector2 endPos;          // 구간반복의 끝
    ///////////////////////

    void Awake()
    {
        if (editorMgr == null)
            editorMgr = this;

        else if (editorMgr != this)
            Destroy(this.gameObject);

        Init();
    }

    void Init()
    {
        b_readyRepeat = false;
        b_startRepeat = false;

        startPos = Vector2.zero;
        endPos = Vector2.zero;

        uiMgr.init();
        stateBar.Init();
        scrollbar.Init();
        effectSoundMgr.Init();
        lineScrollbar.Init();
    }

    void Update()
    {
        ShowCurAudioState();

        Ray2D ray = new Ray2D(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        #region 왼쪽 마우스 클릭
        if (Input.GetMouseButtonDown(0) && hit.collider != null)
        {
            if (hit.collider.gameObject.name == "Gauge" && audioSource.clip != null)
                stateBar.ChangeStatusBar(ref ray);

            if (hit.collider.gameObject.CompareTag("StatusBar") && audioSource.clip != null)
                stateBar.b_Drag = true;

            if ((hit.collider.gameObject.CompareTag("Line") || hit.collider.gameObject.CompareTag("LineLine")) && audioSource.clip != null)
                CreateNote(ref ray, ref hit);

            else if(hit.collider.gameObject.name == "Note")
            {
                if (NoteButton.b_touchNoteButton)
                    hit.collider.gameObject.GetComponent<Note>().SetNoteType();

                else if (ScoreButton.b_touchScoreButton)
                    hit.collider.gameObject.GetComponent<Note>().SetScoreType();

                else if (EffectSoundManager.b_TouchESD)
                    hit.collider.gameObject.GetComponent<Note>().SetEffect(EffectSoundManager.audioClip);

                else
                    hit.collider.gameObject.GetComponent<Note>().PlaySound();
            }

            if (hit.collider.gameObject.CompareTag("Scrollbar"))
                scrollbar.b_Drag = true;

            if (hit.collider.gameObject.CompareTag("EffectSound") && hit.collider.gameObject.GetComponent<EffectSound>().audioClip != null)
            {
                EffectSoundManager.b_TouchESD = true;
                NoteButton.b_touchNoteButton = false;
                ScoreButton.b_touchScoreButton = false;
                EffectSoundManager.audioClip = hit.collider.gameObject.GetComponent<EffectSound>().audioClip;
                AudioSource.PlayClipAtPoint(EffectSoundManager.audioClip, Camera.main.transform.position);
            }

            if (hit.collider.gameObject.CompareTag("LineScrollBar") && audioSource.clip != null)
                lineScrollbar.b_Drag = true;
        }
        #endregion

        #region 오른쪽 마우스 클릭
        else if (Input.GetMouseButtonDown(1) && hit.collider != null)
        {
            if (hit.collider.gameObject.name == "Note")
            {
                Note clone = hit.collider.gameObject.GetComponent<Note>();
                clone.DeleteNote();
                noteMgr.notes.Remove(clone);
            }

            if (hit.collider.gameObject.name == "Gauge" && audioSource.clip != null)
            {
                if (!b_readyRepeat && !b_startRepeat)
                {
                    repeatRange = GameObject.Instantiate(repeatRangePrf);
                    repeatRange.name = "RepeatRange";
                    repeatRange.transform.position = new Vector2(ray.origin.x, hit.collider.gameObject.transform.position.y);
                    startPos = ray.origin;
                    repeatRange.transform.localScale = new Vector3(0.05f, 0.5f, 1);
                    b_readyRepeat = true;
                }

                else if (!b_startRepeat && b_readyRepeat)
                {
                    if (startPos.x <= ray.origin.x)
                    {
                        repeatRange.transform.localScale = new Vector3((ray.origin.x - startPos.x), 0.5f, 1);
                        repeatRange.transform.position = new Vector2(repeatRange.transform.position.x /*+ (ray.origin.x - repeatRange.transform.position.x) * 0.1f*/, repeatRange.transform.position.y);
                        endPos = new Vector2(ray.origin.x, repeatRange.transform.position.y);
                    }

                    else
                    {
                        Vector2 tempPos = new Vector2(ray.origin.x, repeatRange.transform.position.y);
                        endPos = startPos;
                        startPos = tempPos;

                        repeatRange.transform.localScale = new Vector3(endPos.x - startPos.x, 0.5f, 1);
                        repeatRange.transform.position = new Vector2(repeatRange.transform.position.x - (repeatRange.transform.position.x - ray.origin.x), repeatRange.transform.position.y);
                    }

                    b_startRepeat = true;
                }
            }

            if (hit.collider.gameObject.CompareTag("EffectSound") && hit.collider.gameObject.GetComponent<EffectSound>().audioClip != null)
            {
                hit.collider.gameObject.GetComponent<EffectSound>().audioClip = null;
                hit.collider.gameObject.GetComponent<EffectSound>().text_name.text = "NULL";
            }
        }
        #endregion

        #region UI_ScrollBar 드래그
        if (scrollbar.b_Drag && !Input.GetMouseButtonUp(0))
            scrollbar.DragBar(ref ray);
        else if (scrollbar.b_Drag && Input.GetMouseButtonUp(0))
            scrollbar.b_Drag = false;
        #endregion

        #region UI_StatusBar 드래그
        if (stateBar.b_Drag && !Input.GetMouseButtonUp(0))
            stateBar.ChangeStatusBar(ref ray);

        else if (stateBar.b_Drag && Input.GetMouseButtonUp(0))
            stateBar.b_Drag = false;
        #endregion

        #region LineScrollBar
        if (lineScrollbar.b_Drag && !Input.GetMouseButtonUp(0))
            lineScrollbar.DragLine(ref ray);

        else if (lineScrollbar.b_Drag && Input.GetMouseButtonUp(0))
            lineScrollbar.b_Drag = false;
        #endregion

        #region UI_Line위의 타임라인 바
        if (audioSource.clip != null)
        {
            if (audioSource.isPlaying)
            {
                lineMgr.UpdateLine(audioSource.time, audioSource.clip.length);
                stateBar.MoveCurPosBar(audioSource.clip.length, audioSource.time);
            }
            else
            {
                lineMgr.UpdateLine(tempTime, audioSource.clip.length);
                stateBar.MoveCurPosBar(audioSource.clip.length, tempTime);
            }
        }
        #endregion

        #region 구간반복
        if (b_readyRepeat)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Destroy(GameObject.Find("RepeatRange"));
                b_readyRepeat = false;
                b_startRepeat = false;
                ButtonManager.b_Repeating = false;
            }

            if(!b_startRepeat)
            {
                Vector2 offset = ray.origin - startPos;
                repeatRange.transform.localScale = new Vector3(offset.x, repeatRange.transform.localScale.y, 1);
            }
        }

        if(ButtonManager.b_Repeating)
        {
            // UI_StateBar 전체 길이
            float standard = StatusBar.endPos.x - StatusBar.startPos.x;
            // 반복 구간 마지막 위치
            float repeatEndPosX = (endPos.x - StatusBar.startPos.x) / standard * audioSource.clip.length;

            if(audioSource.time > repeatEndPosX)
            {
                audioSource.time = (startPos.x - StatusBar.startPos.x) / standard * audioSource.clip.length;
            }
        }
        #endregion
    }

    /// <summary>
    /// 오디오 클립을 받아와서 셋팅함.
    /// </summary>
    /// <param name="_clip"> 재생될 BGM </param>
    public void SetAudioClip(AudioClip _clip)
    {
        audioSource.clip = _clip;
        uiMgr.SetCurBGMText("현재 BGM : " + _clip.name);
        lineMgr.CreateLine(audioSource.clip.length);
    }

    /// <summary>
    /// 현재 음악 상태 보여줌.
    /// </summary>
    public void ShowCurAudioState()
    {
        if (audioSource.clip != null)
        {
            if (audioSource.isPlaying)
                uiMgr.SetCurBGMStateText(uiMgr.ShowCurretAudioPlayTime(audioSource.time) + uiMgr.ShowCurrentAudioPlayPersent(audioSource.clip.length, audioSource.time));

            else
                uiMgr.SetCurBGMStateText(uiMgr.ShowCurretAudioPlayTime(tempTime) + uiMgr.ShowCurrentAudioPlayPersent(audioSource.clip.length, tempTime));
        }
    }

    /// <summary>
    /// 노트 생성
    /// </summary>
    /// <param name="ray"></param>
    /// <param name="hit"></param>
    public void CreateNote(ref Ray2D ray, ref RaycastHit2D hit)
    {
        Note clone = Note.Instantiate(note_prefab);
        clone.name = "Note";
        
        if (hit.collider.gameObject.CompareTag("Line"))
            clone.transform.position = new Vector3(hit.collider.gameObject.transform.position.x, hit.collider.gameObject.transform.position.y, -1);

        else if (hit.collider.gameObject.CompareTag("LineLine"))
            clone.transform.position = new Vector3(ray.origin.x, hit.collider.gameObject.transform.position.y, -1);

        clone.transform.parent = noteMgr.transform;
        clone.Init();

        noteMgr.notes.Add(clone);
    }

    /// <summary>
    /// 에디터 끌 때 호출
    /// </summary>
    void OnApplicationQuit()
    {
        // 데이터 세이브 다시 쓰기
        xmlTool.WriteTempXml();
    }
}
