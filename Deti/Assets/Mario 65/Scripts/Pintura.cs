using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pintura : MonoBehaviour
{
    private Coroutine rippleRoutine;
    [SerializeField] private float rippleTime = 5f;
    [SerializeField] private float maxRippleStrength = 0.75f;

    private Renderer rend;

    private void Start()
    {
        rend = GetComponent<Renderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Material mat = rend.material;
            Vector3 ripplePoint = other.transform.position; 

            mat.SetVector("_Ripple_center", ripplePoint);

            if (rippleRoutine != null)
                StopCoroutine(rippleRoutine);

            rippleRoutine = StartCoroutine(DoRipple(mat));
        }
    }

    private IEnumerator DoRipple(Material mat)
    {
        for (float t = 0.0f; t < rippleTime; t += Time.deltaTime)
        {
            mat.SetFloat("_Ripple_Strenght", maxRippleStrength * (1.0f - t / rippleTime));
            mat.SetFloat("_Ripple_speed", 2.5f);
            yield return null;
        }

        mat.SetFloat("_Ripple_Strenght", 0f); 
    }
}
