using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Ink.Runtime;
using System.Linq;
using UnityEditor;
using UnityEngine.InputSystem;

/* A new and flexible version of my InkDialogueHandler that might be a bit overkill for a game jam :P
 * by Kris Sian */


public class InkDialogueRunner : MonoBehaviour
{
    [Header("Dialogue Boxes")]
    public TextMeshProUGUI dialogueTextbox;     // This is where Ink script lines will be printed to the screen.
    GameObject dialogueBox;                     // This is the above textbox's gameObject (toggle to display/hide).
    public GameObject playerChoicesDisplay;     // This is where the player's dialogue choices will appear (toggle to display/hide).
    public GameObject playerChoicesContainer;   // This controls the size of the playerChoicesDisplay scroll rect, and will be resized depending on the number of available choices.
    public GameObject playerChoicePrefab;       // This is what will be instantiated as a clickable button, to return a dialogue choice.
    TextMeshProUGUI playerChoiceTextbox;        // This is where the player's dialogue choice will be printed to the screen.

    [Header("Audio")]
    public AudioSource dialogueSFX;             // Call dialogueSFX.Play() or .Stop() to have little speech blips play while typing each line of dialogue.

    [Header("Compiled Ink Script")]
    public TextAsset storyText;
    public Story storyRunner;                   // You don't need to worry about this or the ones below. They're just used to check states and allow the player to skip through.
    string sentence;
    bool currentlyTyping;
    bool finishedTyping;

    [Header("SkillChecks")]
    List<string> skillBasedChoices = new List<string>() {  };       // Add strings separated by commas to fill the list. Use only if there's stats/skillchecks.
    int skillOneAbility;
    string skillOneChoice = "";
    int skillOneChoiceIndex;
    int skillTwoAbility;
    string skillTwoChoice = "";
    int skillTwoChoiceIndex;
    int skillThreeAbility;
    string skillThreeChoice = "";
    int skillThreeChoiceIndex;

    [Header("Inventory")]
    public List<string> inventory = new List<string>();
    Dictionary<string, string> itemDictionary = new Dictionary<string, string>();

    [Header("Special Prefixes")]
    public List<string> prefixes = new List<string>() { "END", "STL", "ITM", "ADD", "RMV", "SK1", "SK2", "SK3" };        // Use these to call specific functions from within an Ink Script.

    public InputActionAsset inputActions;


    private void Awake()
    {
        storyRunner = new Story(storyText.text);
        dialogueBox = dialogueTextbox.gameObject;
        dialogueBox.SetActive(false);
        playerChoicesDisplay.SetActive(false);

        itemDictionary.Add("exampleItem", "Use this heckin' cool example item..!");
    }



    public void NextSentence()
    {
        finishedTyping = false;
        currentlyTyping = false;


        if (storyRunner.canContinue)                // If there's more dialogue to display, check it for special functions, then print it to the screen.
        {
            sentence = storyRunner.Continue();

            if (PrefixCheck(sentence) == true)      // If items, storylets, and manually ending the Ink Script are never going to be used, comment out this whole block.
            {
                if (sentence.StartsWith("ADD"))
                {
                    string itemString = sentence.Remove(0, 4).Trim();
                    inventory.Add(itemString);
                }


                if (sentence.StartsWith("RMV"))
                {
                    string itemString = sentence.Remove(0, 4).Trim();
                    inventory.Remove(itemString);
                }

                if (sentence.StartsWith("SK1+"))
                {
                    skillOneAbility++;
                }
                if (sentence.StartsWith("SK1-"))
                {
                    skillOneAbility--;
                }
                if (sentence.StartsWith("SK2+"))
                {
                    skillTwoAbility++;
                }
                if (sentence.StartsWith("SK2-"))
                {
                    skillTwoAbility--;
                }
                if (sentence.StartsWith("SK3+"))
                {
                    skillThreeAbility++;
                }
                if (sentence.StartsWith("SK3-"))
                {
                    skillThreeAbility--;
                }


                if (sentence.StartsWith("STL"))
                {
                    /* This is a manual way to switch to a different story asset from within an Ink Script. */

                    string storylet = sentence.Remove(0, 3).Trim();
                    string path = "Assets/InkScripts/" + storylet + ".json";                                        // Comment this out when building.
                    storyText = (TextAsset)AssetDatabase.LoadAssetAtPath(path, typeof(TextAsset));                  // Comment this out when building.
                    /*storyText = Resources.Load<TextAsset>(storylet);*/                                            // Uncomment this when building. Put Ink Scripts into Assets/Resources.
                    storyRunner = new Story(storyText.text);
                    sentence = storyRunner.Continue();
                }


                if (sentence.StartsWith("END"))
                {
                    /* This is a manual way to end the current storylet, by calling another function. */
                }

                //if (storyRunner.canContinue) sentence = storyRunner.Continue();
                NextSentence();
                return;
            }



            dialogueTextbox.text = "";


            StopAllCoroutines();
            StartCoroutine(TypeOutSentence(sentence));
        }
    }



