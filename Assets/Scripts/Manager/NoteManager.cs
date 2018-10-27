using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NoteManager : MonoBehaviour {
    public List<Note> notes;

    public void DestroyAllNotes()
    {
        for (int i = 0; i < notes.Count; i++ )
        {
            if (notes[i] != null)
                notes[i].DeleteNote();
        }

        notes.Clear();
    }
}
