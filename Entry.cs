using System;

namespace Guestbook
{
    //Klass för inlägg
    public class GuestbookEntry
    {
        public string Name { get; set; } //Lagra namn
        public string Message { get; set; } //Lagra meddelande

        public GuestbookEntry(string name, string message) //KOnstruktor för att skapa inlägg med namn och meddelande
        {
            Name = name;
            Message = message;
        }

        public override string ToString()
        {
            return $"{Name}: {Message}"; //returnerar Namn och meddelande till inlägg
        }
    }
}