using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Packfortune.Logic.Models
{
    public class Crate
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
    }
}
