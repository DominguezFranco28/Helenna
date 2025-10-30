using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


[System.Serializable]
public class DialogueLine
{

    public string scene;
    public string speaker;
    public int lineId;
    public string text;
    [Header("Audio / Emotion")]
    public string emotionToken; // ej: "happy", "sad", "angry"
}

[System.Serializable]
public class DialogueWrapper
{
    public DialogueLine[] lines;
}

public class DialogueManager : MonoBehaviour

{
    [SerializeField] private Image speakerThumbnail;
    [SerializeField] private Image background;
    [SerializeField] private Image thumbBackground;
    [SerializeField] private Image speakerBackground;
    [SerializeField] private TextMeshProUGUI speakerName;
    [SerializeField] private TextMeshProUGUI body;
    [SerializeField] private TextMeshProUGUI skipTip;
    public float talkSpeed = 0.1f;
    public float endLineDelay = 1f;
    public bool canSkip = true;
    public bool isSpeaking = false;
    [SerializeField] private List<Sprite> thumbnails = new List<Sprite>();
    //Audio
    [SerializeField] private AudioClip typingSFX;
    private bool isTypingSFXPlaying = false;

    private Coroutine writeLineCoroutine;

    private (string, string, int) lastID;

    private Color textColor = Color.white;
    private Color backgroundColor = new Color(0f / 255f, 0f / 255f, 0f / 255f, 160f / 255f); ///new Color(171f / 255f, 109f / 255f, 44f / 255f, 220f / 255f);
    private Color thumbBackgroundColor = new Color(60f / 255f, 40f / 255f, 60f / 255f, 170f / 255f); ///new Color(171f / 255f, 109f / 255f, 44f / 255f, 220f / 255f);
    private Color speakerBackgroundColor = new Color(0f, 0f, 0f, 220f / 255f);

    public TextAsset dialogueFile;
    private DialogueWrapper data;
    public event System.Action<DialogueLine> OnLineStarted; //agregado para cinematicas


    [Header("DEBUG")]
    public string debugScene = "";
    public bool debugSpeak = false;


    private PauseManager pauseManager;

    private void Start()
    {
        //Load dialogue file
        data = JsonUtility.FromJson<DialogueWrapper>(dialogueFile.text);

        MakeClear();

        pauseManager = FindAnyObjectByType<PauseManager>();
    }

    public void StartScene(string scene)
    {
        StartCoroutine(PlaySceneCoroutine(scene));
    }

    private IEnumerator PlaySceneCoroutine(string scene)
    {
        InputManager.Instance.LockInputs();
        isSpeaking = true;
        if (pauseManager)
            pauseManager.SetIsSpeaking(isSpeaking);

        List<DialogueLine> sceneLines = GetLinesFromScene(scene);
        if (sceneLines != null)
        {
            foreach (DialogueLine line in sceneLines)
            {
                Speak(line);
                while (writeLineCoroutine != null)
                    yield return null;
                yield return new WaitForSeconds(endLineDelay);
            }
        }

        isSpeaking = false;
        if (pauseManager)
            pauseManager.SetIsSpeaking(isSpeaking);
    }

    private List<DialogueLine> GetLinesFromScene(string scene)
    {
        List<DialogueLine> sceneLines = new List<DialogueLine>();

        if (data == null || data.lines == null)
            return sceneLines; // return empty list if no data

        foreach (var line in data.lines)
        {
            if (line.scene == scene)
                sceneLines.Add(line);
        }

        return sceneLines;
    }

    public void Speak(DialogueLine line)
    {
        //speakerThumbnail.sprite = GetSpeakerThumbnail(line.speaker);
        //speakerName.text = line.speaker;
        //string text = line.text;
        //if (text.Length > 0)
        //{
        //    body.text = "";
        //    lastID = (line.scene, line.speaker, line.lineId);

        //    MakeVisible();

        //    writeLineCoroutine = StartCoroutine(WriteLine(text));
        //}
        if (line == null) return;

        speakerThumbnail.sprite = GetSpeakerThumbnail(line.speaker);
        speakerName.text = line.speaker;

        body.text = "";
        lastID = (line.scene, line.speaker, line.lineId);

        MakeVisible();

        //audio token de emoción
        if (!string.IsNullOrEmpty(line.emotionToken))
            SFXManager.Instance.PlayEmotion(line.emotionToken);

 
        writeLineCoroutine = StartCoroutine(WriteLine(line.text));

        OnLineStarted?.Invoke(line);//evento para cinematicas

    }

