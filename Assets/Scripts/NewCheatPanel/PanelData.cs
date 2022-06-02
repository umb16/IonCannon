using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace NewCheatPanel
{
    public class PanelData
    {
        public Dictionary<string, Category> Categories = new Dictionary<string, Category>();
        public void TryAddCategory(string name)
        {
            if (Categories.TryGetValue(name, out var value))
            {
                value.Hided = false;
            }
            else
                Categories.Add(name, new Category(name));
        }

        public Dictionary<KeyCode, List<Action>> GetAllBinds(Action<string, string> action)
        {
            Dictionary<KeyCode, List<Action>> dict = new Dictionary<KeyCode, List<Action>>(); 
            foreach (var category in Categories.Values)
            {
                foreach (var cheat in category.Cheats.Values)
                {
                    foreach (var bind in cheat.Binds)
                    {
                        if (!dict.TryGetValue(bind, out var list))
                        {
                            dict[bind] = new List<Action>();
                        }
                        dict[bind].Add(() => action(category.Name, cheat.Name));
                    }
                    
                }
            }
            return dict;
        }
    }
}
