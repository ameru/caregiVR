using System.Collections;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Intent;

public class SpeechTest : MonoBehaviour
{
    async void Start()
    {
        await SpeechService.Say("Hello!");
        await SpeechService.Say("Welcome to care giver!");
        await SpeechService.Say("What's your name?");
        var result = await SpeechService.Interpret();
        var name = GetName(result);
        await SpeechService.Say("Nice to meet you " + name);
    }

    string GetName(InterpretResult result)
    {
        foreach (Entity entity in result.entities)
        {
            if (entity.type == "builtin.personName")
            {
                return entity.entity;
            }
        }
        return "";
    }
}
