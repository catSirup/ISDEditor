# ISDEditor

## 1. 프로젝트 정보

### 플랫폼 
PC(Windows)

### 프로젝트 설명
'Infinite Sound Dodge' 에 들어갈 채보를 찍기 위해 만든 에디터 툴</br>
2015년 11월에 Unity 5.2.3f로 개발하던 환경에서 2018년 10 2018.1.1f1로 업그레이드 함

## 2. 프로젝트 코드 설명

간략하게 몇 개의 클래스만 설명하겠습니다. 나머지 부분은 크게 설명할 부분이 없을 듯 하네요.

### EditorManager.CS
Editor Class는 전반적으로 에디터에서 일어날 모든 일들에 대해서 관리합니다.</br>
예를 들면 초기화, 업데이트, 프로그램 종료시 일어날 일들에 대해 관리를 하는 클래스입니다.</br>
그렇기 때문에 Hierachy에서 최상위 부모객체로 두었습니다.</br>
이 클래스를 참조할 일이 매우 많기 때문에 Singleton 방식으로 작성하여 다른 클래스에서의 참조를 수월하게 했습니다</br>
### OpenFileName.cs
OpenFileName.cs는 Unity에서 Windows 파일 탐색기를 호출하기 위해서 사용합니다. 찾아본 결과 Windows 자체의 탐색기를 사용하는 것이 아닌 Unity 내부 자체 dll을 이용해서 호출하는 것으로 확인했습니다. 파일 탐색기를 자기 입맛에 맞게 수정할 수 있으며, 파일 내에 있는 DLLTest 클래스를 이용해서 파일을 열거나 저장할 수 있습니다.</br></br>
우선 OpenFileName을 사용하기 위해서는 Unity 설치 폴더 내에서 'System.Configuration', 'System.EnterpriseServices', 'System.Windows.Forms'를 복사해 Assets/Plugins 폴더안에 넣어야지만 정상적으로 작동을 합니다.</br>
```c#
using UnityEngine;
using System.Collections;
using System;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]

public class OpenFileName
{
    public int structSize = 0;
    public IntPtr dlgOwner = IntPtr.Zero;
    public IntPtr instance = IntPtr.Zero;
    public String filter = null;
    public String customFilter = null;
    public int maxCustFilter = 0;
    public int filterIndex = 0;
    public String file = null;
    public int maxFile = 0;
    public String fileTitle = null;
    public int maxFileTitle = 0;
    public String initialDir = null;
    public String title = null;
    public int flags = 0;
    public short fileOffset = 0;
    public short fileExtension = 0;
    public String defExt = null;
    public IntPtr custData = IntPtr.Zero;
    public IntPtr hook = IntPtr.Zero;
    public String templateName = null;
    public IntPtr reservedPtr = IntPtr.Zero;
    public int reservedInt = 0;
    public int flagsEx = 0;
}

public class DllTest
{
    [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
    public static extern bool GetOpenFileName([In, Out] OpenFileName ofn);
    public static bool GetOpenFileName1([In, Out] OpenFileName ofn)
    {
        return GetOpenFileName(ofn);
    }

    [DllImport("comdlg32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    public static extern bool GetSaveFileName([In, Out] OpenFileName ofn);
}
```
### LoadData.cs
LoadData Class는 이름 그대로 데이터를 로드하는 클래스입니다. BGM이나 Effect Sound Music(ESM), 저장했던 파일들을 불러오는 기능을 가지고 있습니다.
```c#
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
```
LoadAudioClip은 BGM 혹은 ESM을 불러올 수 있습니다. OpenFileName Class 객체를 생성해서 세팅을 해주며, 저장 버튼을 누를 시 저 함수가 호출이 되며, 파일을 선택할 시에 if문에 들어가게 되는데, ofn.file에 파일의 url이 담겨있어 이 url을 가지고 데이터를 받아야 합니다(url이 비었을 경우 그냥 창만 닫힙니다)
```c#
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
```
Unity 5.2.3f로 개발 당시, 정식 라이선스에서는 모든 파일 형식을 지원하나 그 외의 라이선스에서는 PC 플랫폼에서는 OGG 파일 형식만 불러올 수 있었습니다. 안드로이드는 MP3만 가능했습니다. 아마 지금쯤이면 바뀌었을 수도 있겠네요.
