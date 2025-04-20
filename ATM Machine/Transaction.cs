using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace ATM_Machine
{

    class Transaction
    {
        int transactionNum;
        int accountNum;
        string transactionType;
        double amount;
        int fromAccount;
        int toAccount;
        DateTime date;

        public Transaction(int accNum, string transTy, double amountMoney, int fromAcc, int toAcc)
        {
            accountNum = accNum;
            transactionType = transTy;
            amount = amountMoney;
            fromAccount = fromAcc;
            toAccount = toAcc;
            transactionNum = -1;
        }

        public int getTransNum()
        {
            return transactionNum;
        }

        public double getAmount()
        {
            return amount;
        }
        public void saveTransaction()
        {
            string connStr = "server=csitmariadb.eku.edu;user=student;database=csc340_db;port=3306;password=Maroon@21?";
            MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(connStr);
            try
            {
                Console.WriteLine("Connecting to MySQL...");
                conn.Open();
                //string sql = "SELECT MAX(TransNum) FROM changTransaction WHERE accountNum=@num";
                string sql = "SELECT MAX(transNum) FROM vallabhaneni_transaction;";
                MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn);
                //cmd.Parameters.AddWithValue("@num", accountNum);
                MySqlDataReader myReader = cmd.ExecuteReader();
                if (myReader.Read())
                {
                    transactionNum = Int32.Parse(myReader[0].ToString());
                    Console.WriteLine("newTrans number" + transactionNum);
                }
                myReader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            conn.Close();
            Console.WriteLine("Done.");

            if (transactionNum == -1)
            {
                Console.WriteLine("Error:  Cannot finad and assign a new transaction number.");
            }
            else
            {
                transactionNum = transactionNum + 1;
                connStr = "server=csitmariadb.eku.edu;user=student;database=csc340_db;port=3306;password=Maroon@21?";
                conn = new MySql.Data.MySqlClient.MySqlConnection(connStr);
                try
                {
                    Console.WriteLine("Connecting to MySQL...");
                    conn.Open();
                    string sql = "INSERT INTO vallabhaneni_transaction (transactionNum, accountNum, transactionType, amount, fromAccount, toAccount, dateTime)" +
                        " VALUES (@tNum, @aNum, @tType, @amo, @fAcc, @tAcc, @dT)";
                    MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@tNum", transactionNum);
                    cmd.Parameters.AddWithValue("@aNum", accountNum);
                    cmd.Parameters.AddWithValue("@tType", transactionType);
                    cmd.Parameters.AddWithValue("@amo", amount);
                    cmd.Parameters.AddWithValue("@fAcc", fromAccount);
                    cmd.Parameters.AddWithValue("@tAcc", toAccount);
                    date = DateTime.Now;
                    cmd.Parameters.AddWithValue("@dT", date);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                conn.Close();
                Console.WriteLine("Done.");
            }
        }

    }
}
