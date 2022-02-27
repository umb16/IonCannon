using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PerksMenu : MonoBehaviour
{
    [SerializeField] private PerksMenuElement[] _elements;
    private PlayerPerksController _perksController;

    [Inject]
    private void Construct(PlayerPerksController perksController)
    {
        _perksController = perksController;
    }

    private void OnEnable()
    {
        IPerk[] perks = _perksController.GetRandomAvaliable(_elements.Length);
        for (int i = 0; i < _elements.Length; i++)
        {
            var element = _elements[i];
            if (i < perks.Length)
            {
                var perk = perks[i];

                element.Show(
                    perk.Name + " " + LocalizationManager.Instance.GetPhrase(LocKeys.Lvl) + " " + (perk.Level + 1),
                    perk.Description,
                    () => { gameObject.SetActive(false); perk.AddLevel(); }
                    );
            }
            else
                element.Hide();
        }
    }
}
