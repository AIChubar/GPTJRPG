using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextualInfoHUD : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _name;
    [SerializeField]
    private TextMeshProUGUI _race;
    [SerializeField]
    private TextMeshProUGUI _class;
    [SerializeField]
    private TextMeshProUGUI _profession;
    [SerializeField]
    private TextMeshProUGUI _backStory;
    // Start is called before the first frame update
    void Start()
    {
        var hero = GameManager.gameManager.hero;
        _name.text = hero.heroName;
        _race.text = hero.heroRace;
        _class.text = hero.heroClass;
        _profession.text = hero.heroProfession;
        _backStory.text = hero.heroBackStory;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
