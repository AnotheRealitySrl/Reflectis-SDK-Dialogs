using System;
using System.Collections;
using UnityEngine;
using TMPro;

namespace Reflectis.PLG.Dialogs
{
    public class TypewriterEffect : MonoBehaviour
    {
        // Basic Typewriter Functionality
        private int currentVisibleCharacterIndex;
        private Coroutine typewriterCoroutine;
        private bool readyForNewText = true;
        public bool ReadyForNewText { get => readyForNewText; }

        private WaitForSeconds simpleWait;
        private WaitForSeconds interpunctuationWait;

        // Skipping Functionality
        public bool CurrentlySkipping { get; private set; }
        private WaitForSeconds skipDelay;
        private bool quickSkip;
        private int skipSpeedup = 5;


        // Event Functionality
        private WaitForSeconds textboxFullEventDelay;
        private float sendDoneDelay = 0.25f; // In testing, I found 0.25 to be a good value

        public static event Action CompleteTextRevealed;
        public static event Action<char> CharacterRevealed;


        public void Setup(float charactersPerSecond, float interpunctuationDelay, bool quickSkipEnabled, int speedUp)
        {
            simpleWait = new WaitForSeconds(1 / charactersPerSecond);
            interpunctuationWait = new WaitForSeconds(interpunctuationDelay);

            skipSpeedup = speedUp;
            quickSkip = quickSkipEnabled;

            skipDelay = new WaitForSeconds(1 / (charactersPerSecond * skipSpeedup));
            textboxFullEventDelay = new WaitForSeconds(sendDoneDelay);
        }

        public void PrepareForNewText(TextMeshProUGUI textBox) //Se lo chiamo da altro script, non serve il primo check! 
        {
            if (!readyForNewText)
                return;

            CurrentlySkipping = false;
            readyForNewText = false;

            if (typewriterCoroutine != null)
                StopCoroutine(typewriterCoroutine);

            textBox.maxVisibleCharacters = 0;
            currentVisibleCharacterIndex = 0;

            typewriterCoroutine = StartCoroutine(Typewriter(textBox));
        }

        private IEnumerator Typewriter(TextMeshProUGUI textBox)
        {
            TMP_TextInfo textInfo = textBox.textInfo;

            while (currentVisibleCharacterIndex < textInfo.characterCount + 1)
            {
                var lastCharacterIndex = textInfo.characterCount - 1;

                if (currentVisibleCharacterIndex >= lastCharacterIndex)
                {
                    textBox.maxVisibleCharacters++;
                    yield return textboxFullEventDelay;
                    CompleteTextRevealed?.Invoke();
                    readyForNewText = true; 
                    CurrentlySkipping = false;
                    yield break;
                }

                char character = textInfo.characterInfo[currentVisibleCharacterIndex].character;

                textBox.maxVisibleCharacters++;

                if (!CurrentlySkipping &&
                    (character == '?' || character == '.' || character == ',' || character == ':' ||
                     character == ';' || character == '!' || character == '-'))
                {
                    yield return interpunctuationWait;
                }
                else
                {
                    yield return CurrentlySkipping ? skipDelay : simpleWait;
                }

                CharacterRevealed?.Invoke(character);
                currentVisibleCharacterIndex++;
            }
        }

        public void Skip(TextMeshProUGUI textBox)
        {
            if (CurrentlySkipping)
                return;


            if (!quickSkip)
            {
                CurrentlySkipping = true;
                return;
            }

            StopCoroutine(typewriterCoroutine);
            textBox.maxVisibleCharacters = textBox.textInfo.characterCount;
            readyForNewText = true;
            CompleteTextRevealed?.Invoke();
        }
    }
}
