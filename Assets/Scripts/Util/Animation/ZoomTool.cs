using System.Collections;
using UnityEngine;


public class ZoomTool : MonoBehaviour
{
    public bool isZooming;

    private float originalZ; 

    private void Start() {
        originalZ = transform.position.z;
        isZooming = false;
    }

    public void ZoomIn(Vector3 target, float durationIn)
    {
        isZooming = true;
        StartCoroutine(ZoomCo(target, durationIn));
    }

    public void ZoomPunch(Vector3 target, float zoomInAmount, float durationIn, float durationStay, float durationOut)
    {
        isZooming = true;
        StartCoroutine(ZoomPunchCo(target, zoomInAmount, durationIn, durationStay, durationOut));
    }

    public void ZoomPunch(Transform target, float zoomInAmount, float durationIn, float durationStay, float durationOut)
    {
        isZooming = true;
        StartCoroutine(ZoomPunchCo(target, zoomInAmount, durationIn, durationStay, durationOut));
    }

    private IEnumerator ZoomCo(Vector3 target, float dIn)
    {
        float timeElapse = 0;

        while(!transform.position.Equals(target))
        {
            transform.position = Vector3.Lerp(transform.position, target, timeElapse/dIn);
            
            timeElapse += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        isZooming = false;
    }

    private IEnumerator ZoomPunchCo(Vector3 target, float zoomInAmount, float dIn, float dStay, float dOut)
    {
        float timeElapse = 0;
        Vector3 originalPosition = transform.position;
        target.z = transform.position.z;
        target.z += zoomInAmount;

        while(!transform.position.Equals(target))
        {
            transform.position = Vector3.Lerp(transform.position, target, timeElapse/dIn);
            
            timeElapse += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(dStay);

        timeElapse = 0;

        while(!transform.position.Equals(originalPosition))
        {
            transform.position = Vector3.Lerp(transform.position, originalPosition, timeElapse/dOut);
            
            timeElapse += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        isZooming = false;
    }

    private IEnumerator ZoomPunchCo(Transform targetTransform, float zoomInAmount, float dIn, float dStay, float dOut)
    {
        float timeElapse = 0;
        Vector3 originalPosition = transform.position;

        while(timeElapse < dIn)
        {
            Vector3 target = targetTransform.position;
            target.z += (originalZ + zoomInAmount);

            transform.position = Vector3.Lerp(transform.position, target, timeElapse/dIn);
            
            timeElapse += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        timeElapse = 0;

        while(timeElapse < dStay)
        {
            Vector3 target = targetTransform.position;
            target.z += (originalZ + zoomInAmount);

            transform.position = Vector3.Lerp(transform.position, target, timeElapse/dStay);
            
            timeElapse += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        timeElapse = 0;

        while(timeElapse < dOut)
        {
            Vector3 target = targetTransform.position;
            target.z = originalZ;
            
            transform.position = Vector3.Lerp(transform.position, originalPosition, timeElapse/dOut);
            
            timeElapse += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        isZooming = false;
    }
}
