using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mp3.Comparers
{
    class PlaysComparer
    {
        private static int sortOrderModifier = 1;

        public PlaysComparer(SortOrder sortOrder)
        {
            if (sortOrder == SortOrder.Descending)
            {
                sortOrderModifier = -1;
            }
            else if (sortOrder == SortOrder.Ascending)
            {
                sortOrderModifier = 1;
            }
        }

        public int Compare(object x, object y)
        {
            DataGridViewRow row1 = (DataGridViewRow)x;
            DataGridViewRow row2 = (DataGridViewRow)y;

            int compareResult = String.Compare(row1.Cells[8].Value.ToString(), row2.Cells[8].Value.ToString());

            if (compareResult == 0)
            {
                compareResult = String.Compare(row1.Cells[1].Value.ToString(), row2.Cells[1].Value.ToString());
            }
            if (compareResult == 0)
            {
                compareResult = String.Compare(row1.Cells[3].Value.ToString(), row2.Cells[3].Value.ToString());
            }
            if (compareResult == 0)
            {
                compareResult = String.Compare(row1.Cells[4].Value.ToString(), row2.Cells[4].Value.ToString());
            }
            return compareResult * sortOrderModifier;
        }
    }
}
