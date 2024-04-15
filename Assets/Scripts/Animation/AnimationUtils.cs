using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnimationUtils
{
    public static IEnumerator Lerp(
        float duration,
        Action<float> action,
        bool realTime = false,
        bool smooth = false,
        AnimationCurve curve = null,
        bool inverse = false
    ) {
        float time = 0;

        // Build our evaluation function, default linear if no curve provided
        Func<float, float> tEval = t => t;
        if(smooth) tEval = t => Mathf.SmoothStep(0, 1, t);
        if(curve != null) tEval = t => curve.Evaluate(t);

        while(time < duration) {
            float delta = realTime ? Time.fixedDeltaTime : Time.deltaTime;
            float t = (time + delta > duration) ? 1 : (time/duration);
            if(inverse)
                t = 1-t;
            action(tEval(t));
            time += delta;
            yield return null;
        }

        // Do last evaluate at max value
        action(tEval(inverse ? 0: 1));
    } 

}
