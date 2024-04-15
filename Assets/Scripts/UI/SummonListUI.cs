using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class SummonListUI : MonoBehaviour
{
    private SummonController _summonController;
    private VerticalLayoutGroup _layoutGroup;

    [SerializeField]
    private GameObject summonIconPrefab;

    // Start is called before the first frame update
    void Start()
    {
        _summonController = FindObjectOfType<SummonController>();
        Assert.IsNotNull(_summonController, $"Missing the {nameof(SummonController)} in the Scene!!");

        _layoutGroup = GetComponent<VerticalLayoutGroup>();
        Assert.IsNotNull(_layoutGroup, $"Missing a VerticalComponent on SummonListUI");

        ClearList();
        
    }
    
    public void ClearList()
    {
        // Clear the list
        foreach(Transform t in _layoutGroup.transform)
        {
            Destroy(t.gameObject);
        }
    }

    public void AddSummon(SUMMON_TYPE summonType)
    {
        var newIcon = Instantiate(summonIconPrefab, _layoutGroup.transform);
        newIcon.GetComponentInChildren<Image>().sprite = _summonController.GetSummon(summonType).image;
        StartCoroutine(SummonAnimation(newIcon.transform));
    }

    private IEnumerator SummonAnimation(Transform target)
    {
        yield return AnimationUtils.Lerp(1f, (t) => {
            target.localScale = Vector3.Lerp(Vector3.one*2f,Vector3.one,t);
        });
    }

}
