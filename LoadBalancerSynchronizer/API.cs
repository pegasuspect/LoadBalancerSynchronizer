using Cinar.Database;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadBalancerSynchronizer
{
    public class API
    {
        private List<string> _serverRoots;
        public List<string> ServerRoots
        {
            get
            {
                if (_serverRoots == null)
                {
                    _serverRoots = new List<string>();
                }

                return _serverRoots;
            }
            set
            {
                _serverRoots = value;
            }
        }

        private List<Tuple<string, string>> _clones;
        public List<Tuple<string, string>> CloneServers
        {
            get
            {
                if (_clones == null)
                    _clones = new List<Tuple<string, string>>();

                return _clones;
            }
            set
            {
                _clones = value;
            }
        }


        public Tuple<string, string> MainServer { get; set; }

        public Tuple<string, string> ConnectionString { get; set; }

        public DatabaseProvider ConnectionType { get; set; }

        public void Save()
        {
            FileSerializer.Save(this);
        }

    }
}
