using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Adatrogzito_WPF
{
    public class Data
    {
        public string Name { get; private set; }
        public int Age { get; private set; }
        public string Email { get; private set; }
        public string Mobile { get; private set; }
        public string Address { get; private set; }
        public bool Gender { get; private set; }
        public string Description { get; private set; }

        public Data(string name, int age, string email, string mobile, string address, bool gender, string desc)
        {
            Name = name;
            Age = age;
            Email = email;
            Mobile = mobile;
            Address = address;
            Gender = gender;
            Description = desc;
        }

        public Data(string line)
        {
            string[] splitText = line.Split(",");
            if (splitText.Length != 7)
                throw new FormatException("A felhasználói adatok formátuma nem megfelelő");
            
            Name = splitText[0];
            Age = int.Parse(splitText[1]);
            Email = splitText[2];
            Mobile = splitText[3];
            Address = splitText[4];
            Gender = bool.Parse(splitText[5]);
            Description = splitText[6];
        }

        public override string ToString()
        {
            string gender = Gender ? "Férfi" : "Nő";
            return $"{Name}, {Age}, {Email}, {Mobile}, {Address}, {gender}, {Description}";
        }

        public string SaveString()
        {
            return $"{Name}, {Age}, {Email}, {Mobile}, {Address}, {Gender}, {Description}";
        }
    }
}
