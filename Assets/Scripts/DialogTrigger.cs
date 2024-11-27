using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System;

public class DialogTrigger : MonoBehaviour
{
    private Message[] messages;
    public Actor[] actors;
    private int i = 0;
    public static string rootDirectory = "Audio/NewVoicelines/Edited_files/";
    private string introSoundsPath = "IntroVoiceLines/";
    private string interrogator = "Interrogator/";

    static public string FormDirectoryPath(string[] path)
    {
        return rootDirectory + string.Join("", path);
    }


    public void StartDialogue()
    {
        string currScene = SceneManager.GetActiveScene().name;
        
        if (currScene == "Intro")
        {
            string soundPath = FormDirectoryPath(new string[] {introSoundsPath});
            string interrogatorSoundPath = soundPath + interrogator;
            messages = new Message[]
            {
                new Message(0, "As you know, you're the main suspect in the arson attack that took place last night.", interrogatorSoundPath + "As_you_know"),
                new Message(0, "And you understand that we can use anything you say against you, right?", interrogatorSoundPath + "And_you_understand"),
                new Message(1, "Does it matter if I didn't do it?", soundPath + "Does_it_matter"),
                new Message(0, "Well then these questions should be easy for you.", interrogatorSoundPath + "Well_then"),
                new Message(0, "And we only want the truth, got it?", interrogatorSoundPath + "And_we_only")
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
                    new MultipleChoice(0, "How well did you know the victims?", 2, new Dictionary<string, Message[]>
                    {
                        {"Quite well actually.", new Message[] {new Message(0, "Sounds about right.")}},
                        {"Only in passing.", new Message[] {new Message(0, "Appalling. We have evidence you were a close friend."), new Message(0, "We're suspicious.")}}
                    }),
                    new MultipleChoice(0, "In which direction do you believe they fled the scene?", 2, new Dictionary<string, Message[]>
                    {
                        {"Out the front door.", new Message[] {new Message(0, "Is that right?"), new Message(0, "Doesn't quite explain why that side door was left open.")}},
                        {"Out the side door.", new Message[] {new Message(0, "That checks out with what our guy says."), new Message(0, "The side door was indeed open when we examined the scene.")}}
                    }),
                    new MultipleChoice(0, "How'd they do it?", 2, new Dictionary<string, Message[]>
                    {
                        {"Gasoline and a molotov.", new Message[] {new Message(0, "Wow, a dramatic flair, eh?"), new Message(0, "I'll believe it when I see it.")}},
                        {"Gasoline and a lighter.", new Message[] {new Message(0, "Indeed."), new Message(0, "That molotov cocktail looked pretty... unused.")}}
                    }),
                    new MultipleChoice(0, "What sort of building were the victims in?", 2, new Dictionary<string, Message[]>
                    {
                        {"A cabin in the woods.", new Message[] {new Message(0, "You really didn't know these guys eh?"), new Message(0, "That or your memory is going.")}},
                        {"An old farmhouse.", new Message[] {new Message(0, "Good."), new Message(0, "You can remember where you were, at least.")}}
                    }),
                    new MultipleChoice(0, "How many things did you notice on the scene?", 2, new Dictionary<string, Message[]>
                    {
                        {$"{interactionCount}", new Message[] {new Message(0, "Correct.")}},
                        {$"{interactionCount + 2}", new Message[] {new Message(0, "Not quite.")}}
                    }),
                    new MultipleChoice(0, "Did the victims have any rivals?", 2, new Dictionary<string, Message[]>
                    {
                        {"I don't think so.", new Message[] {new Message(0, "Really? You had no idea, huh.")}},
                        {"I think so.", new Message[] {new Message(0, "Hmph, that must explain the brotherly tiffs witnesses reported on.")}}
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
                        {"In the afternoon.", new Message[] {new Message(0, "Then how come the house wasn't dust at your feet when we got there?"), new Message(0, "OoooOOOooo an unburnable house, haha.")}},
                        {"In the nighttime.", new Message[] {new Message(0, "It sure did."), new Message(0, "Any earlier and you wouldn't have had time for your one-man investigation.")}}
                    }),
                    new MultipleChoice(0, "Aside from the victims, how many witnesses were there?", 2, new Dictionary<string, Message[]>
                    {
                        {"I was the only one.", new Message[] {new Message(0, "Correct, you were the only witness."), new Message(0, "A good reason why you're a prime suspect at the moment.")}},
                        {"I saw someone else.", new Message[] {new Message(0, "Come on. The house is completely isolated."), new Message(0, "There's not a single soul within a mile of the area."), new Message(0, "You must've hurt your head.")}}
                    }),
                    new MultipleChoice(0, "Have you ever been to the house before that day?", 2, new Dictionary<string, Message[]>
                    {
                        {"No, I don't believe so.", new Message[] {new Message(0, "This is not going well for you."), new Message(0, "We've discovered old polaroids showing that you've been there.")}},
                        {"Yes, I believe so.", new Message[] {new Message(0, "Indeed, we're aware you paid some visits to them in the past.")}}
                    }),
                };

                questions = questions.OrderBy(x => UnityEngine.Random.Range(0, 100)).ToList();

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
                finalMessages.Add(new MultipleChoice(0, "So after all that, have you figured it out? Who did it? The grand reveal?", 2, new Dictionary<string, Message[]>
                    {
                        {"It was William.", new Message[] {new Message(0, "... William is dead.")}},
                        {"It was Warren.", new Message[] {new Message(0, "Our informant? We'll have to check your story, that's quite the accusation.")}}
                    }));

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
    public AudioClip voiceline;

    // Constructor that initializes actorid and message
    public Message(int actorid, string message, string voicelinePath = "")
    {
        this.actorid = actorid;
        this.message = message;
        if (!string.IsNullOrEmpty(voicelinePath))
        {
            LoadVoiceline(voicelinePath);
        }
    }
    
    // Method to load the voiceline
    public void LoadVoiceline(string path)
    {
        Debug.Log("Loading voiceline at path: " + path);
        voiceline = Resources.Load(path) as AudioClip;
        if (voiceline == null)
        {
            Debug.LogError("Failed to load voiceline at path: " + path);
        }
    }
}

[System.Serializable]
public class MultipleChoice : Message{
	public int numberOfChoices;
	public Dictionary<string, Message[]> optionStrings;
    public static string positiveResponseSoundsPath = "InterrogationVoiceLines/PositiveResponse";
    public static string negativeResponseSoundsPath = "InterrogationVoiceLines/NegativeResponse";

    
    public static AudioClip LoadRandomAudioClip(string path)
    {
        AudioClip[] clips = Resources.LoadAll<AudioClip>(path);
        if (clips.Length == 0)
        {
            Debug.LogError("No audio clips found at path: " + path);
            return null;
        }
        int randomIndex = UnityEngine.Random.Range(0, clips.Length);
        return clips[randomIndex];
    }

    public static AudioClip LoadRandomPositiveResponse()
    {
        return LoadRandomAudioClip(DialogTrigger.FormDirectoryPath(new string[] {positiveResponseSoundsPath}));
    }

    public static AudioClip LoadRandomNegativeResponse()
    {
        return LoadRandomAudioClip(DialogTrigger.FormDirectoryPath(new string[] {negativeResponseSoundsPath}));
    }


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