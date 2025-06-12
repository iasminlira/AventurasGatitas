using UnityEngine;
using UnityEngine.UI;

public class StarManager : MonoBehaviour
{
    public int maxStars = 3;
    private int currentStars = 0;

    public Image[] starImages; // 3 imagens da UI
    public Sprite fullStar;
    public Sprite emptyStar;



    public int GetMultiplier() => 1 + currentStars;

    public bool UseStar()
    {
        if (currentStars > 0)
        {
            currentStars--;
            UpdateUI();
            return true;
        }
        return false;
    }

    public void CollectStar()
    {
        if (currentStars < maxStars)
        {
            currentStars++;
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        for (int i = 0; i < starImages.Length; i++)
        {
            starImages[i].sprite = i < currentStars ? fullStar : emptyStar;
        }
    }
    
}
