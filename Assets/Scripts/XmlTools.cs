using UnityEngine;
using System.Collections;
using System.IO;
using System.Xml;

public class XmlTools : MonoBehaviour
{
    public XmlDocument xmlFile = new XmlDocument();
    public EffectSoundManager esdMgr;
    public NoteManager noteMgr;

    [SerializeField]
    private Note note_prefab;

    XmlNode rootNode;

    /// <summary>
    /// 위치를 지정해서 세이브함.
    /// </summary>
    /// <param name="filePath"> 파일 경로 </param>
    public void WriteXml(string filePath)
    {
        string fileName = LoadData.redefName(filePath);

        if (!xmlFile.HasChildNodes)
        {
            xmlFile.AppendChild(xmlFile.CreateXmlDeclaration("1.0", "utf-8", "yes"));
            // 루트노드 생성
            rootNode = xmlFile.CreateNode(XmlNodeType.Element, LoadData.redefLastDirectory(filePath), string.Empty);
            xmlFile.AppendChild(rootNode);

            #region Background Sound
            // 배경음 부모노드 생성
            XmlNode BGMNode = xmlFile.CreateNode(XmlNodeType.Element, "BGM", string.Empty);
            rootNode.AppendChild(BGMNode);

            // 배경음 이름
            XmlNode BGMName = xmlFile.CreateNode(XmlNodeType.Element, "Name", string.Empty);
            if (EditorManager.editorMgr.audioSource.clip != null)
                BGMName.InnerText = EditorManager.editorMgr.audioSource.clip.name;
            else
                BGMName.InnerText = "Null";
            BGMNode.AppendChild(BGMName);

            // 배경음 경로
            XmlNode BGMDirectory = xmlFile.CreateNode(XmlNodeType.Element, "Directory", string.Empty);
            if (Data.fileURL != null)
                BGMDirectory.InnerText = Data.fileURL;
            else
                BGMDirectory.InnerText = "Null";
            BGMNode.AppendChild(BGMDirectory);
            #endregion

            #region Effect Sound
            // 효과음 부모 노드
            XmlNode ESNode = xmlFile.CreateNode(XmlNodeType.Element, "EffectSound", string.Empty);
            rootNode.AppendChild(ESNode);

            // 효과음 개수
            XmlNode ESCount = xmlFile.CreateNode(XmlNodeType.Element, "Count", string.Empty);
            ESCount.InnerText = esdMgr.addClipCount.ToString();
            ESNode.AppendChild(ESCount);

            for (int i = 0; i < esdMgr.addClipCount; i++)
            {
                // 효과음 자식 노드
                XmlNode ESChildNode = xmlFile.CreateNode(XmlNodeType.Element, "EffectSound_" + i.ToString(), string.Empty);
                ESNode.AppendChild(ESChildNode);

                // 효과음 이름
                XmlNode ESName = xmlFile.CreateNode(XmlNodeType.Element, "Name", string.Empty);
                ESName.InnerText = esdMgr.esd[i].audioClip.name;
                ESChildNode.AppendChild(ESName);

                // 효과음 경로
                XmlNode ESDirectory = xmlFile.CreateNode(XmlNodeType.Element, "ESDirectory", string.Empty);
                ESDirectory.InnerText = esdMgr.esd[i].fileURL;
                ESChildNode.AppendChild(ESDirectory);
            }
            #endregion

            #region Notes
            // 노트 부모노드 생성
            XmlNode noteNode = xmlFile.CreateNode(XmlNodeType.Element, "Note", string.Empty);
            rootNode.AppendChild(noteNode);

            // 노트 카운트
            XmlNode createNoteCount = xmlFile.CreateNode(XmlNodeType.Element, "Count", string.Empty);
            createNoteCount.InnerText = noteMgr.notes.Count.ToString();
            noteNode.AppendChild(createNoteCount);

            for (int i = 0; i < noteMgr.notes.Count; i++)
            {
                // 노트 자식노드 생성
                XmlNode noteChildNode = xmlFile.CreateNode(XmlNodeType.Element, "Note_" + i.ToString(), string.Empty);
                noteNode.AppendChild(noteChildNode);

                // 노트 타입
                XmlNode noteType = xmlFile.CreateNode(XmlNodeType.Element, "NOTE_TYPE", string.Empty);
                noteType.InnerText = noteMgr.notes[i].note_type.ToString();
                noteChildNode.AppendChild(noteType);

                // 노트 출현 시간
                XmlNode noteTime = xmlFile.CreateNode(XmlNodeType.Element, "Time", string.Empty);
                noteTime.InnerText = noteMgr.notes[i].AppearTime().ToString();
                noteChildNode.AppendChild(noteTime);

                // 노트 점수
                XmlNode noteScore = xmlFile.CreateNode(XmlNodeType.Element, "Score", string.Empty);
                noteScore.InnerText = noteMgr.notes[i].score.ToString();
                noteChildNode.AppendChild(noteScore);

                // 노트 효과음 이름
                XmlNode noteESName = xmlFile.CreateNode(XmlNodeType.Element, "EffectSound", string.Empty);
                noteESName.InnerText = noteMgr.notes[i].GetAudioClipName();
                noteChildNode.AppendChild(noteESName);

                // Position.X
                XmlNode notePosX = xmlFile.CreateNode(XmlNodeType.Element, "XPositon", string.Empty);
                notePosX.InnerText = noteMgr.notes[i].transform.position.x.ToString();
                noteChildNode.AppendChild(notePosX);

                // Position.Y
                XmlNode notePosY = xmlFile.CreateNode(XmlNodeType.Element, "YPosition", string.Empty);
                notePosY.InnerText = noteMgr.notes[i].transform.position.y.ToString();
                noteChildNode.AppendChild(notePosY);
            }
            #endregion

            xmlFile.Save(filePath);
        }

        else
        {
            XmlDocument xmlTempFile = new XmlDocument();
            xmlTempFile.AppendChild(xmlTempFile.CreateXmlDeclaration("1.0", "utf-8", "yes"));
            rootNode = xmlTempFile.CreateNode(XmlNodeType.Element, LoadData.redefLastDirectory(filePath), string.Empty);
            xmlTempFile.AppendChild(rootNode);

            #region Background Sound
            // 배경음 부모노드 생성
            XmlNode BGMNode = xmlTempFile.CreateNode(XmlNodeType.Element, "BGM", string.Empty);
            rootNode.AppendChild(BGMNode);

            // 배경음 이름
            XmlNode BGMName = xmlTempFile.CreateNode(XmlNodeType.Element, "Name", string.Empty);
            if (EditorManager.editorMgr.audioSource.clip != null)
                BGMName.InnerText = EditorManager.editorMgr.audioSource.clip.name;
            else
                BGMName.InnerText = "Null";
            BGMNode.AppendChild(BGMName);

            // 배경음 경로
            XmlNode BGMDirectory = xmlTempFile.CreateNode(XmlNodeType.Element, "Directory", string.Empty);
            if (Data.fileURL != null)
                BGMDirectory.InnerText = Data.fileURL;
            else
                BGMDirectory.InnerText = "Null";
            BGMNode.AppendChild(BGMDirectory);
            #endregion

            #region Effect Sound
            // 효과음 부모 노드
            XmlNode ESNode = xmlTempFile.CreateNode(XmlNodeType.Element, "EffectSound", string.Empty);
            rootNode.AppendChild(ESNode);

            // 효과음 개수
            XmlNode ESCount = xmlTempFile.CreateNode(XmlNodeType.Element, "Count", string.Empty);
            ESCount.InnerText = esdMgr.addClipCount.ToString();
            ESNode.AppendChild(ESCount);

            for (int i = 0; i < esdMgr.addClipCount; i++)
            {
                // 효과음 자식 노드
                XmlNode ESChildNode = xmlTempFile.CreateNode(XmlNodeType.Element, "EffectSound_" + i.ToString(), string.Empty);
                ESNode.AppendChild(ESChildNode);

                // 효과음 이름
                XmlNode ESName = xmlTempFile.CreateNode(XmlNodeType.Element, "Name", string.Empty);
                ESName.InnerText = esdMgr.esd[i].audioClip.name;
                ESChildNode.AppendChild(ESName);

                // 효과음 경로
                XmlNode ESDirectory = xmlTempFile.CreateNode(XmlNodeType.Element, "ESDirectory", string.Empty);
                ESDirectory.InnerText = esdMgr.esd[i].fileURL;
                ESChildNode.AppendChild(ESDirectory);
            }
            #endregion

            #region Notes
            // 노트 부모노드 생성
            XmlNode noteNode = xmlTempFile.CreateNode(XmlNodeType.Element, "Note", string.Empty);
            rootNode.AppendChild(noteNode);

            // 노트 카운트
            XmlNode createNoteCount = xmlTempFile.CreateNode(XmlNodeType.Element, "Count", string.Empty);
            createNoteCount.InnerText = noteMgr.notes.Count.ToString();
            noteNode.AppendChild(createNoteCount);

            for (int i = 0; i < noteMgr.notes.Count; i++)
            {
                // 노트 자식노드 생성
                XmlNode noteChildNode = xmlTempFile.CreateNode(XmlNodeType.Element, "Note_" + i.ToString(), string.Empty);
                noteNode.AppendChild(noteChildNode);

                // 노트 타입
                XmlNode noteType = xmlTempFile.CreateNode(XmlNodeType.Element, "NOTE_TYPE", string.Empty);
                noteType.InnerText = noteMgr.notes[i].note_type.ToString();
                noteChildNode.AppendChild(noteType);

                // 노트 출현 시간
                XmlNode noteTime = xmlTempFile.CreateNode(XmlNodeType.Element, "Time", string.Empty);
                noteTime.InnerText = noteMgr.notes[i].AppearTime().ToString();
                noteChildNode.AppendChild(noteTime);

                // 노트 점수
                XmlNode noteScore = xmlTempFile.CreateNode(XmlNodeType.Element, "Score", string.Empty);
                noteScore.InnerText = noteMgr.notes[i].score.ToString();
                noteChildNode.AppendChild(noteScore);

                // 노트 효과음 이름
                XmlNode noteESName = xmlTempFile.CreateNode(XmlNodeType.Element, "EffectSound", string.Empty);
                noteESName.InnerText = noteMgr.notes[i].GetAudioClipName();
                noteChildNode.AppendChild(noteESName);

                // Position.X
                XmlNode notePosX = xmlTempFile.CreateNode(XmlNodeType.Element, "XPositon", string.Empty);
                notePosX.InnerText = noteMgr.notes[i].transform.position.x.ToString();
                noteChildNode.AppendChild(notePosX);

                // Position.Y
                XmlNode notePosY = xmlTempFile.CreateNode(XmlNodeType.Element, "YPosition", string.Empty);
                notePosY.InnerText = noteMgr.notes[i].transform.position.y.ToString();
                noteChildNode.AppendChild(notePosY);
            }
            #endregion

            xmlTempFile.Save(filePath);
        }

    }

