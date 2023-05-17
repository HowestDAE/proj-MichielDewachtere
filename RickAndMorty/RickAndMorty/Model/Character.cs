using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RickAndMorty.Model
{
    public class Character
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string Species { get; set; }

        private string _type;
        public string Type
        {
            get
            {
                return _type;
            }
            set
            {
                if (value.Length != 0)
                {
                    _type = $"({Type})";
                }
                else
                {
                    _type = value;
                }
            }

        }
        public string Gender { get; set; }
        //public Origin Origin { get; set; }
        //public Location Location { get; set; }
        public string Image
        {
            get
            {
                return $"https://rickandmortyapi.com/api/character/avatar/{Id}.jpeg";
            }
        }
        //public List<Episode> Episodes {get; set; }
    }
}
