using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameSceneUIManager : MonoBehaviour
{
    // [SerializeField] protected UIManager m_UIManager;
    [SerializeField] protected UIDocument m_Document;

    public VisualElement allyHpBarContainer;
    public VisualElement enemyHpBarContainer;
    public VisualElement allyHealthBar;
    public VisualElement enemyHealthBar;
//    public VisualElement healthBar2;
    public List<VisualElement> healthBars = new List<VisualElement>();
    public List<VisualElement> enemyHealthBars = new List<VisualElement>();

    VisualElement m_Root;

    void Start()
    {
        if(m_Document) m_Root = m_Document.rootVisualElement;
        else Debug.LogWarning("There is no m_Document");

        allyHpBarContainer = m_Root.Q<VisualElement>("AllyHpBarContainer");
        enemyHpBarContainer = m_Root.Q<VisualElement>("EnemyHpBarContainer");

        allyHealthBar = m_Root.Q<VisualElement>("HpBar1");
        enemyHealthBar = m_Root.Q<VisualElement>("EnemyHpBar1");

        healthBars.Add(m_Root.Q<VisualElement>("HpBar1"));
        healthBars.Add(m_Root.Q<VisualElement>("HpBar2"));
        healthBars.Add(m_Root.Q<VisualElement>("HpBar3"));
        healthBars.Add(m_Root.Q<VisualElement>("HpBar4"));

        enemyHealthBars.Add(m_Root.Q<VisualElement>("EnemyHpBar1"));
        // later, modify..
        enemyHealthBars[0].name = "ET_HpBar";
    }

/*
    public void AddAllyHpBar()
    {
        VisualElement ve = new VisualElement. (allyHealthBar);
        allyHpBarContainer.Add(allyHealthBar);
        
    }
*/
}
