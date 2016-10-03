using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Phonebook
{
    public partial class Form2 : Form
    {

        private PhoneBook form1;
        private string editName;
        private string editEmail;
        private string editNumber;
        
        public Form2(PhoneBook form1)
        {
            InitializeComponent();
            this.form1 = form1;
            this.editName = EditName.Text;
            this.editEmail = EditEmail.Text;
            this.editNumber = EditNumber.Text;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            DataGridViewRow selectedRowForEditing = this.form1.getTheSelectedRow();
            EditName.Text = selectedRowForEditing.Cells[0].Value.ToString();
            EditEmail.Text = selectedRowForEditing.Cells[1].Value.ToString();
            EditNumber.Text = selectedRowForEditing.Cells[2].Value.ToString();
            this.editName = EditName.Text;
            this.editEmail = EditEmail.Text;
            this.editNumber = EditNumber.Text;
        }

        private void Number_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void Save_Click(object sender, EventArgs e)
        {
            List<Contacts> newContactsList = new List<Contacts>();
            newContactsList = this.form1.getContacts();

            if (String.IsNullOrEmpty(EditName.Text) || String.IsNullOrEmpty(EditEmail.Text) || String.IsNullOrEmpty(EditNumber.Text))
            {
                MessageBox.Show("Dont leave any field empty when you edit !");
            }
            else
            {
                for (int i = 0; i < newContactsList.Count; i++)
                {
                    if (newContactsList[i].getName() == this.editName && newContactsList[i].getEmail() == this.editEmail && newContactsList[i].getNumber() == this.editNumber)
                    {
                        newContactsList.Remove(newContactsList[i]);
                        Contacts newContact = new Contacts(EditName.Text, EditNumber.Text, EditEmail.Text);

                        newContactsList.Add(newContact);
                    }
                }

                File.WriteAllText(this.form1.getFileName(), String.Empty);

                foreach (Contacts contact in newContactsList)
                {

                    string nameConc = contact.getName();
                    string emailConc = contact.getEmail();
                    string numberConc = contact.getNumber();

                    string wholeLine = "Name:" + nameConc + ",Email:" + emailConc + ",Number:" + numberConc;

                    using (StreamWriter sw = File.AppendText(this.form1.getFileName()))
                    {
                        sw.WriteLine(wholeLine);
                    }
                }

                this.form1.setContacts(newContactsList);

                this.Close();
            }
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
