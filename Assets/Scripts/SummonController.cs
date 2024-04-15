using System;
using System.Collections.Generic;
using UnityEngine;

public enum SUMMON_TYPE {
    None = 0,
    DOG,
    DRAGON,
    HORSE,
    MONKEY,
    OX,
    PIG,
    RABBIT,
    RAT,
    ROOSTER,
    SHEEP,
    SNAKE,
    TIGER    

}

[Serializable]
public struct SummonData
{    
    public SUMMON_TYPE type;
    public Sprite image;
    public string name;
    public string description;
}

public class SummonController : MonoBehaviour
{
    private Dictionary<SUMMON_TYPE,SummonData> _summonLibraryDict;
    [SerializeField]
    private SummonData[] summonLibrary;


    void Start()
    {
        _summonLibraryDict = new Dictionary<SUMMON_TYPE,SummonData>();
        foreach(SummonData data in summonLibrary)
        {
            _summonLibraryDict.Add(data.type, data);
        }

    }

    public SummonData[] GetRandomSummonChoices()
    {
        List<SummonData> summons = new List<SummonData>();
        List<SUMMON_TYPE> chosen = new List<SUMMON_TYPE>();
        List<SUMMON_TYPE> options = new List<SUMMON_TYPE>(_summonLibraryDict.Keys);

        // Get three options
        for(int i=0;i<3;i++)
        {
            SUMMON_TYPE choice;
            int tries = 0;
            do
            {
                choice = options[UnityEngine.Random.Range(0,options.Count)];
                tries++;
            } while(chosen.Contains(choice) || tries > 100);
            chosen.Add(choice);
            summons.Add(_summonLibraryDict[choice]);
        }

        return summons.ToArray();
    }

    public SummonData GetSummon(SUMMON_TYPE type) => _summonLibraryDict[type];


}
