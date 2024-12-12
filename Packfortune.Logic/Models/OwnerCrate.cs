using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packfortune.Logic.Models
{
    public class OwnerCrate
    {
        public int Id { get; set; }
        public string SteamId { get; set; }
        public int CrateId { get; set; }
    }
}
