using Cinar.Database;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoadBalancerSyncronizer
{
    public static class Provider
    {
        private static Database __db;

        public static Database Database
        {
            get
            {
                return GetDb();
            }
        }

        public static Database GetDb()
        {
            try
            {
                if (__db == null)
                {
                    Database db = new Database(Form1.DATA.ConnectionString.Item2, Form1.DATA.ConnectionType);
                    db.CreateTablesAutomatically = true;
                    __db = db;
                }
                return __db;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

    }
}
