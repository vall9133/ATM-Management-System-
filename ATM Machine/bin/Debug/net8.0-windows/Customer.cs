using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    class Customer
    {
        int id;

        public Customer(int num)
        {
            id = num;
        }

        public int getID()
        {
            return id;
        }
    }
}
