using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace NewCheatPanel
{
    public class Category
    {
        public bool Hided;
        public string Name;
        public Dictionary<string, Cheat> Cheats = new Dictionary<string, Cheat>();
        public Category(string name)
        {
            Name = name;
        }
        public void AddCheat(string name, string code, IEnumerable<KeyCode> binds)
        {

            if (Cheats.TryGetValue(name, out Cheat cheat))
            {
                if (string.IsNullOrEmpty(code))
                {
                    cheat.Hided = false;
                    return;
                }
            }
                Cheats[name] = new Cheat(name, code, binds);
        }
    }
}
