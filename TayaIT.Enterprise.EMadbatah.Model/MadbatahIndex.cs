using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TayaIT.Enterprise.EMadbatah.Model
{
    public class MadbatahIndexItem: IComparable
    {
        public MadbatahIndexItem(long _id , string _n, string _p, bool _isMain, string _qFrom, string _qTo, bool? isCustom, bool _isMerged)
        {
            ID = _id;
            Name = _n;
            PageNum = _p;
            IsMainItem = _isMain;
            QFrom = _qFrom;
            QTo = _qTo;
            IsCustom = isCustom;
            IsGroupSubAgendaItems = _isMerged;
        }
        public int CompareTo(object obj)
        {
            MadbatahIndexItem it1 = (MadbatahIndexItem)obj;
            if (this.ID == it1.ID)
                return 0;
            else
                if (this.ID > it1.ID)
                    return 1;
                else
                    return -1;
        }
        public override bool Equals(object other)
        {
            MadbatahIndexItem it1 = (MadbatahIndexItem)other;
            if (it1.ID == this.ID )
                return true;
            else
                return false;
        }

        //public string notUsedName { get; set; }
        public string Name { get; set; }
        public string PageNum { get; set; }
        public bool IsMainItem { get; set; }
        public string QFrom { get; set; }
        public string QTo { get; set; }
        public long ID { get; set; }
        public bool? IsCustom { get; set; }
        public bool IsGroupSubAgendaItems { get; set; }
    }
}
