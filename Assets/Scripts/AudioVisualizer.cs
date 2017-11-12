﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class AudioVisualizer : MonoBehaviour
{
    public Transform[] audioSpectrumObjects;
    [Range(1, 100)] public float heightMultiplier;
    [Range(64, 8192)] public int numberOfSamples = 1024; //step by 2
    public FFTWindow fftWindow;
    public float lerpTime = 1;

    public PlayerController player;

    private bool _flipping = false;

    /*
    * The intensity of the frequencies found between 0 and 44100 will be
    * grouped into 1024 elements. So each element will contain a range of about 43.06 Hz.
    * The average human voice spans from about 60 hz to 9k Hz
    * we need a way to assign a range to each object that gets animated. that would be the best way to control and modify animatoins.
    */
    void Start()
    {
    }

    /**
     * @todo fix flipping loop
     * @todo improve isRedPitchUberAlles
     */
    void Update()
    {
        // initialize our float array
        float[] spectrum = new float[numberOfSamples];

        // populate array with fequency spectrum data
        GetComponent<AudioSource>().GetSpectrumData(spectrum, 0, fftWindow);

        float[] heightsDistribution = new float[audioSpectrumObjects.Length];

        // loop over audioSpectrumObjects and modify according to fequency spectrum data
        // this loop matches the Array element to an object on a One-to-One basis.
        for (int i = 0; i < audioSpectrumObjects.Length; i++)
        {
            // apply height multiplier to intensity
            float intensity = spectrum[i] * heightMultiplier;

            // calculate object's scale
            float lerpY = Mathf.Lerp(audioSpectrumObjects[i].localScale.y,intensity,lerpTime);
            Vector3 newScale = new Vector3((float) 0.1, lerpY + (float) 0.2, 1);

            // appply new scale to object
            audioSpectrumObjects[i].localScale = newScale;
            heightsDistribution[i] = 1 / lerpY;
        }

        if (!_flipping && IsRedPitchDominating(heightsDistribution))
        {
            player.FlipPlayer();
            _flipping = true;
        }
        else
        {
            _flipping = false;
        }
    }

    public double GetVariance(float[] floatArray)
    {
        int arraySize = floatArray.Length;
        float average = floatArray[0] / arraySize;
        for (int i = 1; i < arraySize; i++)
        {
            average += floatArray[i] / arraySize;
        }
        double variance = -Math.Pow(average, 2);
        for (int i = 0; i < arraySize; i++)
        {
            variance += 1 / arraySize * floatArray[i];
        }
        return variance;
    }

    public float GetAverage(float[] floatArray)
    {
        int arraySize = floatArray.Length;
        float average = floatArray[0] / arraySize;
        for (int i = 1; i < arraySize; i++)
        {
            average += floatArray[i] / arraySize;
        }
        return average;
    }

    /**
     * @todo to improve
     */
    private bool IsRedPitchDominating(float[] allPitches)
    {
        float[] redPitches = { allPitches[8], allPitches[9], allPitches[10] };
        float[] nommalPitches = {
            allPitches[0],
            allPitches[1],
            allPitches[2],
            allPitches[3],
            allPitches[4],
            allPitches[5],
            allPitches[6],
            allPitches[7],
            allPitches[11],
            allPitches[12],
            allPitches[13],
            allPitches[14],
            allPitches[15],
            allPitches[16],
            allPitches[17],
            allPitches[18],
            allPitches[19],
            allPitches[20],
            allPitches[21],
            allPitches[22],
            allPitches[23],
            allPitches[24],
            allPitches[25],
            allPitches[26],
            allPitches[27],
            allPitches[28],
            allPitches[29]
        };
        return GetAverage(nommalPitches) / GetAverage(redPitches) > 40;
    }
}
