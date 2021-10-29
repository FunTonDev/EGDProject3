using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NPC : MonoBehaviour
{
    public string npcName;

    public List<string> textDesc;
    private List<string> write_queue;

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
            canInteract = false;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            canInteract = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        dialogueBox = GameObject.FindGameObjectWithTag("DialogueBox");
        displayText = dialogueBox.transform.GetChild(1).GetComponent<TMP_Text>();
        textDesc = new List<string>();
        textDesc[0] = "Testing new dialogue box functionality";
        textDesc[1] = "Shouldn't you be doing something else right now?";
        textDesc[2] = "Welp, not like I can stop you";
    }

    public IEnumerator displayAllText(List<string> tts)
    {
        dialogueBox.SetActive(true);
        for (int i = 0; i < tts.Count; i++)
        {
            yield return textDisplay(tts[i]);
        }
        dialogueBox.SetActive(false);
    }

    IEnumerator textDisplay(string tt, bool stop = false)
    {
        scroll_speed = 20;
        //StopCoroutine("textDisplay");
        displayText.text = "";
        write_queue.Add(tt);
        writing = true;
        for (int i = 0; i < write_queue[0].Length && writing; i++)
        {
            if (inputMan.inputSubmit != 0.0f && stop)
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
                displayText.text += write_queue[0][i];
            }
        }
        writing = false;
        if (write_queue.Count >= 1)
        {
            write_queue.RemoveAt(0);
        }
        displayText.text = tt;
        if (stop)
        {
            yield return new WaitForSeconds(0.2f);
            yield return new WaitUntil(new System.Func<bool>(() => inputMan.inputSubmit != 0.0f));
        }
    }
}
