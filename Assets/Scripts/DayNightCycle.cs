using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using System;

public class DayNightCycle : MonoBehaviour
{
    [Header("Time Settings")]
    public float dayTimeInSeconds = 60f;    // 1 minutes for day - Prep Time for the Player
    public float duskTimeInSeconds = 30;   // 1/2 minutes for dusk - The Night is Coming Warnning for the Player
    public float dawnTimeInSeconds = 60f;    // 1 minute for dawn - And the Last Prep has Done by the Player, Fight!
    public float nightTimeInSeconds = 120f;  // 2 minutes for night
    private float totalCycleTime;
    private float currentTimeInSeconds = 0f;

    [Header("Lighting")]
    public UnityEngine.Rendering.Universal.Light2D primaryLight;   // Main directional light
    public UnityEngine.Rendering.Universal.Light2D ambientLight;   // Ambient/fill light

    [Header("Light Colors")]
    public Color dayPrimaryColor = Color.white;
    public Color dayAmbientColor = new Color(0.8f, 0.8f, 1f, 0.5f);
    public Color duskPrimaryColor = new Color(1f, 0.6f, 0.3f);
    public Color duskAmbientColor = new Color(0.7f, 0.5f, 0.3f, 0.5f);
    public Color nightPrimaryColor = new Color(0.1f, 0.1f, 0.2f);
    public Color nightAmbientColor = new Color(0.1f, 0.1f, 0.3f, 0.3f);

    [Header("Light Intensities")]
    public float dayPrimaryIntensity = 1f;
    public float dayAmbientIntensity = 0.3f;
    public float duskPrimaryIntensity = 0.7f;
    public float duskAmbientIntensity = 0.5f;
    public float nightPrimaryIntensity = 0.2f;
    public float nightAmbientIntensity = 0.1f;

    public enum TimeOfDay
    {
        Day,
        Dusk,
        Night,
        Dawn
    }

    private TimeOfDay currentTimeOfDay;

    private void Start()
    {
        totalCycleTime = dayTimeInSeconds + duskTimeInSeconds + nightTimeInSeconds + dawnTimeInSeconds;
        currentTimeOfDay = TimeOfDay.Day;
    }

    private void Update()
    {
        // Update time
        currentTimeInSeconds += Time.deltaTime;
        if (currentTimeInSeconds >= totalCycleTime)
        {
            currentTimeInSeconds = 0f;
        }

        UpdateTimeOfDay();
        UpdateLighting();
    }

    private void UpdateTimeOfDay()
    {
        float time = currentTimeInSeconds;

        if (time < dayTimeInSeconds)
        {
            currentTimeOfDay = TimeOfDay.Day;
        }
        else if (time < dayTimeInSeconds + duskTimeInSeconds)
        {
            currentTimeOfDay = TimeOfDay.Dusk;
        }
        else if (time < dayTimeInSeconds + duskTimeInSeconds + nightTimeInSeconds)
        {
            currentTimeOfDay = TimeOfDay.Night;
        }
        else
        {
            currentTimeOfDay = TimeOfDay.Dawn;
        }
    }

    private void UpdateLighting()
    {
        float transitionProgress = 0f;
        Color targetPrimaryColor = dayPrimaryColor;
        Color targetAmbientColor = dayAmbientColor;
        float targetPrimaryIntensity = dayPrimaryIntensity;
        float targetAmbientIntensity = dayAmbientIntensity;

        switch (currentTimeOfDay)
        {
            case TimeOfDay.Day:
                transitionProgress = currentTimeInSeconds / dayTimeInSeconds;
                targetPrimaryColor = dayPrimaryColor;
                targetAmbientColor = dayAmbientColor;
                targetPrimaryIntensity = dayPrimaryIntensity;
                targetAmbientIntensity = dayAmbientIntensity;
                break;

            case TimeOfDay.Dusk:
                transitionProgress = (currentTimeInSeconds - dayTimeInSeconds) / duskTimeInSeconds;
                targetPrimaryColor = Color.Lerp(dayPrimaryColor, duskPrimaryColor, transitionProgress);
                targetAmbientColor = Color.Lerp(dayAmbientColor, duskAmbientColor, transitionProgress);
                targetPrimaryIntensity = Mathf.Lerp(dayPrimaryIntensity, duskPrimaryIntensity, transitionProgress);
                targetAmbientIntensity = Mathf.Lerp(dayAmbientIntensity, duskAmbientIntensity, transitionProgress);
                break;

            case TimeOfDay.Night:
                transitionProgress = (currentTimeInSeconds - dayTimeInSeconds - duskTimeInSeconds) / nightTimeInSeconds;
                targetPrimaryColor = Color.Lerp(duskPrimaryColor, nightPrimaryColor, transitionProgress);
                targetAmbientColor = Color.Lerp(duskAmbientColor, nightAmbientColor, transitionProgress);
                targetPrimaryIntensity = Mathf.Lerp(duskPrimaryIntensity, nightPrimaryIntensity, transitionProgress);
                targetAmbientIntensity = Mathf.Lerp(duskAmbientIntensity, nightAmbientIntensity, transitionProgress);
                break;

            case TimeOfDay.Dawn:
                transitionProgress = (currentTimeInSeconds - dayTimeInSeconds - duskTimeInSeconds - nightTimeInSeconds) / dawnTimeInSeconds;
                targetPrimaryColor = Color.Lerp(nightPrimaryColor, dayPrimaryColor, transitionProgress);
                targetAmbientColor = Color.Lerp(nightAmbientColor, dayAmbientColor, transitionProgress);
                targetPrimaryIntensity = Mathf.Lerp(nightPrimaryIntensity, dayPrimaryIntensity, transitionProgress);
                targetAmbientIntensity = Mathf.Lerp(nightAmbientIntensity, dayAmbientIntensity, transitionProgress);
                break;
        }

        // Apply the lighting changes
        if (primaryLight != null)
        {
            primaryLight.color = targetPrimaryColor;
            primaryLight.intensity = targetPrimaryIntensity;
        }

        if (ambientLight != null)
        {
            ambientLight.color = targetAmbientColor;
            ambientLight.intensity = targetAmbientIntensity;
        }
    }

    public string GetCurrentTimeOfDay()
    {
        return currentTimeOfDay.ToString();
    }

    public float GetCycleProgress()
    {
        return currentTimeInSeconds / totalCycleTime;
    }
}