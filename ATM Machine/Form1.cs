using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using System.Security.Principal;

namespace ATM_Machine
{
    public partial class Form1 : Form
    {
        Customer currentCustomer = new Customer(1);
        ArrayList accountList = new ArrayList();
        Account selectedAccount;
        double machineCash = 100000;
        int withdrawCode = -1;
        int depositCode = -1;
        int dailydeposit = -1;
        int transferCode = -1;

        public Form1()
        {
            InitializeComponent();
            accountList = Account.retrieveAccounts(currentCustomer.getID());
            /*foreach (Account account in accountList)
        {
                listBox1.Items.Add(account.getAccountNum().ToString());  // Assuming getAccountNum() returns the account number
                Console.WriteLine("Account Number (Constructor): " + account.getAccountNum());
            }*/


        }
        private void button40_Click(object sender, EventArgs e)
        {
            int customerID = int.Parse(textBox3.Text);
            String password = textBox4.Text;

            Customer customer = new Customer(customerID, password);
            try
            {
                if (customer.Login())
                {
                    loginpanel.Visible = false;
                    Mainmenupanel.Visible = true;
                }
                else
                {
                    MessageBox.Show("id or pin is incorrect.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }
        //withdraw
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedAccount = (Account)accountList[listBox1.SelectedIndex];
            if (selectedAccount.checkDailyTransaction() == false)
            {
                withdrawCode = 1;
                label7.Text = "The transactions of this account have exceeded the max limit $3000 for today.\n"
                     + "Please select another account.";
                withdrawaccountpanel.Visible = false;
                wdamountgreaterpanel.Visible = true;
            }
            else
            {
                textBox1.Text = "0";
                withdrawaccountpanel.Visible = false;
                wdenterpanel.Visible = true;
            }

        }
        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

            selectedAccount = (Account)accountList[listBox2.SelectedIndex];
            if (selectedAccount.checkDailyTransaction() == false)
            {
                depositCode = 1;
                label11.Text = "The transactions of this account have exceeded the max limit $3000 for today.\n"
                     + "Please select another account.";
                Depositpanel.Visible = false;
                depsuccpanel.Visible = true;
            }
            else
            {
                textBox2.Text = "0";
                Depositpanel.Visible = false;
                dpenterpanel.Visible = true;

            }
        }
        //check balance
        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedAccount = (Account)accountList[listBox3.SelectedIndex];
            // Assuming you have an instance of the Account class named selectedAccount
            double balance = selectedAccount.GetBalance();

            label15.Text = "The Available Balance In Your Account is:\n"
                     + balance;
            checkbalanceaccountpanel.Visible = false;
            chebalapanel.Visible = true;
        }
        //checkbalance code
        private void button4_Click(object sender, EventArgs e)
        {
            /*label1.Text = "";
            listBox3.Items.Clear();
            listBox3.Items.Add("                                                           " + "123456789");
            listBox3.Items.Add("                                                           " + "135792468");
            listBox3.Items.Add("                                                           " + "246813579");
            listBox3.Items.Add("                                                           " + "987654321");
            Mainmenupanel.Visible = false;
            checkbalanceaccountpanel.Visible = true;*/
            listBox3.Items.Clear();
            Account tempAccount;
            Console.WriteLine("number of account: " + accountList.Count);
            for (int i = 0; i < accountList.Count; i++)
            {
                tempAccount = (Account)accountList[i];
                // Remove extra spaces before account number
                listBox3.Items.Add("                                              " + tempAccount.getAccountNum().ToString());
                Console.WriteLine("Account Number (button4_Click): " + tempAccount.getAccountNum());

            }
            Mainmenupanel.Visible = false;
            checkbalanceaccountpanel.Visible = true;
        }
        //withdraw code
        private void button2_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            Account tempAccount;
            Console.WriteLine("number of account: " + accountList.Count);
            for (int i = 0; i < accountList.Count; i++)
            {
                tempAccount = (Account)accountList[i];
                // Remove extra spaces before account number
                listBox1.Items.Add("                                              " + tempAccount.getAccountNum().ToString());
                Console.WriteLine("Account Number (button2_Click): " + tempAccount.getAccountNum());

            }
            Mainmenupanel.Visible = false;
            withdrawaccountpanel.Visible = true;
        }


