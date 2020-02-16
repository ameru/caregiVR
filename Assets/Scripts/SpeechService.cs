using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Intent;

[Serializable]
public class Intent
{
    public string intent;
    public double score;
}

[Serializable]
public class Entity
{
    public string entity;
    public string type;
    public int startIndex;
    public int endIndex;
}

[Serializable]
public class InterpretResult
{
    public string query;

    public Intent topScoringIntent;

    public Entity[] entities;
}

public class SpeechService : MonoBehaviour
{
    /// <summary>
    /// Region of the Microsoft Azure resources.
    /// </summary>
    public string region = "westus2";

    [Header("Speech API")]
    public string speechKey;
    public string voice = "en-US-JessaNeural";

    [Header("Language Understanding API")]
    public string luisKey;
    public string appId;
    public List<string> intents;

    private SpeechSynthesizer synthesizer;
    private SpeechRecognizer recognizer;
    private IntentRecognizer intentRecognizer;

    private static SpeechService instance;

    private void Awake()
    {
        // Make only one SpeechService instance possible.
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        // Initialize synthesizer and recognizer.
        var speechConfig = SpeechConfig.FromSubscription(speechKey, region);
        speechConfig.SpeechSynthesisVoiceName = voice;
        synthesizer = new SpeechSynthesizer(speechConfig);
        recognizer = new SpeechRecognizer(speechConfig);

        // Initialize intent recognizer.
        var intentConfig = SpeechConfig.FromSubscription(luisKey, region);
        intentRecognizer = new IntentRecognizer(intentConfig);
        var model = LanguageUnderstandingModel.FromAppId(appId);
        foreach (var intent in intents)
        {
            intentRecognizer.AddIntent(model, intent);
        }
    }

    /// <summary>
    /// Read a text out loud to the user.
    /// </summary>
    /// <param name="text">Text to be read out loud.</param>
    /// <returns></returns>
    public static async Task Say(string text)
    {
        if (instance)
        {
            await instance.SayTask(text);
        }
        else
        {
            throw new System.Exception("SpeechService instance not available!");
        }
    }

    /// <summary>
    /// Listen to a few words that the user says.
    /// </summary>
    /// <returns>The words that the user said as a text.</returns>
    public static async Task<string> Hear()
    {
        if (instance)
        {
            return await instance.HearTask();
        }
        else
        {
            throw new System.Exception("SpeechService instance not available!");
        }
    }

    /// <summary>
    /// Interpret what the user says.
    /// </summary>
    /// <returns>An interpret result that contains the user query, the top scoring intent, and entities recognized.</returns>
    public static async Task<InterpretResult> Interpret()
    {
        if (instance)
        {
            return await instance.InterpretTask();
        }
        else
        {
            throw new System.Exception("SpeechService instance not available!");
        }
    }

    private async Task SayTask(string text)
    {
        await synthesizer.SpeakTextAsync(text);
    }

    private async Task<string> HearTask()
    {
        var result = await recognizer.RecognizeOnceAsync().ConfigureAwait(false);
        return result.Text;
    }

    private async Task<InterpretResult> InterpretTask()
    {
        var result = await intentRecognizer.RecognizeOnceAsync();
        
        if (result.Reason == ResultReason.RecognizedIntent)
        {
            var dataJson = result.Properties.GetProperty(PropertyId.LanguageUnderstandingServiceResponse_JsonResult);
            var data = JsonUtility.FromJson<InterpretResult>(dataJson);
            return data;
        }
        else
        {
            throw new System.Exception("Intent Recognizer couldn't recognize the intent!");
        }
    }
}
