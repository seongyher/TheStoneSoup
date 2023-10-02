using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Scripting;

public class DetectionZoneController : MonoBehaviour
{
    [Tooltip("Exposed to editor for debugging purposes.")]
    [SerializeField] private List<FoodItem> FoodList = null;

    [Tooltip("Tick if the customer request requires you to avoid certain flavours. If this is ticked and the FlavoursToAvoid list is empty, the player will fail every time.")]
    [SerializeField] private bool UsingFlavoursToAvoid = false;

    [Tooltip("Flavours required to exist in pot. Duplicate instances of tags can be added for customer requests requiring a lot of a certain flavour.")]
    [SerializeField] private List<string> FlavourRequirements = null;

    [Tooltip("Flavours to avoid. Duplicate instances of tags can be added if you want to give the player an allowance for bad flavours. E.g. adding 2 \"spicy\" tags will only fail the player once two spicy items are present.")]
    [SerializeField] private List<string> FlavoursToAvoid = null;

    [SerializeField] private TextMeshProUGUI RequirementsTMP = null;

    public string requiredIngredient;

    // Start is called before the first frame update
    void Start()
    {
        FoodList = new List<FoodItem>();
        RequirementsTMP.SetText(string.Join(" ", FlavourRequirements));
        RequirementsTMP.color = Color.white;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        FoodList.Add(collision.GetComponent<FoodItem>());
        CheckRequirements();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        FoodItem foodItem = collision.GetComponent<FoodItem>();
        if (foodItem != null)
        {
            if (FoodList.Contains(foodItem))
            {
                FoodList.Remove(foodItem);
            }

            CheckRequirements();
        }
    }

    private void CheckRequirements()
    {
        List<string> RequirementsToCheck = new List<string>(FlavourRequirements);
        List<string> AvoidedFoodToCheck = new List<string>(FlavoursToAvoid);

        foreach (FoodItem foodItem in FoodList)
        {
            foreach (string flavour in foodItem.flavours)
            {
                if (RequirementsToCheck.Contains(flavour))
                {
                    RequirementsToCheck.Remove(flavour);
                }
            }
        }

        foreach (FoodItem foodItem in FoodList)
        {
            foreach (string flavour in foodItem.flavours)
            {
                if (AvoidedFoodToCheck.Contains(flavour))
                {
                    AvoidedFoodToCheck.Remove(flavour);
                }
            }
        }

        if (RequirementsToCheck.Count == 0 && (!UsingFlavoursToAvoid || AvoidedFoodToCheck.Count > 0))
        {
            // Customer requirements met
            RequirementsTMP.color = Color.green;
        }
        else
        {
            // Customer request requirements not met
            RequirementsTMP.color = Color.white;
        }
    }

    public void CookIngredients()
    {
        for (int i = FoodList.Count() - 1; i >= 0; i--)
        {
            if (FoodList[i].flavour == requiredIngredient)
            {
                Destroy(FoodList[i].gameObject);
                //FoodList.RemoveAt(i);
            }
        }
        
    }
}