using UnityEngine;
using System.Collections;

public class StatusBar : MonoBehaviour {
    [SerializeField]
    private Transform[] boundaries;
    public bool b_Drag;
    [SerializeField]
    private LineManager lineMgr;
    [SerializeField]
    private GameObject UI_CurPosBar;
    [SerializeField]
    private Transform[] boundaries_Bar;

    public static Vector2 startPos;
    public static Vector2 endPos;

    public void Init()
    {
        b_Drag = false;
        startPos = boundaries[0].transform.position;
        endPos = boundaries[1].transform.position;

        transform.position = boundaries[0].position;
    }

    /// <summary>
    /// Status Line을 클릭했을 시에 노래 이동.
    /// </summary>
    /// <param name="ray"></param>
    public void ChangeStatusBar(ref Ray2D ray)
    {
        if (ray.origin.x < boundaries[0].position.x)
        {
            transform.position = new Vector2(boundaries[0].position.x, transform.position.y);

            if (EditorManager.editorMgr.audioSource.isPlaying)
                EditorManager.editorMgr.audioSource.time = 0;

            else
                EditorManager.editorMgr.tempTime = 0;
        }

        else if (ray.origin.x > boundaries[1].position.x)
        {
            transform.position = new Vector2(boundaries[1].transform.position.x, transform.position.y);
            
            if (EditorManager.editorMgr.audioSource.isPlaying)
                EditorManager.editorMgr.audioSource.time = EditorManager.editorMgr.audioSource.clip.length - 0.01f;

            else
                EditorManager.editorMgr.tempTime = EditorManager.editorMgr.audioSource.clip.length - 0.01f;
        }

        else
        {
            transform.position = new Vector2(ray.origin.x, transform.position.y);
            
            if (EditorManager.editorMgr.audioSource.isPlaying)
                EditorManager.editorMgr.audioSource.time = (transform.position.x - boundaries[0].transform.position.x) 
                    / (boundaries[1].transform.position.x - boundaries[0].transform.position.x) * EditorManager.editorMgr.audioSource.clip.length;

            else
                EditorManager.editorMgr.tempTime = (transform.position.x - boundaries[0].transform.position.x) 
                    / (boundaries[1].transform.position.x - boundaries[0].transform.position.x) * EditorManager.editorMgr.audioSource.clip.length;
        }
    }

    /// <summary>
    /// 현재 스테이터스바에서도 노래가 어디까지 진행이 됐나 알려주는 소스코드
    /// </summary>
    /// <param name="length"></param>
    /// <param name="time"></param>
    public void MoveCurPosBar(float length, float time)
    {
        UI_CurPosBar.transform.position = new Vector2(Mathf.Lerp(boundaries_Bar[0].position.x, boundaries_Bar[0].position.x + 
            (boundaries_Bar[1].position.x - boundaries_Bar[0].position.x), time / length), UI_CurPosBar.transform.position.y);
    }
}
