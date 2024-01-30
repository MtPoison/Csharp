// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, Bongo!");
Initialisation init = new Initialisation();
Marine marine = new Marine();
Jimbey jimbey = new Jimbey();
Combat combat = new Combat();
init.creationEntity(marine, jimbey);
marine.DisplayDetails();
jimbey.DisplayDetails();
combat.startCombat(jimbey, marine);