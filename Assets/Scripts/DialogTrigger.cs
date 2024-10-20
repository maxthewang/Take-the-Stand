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
            // int interactionCount = GameManager.instance.GetInteractionCount();
            int interactionCount = 1;
            
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
                // Create a list to hold the questions
                List<MultipleChoice> questions = new List<MultipleChoice>
                {
                    new MultipleChoice(0, "How do you know the victims?", 2, new Dictionary<string, Message[]>
                    {
                        {"I'm a close friend.", new Message[] {new Message(0, "Sounds about right."), new Message(1, "uh huh")}},
                        {"Never known them.", new Message[] {new Message(0, "Appalling. Send him to jail."), new Message(1, "wait let me expla-")}}
                    }),

                    new MultipleChoice(0, "What were you doing last night?", 2, new Dictionary<string, Message[]>
                    {
                        {"I was at home.", new Message[] {new Message(0, "Alone?"), new Message(1, "I don't see how that's relevant.")}},
                        {"I was out with friends.", new Message[] {new Message(0, "Who were you with?"), new Message(1, "They'll back me up!")}}
                    }),

                    new MultipleChoice(0, "Why should we believe you?", 2, new Dictionary<string, Message[]>
                    {
                        {"I have nothing to hide.", new Message[] {new Message(0, "Then prove it."), new Message(1, "I swear, I'm innocent!")}},
                        {"This is a setup!", new Message[] {new Message(0, "Every suspect says that."), new Message(1, "But I'm telling the truth!")}}
                    })
                };

                // Shuffle the questions randomly
                questions = questions.OrderBy(x => Random.Range(0, 100)).ToList();

                // Create a final messages array combining fixed messages with the randomized questions
                List<Message> finalMessages = new List<Message>();
                finalMessages.Add(new Message(0, "Is it all coming back to you now?")); // Optional fixed message before questions
                finalMessages.Add(new Message(1, "Yeah, I remember what I did."));
                finalMessages.Add(new Message(0, "Our investigators have evidence of " + interactionCount + " suspicious things you did on the scene."));

                // Add shuffled questions to final messages
                foreach (var question in questions)
                {
                    finalMessages.Add(question);
                }

                messages = finalMessages.ToArray(); // Convert back to array
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


