using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AzureStorage
{
    public class Employee
    {
        public Dictionary<string, object> properties = new Dictionary<string, object>();
        public object this[string name]
        {
            get => ContainsProperty(name) ? properties[name] : null;
            set => properties[name] = value;
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UID { get; set; }

        public string EmailID { get; set; }

        [Range(1,100)]
        public int Age { get; set; }
        public bool ContainsProperty (string name)=> properties.ContainsKey(name);
        public ICollection<string> PropertyNames => properties.Keys;

    }
}