    public void Speak(string scene, string speaker, int lineId)
    {
        //speakerThumbnail.sprite = GetSpeakerThumbnail(speaker);
        //string text = GetLine(scene, speaker, lineId);
        //if (text.Length > 0)
        //{
        //    body.text = "";
        //    lastID = (scene, speaker, lineId);

        //    MakeVisible();

        //    writeLineCoroutine = StartCoroutine(WriteLine(text));
        //}
        // Obtener el objeto completo
        DialogueLine lineObj = GetLineObject(scene, speaker, lineId);
        if (lineObj == null) return; 

        // thumbnail y nombre
        speakerThumbnail.sprite = GetSpeakerThumbnail(speaker);
        speakerName.text = speaker;

        // limpia texto y guardar ID
        body.text = "";
        lastID = (scene, speaker, lineId);

        MakeVisible();

        //audio según token de emoción
        if (!string.IsNullOrEmpty(lineObj.emotionToken))
            SFXManager.Instance.PlayEmotion(lineObj.emotionToken);
        writeLineCoroutine = StartCoroutine(WriteLine(lineObj.text));
        OnLineStarted?.Invoke(lineObj);

    }

    private Sprite GetSpeakerThumbnail(string speaker)
    {
        int speakerId = 0;
        string name = speaker.ToLower().Trim();
        switch (name)
        {
            case "harold":
                speakerId = 1;
                break;
            case "rex":
                speakerId = 2;
                break;
            case "nina":
                speakerId = 3;
                break;
            default:
                speakerId = 0; //narrator
                break;
        }

        return thumbnails[speakerId];
    }
    private DialogueLine GetLineObject(string scene, string speaker, int lineId) //para obtener el objeto y ubicar la emocion
    {
        foreach (var line in data.lines)
        {
            if (line.scene.ToLower().Trim() == scene.ToLower().Trim() &&
                line.speaker.ToLower().Trim() == speaker.ToLower().Trim() &&
                line.lineId == lineId)
            {
                return line;
            }
        }
        return null;
    }
    private string GetLine(string scene, string speaker, int lineId)
    {
        if (data == null || data.lines == null)
            return null;

        foreach (var line in data.lines)
        {
            if (line.scene.ToLower().Trim() == scene.ToLower().Trim() && line.speaker.ToLower().Trim() == speaker.ToLower().Trim() && line.lineId == lineId)
            {
                return line.text;
            }
        }

        return null; // not found
    }

    private IEnumerator WriteLine(string line)
    {
        body.text = "";
        if (isTypingSFXPlaying) //si habia sonido de tipeo lo detengo
        {
            SFXManager.Instance.StopLoop();
            isTypingSFXPlaying = false;
        }
        if (typingSFX != null)
        {
            SFXManager.Instance.PlayLoop(typingSFX);
            isTypingSFXPlaying = true;
        }
        foreach (char c in line)
        {
            body.text += c;
            yield return new WaitForSeconds(talkSpeed);
        }
        if (isTypingSFXPlaying) //freno el tipeo
        {
            SFXManager.Instance.StopLoop(); 
            isTypingSFXPlaying = false;
        }
        /*
        writeLineCoroutine = null;
        yield return new WaitForSeconds(endLineDelay);
        MakeClear();
        */
    }

    private IEnumerator EndLine()
    {
        writeLineCoroutine = null;
        lastID = ("", "", -1);
        yield return new WaitForSeconds(endLineDelay);
        MakeClear();
        body.text = "";
    }

    private void OnEnable()
    {
        InputManager.Instance.SkipDialoguePressed += SkipLine;
    }
    private void OnDisable()
    {
        InputManager.Instance.SkipDialoguePressed -= SkipLine;
    }

    private void SkipLine()
    {
        if (writeLineCoroutine != null)
        {
            StopCoroutine(writeLineCoroutine);
            if(body.text != GetLine(lastID.Item1, lastID.Item2, lastID.Item3))
                body.text = GetLine(lastID.Item1, lastID.Item2, lastID.Item3);
            else
                StartCoroutine(EndLine());
        }
        if (isTypingSFXPlaying) //freno aca tambien xq sino se bugeaba
        {
            SFXManager.Instance.StopLoop();
            isTypingSFXPlaying = false;
        }

    }

    private void MakeVisible()
    {
        speakerName.color = textColor;
        body.color = textColor;
        speakerThumbnail.color = Color.white;
        background.color = backgroundColor;
        thumbBackground.color = thumbBackgroundColor;
        speakerBackground.color = speakerBackgroundColor;
        skipTip.color = textColor;
    }

    private void MakeClear()
    {
        speakerName.color = Color.clear;
        body.color = Color.clear;
        speakerThumbnail.color = Color.clear;
        background.color = Color.clear;
        thumbBackground.color = Color.clear;
        speakerBackground.color = Color.clear;
        skipTip.color = Color.clear;
    }

    
    
    private void Update()
    {
        if (debugSpeak)
        {
            debugSpeak = false;
            StartScene(debugScene);
        }
    }
}
