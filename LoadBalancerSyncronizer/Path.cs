using Cinar.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadBalancerSyncronizer
{
    public class SyncronizedFilePath : DatabaseEntity
    {
        public bool isSynced { get; set; }
        public string path { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string ErrorMessage { get; set; }

        public void Save() {
            Provider.Database.Save(this);
        }
    }
}