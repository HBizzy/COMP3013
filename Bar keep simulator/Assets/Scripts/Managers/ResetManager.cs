using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Audio;


public class ResetManager : MonoBehaviour
{
    public DrinkAssemblyController drinkController;
    public Button resetButton;
    public int toxicity = 0;
    public Vector3 camPos;
    public float maxSway;
    public float swaySpeed;
    public AudioSource slurpSound;
    private bool isFailing = false;
    private Transform cam;

    public Image fadeImage;

    public Volume volume;
    private Vignette vignette;
    private DepthOfField dof;
    public AudioMixer mixer;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.transform;
        camPos = cam.localPosition;
        toxicity = 0;
        volume.profile.TryGet(out vignette);
        volume.profile.TryGet(out dof);

        resetButton.onClick.RemoveAllListeners();
        resetButton.onClick.AddListener(() =>
        {
            resetClicked();
        }
        );
        StartCoroutine(tickToxicity());

    }
    private void Awake()
    {
        GameStateManager.Instance.OnNightStart += resetNight;
    }

    // Update is called once per frame
    void Update()
    {
        generateToxicityEffects();


    }
    public void resetClicked()
    {
        PlaySlurpSound();
        drinkController.currentSteps.Clear();
        GameStateManager.Instance.economyManager.AddMoney(Random.RandomRange(-10, -5));
        StartCoroutine(SlowAddToxicity(Random.RandomRange(8, 15)));
        toxicity = Mathf.Clamp(toxicity, 0, 100);

        if(toxicity >= 80 && !isFailing)
        {
            StartCoroutine(TriggerFailerAfterTime());
            isFailing = true;
        }
        
        
    }
    public void generateToxicityEffects()
    {
        float t = (float)toxicity / 100f;

        float swayAmount = maxSway * t * t;
        
        float offsetX = Mathf.Sin(Time.time*swaySpeed) *swayAmount;
        float offsetY = Mathf.Cos(Time.time * swaySpeed *0.8f) * swayAmount *0.5f;

        cam.localPosition = camPos + new Vector3(offsetX, offsetY, 0);

        float vignetteT = Mathf.Clamp01((toxicity - 80f) / 20f);

        vignette.intensity.value = Mathf.Lerp(0f, 0.4f, vignetteT);

        float audioT = Mathf.Clamp01((toxicity - 60f) / 40f);

        float lowPass = Mathf.Lerp(22000f, 4000f, audioT);

        float pitch = Mathf.Lerp(1f, 0.96f, audioT);

        mixer.SetFloat("LowPass", lowPass);
        //mixer.SetFloat("Pitch", pitch);

        t = Mathf.Clamp01((toxicity - 30f) / 70f);

        if (t <= 0)
        {
            dof.active = false;
        }
        else
        {
            dof.active = true;

            float blurT = Mathf.Pow(t, 1.5f);
            dof.gaussianMaxRadius.value = Mathf.Lerp(0f, 1.2f, blurT);
        }
    }
    public IEnumerator tickToxicity()
    {
        while (GameStateManager.Instance.nightManager.isNightRunning)
        {
            float t = (float)toxicity / 100f;
            int time = Mathf.CeilToInt(1 + (t * 2));
            yield return new WaitForSeconds(time);
            toxicity -= 1;
            toxicity = Mathf.Clamp(toxicity, 0, 100);
        }
    }
    public IEnumerator TriggerFailerAfterTime()
    {
        while (toxicity >= 80)
        {
            yield return new WaitForSeconds(15f);

            if (toxicity >= 80)
            {
                StartCoroutine(FadeToBlack());
                yield break;
            }
            else
            {
                isFailing = false;
                yield break;
            }
        }
    }
    public IEnumerator SlowAddToxicity(int amount)
    {
        int toAdd = amount;
        while(toAdd > 0)
        {
            toxicity += 1;
            toAdd--;
            toxicity = Mathf.Clamp(toxicity, 0, 100);
            yield return new WaitForSeconds(0.5f);
        }
        yield break;
    }
    public IEnumerator FadeToBlack()
    {
        float duration = 2f;
        float time = 0f;

        Color c = fadeImage.color;

        while (time < duration)
        {
            time += Time.deltaTime;
            float alpha = time / duration;

            fadeImage.color = new Color(c.r, c.g, c.b, alpha);
            yield return null;
        }
        toxicity = 0;
        GameStateManager.Instance.nightManager.OnTimerExpired();
        GameStateManager.Instance.EndNight();
        
    }
    public void PlaySlurpSound()
    {
        slurpSound.pitch = UnityEngine.Random.RandomRange(0.9f, 1.1f);
        slurpSound.Play();
    }
    public void resetNight()
    {
        toxicity = 0;
    }
}
