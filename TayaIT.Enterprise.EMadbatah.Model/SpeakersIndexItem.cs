using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TayaIT.Enterprise.EMadbatah.Model
{
    public class SpeakersIndexItem : IComparable 
    {
        public string Name { get; set; }
        public string PageNum { get; set; }
        public int? attType;
        public SpeakersIndexItem(string _n, string _p, int? _attType)
        {
            Name = _n;
            PageNum = _p;
            attType = _attType;
        }

        public int CompareTo(Object obj)
        {
            if (((SpeakersIndexItem)obj).Name.Equals(Name))
                return 0;
            else
                return 1;
            // This may need to be otherIndex.CompareTo(thisIndex) depending on the direction of sort you want.
            //return thisIndex.CompareTo(otherIndex);
        }
        public override bool Equals(Object obj)
        {
            return ((SpeakersIndexItem)obj).Name == Name;
            //Employee e = obj as Employee;
            //return e._name == _name;
        }



    }
}
