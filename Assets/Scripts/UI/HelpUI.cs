using System.Collections;
using System.Collections.Generic;
using Audio;
using Audio.SoundFX;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HelpUI : MonoBehaviour
{
    private bool _toggleState = false;
    
    public Vector2 _startOffset;

    [SerializeField]
    private Vector2 targetOffset;
    
    [SerializeField]
    private float animDuration = .3f;

    [SerializeField]
    private Button uiButton;
    private RectTransform _rect;

    private Coroutine _currentAnim;

    void Start()
    {
        _rect = uiButton.GetComponent<RectTransform>();
        _startOffset = _rect.anchoredPosition;
        uiButton.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        _toggleState = !_toggleState;
        SFX.SELECTION.PlaySound();
        Animate();
    }

    private void Animate()
    {
        if(_currentAnim != null)
            StopCoroutine(_currentAnim);
        _currentAnim = StartCoroutine(StartAnimate());
    }

    private IEnumerator StartAnimate()
    {
        Vector2 from = _rect.anchoredPosition;
        Vector2 to = _toggleState ? targetOffset : _startOffset;
        
        float multiplier = Vector2.Distance(from,to)/Vector2.Distance(_startOffset,targetOffset);

        yield return AnimationUtils.Lerp(animDuration*multiplier, t => {
            _rect.anchoredPosition = Vector2.Lerp(from, to, t);
        }, smooth: true);

    }



}
