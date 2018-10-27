using UnityEngine;
using System.Collections;

public class LineManager : MonoBehaviour {
    [SerializeField]
    private GameObject line_Prefab; 
    public Transform startTransform;
    public Transform endTransform;
    public Vector3 InitPos = new Vector2(-6.16f, -1);
    [SerializeField]
    private Transform curBGMPosition;

    public static Vector2 startPos;
    public static Vector2 endPos;
    private GameObject[] clones;
    [SerializeField]
    private NoteManager noteMgr;
    public int createCount;

    /// <summary>
    /// 음악의 길이에 맞게 노트찍는 라인 생성.
    /// </summary>
    /// <param name="length"> 음악 길이 </param>
    public void CreateLine(float length)
    {
        startTransform.position = InitPos;
        curBGMPosition.position = new Vector3(-0.5f, 0);

        if (clones != null)
        {
            noteMgr.DestroyAllNotes();
            for(int i = 0; i < clones.Length; i++)
            {
                Destroy(clones[i]);
            }
        }
        clones = new GameObject[(int)length + 1];
        createCount = clones.Length;
        for(int i = 0; i < clones.Length; i++)
        {
            clones[i] = GameObject.Instantiate(line_Prefab);
            clones[i].name = "Line";
            clones[i].transform.parent = startTransform;
            clones[i].transform.position = new Vector2(startTransform.position.x + i * 1.2f, startTransform.position.y);

            if (i == 0)
                startPos = clones[i].transform.position;

            else if (i == clones.Length - 1)
                endTransform.position = clones[i].transform.position;
        }

        endPos = endTransform.position;
    }

    /// <summary>
    /// 현재 음악의 위치가 전체 라인 위치 상에서 얼마나 진행됐는가를 알려줌.
    /// </summary>
    /// <param name="time"></param>
    /// <param name="length"></param>
    public void UpdateLine(float time, float length)
    {
        if (clones != null)
            curBGMPosition.position = new Vector2(Mathf.Lerp(startTransform.position.x, endTransform.position.x, time / (length + 0.6f)), -1);
    }
}
