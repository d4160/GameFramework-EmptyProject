using System.Collections;
using DG.Tweening;
using UnityEngine;

public class BlinkManager : MonoBehaviour
{
    // Public variables
    public float idleDurationSeconds;
    public float pressDurationSeconds;
    public float waitSeconds;
    public Material textMaterial;
    public Ease _easeType;
    public AudioSource _pressStartSource;

    private void Start()
    {
        this.textMaterial.DOFade(0.0f, this.idleDurationSeconds).SetEase(this._easeType).SetLoops(-1, LoopType.Yoyo);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            this.textMaterial.DOFade(0.0f, this.pressDurationSeconds).SetEase(this._easeType).SetLoops(-1, LoopType.Yoyo);
            _pressStartSource.Play();
            StartCoroutine(StartTime(waitSeconds));
        }
    }

    IEnumerator StartTime(float waitSeconds)
    {
        yield return new WaitForSeconds(waitSeconds);
        this.gameObject.SetActive(false);
    }
}
