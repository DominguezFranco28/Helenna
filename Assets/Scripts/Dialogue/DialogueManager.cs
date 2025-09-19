using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private Image speakerThumbnail;
    [SerializeField] private Image background;
    [SerializeField] private TextMeshProUGUI speakerName;
    [SerializeField] private TextMeshProUGUI body;
    public float talkSpeed = 0.1f;
    public bool canSkip = true;

    [SerializeField] private List<Sprite> thumbnails = new List<Sprite>();
    [SerializeField] private List<string> lines = new List<string>();

    private Coroutine writeLineCoroutine;

    private int lastLineID;

    [SerializeField] private Color textColor = Color.white;
    [SerializeField] private Color backgroundColor = new Color(226f,211f,195f,220f);

    [Header("DEBUG")]
    public int debugSpeakerID = 0;
    public int debugLineID = 0;
    public bool debugSpeak = false;

    private void Start()
    {
        MakeClear();
    }

    public void Speak(int speakerID, int lineID)
    {
        speakerThumbnail.sprite = GetSpeakerThumbnail(speakerID);
        string line = GetLine(lineID);
        body.text = "";
        lastLineID = lineID;

        MakeVisible();

        writeLineCoroutine = StartCoroutine(WriteLine(line));
    }

    private Sprite GetSpeakerThumbnail(int speakerID)
    {
        return thumbnails[speakerID];
    }

    private string GetLine(int lineID)
    {
        return lines[lineID];
    }

    private IEnumerator WriteLine(string line)
    {
        foreach(char c in line)
        {
            body.text = body.text + c;
            yield return new WaitForSeconds(talkSpeed);
        }
        StartCoroutine(EndLine());
        
    }

    private IEnumerator EndLine()
    {
        writeLineCoroutine = null;
        lastLineID = -1;
        yield return new WaitForSeconds(1f);
        MakeClear();
        body.text = "";
    }

    private void OnEnable()
    {
        InputManager.Instance.InteractPressed += SkipLine;
    }
    private void OnDisable()
    {
        InputManager.Instance.InteractPressed -= SkipLine;
    }

    private void SkipLine()
    {
        StopCoroutine(writeLineCoroutine);
        body.text = GetLine(lastLineID);
        StartCoroutine(EndLine());
    }

    private void MakeVisible()
    {
        speakerName.color = textColor;
        body.color = textColor;
        speakerThumbnail.color = Color.white;
        background.color = backgroundColor;
    }

    private void MakeClear()
    {
        speakerName.color = Color.clear;
        body.color = Color.clear;
        speakerThumbnail.color = Color.clear;
        background.color = Color.clear;
    }

    private void Update()
    {
        if (debugSpeak)
        {
            debugSpeak = false;
            Speak(debugSpeakerID, debugLineID);
        }
    }
}
