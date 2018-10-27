using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;

public class SaveData : MonoBehaviour {
    //public EffectSoundManager esdMgr;
    public XmlTools xmlTools;
    public GameObject[] Notes;
    public static string fileName;
    public NoteManager noteMgr;

    public void SortNotes()
    {
        Notes = new GameObject[noteMgr.notes.Count];
        Notes = GameObject.FindGameObjectsWithTag("Note");
        for (int i = 0; i < Notes.Length; i++)
        {
            for (int j = 0; j < Notes.Length - 1; j++)
            {
                if (Notes[j].transform.position.x > Notes[j + 1].transform.position.x)
                {
                    GameObject temp = Notes[j];
                    Notes[j] = Notes[j + 1];
                    Notes[j + 1] = Notes[j];
                    continue;
                }

                else
                    continue;
            }
        }
    }

    public void Save()
    {
        OpenFileName ofn = new OpenFileName();
        ofn.structSize = Marshal.SizeOf(ofn);
        ofn.filter = ".xml\0*.*\0\0";
        ofn.file = new string(new char[256]);
        ofn.maxFile = ofn.file.Length;
        ofn.fileTitle = new string(new char[64]);
        ofn.initialDir = UnityEngine.Application.dataPath;
        ofn.title = "Save Data";
        ofn.defExt = "xml";
        ofn.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;

        if(DllTest.GetSaveFileName(ofn))
        {
            fileName = ofn.file;
            SortNotes();
            xmlTools.WriteXml(fileName);
        }

        if(ofn.file == "")
        {
            return;
        }
    }
    #region Notepad
    /// <summary>
    /// 메모장 세이브
    /// </summary>
    /// <summary>
    /// 메모장 저장.
    /// </summary>
    /// <param name="fileName"> 저장이름 </param>
    //public void WriteData(string fileName)
    //{
    //    if (fileName != "")
    //    {
    //        SortNotes();
    //        StreamWriter sw = new StreamWriter(new FileStream(fileName, FileMode.Create, FileAccess.Write));
    //        sw.WriteLine("BGM 저장경로");
    //        sw.WriteLine(Data.fileURL);
    //        sw.WriteLine("ESD 개수");
    //        sw.WriteLine(esdMgr.addClipCount);
    //        sw.WriteLine("ESD 저장경로");

    //        for (int i = 0; i < esdMgr.esd.Count; i++)
    //        {
    //            if(esdMgr.esd[i].audioClip != null)
    //                sw.WriteLine(esdMgr.esd[i].fileURL);
    //        }

    //        sw.WriteLine("노트 생성 개수");
    //        sw.WriteLine(noteMgr.notes.Count);
    //        sw.WriteLine("노트 타입" + '\t' + "출현 시간" + '\t' + "노트 점수" + '\t' + "효과음 이름" + '\t' + "posX" + '\t' + "posY");

    //        for(int i = 0; i < Notes.Length; i++)
    //        {
    //            sw.WriteLine(noteMgr.notes[i].note_type.ToString() + '\t' + noteMgr.notes[i].AppearTime() + '\t' + noteMgr.notes[i].score.ToString() 
    //                + '\t' + noteMgr.notes[i].GetAudioClipName() + '\t' + noteMgr.notes[i].transform.position.x + '\t' + noteMgr.notes[i].transform.position.y);
    //        }
    //        sw.Close();
    //    }
    //}
    #endregion
}
