# ISDEditor

![img view][1]

[1]: https://scontent-icn1-1.xx.fbcdn.net/v/t1.0-9/13442329_873351602794391_7955708399430878779_n.jpg?_nc_cat=105&_nc_ht=scontent-icn1-1.xx&oh=b02dcf1a1b0e3c541d2a7327e52e954a&oe=5C8A5C97 (preview)

## 1. 프로젝트 정보

### 플랫폼 
PC(Windows)

### 프로젝트 정보
'Infinite Sound Dodge' 에 들어갈 채보를 찍기 위해 만든 에디터 툴</br>
Unity 5.2.3f 에서 2018.1.1f1로 업그레이드 함

### 영상
[![Video Label](http://img.youtube.com/vi/Cc_Bc2ZyBZ0/0.jpg)](https://youtu.be/Cc_Bc2ZyBZ0?t=0s)


## 2. 프로젝트 코드 설명

![img view][2]

[2]: https://scontent-icn1-1.xx.fbcdn.net/v/t1.0-9/44843876_1727730660689810_971326632314798080_n.jpg?_nc_cat=100&_nc_ht=scontent-icn1-1.xx&oh=2ddd58d2e2a1a51d7a780475ad9a0d6b&oe=5C514F26 (preview)
(간단하게 에디터에 대한 기능을 설명한 스크린샷, ui는 uGUI를 사용함.)</br>


### EditorManager.CS
Editor Class는 전반적으로 에디터에서 일어날 모든 일들에 대해서 관리합니다.</br>
예를 들면 초기화, 업데이트, 프로그램 종료시 일어날 일들에 대해 관리를 하는 클래스입니다.</br>
그렇기 때문에 Hierachy에서 최상위 부모객체로 두었습니다.</br>
이 클래스를 참조할 일이 매우 많기 때문에 Singleton 방식으로 작성하여 다른 클래스에서의 참조를 수월하게 했습니다</br>
### OpenFileName.cs
OpenFileName.cs는 Unity에서 Windows 파일 탐색기를 호출하기 위해서 사용합니다. 찾아본 결과 Windows 자체의 탐색기를 사용하는 것이 아닌 Unity 내부 자체 dll을 이용해서 호출하는 것으로 확인했습니다. 파일 탐색기를 자기 입맛에 맞게 수정할 수 있으며, 파일 내에 있는 DLLTest 클래스를 이용해서 파일을 열거나 저장할 수 있습니다.</br></br>
우선 OpenFileName을 사용하기 위해서는 Unity 설치 폴더 내에서 'System.Configuration', 'System.EnterpriseServices', 'System.Windows.Forms'를 복사해 Assets/Plugins 폴더안에 넣어야지만 정상적으로 작동을 합니다.</br>
<OpenFileName.cs>
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