        private void button20_Click_1(object sender, EventArgs e)
        {
            double amount = Double.Parse(textBox1.Text);
            withdrawCode = selectedAccount.withdrawMoney(amount, machineCash);
            if (withdrawCode == 0)
            {
                label7.Text = "Please take the money.\n Transaction number: "
                    + selectedAccount.getNewTransaction().getTransNum() + "\n" + "Withdrawal amount: $"
                    + selectedAccount.getNewTransaction().getAmount() + "\n" + "From account: "
                    + selectedAccount.getAccountNum();
            }
            else if (withdrawCode == 1)
            {
                label7.Text = "The transactions of this account have exceeded the max limit $3000 for today.\n"
                    + "Please select another account.";
            }
            else if (withdrawCode == 2)
            {
                label7.Text = "The amount will make the transactions of this account exceed the max limit $3000 for today.\n"
                    + "Please enter a smaller amount.";
            }
            else if (withdrawCode == 3)
            {
                label7.Text = "The amount you entered is greater than the balance of the selected account.\n"
                    + "Please enter a smaller amount.";
            }
            else if (withdrawCode == 4)
            {
                label7.Text = "The machince doesn't have enough cash for your withdrawal.\n"
                    + "Please enter a smaller amount.";
            }

            wdenterpanel.Visible = false;
            wdamountgreaterpanel.Visible = true;
        }
        private void button37_Click(object sender, EventArgs e)
        {
            wdamountgreaterpanel.Visible = false;
            if (withdrawCode == 1)
                withdrawaccountpanel.Visible = true;
            else if (withdrawCode == 2)
                wdenterpanel.Visible = true;
            else if (withdrawCode == 3)
                wdenterpanel.Visible = true;
            else if (withdrawCode == 4)
                wdenterpanel.Visible = true;
            else
                Mainmenupanel.Visible = true;
            withdrawCode = -1;
        }
        private void button30_Click_1(object sender, EventArgs e)
        {
            double amount = Double.Parse(textBox2.Text);
            depositCode = selectedAccount.depositMoney(amount);
            if (depositCode == 0)
            {
                label11.Text = "The amount has been Deposited Successfully"
                    + selectedAccount.getNewTransaction().getTransNum() + "\n" + "Withdrawal amount: $"
                    + selectedAccount.getNewTransaction().getAmount() + "\n" + "From account: "
                    + selectedAccount.getAccountNum();
                depsuccpanel.Visible = true;
                dpenterpanel.Visible = false;
            }
            else if (depositCode == 1)
            {
                label11.Text = " The amount you entered is greater than the maximum limit\n"
                    + "Please enter smaller amount;";
                dpenterpanel.Visible = false;
                depsuccpanel.Visible = true;
            }
            
        }
        private void button43_Click(object sender, EventArgs e)
        {
            depsuccpanel.Visible = false;
            Mainmenupanel.Visible = true;
        }


        private void button3_Click(object sender, EventArgs e)
        {

            /*label1.Text = "";
            listBox2.Items.Clear();
            listBox2.Items.Add("                                                                       " + "123456789");
            listBox2.Items.Add("                                                                       " + "135792468");
            listBox2.Items.Add("                                                                       " + "246813579");
            listBox2.Items.Add("                                                                       " + "987654321");*/
            listBox2.Items.Clear();
            Account tempAccount;
            Console.WriteLine("number of account: " + accountList.Count);
            for (int i = 0; i < accountList.Count; i++)
            {
                tempAccount = (Account)accountList[i];
                // Remove extra spaces before account number
                listBox2.Items.Add("                                              " + tempAccount.getAccountNum().ToString());
                Console.WriteLine("Account Number (button_Click): " + tempAccount.getAccountNum());

            }
            Mainmenupanel.Visible = false;
            Depositpanel.Visible = true;
        }
        //trasfer source paNEL
        private void button5_Click(object sender, EventArgs e)
        {
            /*label1.Text = "";
            listBox4.Items.Clear();
            listBox4.Items.Add("                                                               " + "123456789");
            listBox4.Items.Add("                                                               " + "135792468");
            listBox4.Items.Add("                                                               " + "246813579");
            listBox4.Items.Add("                                                               " + "987654321");*/
            listBox4.Items.Clear();
            Account tempAccount;
            Console.WriteLine("number of account: " + accountList.Count);
            for (int i = 0; i < accountList.Count; i++)
            {
                tempAccount = (Account)accountList[i];
                // Remove extra spaces before account number
                listBox4.Items.Add("                                              " + tempAccount.getAccountNum().ToString());
                Console.WriteLine("Account Number (button_Click): " + tempAccount.getAccountNum());

            }
           
            Mainmenupanel.Visible = false;
            sourcepanel.Visible = true;
        }
        //destination
        private void listBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox5.Items.Clear(); // Clear existing items before populating with new ones

