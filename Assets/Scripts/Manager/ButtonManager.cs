using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour {
    [SerializeField]
    private LoadData loadData;              // 데이터로드 전용 스크립트.
    [SerializeField]
    private UIManager uiMgr;                // UIManager.
    [SerializeField]
    private SaveData saveData;
    [SerializeField]
    private Image img_Repeat;
    [SerializeField]
    private Image img_Start;
    [SerializeField]
    private Image img_Pause;
    [SerializeField]
    private Image img_Stop;

    public static bool b_LoadBGM = false;
    public static bool b_LoadESD = false;
    public static bool b_Repeating = false;

    private bool b_Pause = false;
    private bool b_Stop = false;

    /// <summary>
    /// BGM(Background Music) 로딩
    /// </summary>
    public void LoadBGM()
    {
        b_LoadBGM = true;
        b_LoadESD = false;

        loadData.LoadAudioClip();
    }
    /// <summary>
    /// ESD(Effect Sound) 로딩
    /// </summary>
    public void LoadESD()
    {
        b_LoadBGM = false;
        b_LoadESD = true;

        loadData.LoadAudioClip();
    }
  
    /// <summary>
    /// 음악 재생
    /// </summary>
    public void Play()
    {
        // 아직 실행 중이지 않으면서 로딩이 완료된 상태일 때
        if (EditorManager.editorMgr.audioSource.clip != null
            && !EditorManager.editorMgr.audioSource.isPlaying
            && EditorManager.editorMgr.audioSource.clip.loadState == AudioDataLoadState.Loaded)
        {
            EditorManager.editorMgr.audioSource.time = EditorManager.editorMgr.tempTime;
            EditorManager.editorMgr.audioSource.Play();
            b_Pause = false;
            b_Stop = false;
        }
    }
    /// <summary>
    /// 음악 종료
    /// </summary>
    public void Stop()
    {
        uiMgr.SetCurBGMStateText("현재 위치 00 : 00 : 00 (00.00%)");
        EditorManager.editorMgr.audioSource.Stop();
        EditorManager.editorMgr.tempTime = 0.0f;

        b_Pause = false;
        b_Stop = true;
    }
    /// <summary>
    /// 음악 일시정지
    /// </summary>
    public void Pause()
    {
        if(EditorManager.editorMgr.audioSource.isPlaying)
        {
            EditorManager.editorMgr.tempTime = EditorManager.editorMgr.audioSource.time;
            EditorManager.editorMgr.audioSource.Stop();

            b_Pause = true;
            b_Stop = false;
        }
    }
    /// <summary>
    /// 구간 반복
    /// </summary>
    public void Repeat()
    {
        if (EditorManager.editorMgr.audioSource.clip != null)
        {
            float standard = StatusBar.endPos.x - StatusBar.startPos.x;
            float length = EditorManager.editorMgr.audioSource.clip.length;
            float repeatStartPosX = EditorManager.editorMgr.startPos.x - StatusBar.startPos.x;

            if (!b_Repeating && EditorManager.editorMgr.b_startRepeat)
            {
                if (EditorManager.editorMgr.audioSource.isPlaying)
                    EditorManager.editorMgr.audioSource.time = repeatStartPosX / standard * length;

                else
                    EditorManager.editorMgr.tempTime = repeatStartPosX / standard * length;

                img_Repeat.color = Color.gray;
                b_Repeating = true;
            }
        }
    }
    /// <summary>
    /// 효과음 듣기
    /// </summary>
    public void Listen()
    {
        NoteButton.b_touchNoteButton = false;
        ScoreButton.b_touchScoreButton = false;
        EffectSoundManager.b_TouchESD = false;
    }
    /// <summary>
    /// 파일 저장
    /// </summary>
    public void Save()
    {
        if(EditorManager.editorMgr.audioSource.clip != null)
            saveData.Save();
    }
    /// <summary>
    /// 작업 중인 파일 로드
    /// </summary>
    public void LoadData()
    {
        loadData.LoadTxt();
    }

    void Update()
    {
        if (EditorManager.editorMgr.audioSource.clip != null)
        {
            if (!b_Stop && !b_Pause && EditorManager.editorMgr.audioSource.isPlaying)
            {
                img_Start.color = Color.gray;
                img_Pause.color = Color.white;
                img_Stop.color = Color.white;
            }

            else if (!b_Stop && !b_Pause && !EditorManager.editorMgr.audioSource.isPlaying)
            {
                img_Start.color = Color.white;
                img_Pause.color = Color.white;
                img_Stop.color = Color.white;
            }

            else if (!b_Stop && b_Pause && !EditorManager.editorMgr.audioSource.isPlaying)
            {
                img_Start.color = Color.white;
                img_Pause.color = Color.gray;
                img_Stop.color = Color.white;
            }

            else if (b_Stop && !b_Pause && !EditorManager.editorMgr.audioSource.isPlaying)
            {
                img_Start.color = Color.white;
                img_Pause.color = Color.white;
                img_Stop.color = Color.gray;
            }
        }


        if (!b_Repeating && Input.GetKeyDown(KeyCode.Escape))
            img_Repeat.color = Color.white;
    }
}
