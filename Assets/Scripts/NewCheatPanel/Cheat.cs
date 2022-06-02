using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace NewCheatPanel
{
    public class Cheat 
    {
        public bool Hided;
        public string Name;
        public string Code;
        public List<KeyCode> Binds = new List<KeyCode>();

        public Cheat(string name, string code, IEnumerable<KeyCode> binds)
        {
            Name = name;
            Code = code;
            Binds = new List<KeyCode>(binds);
        }
    }
}
