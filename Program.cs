using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Guestbook
{
    class Program
    {
        // Lista för att lagra inlägg (nu av typen GuestbookEntry)
        static List<GuestbookEntry> guestbookMessages = new List<GuestbookEntry>();
        static string filePath = "guestbook.json"; // Filen för lagring av meddelanden

        static void Main(string[] args)
        {
            LoadMessages(); // Metod för att ladda in meddelanden

            bool running = true;
            while (running)
            {
                Console.Clear();
                Console.WriteLine("V Ä L K O M M E N  T I L L  G Ä S T B O K E N!\n\n");
                Console.WriteLine("1. Lägg till ett inlägg");
                Console.WriteLine("2. Ta bort ett inlägg");
                Console.WriteLine("3. Avsluta\n");
                ShowMessages(); // Visa befintliga inlägg
                string? choice = Console.ReadLine();

                switch (choice) //Switch för olika val
                {
                    case "1":
                        AddMessages(); // Metod för att lägga till meddelande
                        break;
                    case "2":
                        DeleteMessage(); // Metod för att ta bort inlägg
                        break;
                    case "3":
                        SaveMessages();  // Spara alla inlägg till fil innan programmet avslutas
                        running = false;
                        Console.WriteLine("Avslutar gästboken");
                        Console.Clear(); //Rensa konsollen
                        break;
                    default:
                        Console.WriteLine("Ogiltigt val, försök igen.");
                        break;
                }
            }
        }

        // Metod för att lägga till meddelande
        static void AddMessages()
        {
            Console.Clear();
            Console.Write("\nSkriv ditt namn: ");
            string? name = Console.ReadLine(); //Input för namn

            if (string.IsNullOrWhiteSpace(name)) //Ifall tom eller bara mellanslag
            {
                Console.WriteLine("Namnet får inte vara tomt."); //Skrivs ut
                Console.WriteLine("Tryck på valfri tangent för att gå tillbaks till menyn");
                Console.ReadKey(); // Pausar programmet tills användaren trycker på en tangent
                return;
            }

            Console.Write("Skriv ditt inlägg: ");
            string? entry = Console.ReadLine(); //Input för meddelande

            if (!string.IsNullOrWhiteSpace(entry)) //Om input är ifylld korrekt
            {
                guestbookMessages.Add(new GuestbookEntry(name, entry)); // Skapa nytt inlägg med namn och meddelande
                Console.WriteLine("Ditt meddelande är tillagt.");
                SaveMessages(); //Sparar meddelande dirket efter det lagt till
            }
            else
            {
                Console.WriteLine("Tomma meddelanden läggs inte till.");
                Console.WriteLine("Tryck på valfri tangent för att gå tillbaks till menyn");
                Console.ReadKey(); // Pausar programmet tills användaren trycker på en tangent
            }
        }

        // Metod för att visa alla meddelanden
        static void ShowMessages()
        {
            Console.WriteLine("\nInlägg i gästboken:");
            if (guestbookMessages.Count > 0) //Ifall det finns inlägg
            {
                for (int i = 0; i < guestbookMessages.Count; i++) //Loppar genom inläggen
                {
                    Console.WriteLine($"{i + 1}. {guestbookMessages[i]}"); //Skriver ut inläggen lägger till + 1 så att första meddelande får index 1 istället för 0
                }
            }
            else // Om inte 
            {
                Console.WriteLine("Gästboken är tom."); //Skrivs ut ifall inte inlägg finns
            }
        }

        // Metod för att ta bort ett inlägg baserat på index
        static void DeleteMessage()
        {
            Console.Clear(); //Rensa konsollen
            ShowMessages(); //Visa meddelande
            if (guestbookMessages.Count == 0) //Ifall noll inlägg
            {
                Console.WriteLine("Det finns inga inlägg att ta bort.");
                return;
            }

            Console.Write("\nAnge numret på inlägget du vill ta bort: ");
            string? input = Console.ReadLine();

            // Tolkar inputen som ett heltal istället för sträng
            if (int.TryParse(input, out int index) && index > 0 && index <= guestbookMessages.Count) //Måste vara större än 0 (börjar på 1) och mindre än eller lika med antalet inlägg
            {
                // Ta bort inlägget baserat på det valda indexet
                guestbookMessages.RemoveAt(index - 1); // Eftersom listan börjar på noll egentligen så behövs -1
                Console.WriteLine("Inlägget har tagits bort.");
                return;
            }
            else //Om inte ovan stämmer
            {
                Console.WriteLine($"Fel: Du måste välja ett nummer mellan 1 och {guestbookMessages.Count}.");
            }

            Console.WriteLine("Tryck på 1 för att gå tillbaks, tryck 2 för att ta bort inlägg");
            string? value = Console.ReadLine();
            switch (value) //Switch för val ifall fel index 
            {
                case "1":
                    return; //Om 1 gå tillbaks till menyn

                case "2":
                    DeleteMessage(); // Om två ny chans att ta bort meddelande
                    break;

                default:
                    Console.WriteLine("Ogiltligt val!"); //Om annat fel val
                    Console.WriteLine("Tryck på valfri tangent för att gå tillbaks till menyn");
                    Console.ReadKey(); // Pausar programmet tills användaren trycker på en tangent
                    break;

            }
        }


        // Metod för att spara inlägg till json-fil
        static void SaveMessages()
        {
            try
            {
                string jsonString = JsonSerializer.Serialize(guestbookMessages); //Serialisera till Json-string
                File.WriteAllText(filePath, jsonString); //sparar det till angiven filePath (guestbook.json)
                Console.WriteLine("Inlägg har sparats.");
            }
            catch (Exception ex) //Om fel uppstår
            {
                Console.WriteLine($"Fel vid sparning: {ex.Message}"); //Skrivs ut vid fel
            }
        }

        // Ladda inlägg från JSON-fil
        static void LoadMessages()
        {
            if (File.Exists(filePath)) //Ifall filen finns
            {
                try
                {
                    string jsonString = File.ReadAllText(filePath); //Läser in innehållet från filen lagras i variablen
                    guestbookMessages = JsonSerializer.Deserialize<List<GuestbookEntry>>(jsonString) ?? new List<GuestbookEntry>(); //Omvanlda Json stringen till en lista med objekt
                    Console.WriteLine("Tidigare inlägg har laddats in.");
                }
                catch (Exception ex) //OM fel uppstår
                {
                    Console.WriteLine($"Fel vid inläsning: {ex.Message}"); //skrivs ut vid fel
                }
            }
            else
            {
                Console.WriteLine("Ingen data hittades.");
            }
        }
    }
}



