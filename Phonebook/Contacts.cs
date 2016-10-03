using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Phonebook
{
    public class Contacts
    {
        private String name;
        private String number;
        private String email;

        public Contacts(String name, String number, String email)
        {
            setName(name);
            setNumber(number);
            setEmail(email);
        }

        public String getName()
        {
            return this.name;
        }

        public String getNumber()
        {
            return this.number;
        }

        public String getEmail()
        {
            return this.email;
        }

        private void setName(String name)
        {
            if(String.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("You have to add some name to this contact!");
            }
            else if (name.Contains(':') || name.Contains(','))
            {
                throw new ArgumentOutOfRangeException("You cant have speacial symbols as ':' or ',' in your contact name ! ");
            }
            this.name = name;
        }

        private void setNumber(String number)
        {
            if (String.IsNullOrEmpty(number))
            {
                throw new ArgumentNullException("You have to add some number to this contact!");
            }
            else if(number.Length != 10)
            {
                throw new ArgumentOutOfRangeException("The number length is " + number.Length + ". It must be exact 10 numbers!");
            }

            for (int i = 0; i < number.Length; i++)
            {
                if(number[i] < '0' || number[i] > '9')
                {
                    throw new ArgumentOutOfRangeException("The number can contain only number characters from 0 to 9. And it must be exact 10 numbers!");
                }
              
            }
            this.number = number;
        }

        private void setEmail(String email)
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(email);

            if (String.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException("You have to add some email to this contact!");
            }
            else if(!match.Success)
            {
                throw new ArgumentOutOfRangeException("The email address of this contact is invalid !");
            }
            this.email = email;
        }
    }
}