            // Get the selected source account from listBox4
            selectedAccount = (Account)accountList[listBox4.SelectedIndex];

            // Check if the selected account has exceeded the daily transaction limit
            if (selectedAccount.checkDailyTransaction() == false)
            {
                transferCode = 1;
                label21.Text = "The transactions of this account have exceeded the max limit $3000 for today.\n"
                    + "Please select another account.";
                sourcepanel.Visible = false;
                trangreterpanel.Visible = true;
            }
            else
            {
                // Populate listBox5 with destination account numbers
                foreach (Account tempAccount in accountList)
                {
                    if (tempAccount != selectedAccount) // Exclude the source account from destination options
                    {
                        listBox5.Items.Add("                                              " + tempAccount.getAccountNum().ToString());
                        Console.WriteLine("Account Number: " + tempAccount.getAccountNum());
                    }
                }

                sourcepanel.Visible = false;
                destinationaccountpanel.Visible = true;
            }
        }
        
            private void button62_Click_1(object sender, EventArgs e)
            {
                double amount = Double.Parse(textBox5.Text);

                // Get the selected destination account from listBox5
                Account destinationAccount = (Account)accountList[listBox5.SelectedIndex];

                // Transfer money from the selected source account to the destination account
                transferCode = selectedAccount.transferMoney(destinationAccount, amount);

                // Handle transfer result
                if (transferCode == 0)
                {
                    label21.Text = "The amount has been transferred successfully."
                        + "\nTransfer amount: $" + amount
                        + "\nFrom account: " + selectedAccount.getAccountNum()
                        + "\nTo account: " + destinationAccount.getAccountNum(); // Corrected line
                    trangreterpanel.Visible = true;
                    tranenterpanel.Visible = false;
                }
                else if (transferCode == 1)
                {
                    label21.Text = "The amount you entered exceeds the daily transfer limit.";
                }
                else if (transferCode == 2)
                {
                    label21.Text = "The amount will make the transactions of this account exceed the max limit $3000 for today.\n"
                        + "Please enter a smaller amount.";
                }
                else if (transferCode == 3)
                {
                    label21.Text = "Insufficient balance in the source account.";
                }
                else
                {
                    label21.Text = "An error occurred during the transfer process.";
                }
            }
        

            private void button64_Click(object sender, EventArgs e)
        {
           /* trangreterpanel.Visible = false;
            tranenterpanel.Visible = true;*/

            trangreterpanel.Visible = false;
            if (transferCode == 1)
                sourcepanel.Visible = true;
            else if (transferCode == 2)
                tranenterpanel.Visible = true;
            else if (transferCode == 3)
                tranenterpanel.Visible = true;
            else
                Mainmenupanel.Visible = true;
            transferCode = -1;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            //System.Windows.Forms.Application.Exit();
            loginpanel.Visible = true;
            Mainmenupanel.Visible = false;
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }



       


        private void button6_Click(object sender, EventArgs e)
        {
            Mainmenupanel.Visible = true;
            withdrawaccountpanel.Visible = false;

        }



        private void button6_Click_1(object sender, EventArgs e)
        {

            Mainmenupanel.Visible = true;
            withdrawaccountpanel.Visible = false;



        }



