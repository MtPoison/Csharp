using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace Project.Attack
{
    
    public enum AttackType
    {
        FruitParamecia,
        FruitZoan,
        FruitLogia,
        Haki,
        Armes
    }
    public class Attack
    {
        public string name { get; set; }
        public AttackType type { get; set;}

        public virtual void Execute()
        {
            Console.WriteLine($"Nom de l'attaque : {name} ||| type : {type}");
        }
    }

    public class Haki : Attack
    {
        public int damage { get; set; }
        public int countEndurance { get; set; }

        public override void Execute()
        {
            base.Execute();
            Console.WriteLine($"Dégats : {damage}");
        }
    }
}
