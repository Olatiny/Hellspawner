using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SecretMan : MonoBehaviour
{
    [SerializeField]
    Canvas dialogueCanvas;

    [SerializeField]
    TextMeshProUGUI dialogueText;

    [SerializeField]
    float dialogueOnTime = 5f;
    
    [SerializeField]
    float dialogueOffTime = 2.5f;

    [SerializeField, TextArea]
    List<string> sentences;

    [SerializeField]
    Canvas jumpScareCanvas;

    [SerializeField]
    float jumpScareTime = .5f;

    [SerializeField]
    Image playerImg;

    [SerializeField]
    Sprite happySprite;

    [SerializeField]
    Sprite sadSprite;

    private void Start()
    {
        dialogueCanvas.gameObject.SetActive(false);
        jumpScareCanvas.gameObject.SetActive(false);

        StartCoroutine(cutscene());
        StartCoroutine(backgroundChangeColors());
    }

    IEnumerator backgroundChangeColors()
    {
        Camera cam = Camera.main;

        float hue = 0f;
        while (true)
        {
            cam.backgroundColor = Color.HSVToRGB(hue, 1f, .5f);
            hue += Time.deltaTime / 10f;

            if (hue > 1f)
                hue = 0f;

            yield return null;
        }
    }

    IEnumerator cutscene()
    {
        yield return new WaitForSeconds(dialogueOffTime);

        foreach (string sentence in sentences)
        {
            if (sentence.Equals("_happi"))
            {
                playerImg.sprite = happySprite;
                continue;
            }
            else if (sentence.Equals("_sadi"))
            {
                playerImg.sprite = sadSprite;
                continue;
            }

            dialogueCanvas.gameObject.SetActive(true);
            dialogueText.text = sentence;

            yield return new WaitForSeconds(dialogueOnTime);
            dialogueCanvas.gameObject.SetActive(false);

            yield return new WaitForSeconds(dialogueOffTime);
        }

        float timeElapsed = 0f;

        while (timeElapsed < jumpScareTime)
        {
            jumpScareCanvas.gameObject.SetActive(true);
            yield return new WaitForSeconds(.1f * jumpScareTime);
            jumpScareCanvas.gameObject.SetActive(false);
            yield return new WaitForSeconds(.2f * jumpScareTime);
            timeElapsed += Time.deltaTime;
        }

        Application.Quit();
    }
}
