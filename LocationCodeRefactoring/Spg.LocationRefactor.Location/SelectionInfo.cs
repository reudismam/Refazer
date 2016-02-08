using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spg.ExampleRefactoring.Bean;
using Spg.LocationRefactor.TextRegion;

namespace ExampleRefactoring.Spg.ExampleRefactoring.Bean
{
    public class SelectionInfo
    {
        public string Id { get; set; }

        public List<TRegion> Regions { get; set; }

        public SelectionInfo(string id, List<TRegion> regions)
        {
            this.Id = id;
            this.Regions = regions;
        }
        public override string ToString()
        {
            return Id.ToString();
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if(!(obj is SelectionInfo)) return false;

            SelectionInfo info = (SelectionInfo) obj;

            return Id.Equals(info.Id) && Regions.SequenceEqual(info.Regions);
        }
    }
}
