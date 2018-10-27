using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class EffectSoundManager : MonoBehaviour {
    public static AudioClip audioClip;
    public EffectSound esd_prefab;
    public List<EffectSound> esd;
    public Vector2 InitPos;
    public int createCount;
    public static bool b_TouchESD;
    public string fileURL;
    public int addClipCount;

    public void Init()
    {
        createCount = 0;
        addClipCount = 0;
        for(int i = 0; i < 7; i++)
        {
            EffectSound clone = EffectSound.Instantiate(esd_prefab);
            clone.name = "EffectSound" + i.ToString();
            esd.Add(clone);

            esd[i].transform.parent = transform;
            esd[i].transform.position = new Vector2(InitPos.x, InitPos.y - (0.343f * 2 * i));
            createCount++;
        }
    }

    public void SetClip(AudioClip _clip)
    {
        for(int i = 0; i < esd.Count; i++)
        {
            if (esd[i].audioClip != null)
            {
                if (esd[esd.Count - 1].audioClip != null)
                {
                    EffectSound clone = EffectSound.Instantiate(esd_prefab);
                    clone.name = "EffectSound" + esd.Count.ToString();
                    clone.transform.parent = transform;
                    clone.transform.position = new Vector2(InitPos.x, InitPos.y - (0.343f * 2 * esd.Count));
                    clone.audioClip = _clip;
                    clone.text_name.text = _clip.name;
                    clone.fileURL = fileURL;
                    esd.Add(clone);
                    createCount++;
                    addClipCount++;
                    break;
                }

                else
                    continue;
            }

            else
            {
                esd[i].audioClip = _clip;
                esd[i].fileURL = fileURL;
                esd[i].text_name.text = _clip.name;
                addClipCount++;
                break;
            }
        }
    }

    public AudioClip GetClip(string str)
    {
        for(int i = 0; i < esd.Count; i++)
        {
            if (str != "NULL")
            {
                if (esd[i].audioClip.name == str)
                    return esd[i].audioClip;
            }

            else
                return null;
        }
        
        return null;
    }

    void Update()
    {
        if (b_TouchESD)
        {
            for (int i = 0; i < esd.Count; i++)
            {
                if (esd[i].audioClip == audioClip)
                    esd[i].spriteRenderer.color = Color.gray;

                else
                    esd[i].spriteRenderer.color = Color.white;
            }
        }

        else
        {
            for (int i = 0; i < esd.Count; i++)
            {
                esd[i].spriteRenderer.color = Color.white;
            }
        }
    }
}
