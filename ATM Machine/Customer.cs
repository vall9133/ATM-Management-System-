using MySql.Data.MySqlClient;
using System;

namespace ATM_Machine
{
    class Customer
    {
       
        private int customerID;
        private String password;
        private int v;

        public Customer(int v)
        {
            this.v = v;
        }

        public Customer(int id,String pin)
        {
            
            this.customerID = id;
            this.password = pin;
        }
        public int getID()
        {
            return customerID;
        }

        public bool Login()
        {
            bool result = false;
            string connStr = "server=csitmariadb.eku.edu;user=student;database=csc340_db;port=3306;password=Maroon@21?";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM vallabhaneni_customer WHERE customerID=@customerID AND password=@password";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@customerID",customerID);
                    cmd.Parameters.AddWithValue("@password",password);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());

                    if (count > 0)
                    {
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return result;
        }
    }
}
