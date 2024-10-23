using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class DialogTrigger : MonoBehaviour
{
    private Message[] messages;
    public Actor[] actors;
    private int i = 0;

    public void StartDialogue()
    {
        string currScene = SceneManager.GetActiveScene().name;
        Debug.Log(currScene);
        
        if (currScene == "Intro")
        {
            messages = new Message[]
            {
                new Message(0, "As you know, you're the main suspect in the arson attack that took place last night."),
                new Message(0, "And you understand that we can use anything you say against you, right?"),
                new Message(1, "Does it matter if I didn't do it?"),
                new Message(0, "Well then these questions should be easy for you."),
                new Message(0, "And we only want the truth, got it?")
            };
        }
        else
        {
            int interactionCount = GameManager.instance.GetInteractionCount();
            
            if (interactionCount == 0)
            {
                messages = new Message[]
                {
                    new Message(0, "Is it all coming back to you now?"),
                    new Message(1, "I... don't remember doing anything."),
                    new Message(0, "We're looking for the truth."),
                    new Message(0, "Either you're lying to us or you're not thinking hard enough.")
                };
            }
            else
            {
                messages = new Message[]
                {
                    new Message(0, "Is it all coming back to you now?"),
                    new Message(1, "Yeah, I remember what I did."),
                    new Message(0, "Our investigators have evidence of some suspicious things you did on the scene."),
                    
                    new MultipleChoice(0, "How do you know the victims?", 2, new Dictionary<string, Message[]>
                    {
                        {"I'm a close friend.", new Message[] {new Message(0, "Sounds about right.")}},
                        {"Never known them.", new Message[] {new Message(0, "Appalling. We're suspicious.")}}
                    }),
                    new Message(0, "We have evidence you knew them."),
                    new MultipleChoice(0, "And what happened to the house?", 2, new Dictionary<string, Message[]>
                    {
                        {"It was blown up.", new Message[] {new Message(0, "Don't pretend to be naive here.")}},
                        {"It was set ablaze.", new Message[] {new Message(0, "Indeed.")}}
                    }),
                    new MultipleChoice(0, "How many things did you notice on the scene?", 2, new Dictionary<string, Message[]>
                    {
                        {$"{interactionCount}", new Message[] {new Message(0, "Correct.")}},
                        {$"{interactionCount + 2}", new Message[] {new Message(0, "Not quite.")}}
                    }),
                    new Message(1, "I don't see how that's relevant."),
                    new Message(0, "Let's move on."),
                    new Message(1, "Fine."),
                    new Message(0, "Listen, so you know the victims."),
                    new MultipleChoice(0, "Did they have any rivals?", 2, new Dictionary<string, Message[]>
                    {
                        {"Uh, not that I know of.", new Message[] {new Message(0, "Really? You're absolutely hiding something from us.")}},
                        {"Well, one argued with his brother a lot.", new Message[] {new Message(0, "That checks out.")}}
                    }),
                };
            }
        }

        FindObjectOfType<DialogManager>().OpenDialog(messages, actors);
    }

    private void Update()
    {
        if (i == 0)
        {
            StartDialogue();
            i++;
        }
    }

}

[System.Serializable]
public class Message
{
    public int actorid;
    public string message;

    // Constructor that initializes actorid and message
    public Message(int actorid, string message)
    {
        this.actorid = actorid;
        this.message = message;
    }
}

[System.Serializable]
public class MultipleChoice : Message{
	public int numberOfChoices;
	public Dictionary<string, Message[]> optionStrings;

	public MultipleChoice(int actorid, string message, int numberOfChoices, Dictionary<string, Message[]> optionStrings) : base(actorid, message){
		this.optionStrings = optionStrings;
		this.numberOfChoices = numberOfChoices;
	}
}

[System.Serializable]
public class Actor
{
    public string name;
    public Sprite sprite;
}