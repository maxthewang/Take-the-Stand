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
                List<MultipleChoice> questions = new List<MultipleChoice>
                {
                    new MultipleChoice(0, "How do you know the victims?", 2, new Dictionary<string, Message[]>
                    {
                        {"I'm a close friend.", new Message[] {new Message(0, "Sounds about right.")}},
                        {"Never known them.", new Message[] {new Message(0, "Appalling. We have evidence you were a close friend."), new Message(0, "We're suspicious.")}}
                    }),
                    new MultipleChoice(0, "So, what happened to the house?", 2, new Dictionary<string, Message[]>
                    {
                        {"It was blown up.", new Message[] {new Message(0, "Don't pretend to be naive here."), new Message(0, "We know for certain there was never an explosion.")}},
                        {"It was set ablaze.", new Message[] {new Message(0, "Indeed.")}}
                    }),
                    new MultipleChoice(0, "What sort of building were the victims in?", 2, new Dictionary<string, Message[]>
                    {
                        {"A brick house.", new Message[] {new Message(0, "Absolutely not. No bricks. A brick house doesn't burn down as quickly as that."), new Message(0, "What's up with your memory? Nevermind, I think you're trying to lie your way out.")}},
                        {"A wooden house.", new Message[] {new Message(0, "Good."), new Message(0, "You can remember the structure, at least.")}}
                    }),
                    new MultipleChoice(0, "How many things did you notice on the scene?", 2, new Dictionary<string, Message[]>
                    {
                        {$"{interactionCount}", new Message[] {new Message(0, "Correct.")}},
                        {$"{interactionCount + 2}", new Message[] {new Message(0, "Not quite.")}}
                    }),
                    new MultipleChoice(0, "Did the victims have any rivals?", 2, new Dictionary<string, Message[]>
                    {
                        {"Uh, not that I know of.", new Message[] {new Message(0, "Really? You're absolutely hiding something from us.")}},
                        {"Well, one argued with his brother a lot.", new Message[] {new Message(0, "That checks out.")}}
                    }),
                    new MultipleChoice(0, "Be real with us. What were you up to before the attack?", 2, new Dictionary<string, Message[]>
                    {
                        {"We stopped at a gas station.", new Message[] {new Message(0, "Exactly."), new Message(1, "Why's that important?"), new Message(0, "Can't say too much, but the gas station is pretty crucial to our findings.")}},
                        {"We were sharing drinks at a bar.", new Message[] {new Message(0, "Just drinking?? You're leaving out something crucial."), new Message(0, "We know about the gas station trip you made.")}}
                    }),
                    new MultipleChoice(0, "Why were you so intent on inspecting the evidence?", 2, new Dictionary<string, Message[]>
                    {
                        {"The flames would've disintegrated it all.", new Message[] {new Message(0, "Fine. That's reasonable.")}},
                        {"I was just curious about what happened.", new Message[] {new Message(0, "That won't cut it. What a suspicious answer."), new Message(0, "It should be common sense not to mess with stuff before the police gets there.")}}
                    }),
                    new MultipleChoice(0, "When did the crime happen?", 2, new Dictionary<string, Message[]>
                    {
                        {"In the afternoon.", new Message[] {new Message(0, "Wrong. It was completely dark out.")}},
                        {"In the nighttime.", new Message[] {new Message(0, "It sure did happen at night.")}}
                    }),
                    new MultipleChoice(0, "Aside from the victims, how many witnesses were there?", 2, new Dictionary<string, Message[]>
                    {
                        {"I was the only one.", new Message[] {new Message(0, "Correct, you were the only witness."), new Message(0, "A good reason why you're a prime suspect at the moment.")}},
                        {"Someone else must've seen it.", new Message[] {new Message(0, "Come on. The house is completely isolated."), new Message(0, "There's not a single soul within a mile of the area.")}}
                    }),
                    new MultipleChoice(0, "Have you ever been to the house before that day?", 2, new Dictionary<string, Message[]>
                    {
                        {"I never have.", new Message[] {new Message(0, "This is not going well for you."), new Message(0, "We've discovered old polaroids showing that you've been there.")}},
                        {"Just a few times.", new Message[] {new Message(0, "Indeed, we're aware you paid some visits to them in the past.")}}
                    }),
                };

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

                messages = finalMessages.ToArray();
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