    IEnumerator TypeOutSentence(string sentence)
    {
        currentlyTyping = true;

        if (sentence == "" || sentence == null)
        {
            dialogueBox.SetActive(false);
        }
        else 
        { 
            dialogueBox.SetActive(true); 
        }

        dialogueSFX.Play();

        foreach (char letter in sentence)
        {
            if (currentlyTyping)
            {
                dialogueTextbox.text += letter;
                yield return new WaitForSeconds(0.03f);     // Time between typing each letter.
            }
        }

        dialogueSFX.Stop();
        finishedTyping = true;

        if (storyRunner.canContinue)
        {
            yield return new WaitForSeconds(2f);            // Wait for this long, then automatically move on to the next line of dialogue, if one exists.
            NextSentence();
        }
        else
        {
            yield return new WaitForSeconds(2f);            // Wait this long, then disable the dialogueBox and instantiate the player's dialogue options.
            dialogueBox.SetActive(false);

            PresentChoices();
        }
    }



    public void Interrupt()
    {
        StopAllCoroutines();

        while (storyRunner.canContinue)         // This will automatically stop at the next point where branching dialogue options exist.
        {
            sentence = storyRunner.Continue();
            if (sentence.StartsWith("END"))
            {
                /* End the dialogue and call another function ? */
            }
        }

        dialogueTextbox.text = "";
    }



    public void PresentChoices()
    {
        if (storyRunner.currentChoices.Count > 0)
        {
            playerChoicesDisplay.SetActive(true);

            for (int optionIndex = 0; optionIndex < storyRunner.currentChoices.Count; optionIndex++)
            {
                Choice choice = storyRunner.currentChoices[optionIndex];
                string choiceString = choice.text;

                if (PrefixCheck(choiceString) == false)                    // If skills and items are not going to be used, we can get rid of most of this.
                {
                    InstantiateChoice(choiceString, optionIndex);
                }
                else
                {
                    /* Performs SkillChecks by reading the first 4 characters of choiceString, then assigns any that the player passes
                     * to variables for each skill type, so the player only sees those the highest value options that they can access, if any. */
                    
                    float value;
                    string skillCheck = choiceString[3].ToString();
                    float.TryParse(skillCheck, out value);

                    if (choiceString.StartsWith("SK1") && skillOneAbility >= value)
                    {
                        skillOneChoice = choiceString.Remove(0, 5);
                        skillOneChoiceIndex = optionIndex;
                    }
                    if (choiceString.StartsWith("SK2") && skillTwoAbility >= value)
                    {
                        skillTwoChoice = choiceString.Remove(0, 5);
                        skillTwoChoiceIndex = optionIndex;
                    }
                    if (choiceString.StartsWith("SK3") && skillThreeAbility >= value)
                    {
                        skillThreeChoice = choiceString.Remove(0, 5);
                        skillThreeChoiceIndex = optionIndex;
                    }



                    string itemString = choiceString.Remove(0, 4);
                    if (choiceString.StartsWith("ITM") && inventory.Contains(itemString))
                    {
                        //choiceString = "Use " + itemString;               // You could swap the one below for this if you don't want to use the dictionary.
                        choiceString = itemDictionary[itemString];
                        InstantiateChoice(choiceString, optionIndex);
                    }

                    else if (choiceString.StartsWith("ADD") || choiceString.StartsWith("RMV"))
                    {
                        continue;
                    }
                }
            }


            /* Instantiate, and then reset, each skill-based dialogue option available. */
            if (skillOneChoice != "")
            {
                InstantiateChoice(skillOneChoice, skillOneChoiceIndex);
                skillOneChoice = "";
            }
            if (skillTwoChoice != "")
            {
                InstantiateChoice(skillTwoChoice, skillTwoChoiceIndex);
                skillTwoChoice = "";
            }
            if (skillThreeChoice != "")
            {
                InstantiateChoice(skillThreeChoice, skillThreeChoiceIndex);
                skillThreeChoice = "";
            }



            /* Resize the player's choices container depending on the number of available dialogue options. */
            int numberOfOptions = playerChoicesContainer.transform.childCount;
            if (numberOfOptions < 4)
            {
                playerChoicesDisplay.GetComponent<RectTransform>().sizeDelta = new Vector2(1200, (numberOfOptions * 55));
            }
            else
            {
                playerChoicesDisplay.GetComponent<RectTransform>().sizeDelta = new Vector2(1200, 220);
            }
        }
    }



