using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
				new MultipleChoice(1, "How would you like to respond?", 2, new Dictionary<string, Message[]>{{"Deny" , new Message[] {new Message(1, "What are you talking about?"), new Message(1, "I was home last night.")}}, 
				{"Admit" , new Message[] {new Message(1, "I was there but I didn't do it."), new Message(1, "Whoever did... is despicable.")}}}),
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
                    new Message(0, "Our investigators have evidence of " + interactionCount + " suspicious things you did on the scene."),
                    new Message(0, "Care to explain all that?")
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