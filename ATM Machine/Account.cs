using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace ATM_Machine
{

    class Account
    {
        int accountNum;
        int customerID;
        double balance;
        double dailyDepositAmount;
        double dailyTransactionTotal;

        double dailyTransactionLimit = 3000.0;
        double dailyDepositLimit = 3000.0;
        double dailyTransferLimit = 3000.0;
        string dailyTransactionDate;
        Transaction newTransaction;

        public Transaction getNewTransaction()
        {
            return newTransaction;
        }

       
        public int withdrawMoney(double amount, double machineCash)
        {
            updateDailyTransaction();
            if (checkDailyTransaction() == false)
                return 1;
            if (verifyDailyTransaction(amount) == false)
                return 2;
            if (verifyAccountBalance(amount) == false)
                return 3;
            if (checkMachineCash(amount, machineCash) == false)
                return 4;
            updateBalance(amount);
            updateDailyTransactionTotal(amount);
            updateAccountData();
            newTransaction = new Transaction(accountNum, "Withdraw", amount, -1, -1);
            newTransaction.saveTransaction();
            return 0;
        }
        public int transferMoney(Account destinationAccount, double amount)
        {
            updateDailyTransaction();
            if (checkDailyTransaction() == false)
                return 1;
            if (verifyDailyTransaction(amount) == false)
                return 2;
            if (verifyAccountBalance(amount) == false)
                return 3;
            updateBalance(amount);
            updateDailyTransactionTotal(amount);
            updateAccountData();
            newTransaction = new Transaction(accountNum, "Transfer", amount, -1, -1);
            newTransaction.saveTransaction();
            return 0;
        }
        public int depositMoney(double amount)
        {
            updateDailyTransaction();
            if (verifyDailyDeposit(amount) == false)
                return 1;

            updateDepBalance(amount);
            updateDailyTransactionTotal(amount);
            updateAccountData();
            newTransaction = new Transaction(accountNum, "Deposit", amount, -1, -1);
            newTransaction.saveTransaction();
            return 0;
        }
        private void updateDepBalance(double amount)
        {
            dailyDepositAmount=dailyDepositAmount+amount;
            balance = balance +amount;
        }
        

        private bool verifyDailyDeposit(double amount)
        {
            if ((dailyDepositAmount + amount) > dailyDepositLimit)
                return false;
            else
                return true;
        }
        

        private bool verifyDailyTransfer(double amount)
        {
            // Check if the transfer amount exceeds the daily transfer limit
            if ((dailyTransactionTotal + amount) > dailyTransferLimit)
            {
                return false;
            }
            else
            {
                return true;
            }
        }




        private void updateAccountData()
        {
            string connStr = "server=csitmariadb.eku.edu;user=student;database=csc340_db;port=3306;password=Maroon@21?";
            MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(connStr);
            try
            {
                Console.WriteLine("Connecting to MySQL...");
                conn.Open();
                //string sql = "INSERT INTO changstudent (id, name) VALUES (@uid, @uname)";
                string sql = "UPDATE vallabhaneni_account SET dailyTransactionDate=@date, dailyTransactionTotal=@total, balance=@newBalance WHERE accountNum=@accNum";
                MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@date", dailyTransactionDate);
                cmd.Parameters.AddWithValue("@total", dailyTransactionTotal);
                cmd.Parameters.AddWithValue("@newBalance", balance);
                cmd.Parameters.AddWithValue("@accNum", accountNum);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            conn.Close();
            Console.WriteLine("Done.");

        }
        public double GetBalance()
        {
            return balance;
        }

        private void updateDailyTransactionTotal(double amount)
        {
            dailyTransactionTotal = dailyTransactionTotal + amount;
        }

        private void updateBalance(double amount)
        {
            balance = balance - amount;
        }
       

        private bool checkMachineCash(double amount, double machineCash)
        {
            if (amount > machineCash)
                return false;
            return true;
        }
        private bool verifyAccountBalance(double amount)
        {
            if (amount > balance)
                return false;
            return true;
        }
        private bool verifyDailyTransaction(double amount)
        {
            if ((dailyTransactionTotal + amount) > 3000.0)
                return false;
            else
                return true;
        }
        public bool checkDailyTransaction()
        {
            if (dailyTransactionTotal >= 3000.0)
                return false;
            return true;
        }
        private void updateDailyTransaction()
        {
            string todayDate = DateTime.Now.ToString("yyyy-MM-dd");
            Console.WriteLine("old date: " + dailyTransactionDate);
            Console.WriteLine("new date: " + todayDate);
            if (!dailyTransactionDate.Equals(todayDate))
            {
                dailyTransactionDate = todayDate;
                dailyTransactionTotal = 0.0;
                dailyDepositAmount = 0.0;
                Console.WriteLine("Date being changed.");
            }
        }
        public int getAccountNum()
        {
            return accountNum;
        }
        public static ArrayList retrieveAccounts(int id)
        {
            ArrayList accountList = new ArrayList();
            //ArrayList eventList = new ArrayList();  //a list to save the events
            //prepare an SQL query to retrieve all the events on the same, specified date
            DataTable myTable = new DataTable();
            string connStr = "server=csitmariadb.eku.edu;user=student;database=csc340_db;port=3306;password=Maroon@21?";
            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                Console.WriteLine("Connecting to MySQL...");
                conn.Open();
                string sql = "SELECT * FROM vallabhaneni_account WHERE customerID=49894 ORDER BY accountNum ASC";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("49894", id);
                MySqlDataAdapter myAdapter = new MySqlDataAdapter(cmd);
                myAdapter.Fill(myTable);
                Console.WriteLine("Table is ready.");

                // Debug output: Print the number of rows retrieved
                Console.WriteLine("Number of rows retrieved: " + myTable.Rows.Count);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            conn.Close();
            //convert the retrieved data to events and save them to the list
            foreach (DataRow row in myTable.Rows)
            {
                Account newAccount = new Account();

                newAccount.accountNum = Int32.Parse(row["accountNum"].ToString());
                newAccount.customerID = Int32.Parse(row["customerID"].ToString());
                newAccount.balance = Double.Parse(row["balance"].ToString());
                newAccount.dailyTransactionTotal = Double.Parse(row["dailyTransactionTotal"].ToString());
                newAccount.dailyTransactionDate = row["dailyTransactionDate"].ToString();
                newAccount.dailyDepositAmount = Double.Parse(row["dailyDepositAmount"].ToString());

                // Debug output: Print the account number for each account retrieved
                Console.WriteLine("Account number: " + newAccount.getAccountNum());

                accountList.Add(newAccount);
            }
            Console.WriteLine("*********" + accountList.Count);
            return accountList;  //return the event list
        }

    }
}
