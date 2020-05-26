using UnityEngine;

public class PerksText : MonoBehaviour
{
    public TextMesh[] PerksTexts;

    private string[] PerksDescriptions = new string[8]
    {
        "Movement speed UP",
        "Beam speed UP",
        "Beam path lenght UP",
        "Beam charge speed UP",
        "Beam splash UP",
        "Beam damage UP",
        "Ionizing",
        "Explosive barrels"
    };

    public void SetPerks(int[] perksIndex, int[] perksLvls)
    {
        for (int i = 0; i < PerksTexts.Length; i++)
        {
            if (perksIndex.Length > i)
            {
                PerksTexts[i].text = i + 1 + " " + PerksDescriptions[perksIndex[i]] + " [" + perksLvls[i] + "]";
            }
            else
            {
                PerksTexts[i].text = string.Empty;
            }
        }
    }

    private void Start()
    {
    }

    private void Update()
    {
    }
}