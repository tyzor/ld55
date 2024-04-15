using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class EnergyParticleVFX : HiddenSingleton<EnergyParticleVFX>
{
    [SerializeField]
    private GameObject energyParticlePrefab;

    [SerializeField]
    private float travelTime = 1f;

    [SerializeField]
    private float randomSpread = .2f;
    
    [SerializeField]
    private float rotationSpeed = 360f/4; // in degrees per second

    [SerializeField]
    private AnimationCurve curve;

    public void CreateParticles(int number, Vector3 start, Vector3 target)
    {
        StartCoroutine(RunParticles(number, start, target));
    }
    private IEnumerator RunParticles(int number, Vector3 start, Vector3 target)
    {
        List<GameObject> particles = new List<GameObject>();
        for(int i=0;i<number;i++)
        {
            var particle = Instantiate(energyParticlePrefab,transform);
            var randomDisplace = Vector3.ProjectOnPlane(UnityEngine.Random.onUnitSphere,Vector3.up).normalized * randomSpread;
            particle.transform.position = start + randomDisplace;
            particle.transform.Rotate(Vector3.up, UnityEngine.Random.Range(0,360));
            particles.Add(particle);
        }

        Vector3[] startPos = new Vector3[particles.Count];
        for(int i=0;i<particles.Count;i++)
        {
            startPos[i] = particles[i].transform.position;
        }

        yield return AnimationUtils.Lerp(travelTime, (t) => {

            for(int i=0;i<particles.Count;i++)
            {
                particles[i].transform.position = Vector3.Lerp(startPos[i],target,t);
                particles[i].transform.Rotate(Vector3.up, Time.deltaTime * rotationSpeed); 
            }

        }, smooth: true);

        // Cleanup
        for(int i=0;i<particles.Count;i++)
        {
            Destroy(particles[i].gameObject);
        }

    }

    public static void Create(int number, Vector3 start, Vector3 target)
    {
        Instance.CreateParticles(number,start,target);
    }

}
