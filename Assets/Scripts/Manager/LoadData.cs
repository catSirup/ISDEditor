using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System.IO;

public class LoadData : MonoBehaviour {
    public string fileUrl;
    private AudioClip audioClip;
    [SerializeField]
    private EffectSoundManager effectSoundMgr;
    [SerializeField]
    private LineManager lineMgr;
    [SerializeField]
    private XmlTools xmlTool;

    /// <summary>
    /// 탐색기를 열어서 파일 가져옴.
    /// </summary>
    public void LoadAudioClip()
    {
        if (EditorManager.editorMgr.audioSource.isPlaying)
            EditorManager.editorMgr.audioSource.Stop();


        OpenFileName ofn = new OpenFileName();
        ofn.structSize = Marshal.SizeOf(ofn);
        ofn.filter = "OGG Files\0*.*\0\0";
        ofn.file = new string(new char[256]);
        ofn.maxFile = ofn.file.Length;
        ofn.fileTitle = new string(new char[64]);
        ofn.initialDir = UnityEngine.Application.dataPath;
        ofn.title = "Open Music";
        ofn.defExt = "ogg";
        ofn.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;

        if (DllTest.GetOpenFileName(ofn))
        {
            fileUrl = ofn.file;

            if (ButtonManager.b_LoadBGM)
                Data.fileURL = fileUrl;
            else if (ButtonManager.b_LoadESD)
                effectSoundMgr.fileURL = fileUrl;

            StartCoroutine(WaitLoadBGM(fileUrl));
        }

        if (ofn.file == "")
        {
            return;
        }
    }


    public void LoadTxt()
    {
        if (EditorManager.editorMgr.audioSource.isPlaying)
            EditorManager.editorMgr.audioSource.Stop();


        OpenFileName ofn = new OpenFileName();
        ofn.structSize = Marshal.SizeOf(ofn);
        ofn.filter = "OGG Files\0*.*\0\0";
        ofn.file = new string(new char[256]);
        ofn.maxFile = ofn.file.Length;
        ofn.fileTitle = new string(new char[64]);
        ofn.initialDir = UnityEngine.Application.dataPath;
        ofn.title = "Open Data";
        ofn.defExt = "xml";
        ofn.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;

        if (DllTest.GetOpenFileName(ofn))
        {
            fileUrl = ofn.file;
            StartCoroutine(xmlTool.LoadXml(fileUrl));
            //StartCoroutine(WaitLoadTXT(fileUrl));
        }

        if (ofn.file == "")
        {
            if (EditorManager.editorMgr.audioSource.clip != null) EditorManager.editorMgr.audioSource.Play();
            return;
        }
    }
    /// <summary>
    /// BGM로딩..
    /// 파일 형식을 무조건 OGG 형식으로 해야 WepPlayer와 WindowStandard에서 쓸 수 있다.
    /// 안드로이드에서는 mp3형식.
    /// </summary>
    /// <param name="url"> 파일 경로 </param>
    /// <returns></returns>
    IEnumerator WaitLoadBGM(string url)
    {
        WWW www_Bgm = new WWW("file://" + url);
        // 데이터가 로딩될 때까지 기다림.
        yield return www_Bgm;
        audioClip = www_Bgm.GetAudioClip();
        audioClip.name = redefName(fileUrl);

        Data.name = audioClip.name;
        Data.length = audioClip.length;

        if (ButtonManager.b_LoadBGM)
            EditorManager.editorMgr.SetAudioClip(audioClip);

        else if (ButtonManager.b_LoadESD)
            effectSoundMgr.SetClip(audioClip);
    }

    #region Txt Load
    /// <summary>
    /// TXT파일 로딩
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    //IEnumerator WaitLoadTXT(string url)
    //{
    //    StreamReader sr = new StreamReader(url);
    //    sr.ReadLine(); // "BGM 저장경로"
    //    string fileURL = sr.ReadLine(); // 저장경로 읽어옴.
    //    WWW www_Bgm = new WWW("file://" + fileURL);
    //    yield return www_Bgm;

    //    audioClip = www_Bgm.audioClip;
    //    audioClip.name = redefName(fileURL);
    //    Data.name = audioClip.name;
    //    Data.length = audioClip.length;
    //    EditorManager.editorMgr.SetAudioClip(audioClip);

    //    sr.ReadLine(); // "ESD 개수"
    //    int esdCount = int.Parse(sr.ReadLine()); // ESD 카운트 체크함.
    //    sr.ReadLine(); // "ESD 저장경로"
    //    // effect sound 채워넣기.
    //    for (int i = 0; i < esdCount; i++ )
    //    {
    //        string file_url = sr.ReadLine();
    //        WWW www_esd = new WWW("file://" + file_url);
    //        yield return www_esd;
    //        effectSoundMgr.esd[i].audioClip = www_esd.audioClip;
    //        effectSoundMgr.esd[i].audioClip.name = redefName(file_url);
    //        effectSoundMgr.esd[i].text_name.text = effectSoundMgr.esd[i].audioClip.name;
    //    }

    //    sr.ReadLine(); // "노트 생성 개수"
    //    int count = int.Parse(sr.ReadLine());
    //    sr.ReadLine(); // "설명줄"
    //    for(int i = 0; i < count; i++)
    //    {
    //        string notes = sr.ReadLine();
    //        string[] notes_divide = notes.Split('\t');
    //        Note clone = Note.Instantiate(note_prefab);
    //        clone.transform.position = new Vector3(float.Parse(notes_divide[4]), -1, -1);
    //        clone.Init();
    //        NoteButton.curType = NoteButton.GetNoteType(notes_divide[0]);
    //        clone.SetNoteType();
    //        ScoreButton.curScore = int.Parse(notes_divide[2]);
    //        clone.SetScoreType();
    //        EffectSoundManager.audioClip = effectSoundMgr.GetClip(notes_divide[3]);
    //        clone.SetEffect();
    //        clone.transform.parent = EditorManager.editorMgr.noteMgr.transform;
    //        clone.name = "Note";
    //        EditorManager.editorMgr.noteMgr.notes.Add(clone);
    //    }

    //    NoteButton.b_touchNoteButton = false;
    //    ScoreButton.b_touchScoreButton = false;
    //    EffectSoundManager.b_TouchESD = false;

    //    yield return null;
    //}
    #endregion

    /// <summary>
    /// 파일 경로로 불러왔기 때문에 파일 이름명만 따로 떼내기 위해서 사용.
    /// </summary>
    /// <param name="name"> 파일 경로 + 파일 이름 </param>
    /// ex) C:\.......bgm.ogg -> bgm
    /// <returns></returns>
    public static string redefName(string name)
    {
        string temp_name;
        // 첫번째로 \로 떼어내서 파일이름.확장자 만 남김.
        string[] divideName_First = name.Split('\\');   
        temp_name = divideName_First[divideName_First.Length - 1];
        // 파일 이름과 확장자를 떼어내기 위해서 한번 더 분리.
        string[] divideName_Second = temp_name.Split('.');
        return divideName_Second[0];
    }

    /// <summary>
    /// 마지막 저장 경로 이름 받아옴.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static string redefLastDirectory(string name)
    {
        string temp_name;
        // 첫번째로 \로 떼어내서 파일이름.확장자 만 남김.
        string[] divideName_First = name.Split('\\');
        temp_name = divideName_First[divideName_First.Length - 2];
        return temp_name;
    }
}
