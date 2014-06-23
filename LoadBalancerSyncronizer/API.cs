using Cinar.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadBalancerSyncronizer
{
    public class API
    {
        private string[] _clones;
        public string[] cloneServers
        {
            get
            {
                if (_clones == null
                        || string.IsNullOrEmpty(_clones[0])
                        || string.IsNullOrEmpty(_clones[1])
                        || string.IsNullOrEmpty(_clones[2])
                    )
                    return _clones = new string[]{
                            "C:\\Users\\student\\Desktop\\Server1",
                            "C:\\Users\\student\\Desktop\\Server2",
                            "C:\\Users\\student\\Desktop\\Server3"
                        };
                else
                {
                    return _clones;
                }
            }
            set {
                _clones = value;
            }
        }

        private string _mainServer;
        public string mainServer
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_mainServer))
                {
                    return _mainServer = "C:\\Users\\student\\Desktop\\MainServer";
                }
                else
                {
                    return _mainServer;
                }
            }
            set {
                _mainServer = value;
            }
        }

        private string _connectionString;
        public string ConnectionString
        {
            get
            {
                if (_connectionString == null)
                {
                    return "Server=localhost;Database=cinarcms;Uid=root;Pwd=;old syntax=yes;charset=utf8";
                }
                return _connectionString;
            }
            set
            {
                _connectionString = value;
            }
        }

        private DatabaseProvider _connType;
        public DatabaseProvider ConnectionType
        {
            get
            {
                return _connType;
            }
            set { _connType = value; }
        }

        public void Save(){
            FileSerializer.Save(this);
        }

    }
}
