using UnityEngine;
using System.Collections;

public class LineScrollbar : MonoBehaviour {
    public bool b_Drag;         // 드래그 중인가를 판단하기 위해 사용.
    public LineManager lineMgr; // 라인 매니저. startTransform 가져오기 위해서 씀.
    public Transform[] boundaries;
    private Vector2 initPos;

    public void Init()
    {
        b_Drag = false;
        initPos = lineMgr.startTransform.position;
    }

    public void DragLine(ref Ray2D ray)
    {
        lineMgr.startTransform.position = new Vector2(initPos.x - ((transform.position.x - boundaries[0].transform.position.x) / (boundaries[1].transform.position.x - boundaries[0].transform.position.x)
                                                                   * (LineManager.endPos.x - LineManager.startPos.x)), initPos.y);

        if (ray.origin.x < boundaries[0].position.x)
            transform.position = new Vector2(boundaries[0].position.x, transform.position.y);

        else if (ray.origin.x > boundaries[1].position.x)
            transform.position = new Vector2(boundaries[1].position.x, transform.position.y);

        else
            transform.position = new Vector2(ray.origin.x, transform.position.y);
    }
}
