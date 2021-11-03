using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NPC : MonoBehaviour
{
    public string npcName;

    public List<string> textDesc;
    public List<string> write_queue;

    public bool canInteract;
    public bool writing;
    public float scroll_speed;

    public GameObject dialogueBox;
    private TMP_Text displayText;
    private InputManager inputMan;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            canInteract = true;
        }
        else
        {
            StopAllCoroutines();
            canInteract = false;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            canInteract = false;
            for (int x = 0; x < dialogueBox.transform.childCount; x++)
            {
                dialogueBox.transform.GetChild(x).gameObject.SetActive(false);
                write_queue.Clear();
                displayText.text = "";
            }
            StopAllCoroutines();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        npcName = "Botly";
        inputMan = GameObject.Find("[MANAGER]").GetComponent<InputManager>();
        dialogueBox = GameObject.FindGameObjectWithTag("DialogueBox");
        displayText = dialogueBox.transform.GetChild(2).GetComponent<TMP_Text>();
        textDesc = new List<string>();
        textDesc.Add("Testing new dialogue box functionality");
        textDesc.Add("Shouldn't you be doing something else right now?");
        textDesc.Add("Welp, not like I can stop you");
        write_queue = new List<string>();
        scroll_speed = 20;
    }

    private void Update()
    {
        if (canInteract && inputMan.inputSubmit_D)
        {
            canInteract = false;
            StartCoroutine(displayAllText(textDesc));
        }
    }

    public IEnumerator displayAllText(List<string> tts)
    {
        for (int x = 0; x < dialogueBox.transform.childCount; x++)
        {
            dialogueBox.transform.GetChild(x).gameObject.SetActive(true);
        }
        for (int i = 0; i < tts.Count; i++)
        {
            yield return textDisplay(npcName + ": " + tts[i]);
        }
        for (int x = 0; x < dialogueBox.transform.childCount; x++)
        {
            dialogueBox.transform.GetChild(x).gameObject.SetActive(false);
        }
        canInteract = true;
    }

    IEnumerator textDisplay(string tt, bool stop = false)
    {
        scroll_speed = 40;
        //StopCoroutine("textDisplay");
        displayText.text = "";
        write_queue.Add(tt);
        writing = true;
        for (int i = 0; i < tt.Length && writing; i++)
        {
            if (inputMan.inputSubmit_D && stop)
            {
                displayText.text = "";
                displayText.text = tt;
                write_queue.RemoveAt(0);
                writing = false;
                //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                break;
            }
            if (writing)
            {
                yield return new WaitForSeconds(1f / scroll_speed);
                displayText.text += tt[i];
            }
        }
        writing = false;
        if (write_queue.Count >= 1)
        {
            write_queue.RemoveAt(0);
        }
        displayText.text = tt;

        yield return new WaitForSeconds(0.2f);
        yield return new WaitUntil(new System.Func<bool>(() => inputMan.inputSubmit_D));
    }
}