    /* Fills the player's dialogue choices scoll rect with optionPrefabs, each with their own text. When clicked, they'll return the chosen dialogue option. */
    private void InstantiateChoice(string choiceString, int optionIndex)
    {
        GameObject option = Instantiate(playerChoicePrefab, playerChoicesContainer.transform);
        option.GetComponent<Button>().onClick.AddListener(() => ReturnChoice(optionIndex));

        playerChoiceTextbox = option.GetComponentInChildren<TextMeshProUGUI>();
        playerChoiceTextbox.text = choiceString;
    }



    public void ReturnChoice(int optionIndex)
    {
        playerChoicesDisplay.SetActive(false);
        storyRunner.ChooseChoiceIndex(optionIndex);

        foreach (Transform option in playerChoicesContainer.transform)
        {
            Destroy(option.gameObject);
        }

        NextSentence();
    }



    private void Skip()
    {
        if (currentlyTyping)                    // Allows the player to skip the 'typing' and simply displays the line of dialogue in full.
        {
            dialogueTextbox.text = sentence;
            currentlyTyping = false;
        }

        if (finishedTyping)                     // Allows the player to manually go to the next line of dialogue, rather than wait for the automatic timer.
        {
            if (storyRunner.canContinue)
            {
                NextSentence();
            }
        }
    }




    /* Call this if you need to hard-reset an Ink Script, i.e. if the player dies. */
    public void ResetDialogue()
    {
        StopAllCoroutines();

        storyRunner = new Story(storyText.text);
        dialogueBox.SetActive(false);
        playerChoicesDisplay.SetActive(false);

        foreach (Transform Option in playerChoicesContainer.transform)
        {
            Destroy(Option.gameObject);
        }
    }



    /* If you need to perform a skillcheck to determine whether the player has access to specific dialogue options, use this. */
    private bool PrefixCheck(string sentence)
    {
        for (int i = 0; i < prefixes.Count; ++i)
        {
            if (sentence.StartsWith(prefixes[i]))
            {
                return true;
            }
        }

        return false;
    }



    /* If you need to perform a skillcheck to determine whether the player has access to specific dialogue options, use this. */
    private bool SkillCheck(string choiceString)
    {
        for (int i = 0; i < skillBasedChoices.Count; ++i)
        {
            if (choiceString.StartsWith(skillBasedChoices[i]))
            {
                return true;
            }
        }

        return false;
    }



    private void OnEnable()
    {
        inputActions.FindAction("Interrupt").performed += context => Skip();
    }

    private void OnDisable()
    {
        inputActions.FindAction("Interrupt").performed -= context => Skip();
    }
}
