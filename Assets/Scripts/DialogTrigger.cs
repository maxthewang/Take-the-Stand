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
    private string interrogationVoiceLines = "InterrogationVoiceLines/";
    private string mainCharResponseSoundsPath = "MainCharResponseSFX/";
    private string introSoundsPath = "IntroVoiceLines/";
    private string interrogatorPath = "Interrogator/";

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
            string interrogatorSoundPath = soundPath + interrogatorPath;
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
            string soundPath = FormDirectoryPath(new string[] {interrogationVoiceLines});
            string interrogatorSoundPath = soundPath + interrogatorPath;
            Debug.Log("Interrogator sound path: " + interrogatorSoundPath);
            if (interactionCount == 0)
            {

                messages = new Message[]
                {
                    new Message(0, "Is it all coming back to you now?", interrogatorSoundPath + "Is_it_all"),
                    new Message(1, "I... don't remember doing anything.", soundPath + "I_dont_remember"),
                    new Message(0, "We're looking for the truth.", interrogatorSoundPath + "Were_looking_for"),
                    new Message(0, "Either you're lying to us or you're not thinking hard enough.", interrogatorSoundPath + "Either_youre"),
                };
            }
            else
            {
                List<MultipleChoice> questions = new List<MultipleChoice>
                {
                    new MultipleChoice(0, "Aside from the victims, how many witnesses were there?", 2, new Dictionary<string, Message[]>
                    {
                        {"I was the only one.", new Message[] {new Message(0, "Correct, you were the only witness."), new Message(0, "A good reason why you're a prime suspect at the moment.", interrogatorSoundPath + "a_good_reason_why")}},
                        {"I saw someone else.", new Message[] {new Message(0, "Come on. The house is completely isolated."), new Message(0, "There's not a single soul within a mile of the area.", interrogatorSoundPath + "theres_not_a_single"), new Message(0, "You must've hurt your head.", interrogatorSoundPath + "you_mustve_hurt_your")}}
                    }, interrogatorSoundPath + "aside_from"),
                    new MultipleChoice(0, "What sort of building were the victims in?", 2, new Dictionary<string, Message[]>
                    {
                        {"A cabin in the woods.", new Message[] {new Message(0, "Get your eyes checked.", interrogatorSoundPath + "get_your_eyes_checked"), new Message(0, "Or your head.", interrogatorSoundPath + "or_your_head")}},
                        {"An old farmhouse.", new Message[] {new Message(0, "Good.", interrogatorSoundPath + "good"), new Message(0, "You can remember where you were, at least.", interrogatorSoundPath + "you_can_remember_where")}}
                    }, interrogatorSoundPath + "what_sort"),
                    new MultipleChoice(0, "Have you ever been to that house before that day?", 2, new Dictionary<string, Message[]>
                    {
                        {"No, I don't believe so.", new Message[] {new Message(0, "This is not going well for you.", interrogatorSoundPath + "this_is_not_going"), new Message(0, "We've discovered old polaroids of you soliciting the premises.", interrogatorSoundPath + "weve_discovered_old_polaroids")}},
                        {"Yes, I believe so.", new Message[] {new Message(0, "Indeed, we're aware you paid some visits to them in the past.", interrogatorSoundPath + "indeed_were_aware_you")}}
                    }, interrogatorSoundPath + "have_you_been"),
                    new MultipleChoice(0, "How well did you know the victims?", 2, new Dictionary<string, Message[]>
                    {
                        {"Quite well actually.", new Message[] {new Message(0, "Sounds about right. Although I'm sure that family knew just about everyone in this town.", interrogatorSoundPath + "sounds_about_right_although")}},
                        {"Only in passing.", new Message[] {new Message(0, "Yeah right. We have evidence you were a close friend of the family.", interrogatorSoundPath + "yeah_right_we_have"), new Message(0, "I mean how could you even get that one wrong?", interrogatorSoundPath + "i_mean_how_could")}}
                    }, interrogatorSoundPath + "how_well_did"),
                    new MultipleChoice(0, "Alright, I'm asking you to think back to the crime itself, how many things did you notice on the scene?", 2, new Dictionary<string, Message[]>
                    {
                        {$"{interactionCount}", new Message[] {new Message(0, "Correct.")}},
                        {$"{interactionCount + 2}", new Message[] {new Message(0, "Not quite.")}}
                    }, interrogatorSoundPath + "alright_im_asking"),
                    new MultipleChoice(0, "When did the crime happen?", 2, new Dictionary<string, Message[]>
                    {
                        {"In the afternoon.", new Message[] {new Message(0, "Then how come the house wasn't dust at your feet when we got there?", interrogatorSoundPath + "then_how_come_the"), new Message(0, "OoooOOOooo an unburnable house, haha.", interrogatorSoundPath + "oh_an_unburnable_house")}},
                        {"In the nighttime.", new Message[] {new Message(0, "It sure did.", interrogatorSoundPath + "it_sure_did"), new Message(0, "Any earlier and you wouldn't have had time for your one-man investigation.", interrogatorSoundPath + "any_earlier_and_you")}}
                    }, interrogatorSoundPath + "when_did"),
                    new MultipleChoice(0, "In which direction do you believe the perpetrator fled the scene?", 2, new Dictionary<string, Message[]>
                    {
                        {"Out the front door.", new Message[] {new Message(0, "Is that right?", interrogatorSoundPath + "is_that_right"), new Message(0, "Doesn't quite explain all the blood by the side door.", interrogatorSoundPath + "doesnt_quite_explain_all")}},
                        {"Out the side door.", new Message[] {new Message(0, "That checks out with what our guy says.", interrogatorSoundPath + "that_checks_out_with"), new Message(0, "The side door was indeed ajar and bloodied when we examined the scene.", interrogatorSoundPath + "the_side_door_was")}}
                    }, interrogatorSoundPath + "in_which_dir"),
                    new MultipleChoice(0, "And how do you think they made it into the building in the first place?", 2, new Dictionary<string, Message[]>
                    {
                        {"In through the garage.", new Message[] {new Message(0, "That's silly. The garage only has one entrance and exit.", interrogatorSoundPath + "thats_silly_the_garage")}},
                        {"In through the window.", new Message[] {new Message(0, "You must be right.", interrogatorSoundPath + "you_must_be_right"), new Message(0, "It seems they made it halfway through the house before they woke someone up.", interrogatorSoundPath + "it_seems_they_made")}}
                    }, interrogatorSoundPath + "and_how_do"),
                    new MultipleChoice(0, "Any guesses on how they set the fire?", 2, new Dictionary<string, Message[]>
                    {
                        {"Gasoline and a molotov.", new Message[] {new Message(0, "Wow, a dramatic flair, eh?", interrogatorSoundPath + "wow_a_dramatic_flair"), new Message(0, "I'll believe it when I see it.", interrogatorSoundPath + "ill_believe_it_when")}},
                        {"Gasoline and a lighter.", new Message[] {new Message(0, "Indeed.", interrogatorSoundPath + "indeed"), new Message(0, "That molotov cocktail looked pretty... unused.", interrogatorSoundPath + "that_molotov_cocktail_looked")}}
                    }, interrogatorSoundPath + "any_guesses"),
                    new MultipleChoice(0, "How about the blood?", 2, new Dictionary<string, Message[]>
                    {
                        {"It must've been the knife.", new Message[] {new Message(0, "What knife?", interrogatorSoundPath + "what_knife"), new Message(0, "You're one hell of a nut case.", interrogatorSoundPath + "yore_one_hell_of")}},
                        {"It must've been the gun.", new Message[] {new Message(0, "Our forensics team found the same.", interrogatorSoundPath + "our_forensics_team_found"), new Message(0, "It must've been quite the struggle, the way we found the Father.", interrogatorSoundPath + "it_mustve_been_quite")}}
                    }, interrogatorSoundPath + "how_about_the_blood"),
                    new MultipleChoice(0, "Anything else you think might help us?", 2, new Dictionary<string, Message[]>
                    {
                        {"William was a jerk.", new Message[] {new Message(0, "With that much money? It's no surprise.", interrogatorSoundPath + "with_that_much_money"), new Message(0, "I'd hate to have known him too.", interrogatorSoundPath + "id_hate_to_have")}},
                        {"Warren was a liar.", new Message[] {new Message(0, "What do you mean?", interrogatorSoundPath + "what_do_you_mean"), new Message(1, "He's in a lot more debt than he told me."), new Message(0, "That is... interesting.", interrogatorSoundPath + "that_is_interesting")}}
                    }, interrogatorSoundPath + "anything_else"),
                    new MultipleChoice(0, "What about something more tangible?", 2, new Dictionary<string, Message[]>
                    {
                        {"There was a photo.", new Message[] {new Message(0, "We're already aware of the photos.", interrogatorSoundPath + "were_already_aware_of"), new Message(0, "Family portraits aren't exactly what we're looking for from a dead family.", interrogatorSoundPath + "family_portraits_arent_exactly")}},
                        {"There was a document.", new Message[] {new Message(0, "Go on.", interrogatorSoundPath + "go_on"), new Message(1, "It seems William declined a big offer for the farm recently.", , interrogatorSoundPath + "i_wonder_what_that")}}
                    }, interrogatorSoundPath + "what_about_something"),
                    new MultipleChoice(0, "Ok, let's talk motives, did the victims have any rivals?", 2, new Dictionary<string, Message[]>
                    {
                        {"I don't think so.", new Message[] {new Message(0, "Really? You had no idea, huh.", interrogatorSoundPath + "really_you_had_no")}},
                        {"I think so.", new Message[] {new Message(0, "Hmph, that must explain the brotherly tiffs witnesses reported on.", interrogatorSoundPath + "hmph_that_must_explain")}}
                    }, interrogatorSoundPath + "ok_lets_talk"),
                    new MultipleChoice(0, "And how about you? Why were you so intent on inspecting the evidence?", 2, new Dictionary<string, Message[]>
                    {
                        {"The flames would've disintegrated it all.", new Message[] {new Message(0, "Fine. That's reasonable.", interrogatorSoundPath + "fine_thats_reasonable")}},
                        {"I didn't mean to.", new Message[] {new Message(0, "You didn't mean to tamper with an active crime scene?", interrogatorSoundPath + "you_didnt_mean_to"), new Message(0, "It should be common sense not to mess with stuff before the police gets there.", interrogatorSoundPath + "it_should_be_common")}}
                    }, interrogatorSoundPath + "and_how_about_you"),
                    new MultipleChoice(0, "Be real with us. What were you up to before the attack?", 2, new Dictionary<string, Message[]>
                    {
                        {"We stopped at a gas station.", new Message[] {new Message(0, "... We?", interrogatorSoundPath + "we")}},
                        {"We were sharing drinks at a bar.", new Message[] {new Message(0, "Just drinking?? You're leaving out something crucial.", interrogatorSoundPath + "just_drinking_youre_leaving"), new Message(0, "We know about the gas station trip you made.", interrogatorSoundPath + "we_know_about_the")}}
                    }, interrogatorSoundPath + "be_real_with_us"),
                };

                questions = questions.ToList();

            // Create a final messages array combining fixed messages with the randomized questions
                List<Message> finalMessages = new List<Message>();
                finalMessages.Add(new Message(0, "Is it all coming back to you now?", interrogatorSoundPath + "Is_it_all")); // Optional fixed message before questions
                finalMessages.Add(new Message(1, "Yeah, I remember what I did.", soundPath + mainCharResponseSoundsPath + "sfx_yeah"));
                finalMessages.Add(new Message(0, "Our investigators have evidence of " + interactionCount + " suspicious things you did on the scene.", soundPath + "NegativeResponse/sfx_hm"));
                finalMessages.Add(new Message(0, "Let's start with the easy questions shall we?", soundPath + "NegativeResponse/sfx_ha_nl01"));

                // Add shuffled questions to final messages
                foreach (var question in questions)
                {
                    finalMessages.Add(question);
                }
                finalMessages.Add(new MultipleChoice(0, "So after all that, have you figured it out? Who did it? The grand reveal?", 2, new Dictionary<string, Message[]>
                    {
                        {"It was William.", new Message[] {new Message(0, "... William is dead.", interrogatorSoundPath + "william")}},
                        {"It was Warren.", new Message[] {new Message(0, "Our informant? We'll have to check your story, that's quite the accusation.", interrogatorSoundPath + "warren"), new Message(0, "Especially since he has been so helpful.", interrogatorSoundPath + "especially_since_he")}}
                    }, interrogatorSoundPath + "so_after_all_that"));

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
        Debug.Log("Loaded " + clips.Length + " audio clips from path: " + path);
        Debug.Log("Clips: " + clips);
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
        string path = DialogTrigger.FormDirectoryPath(new string[] {positiveResponseSoundsPath});
        Debug.Log("Loading positive response from path: " + path);
        return LoadRandomAudioClip(path);
    }

    public static AudioClip LoadRandomNegativeResponse()
    {
        string path = DialogTrigger.FormDirectoryPath(new string[] {negativeResponseSoundsPath});
        Debug.Log("Loading negative response from path: " + path);
        return LoadRandomAudioClip(path);
    }


	public MultipleChoice(int actorid, string message, int numberOfChoices, Dictionary<string, Message[]> optionStrings, string voicelinePath = "") : base(actorid, message, voicelinePath){
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