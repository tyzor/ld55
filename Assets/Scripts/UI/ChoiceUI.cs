using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using Audio;
using Audio.SoundFX;

public class ChoiceUI : MonoBehaviour
{
    [SerializeField]
    protected Button[] choices;

    public int selectedIndex = -1;

    // Start is called before the first frame update
    protected void Start()
    {
        for(int i=0;i<choices.Length;i++)
        {   
            var index = i;
            choices[i].onClick.AddListener(() => {OnChoiceSelect(index);});
        }
    }

    private void OnChoiceSelect(int index)
    {
        Debug.Log($"Choice {index} selected");
        SFX.SELECTION.PlaySound();
        selectedIndex = index;
    }

    public void Show() => gameObject.SetActive(true);
    public void Hide() => gameObject.SetActive(false);
    

}
