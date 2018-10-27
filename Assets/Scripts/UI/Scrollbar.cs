using UnityEngine;
using System.Collections;

public class Scrollbar : MonoBehaviour {
    public Transform[] boundaries;
    public GameObject bar;
    public bool b_Drag;
    public EffectSoundManager effectSoundMgr;
    public float init_Y;

    public void Init()
    {
        b_Drag = false;
        init_Y = effectSoundMgr.transform.position.y;
        bar.transform.position = boundaries[0].position;
    }

    public void DragBar(ref Ray2D ray)
    {
        bar.transform.position = new Vector2(bar.transform.position.x, ray.origin.y);

        if(bar.transform.position.y > boundaries[0].position.y)
            bar.transform.position = new Vector2(bar.transform.position.x, boundaries[0].position.y);

        if(bar.transform.position.y < boundaries[1].position.y)
            bar.transform.position = new Vector2(bar.transform.position.x, boundaries[1].position.y);



        // 드래그 시에 ESM 오브젝트들도 따라 움직이게 해줌.
        float totalLength_Y = (effectSoundMgr.createCount - 6) * 2 * 0.343f;
        float ratio = (boundaries[0].transform.position.y - bar.transform.position.y) / (boundaries[0].transform.position.y - boundaries[1].transform.position.y);

        effectSoundMgr.transform.position = new Vector2(effectSoundMgr.transform.position.x,
                                                        init_Y + (totalLength_Y * ratio));
    }
}
