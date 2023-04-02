using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace HudElements
{
    public class HealthBar : VisualElement
    {
        public int width { get; set; }
        public int height { get; set; }

        public new class UxmlFactory : UxmlFactory<HealthBar, UxmlTraits>{ }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            UxmlIntAttributeDescription m_width = new UxmlIntAttributeDescription() {
                name = "width", defaultValue = 300
            };
            UxmlIntAttributeDescription m_height = new UxmlIntAttributeDescription () {
                name = "height", defaultValue = 50
            };

            public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
            {
                get {yield break;}
            }
/*
            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                var ate = ve as HealthBar;
                ate.width = m_width.GetValueFromBag(bag, cc);
                ate.height = m_height.GetValueFromBag(bag, cc);

                //ate.Clear();
                VisualTreeAsset vt = Resources.Load<VisualTreeAsset>("HealthBar");
                VisualElement healthbar = vt.Instantiate();
                ate.Add(healthbar);
            }*/
        }
        
    }
}

