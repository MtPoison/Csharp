public class Combat
{
    private bool tourAlier = false;
    Random random = new Random();
    public void startCombat(EntityAbstrac alier, EntityAbstrac enemie)
    {
        while (alier._heal >= 0 || enemie._heal >= 0) 
        { 
            if(alier._speed > enemie._speed || tourAlier) 
            {
                tourAlier = true;
            }
            else 
            {
                tourAlier = false;
            }

            if(tourAlier ) 
            {
                Console.WriteLine("les differentes option: \n A.Attaque B.Soin C.Stamina ");
                ConsoleKeyInfo keyInfo = Console.ReadKey();
                switch (keyInfo.Key)
                {
                    case ConsoleKey.A: 
                        enemie.TakeDamage(50);
                        break;
                    case ConsoleKey.B: 
                        alier.AddHeal(10);
                        break;
                    case ConsoleKey.C: 
                        alier.AddStamina(50);
                        break;
                   
                }
                Console.WriteLine($"vie {alier._name} : {alier._heal} ");
                Console.WriteLine($"vie {enemie._name} : {enemie._heal} ");
                if (enemie._heal <= 0) {
                    Console.WriteLine("Tu as gagner");
                    int nombreAleatoire = random.Next(1, 10 * enemie._level);
                    alier.AddExperience(nombreAleatoire);
                    
                    break; 
                }
                tourAlier = false;
            }
            if (!tourAlier )
            {

                int nombreAleatoire = random.Next(1, 3);
                if(nombreAleatoire == 1) alier.TakeDamage(50);
                if(nombreAleatoire == 2) enemie.AddHeal(10);
                if(nombreAleatoire == 3) enemie.AddStamina(10);
                Console.WriteLine($"vie {alier._name} : {alier._heal} ");
                Console.WriteLine($"vie {enemie._name} : {enemie._heal} ");
                if (alier._heal <= 0) 
                {
                    Console.WriteLine("Tu as perdu");
                    break; 
                }
                tourAlier = true;
            }
        }
    }
}