    /// <summary>
    /// 불의의사고로 프로젝트가 꺼지거나 저장을 하지 않고 끌 시에 자동으로 세이브해줌.
    /// </summary>
    public void WriteTempXml()
    {
        DirectoryInfo folder = new DirectoryInfo(".\\TempFile");
        folder.Create();

        string fileName = ".\\TempFile\\AutoSave_" + System.DateTime.Now.ToString("yyyy-MM-dd") + ".xml";

        WriteXml(fileName);
    }

    /// <summary>
    /// 저장해놓은 데이터 다시 불러오기.
    /// </summary>
    /// <param name="filePath"> 파일 경로 </param>
    /// <returns></returns>
    public IEnumerator LoadXml(string filePath)
    {
        xmlFile.Load(filePath);     // 데이터 읽어옴.

        // BGM 읽어옴.
        XmlNodeList bgmList = xmlFile.SelectNodes("/" + LoadData.redefLastDirectory(filePath) + "/BGM");
        // BGM 디렉토리 변수에 저장.
        string bgmDirectory = bgmList[0].ChildNodes[1].InnerText;
        WWW www_Bgm = new WWW("file://" + bgmDirectory);
        yield return www_Bgm;

        // 이름 재정의 후 넣어줌.
        AudioClip audioClip = www_Bgm.GetAudioClip();
        audioClip.name = LoadData.redefName(bgmDirectory);
        Data.name = audioClip.name;
        Data.length = audioClip.length;
        EditorManager.editorMgr.SetAudioClip(audioClip);

        // SE 읽어옴.
        XmlNodeList seList = xmlFile.SelectNodes("/" + LoadData.redefLastDirectory(filePath) + "/EffectSound");
        int count = int.Parse(seList[0].ChildNodes[0].InnerText);

        if (count != 0)
        {
            for (int i = 0; i < count; i++)
            {
                string esDirectory = seList[0].ChildNodes[i + 1].ChildNodes[1].InnerText;
                WWW www_es = new WWW("file://" + esDirectory);
                yield return www_es;

                // 이름 재정의 후 넣어줌.
                AudioClip esClip = www_es.GetAudioClip();
                esClip.name = LoadData.redefName(esDirectory);
                esdMgr.esd[i].audioClip = esClip;
                esdMgr.esd[i].text_name.text = esClip.name;
            }
        }
        // Note 읽어옴.
        XmlNodeList noteList = xmlFile.SelectNodes("/" + LoadData.redefLastDirectory(filePath) + "/Note");
        int noteCount = int.Parse(noteList[0].ChildNodes[0].InnerText);

        for (int i = 0; i < noteCount; i++)
        {
            Note clone = Note.Instantiate(note_prefab);
            XmlNode noteDirectory = noteList[0].ChildNodes[i + 1];

            // 포지션 설정
            float posX = float.Parse(noteDirectory.ChildNodes[4].InnerText);
            float posY = float.Parse(noteDirectory.ChildNodes[5].InnerText);
            clone.transform.position = new Vector3(posX, posY, -1);
            clone.transform.parent = EditorManager.editorMgr.noteMgr.transform;
            clone.Init();

            // 노트 타입 설정
            NoteButton.curType = NoteButton.GetNoteType(noteDirectory.ChildNodes[0].InnerText);
            clone.SetNoteType();

            // 노트 점수 설정
            ScoreButton.curScore = int.Parse(noteDirectory.ChildNodes[2].InnerText);
            clone.SetScoreType();

            // 노트 효과음 설정
            clone.audioClip = esdMgr.GetClip(noteDirectory.ChildNodes[3].InnerText);

            if (esdMgr.esd.Count != 0)
            {
                for (int j = 0; j < esdMgr.esd.Count; j++)
                {
                    if (esdMgr.esd[j].audioClip.name == noteDirectory.ChildNodes[3].InnerText)
                    {
                        clone.SetEffect(esdMgr.esd[j].audioClip);
                        break;
                    }
                }
            }

            clone.name = "Note";
            EditorManager.editorMgr.noteMgr.notes.Add(clone);
        }

        NoteButton.b_touchNoteButton = false;
        ScoreButton.b_touchScoreButton = false;
        EffectSoundManager.b_TouchESD = false;

        yield return null;
    }
}