        private void tableLayoutPanel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            withdrawaccountpanel.Visible = true;
            wdenterpanel.Visible = false;
        }

        private void button8_Click(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "0")
                textBox1.Text = "4";
            else if (textBox1.TextLength <= 3)
                textBox1.Text = textBox1.Text + "4";

        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "0")
                textBox1.Text = "0";
            else if (textBox1.TextLength <= 3)
                textBox1.Text = textBox1.Text + "0";
        }

        private void button13_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "0")
                textBox1.Text = "1";
            else if (textBox1.TextLength <= 3)
                textBox1.Text = textBox1.Text + "1";
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "0")
                textBox1.Text = "2";
            else if (textBox1.TextLength <= 3)
                textBox1.Text = textBox1.Text + "2";
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "0")
                textBox1.Text = "3";
            else if (textBox1.TextLength <= 3)
                textBox1.Text = textBox1.Text + "3";
        }

        private void button14_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "0")
                textBox1.Text = "5";
            else if (textBox1.TextLength <= 3)
                textBox1.Text = textBox1.Text + "5";
        }

        private void button15_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "0")
                textBox1.Text = "6";
            else if (textBox1.TextLength <= 3)
                textBox1.Text = textBox1.Text + "6";
        }

        private void button16_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "0")
                textBox1.Text = "7";
            else if (textBox1.TextLength <= 3)
                textBox1.Text = textBox1.Text + "7";
        }

        private void button17_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "0")
                textBox1.Text = "8";
            else if (textBox1.TextLength <= 3)
                textBox1.Text = textBox1.Text + "8";
        }

        private void button18_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "0")
                textBox1.Text = "9";
            else if (textBox1.TextLength <= 3)
                textBox1.Text = textBox1.Text + "9";
        }

        private void button19_Click(object sender, EventArgs e)
        {
            if (textBox1.TextLength == 1)
                textBox1.Text = "0";
            else
                textBox1.Text = textBox1.Text.Substring(0, textBox1.TextLength - 1);
        }

        private void button8_Click_1(object sender, EventArgs e)
        {
            textBox1.Text = "0";
        }

        private void button20_Click(object sender, EventArgs e)
        {

            if (textBox1.Text != "0" && textBox1.TextLength <= 3)
                textBox1.Text = textBox1.Text + "0";
            // accountpanel.Visible = false;
            // wdlimitpanel.Visible = true; //limit exceeded

            //wdamountgreaterpanel.Visible = true;
            wdenterpanel.Visible = false;
            wdtakemoneypanel.Visible = true;

        }


        private void button21_Click(object sender, EventArgs e)
        {
            Mainmenupanel.Visible = true;
            Depositpanel.Visible = false;

        }

       

        private void depositaccountpanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button23_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "0" && textBox1.TextLength <= 3)
                textBox1.Text = textBox1.Text + "0";
            dpenterpanel.Visible = false;
            //deplimitpanel.Visible = true;
            depsuccpanel.Visible = true;
        }

        private void button22_Click(object sender, EventArgs e)
        {
            dpenterpanel.Visible = false;
            Depositpanel.Visible = true;


        }

        private void button36_Click(object sender, EventArgs e)
        {
            wdlimitpanel.Visible = false;
            wdenterpanel.Visible = true;
        }



        private void button38_Click(object sender, EventArgs e)
        {
            wdnocashpanel.Visible = false;
            wdenterpanel.Visible = true;
        }

        private void button39_Click(object sender, EventArgs e)
        {
            wdtakemoneypanel.Visible = false;
            wdenterpanel.Visible = true;

        }

        private void button41_Click(object sender, EventArgs e)
        {
            //loginpanel.Visible = false;
            Mainmenupanel.Visible = true;
        }


        private void button41_Click_1(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void loginpanel_Paint(object sender, PaintEventArgs e)
        {

        }

        /*private void button31_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == "0")
                textBox2.Text = textBox2.Text + "0";

            else if (textBox2.TextLength <= 3)
                textBox2.Text = textBox2.Text + "0";

        }

        private void button30_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == "0")
                textBox2.Text = "1";
            else if (textBox2.TextLength <= 3)
                textBox2.Text = textBox2.Text + "1";
        }*/

        private void button29_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == "0")
                textBox2.Text = "2";
            else if (textBox2.TextLength <= 3)
                textBox2.Text = textBox2.Text + "2";
        }

        private void button28_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == "0")
                textBox2.Text = "3";
            else if (textBox2.TextLength <= 3)
                textBox2.Text = textBox2.Text + "3";
        }

        private void button27_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == "0")
                textBox2.Text = "4";
            else if (textBox2.TextLength <= 3)
                textBox2.Text = textBox2.Text + "4";
        }

        private void button26_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == "0")
                textBox2.Text = "5";
            else if (textBox2.TextLength <= 3)
                textBox2.Text = textBox2.Text + "5";
        }

        private void button32_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == "0")
                textBox2.Text = "6";
            else if (textBox2.TextLength <= 3)
                textBox2.Text = textBox2.Text + "6";
        }

        private void button33_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == "0")
                textBox2.Text = "7";
            else if (textBox2.TextLength <= 3)
                textBox2.Text = textBox2.Text + "7";
        }

        private void button34_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == "0")
                textBox2.Text = "8";
            else if (textBox2.TextLength <= 3)
                textBox2.Text = textBox2.Text + "8";

        }

        private void button35_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == "0")
                textBox2.Text = "9";
            else if (textBox2.TextLength <= 3)
                textBox2.Text = textBox2.Text + "9";
        }

        private void button24_Click(object sender, EventArgs e)
        {
            textBox2.Text = "0";
        }

        private void button25_Click(object sender, EventArgs e)
        {

            if (textBox2.TextLength == 1)
                textBox2.Text = "0";
            else
                textBox2.Text = textBox2.Text.Substring(0, textBox2.TextLength - 1);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button42_Click(object sender, EventArgs e)
        {
            deplimitpanel.Visible = false;
            dpenterpanel.Visible = true;
        }

        




        private void button44_Click(object sender, EventArgs e)
        {
            checkbalanceaccountpanel.Visible = false;
            Mainmenupanel.Visible = true;

        }

        private void button45_Click(object sender, EventArgs e)
        {
            chebalapanel.Visible = false;
            Mainmenupanel.Visible = true;
        }





        private void label16_Click(object sender, EventArgs e)
        {

        }

       

        private void button46_Click(object sender, EventArgs e)
        {
            sourcepanel.Visible = false;
            Mainmenupanel.Visible = true;
        }

      

        private void button47_Click(object sender, EventArgs e)
        {
            destinationaccountpanel.Visible = false;
            Mainmenupanel.Visible = true;
        }

        private void button48_Click(object sender, EventArgs e)
        {
            transsuccpanel.Visible = false;
            Mainmenupanel.Visible = true;
        }

        private void listBox5_SelectedIndexChanged(object sender, EventArgs e)
        {

            destinationaccountpanel.Visible = false;
            tranenterpanel.Visible = true;

        }

        private void button49_Click(object sender, EventArgs e)
        {
            tranenterpanel.Visible = false;
            Mainmenupanel.Visible = true;
        }

        private void button55_Click(object sender, EventArgs e)
        {
            if (textBox5.Text == "0")
                textBox5.Text = "0";
            else if (textBox5.TextLength <= 3)
                textBox5.Text = textBox5.Text + "0";

        }

        private void button54_Click(object sender, EventArgs e)
        {
            if (textBox5.Text == "0")
                textBox5.Text = "1";
            else if (textBox5.TextLength <= 3)
                textBox5.Text = textBox5.Text + "1";
        }

        private void button53_Click(object sender, EventArgs e)
        {
            if (textBox5.Text == "0")
                textBox5.Text = "2";
            else if (textBox5.TextLength <= 3)
                textBox5.Text = textBox5.Text + "2";
        }

        private void button56_Click(object sender, EventArgs e)
        {
            if (textBox5.Text == "0")
                textBox5.Text = "3";
            else if (textBox5.TextLength <= 3)
                textBox5.Text = textBox5.Text + "3";
        }

        private void button57_Click(object sender, EventArgs e)
        {
            if (textBox5.Text == "0")
                textBox5.Text = "4";
            else if (textBox5.TextLength <= 3)
                textBox5.Text = textBox5.Text + "4";
        }

        private void button58_Click(object sender, EventArgs e)
        {
            if (textBox5.Text == "0")
                textBox5.Text = "5";
            else if (textBox5.TextLength <= 3)
                textBox5.Text = textBox5.Text + "5";
        }

        private void button59_Click(object sender, EventArgs e)
        {
            if (textBox5.Text == "0")
                textBox5.Text = "6";
            else if (textBox5.TextLength <= 3)
                textBox5.Text = textBox5.Text + "6";
        }

        private void button60_Click(object sender, EventArgs e)
        {
            if (textBox5.Text == "0")
                textBox5.Text = "7";
            else if (textBox5.TextLength <= 3)
                textBox5.Text = textBox5.Text + "7";
        }

        private void button61_Click(object sender, EventArgs e)
        {
            if (textBox5.Text == "0")
                textBox5.Text = "8";
            else if (textBox5.TextLength <= 3)
                textBox5.Text = textBox5.Text + "8";
        }

        private void button62_Click(object sender, EventArgs e)
        {
            if (textBox5.Text == "0")
                textBox5.Text = "9";
            else if (textBox5.TextLength <= 3)
                textBox5.Text = textBox5.Text + "9";
        }

        private void button51_Click(object sender, EventArgs e)
        {
            textBox5.Text = "0";
        }

        private void button52_Click(object sender, EventArgs e)
        {
            if (textBox5.TextLength == 1)
                textBox5.Text = "0";
            else
                textBox5.Text = textBox5.Text.Substring(0, textBox5.TextLength - 1);
        }

        private void button63_Click(object sender, EventArgs e)
        {
            translimitpanel.Visible = false;
            Mainmenupanel.Visible = true;
        }

        private void button50_Click(object sender, EventArgs e)
        {
            tranenterpanel.Visible = false;
            //translimitpanel.Visible = true;
            //trangreterpanel.Visible = true;
            transsuccpanel.Visible = true;
        }

        

        private void button16_Click_1(object sender, EventArgs e)
        {
            if (textBox1.Text == "0")
                textBox1.Text = "7";
            else if (textBox1.TextLength <= 3)
                textBox1.Text = textBox1.Text + "7";
        }

        private void button14_Click_1(object sender, EventArgs e)
        {
            if (textBox1.Text == "0")
                textBox1.Text = "5";
            else if (textBox1.TextLength <= 3)
                textBox1.Text = textBox1.Text + "5";
        }

        private void button15_Click_1(object sender, EventArgs e)
        {
            if (textBox1.Text == "0")
                textBox1.Text = "6";
            else if (textBox1.TextLength <= 3)
                textBox1.Text = textBox1.Text + "6";
        }

        private void button17_Click_1(object sender, EventArgs e)
        {
            if (textBox1.Text == "0")
                textBox1.Text = "8";
            else if (textBox1.TextLength <= 3)
                textBox1.Text = textBox1.Text + "8";
        }

        private void button18_Click_1(object sender, EventArgs e)
        {
            if (textBox1.Text == "0")
                textBox1.Text = "9";
            else if (textBox1.TextLength <= 3)
                textBox1.Text = textBox1.Text + "9";
        }



        private void button31_Click_1(object sender, EventArgs e)
        {
            dpenterpanel.Visible = false;
            Depositpanel.Visible = true;
        }

       
        private void button27_Click_1(object sender, EventArgs e)
        {
            if (textBox2.Text == "0")
                textBox2.Text = "4";
            else if (textBox2.TextLength <= 3)
                textBox2.Text = textBox2.Text + "4";
        }

        private void button25_Click_1(object sender, EventArgs e)
        {

            if (textBox2.Text == "0")
                textBox2.Text = "3";
            else if (textBox2.TextLength <= 3)
                textBox2.Text = textBox2.Text + "3";
        }

        private void button24_Click_1(object sender, EventArgs e)
        {

            if (textBox2.Text == "0")
                textBox2.Text = "2";
            else if (textBox2.TextLength <= 3)
                textBox2.Text = textBox2.Text + "2";
        }

        private void button23_Click_1(object sender, EventArgs e)
        {

            if (textBox2.Text == "0")
                textBox2.Text = "1";
            else if (textBox2.TextLength <= 3)
                textBox2.Text = textBox2.Text + "1";
        }

        private void button22_Click_1(object sender, EventArgs e)
        {

            if (textBox2.Text == "0")
                textBox2.Text = "0";
            else if (textBox2.TextLength <= 3)
                textBox2.Text = textBox2.Text + "0";
            
        }

        private void button29_Click_1(object sender, EventArgs e)
        {
            textBox2.Text = "0";
        }

        private void button28_Click_1(object sender, EventArgs e)
        {
            if (textBox2.TextLength == 1)
                textBox2.Text = "0";
            else
                textBox2.Text = textBox2.Text.Substring(0, textBox2.TextLength - 1);

        }

        private void button50_Click_1(object sender, EventArgs e)
        {
            if (textBox5.Text == "0")
                textBox5.Text = "5";
            else if (textBox5.TextLength <= 3)
                textBox5.Text = textBox5.Text + "5";
        }

        private void button58_Click_1(object sender, EventArgs e)
        {
            if (textBox5.Text == "0")
                textBox5.Text = "6";
            else if (textBox5.TextLength <= 3)
                textBox5.Text = textBox5.Text + "6";
        }

        private void button59_Click_1(object sender, EventArgs e)
        {
            if (textBox5.Text == "0")
                textBox5.Text = "7";
            else if (textBox5.TextLength <= 3)
                textBox5.Text = textBox5.Text + "7";
        }

        private void button60_Click_1(object sender, EventArgs e)
        {
            if (textBox5.Text == "0")
                textBox5.Text = "8";
            else if (textBox5.TextLength <= 3)
                textBox5.Text = textBox5.Text + "8";
        }

        private void button61_Click_1(object sender, EventArgs e)
        {
            if (textBox5.Text == "0")
                textBox5.Text = "9";
            else if (textBox5.TextLength <= 3)
                textBox5.Text = textBox5.Text + "9";
        }

       

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void failurepanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }
    }
}


