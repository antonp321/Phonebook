using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Header;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Phonebook
{
    public partial class PhoneBook : Form
    {
        // This list collects all the conctacts(the data) and it is used so to not need enter everytime into the txt file to get the data.
        private List<Contacts> contacts;

        // This counter is following number of records in the contacts collection which refers to the data file
        private int counter;

        private string fileName;
        private List<string> searchCriterias;
        private string searchSelection;

        public PhoneBook()
        {
            InitializeComponent();
            this.contacts = new List<Contacts>();
            this.searchCriterias = new List<string>();
            this.counter = 0;
            this.fileName = "phonebook.txt";
            this.searchSelection = "name";
            dataGridView1.ReadOnly = true;
        }

        public List<Contacts> setContacts(List<Contacts> contacts)
        {
            return this.contacts = contacts;
        }

        public List<Contacts> getContacts()
        {
            return this.contacts;
        }

        public string getFileName()
        {
            return this.fileName;
        }

        // From this method we catch the "Add" buttons event and we take the record from the text fields from name, email and number and first
        // we are putting them into the database (txt) file and then into the contacts list and after that we are puting the row into the dataGrid 
        private void button3_Click(object sender, EventArgs e)
        {

            if (String.IsNullOrEmpty(Contact_Name.Text) || String.IsNullOrEmpty(Contact_Email.Text) || String.IsNullOrEmpty(Contact_Number.Text))
            {
                MessageBox.Show("Please Fill All The Contact's fields.");
            }
            else
            {
                try
                {
                    string name = Contact_Name.Text;
                    string email = Contact_Email.Text;
                    string number = Contact_Number.Text;

                    string wholeLine = "Name:" + name + ",Email:" + email + ",Number:" + number;
                    Contacts newContact = new Contacts(name, number, email);

                    contacts.Add(newContact);

                    using (StreamWriter sw = File.AppendText(this.fileName))
                    {
                        sw.WriteLine(wholeLine);
                    }

                    dataGridView1.Rows.Add(contacts[this.counter].getName(), contacts[this.counter].getEmail(), contacts[this.counter].getNumber());
                    this.counter++;

                    Contact_Name.Text = String.Empty;
                    Contact_Email.Text = String.Empty;
                    Contact_Number.Text = String.Empty;
                }

                //This exceptions checks if any of the text fields are not empty or if the file is not missing. If any of this do so, there are exceptions which are transformed into a
                // MessageBox that says to the user what he does wrong.
                catch(Exception ex)
                {
                    if (ex is IOException)
                    {
                        MessageBox.Show(ex.Message, "The file does not exist!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (ex is ArgumentNullException)
                    {
                        MessageBox.Show("Please Fill All The Contact's fields.");
                    }
                    else if (ex is ArgumentOutOfRangeException)
                    {
                        MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        //This methods just clears any text into the text fields (name, email, number).
        private void Cancel_Click(object sender, EventArgs e)
        {
            Contact_Name.Text = String.Empty;
            Contact_Email.Text = String.Empty;
            Contact_Number.Text = String.Empty;
        }

        // With this method we create new columns into our datagridview, as we pass as parameters the name of the column and the datagrid where we want to add this columns
        private void CreateNewColumn(string columnName, DataGridView dataGridView)
        {
            DataGridViewColumn column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = columnName;
            column.Name = columnName;
            dataGridView.Columns.Add(column);
        }

        // 1. Into this method we are creating the txt file if its not created already.
        // 2. Writing all the content into the contacts list.
        // 3. Creating the columns into the gridview with the method above.
        // 4. Getting all the data from the contacts list and rendering it into the datagrid.
        // 5. Adding the search types into the dropdown list for choosing search category.
        private void PhoneBook_Load(object sender, EventArgs e)
        {

            try
            {

                if (!File.Exists(this.fileName))
                {
                    FileStream fs = File.Create(this.fileName);
                    fs.Close();
                }

                    using (StreamReader sr = new StreamReader(this.fileName))
                    {
                        string line = sr.ReadLine();

                        while (line != null)
                        {
                            string[] fullContactInfoArr = line.Split(',');

                            string[] nameSplitArr = fullContactInfoArr[0].Split(':');
                            string name = nameSplitArr[1];

                            string[] emailSplitArr = fullContactInfoArr[1].Split(':');
                            string email = emailSplitArr[1];

                            string[] numberSplitArr = fullContactInfoArr[2].Split(':');
                            string number = numberSplitArr[1];

                            Contacts newContact = new Contacts(name, number, email);

                            contacts.Add(newContact);

                            line = sr.ReadLine();
                        }

                        CreateNewColumn("Name", dataGridView1);
                        CreateNewColumn("Email", dataGridView1);
                        CreateNewColumn("Number", dataGridView1);

                        for (var i = 0; i < dataGridView1.ColumnCount; i++)
                        {
                            dataGridView1.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        }

                        foreach (Contacts contact in contacts)
                        {
                            dataGridView1.Rows.Add(contact.getName(), contact.getEmail(), contact.getNumber());
                            this.counter++;
                        }
                    }

                    searchCriterias.Add("name");
                    searchCriterias.Add("email");
                    searchCriterias.Add("number");

                    this.comboBox1.DataSource = searchCriterias;
                    this.comboBox1.DisplayMember = "Name";
                    this.comboBox1.ValueMember = "Value";

                    this.comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
                }
            
            catch (Exception ex)
            {
                if (ex is IOException)
                {
                    MessageBox.Show(ex.Message, "The file does not exist!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (ex is ArgumentNullException)
                {
                    MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (ex is ArgumentOutOfRangeException)
                {
                    MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Here we delete all the gridview content and filling the gridview again by taking the data from the list again.
        private void reWriteListContains()
        {
            File.WriteAllText(this.fileName, String.Empty);

            foreach (Contacts contact in contacts)
            {

                string nameConc = contact.getName();
                string emailConc = contact.getEmail();
                string numberConc = contact.getNumber();

                string wholeLine = "Name:" + nameConc + ",Email:" + emailConc + ",Number:" + numberConc;

                using (StreamWriter sw = File.AppendText(this.fileName))
                {
                    sw.WriteLine(wholeLine);
                }
            }
        }


        // We delete the whole selected row. If there is no row selected we get messageBox that tells us that we should choose a row to delete.
        // This method can delete multiple selection.
        private void Delete_Click(object sender, EventArgs e)
        {

            bool isRowEmpty = isTheRowEmpty();

            if (isRowEmpty)
            {
                MessageBox.Show("Choose non empty to row to delete !");
            }
            else
            {
                string name = "";
                string email = "";
                string number = "";

                foreach (DataGridViewRow item in this.dataGridView1.SelectedRows)
                {
                    dataGridView1.Rows.RemoveAt(item.Index);

                    name = item.Cells[0].Value.ToString();
                    email = item.Cells[1].Value.ToString();
                    number = item.Cells[2].Value.ToString();
                }


                for (int i = 0; i < contacts.Count; i++)
                {
                    if (contacts[i].getName() == name && contacts[i].getEmail() == email && contacts[i].getNumber() == number)
                    {
                        contacts.RemoveAt(i);
                        this.counter--;
                    }

                }

                reWriteListContains();
            }

        }

        public DataGridViewRow getTheSelectedRow()
        {
            DataGridViewRow item1 = new DataGridViewRow();

            foreach (DataGridViewRow item in this.dataGridView1.SelectedRows)
            {
                item1 = item;
            }

            return item1;
        }

        private bool isTheRowEmpty()
        {
            bool rowIsEmpty = true;
            int cellsWithValuesCounter = 0;

            for (int i = 0; i < getTheSelectedRow().Cells.Count; i++)
            {
                if (getTheSelectedRow().Cells[i].Value != null)
                {
                    cellsWithValuesCounter++;
                }

                if (cellsWithValuesCounter == 3)
                {
                    rowIsEmpty = false;
                }
            }

            return rowIsEmpty;
        }

        private void Edit_Click(object sender, EventArgs e)
        {
            Form2 settingsForm = new Form2(this);

            int c = dataGridView1.SelectedRows.Count;

            bool rowIsEmpty = isTheRowEmpty();

            if (c > 0 && !rowIsEmpty)
            {
                settingsForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please select a non empty row to edit !");
            }

            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();

            foreach (Contacts contact in contacts)
            {
                dataGridView1.Rows.Add(contact.getName(), contact.getEmail(), contact.getNumber());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<Contacts> onlyMatchedContacts = new List<Contacts>();
            string searchValue = "";

            switch (this.searchSelection)
            {
                case "name":

                    searchValue = Search.Text;

                    for (int i = 0; i < this.contacts.Count; i++)
                    {
                        if (contacts[i].getName().ToLower().Contains(searchValue.ToLower()))
                        {
                            onlyMatchedContacts.Add(contacts[i]);
                        }
                    }

                    dataGridView1.Rows.Clear();
                    dataGridView1.Refresh();

                    foreach (Contacts contact in onlyMatchedContacts)
                    {
                        dataGridView1.Rows.Add(contact.getName(), contact.getEmail(), contact.getNumber());
                    }

                    searchValue = "";
                    onlyMatchedContacts.Clear();

                    break;
                case "email":

                    searchValue = Search.Text;

                    for (int i = 0; i < this.contacts.Count; i++)
                    {
                        if (contacts[i].getEmail().ToLower().Contains(searchValue.ToLower()))
                        {
                            onlyMatchedContacts.Add(contacts[i]);
                        }
                    }

                    dataGridView1.Rows.Clear();
                    dataGridView1.Refresh();

                    foreach (Contacts contact in onlyMatchedContacts)
                    {
                        dataGridView1.Rows.Add(contact.getName(), contact.getEmail(), contact.getNumber());
                    }

                    searchValue = "";
                    onlyMatchedContacts.Clear();

                    break;
                case "number":

                    searchValue = Search.Text;

                    for (int i = 0; i < this.contacts.Count; i++)
                    {
                        if (contacts[i].getNumber().ToLower().Contains(searchValue.ToLower()))
                        {
                            onlyMatchedContacts.Add(contacts[i]);
                        }
                    }

                    dataGridView1.Rows.Clear();
                    dataGridView1.Refresh();

                    foreach (Contacts contact in onlyMatchedContacts)
                    {
                        dataGridView1.Rows.Add(contact.getName(), contact.getEmail(), contact.getNumber());
                    }

                    searchValue = "";
                    onlyMatchedContacts.Clear();

                    break;
            }
        }

        private void Sort_Click(object sender, EventArgs e)
        {
            List<Contacts> sortedList = this.contacts.OrderBy(o => o.getName()).ToList();

            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();

            foreach (Contacts contact in sortedList)
            {
                dataGridView1.Rows.Add(contact.getName(), contact.getEmail(), contact.getNumber());
            }

            this.contacts = sortedList;

            reWriteListContains();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.searchSelection = comboBox1.Text;
        }
    }
}
