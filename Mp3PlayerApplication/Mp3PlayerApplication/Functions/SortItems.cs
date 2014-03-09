using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mp3.Containers;

namespace Mp3.Functions
{
    class SortItems
    {
        public static void columnSortClick(Mp3_Container container, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                if (!container.gui.sortOptions.nameAscending.Checked && !container.gui.sortOptions.nameDescending.Checked)
                {
                    container.gui.sortOptions.nameDescending.Checked = true;
                }
                if (container.gui.sortOptions.nameAscending.Checked)
                {
                    SortItems.nameDescending(container);
                }
                else
                {
                    SortItems.nameAscending(container);
                }
            }
            else if (e.ColumnIndex == 2)
            {
                if (!container.gui.sortOptions.lengthAscending.Checked && !container.gui.sortOptions.lengthDescending.Checked)
                {
                    container.gui.sortOptions.lengthDescending.Checked = true;
                }
                if (container.gui.sortOptions.lengthAscending.Checked)
                {
                    SortItems.lengthDescending(container);
                }
                else
                {
                    SortItems.lengthAscending(container);
                }
            }
            else if (e.ColumnIndex == 3)
            {
                if (!container.gui.sortOptions.artistAscending.Checked && !container.gui.sortOptions.artistDescending.Checked)
                {
                    container.gui.sortOptions.artistDescending.Checked = true;
                }
                if (container.gui.sortOptions.artistAscending.Checked)
                {
                    SortItems.artistDescending(container);
                }
                else
                {
                    SortItems.artistAscending(container);
                }
            }
            else if (e.ColumnIndex == 4)
            {
                if (!container.gui.sortOptions.albumAscending.Checked && !container.gui.sortOptions.albumDescending.Checked)
                {
                    container.gui.sortOptions.albumDescending.Checked = true;
                }
                if (container.gui.sortOptions.albumAscending.Checked)
                {
                    SortItems.albumDescending(container);
                }
                else
                {
                    SortItems.albumAscending(container);
                }
            }
            else if (e.ColumnIndex == 6)
            {
                if (!container.gui.sortOptions.yearAscending.Checked && !container.gui.sortOptions.yearDescending.Checked)
                {
                    container.gui.sortOptions.yearDescending.Checked = true;
                }
                if (container.gui.sortOptions.yearAscending.Checked)
                {
                    SortItems.yearDescending(container);
                }
                else
                {
                    SortItems.yearAscending(container);
                }
            }
            else if (e.ColumnIndex == 7)
            {
                if (!container.gui.sortOptions.genreAscending.Checked && !container.gui.sortOptions.genreDescending.Checked)
                {
                    container.gui.sortOptions.genreDescending.Checked = true;
                }
                if (container.gui.sortOptions.genreAscending.Checked)
                {
                    SortItems.genreDescending(container);
                }
                else
                {
                    SortItems.genreAscending(container);
                }
            }
            else if (e.ColumnIndex == 8)
            {
                if (!container.gui.sortOptions.playsAscending.Checked && !container.gui.sortOptions.playsDescending.Checked)
                {
                    container.gui.sortOptions.playsDescending.Checked = true;
                }
                if (container.gui.sortOptions.playsAscending.Checked)
                {
                    SortItems.playsDescending(container);
                }
                else
                {
                    SortItems.playsAscending(container);
                }
            }
            else if (e.ColumnIndex == 9)
            {
                if (!container.gui.sortOptions.ratingAscending.Checked && !container.gui.sortOptions.ratingDescending.Checked)
                {
                    container.gui.sortOptions.ratingDescending.Checked = true;
                }
                if (container.gui.sortOptions.ratingAscending.Checked)
                {
                    SortItems.ratingDescending(container);
                }
                else
                {
                    SortItems.ratingAscending(container);
                }
            }
        }

        public static void nameAscending(Mp3_Container container)
        {
            container.gui.sortOptions.nameAscending.Checked = true;
            
            container.gui.sortOptions.nameDescending.Checked = false;
            container.gui.sortOptions.lengthAscending.Checked = false;
            container.gui.sortOptions.lengthDescending.Checked = false;
            container.gui.sortOptions.artistAscending.Checked = false;
            container.gui.sortOptions.artistDescending.Checked = false;
            container.gui.sortOptions.albumAscending.Checked = false;
            container.gui.sortOptions.albumDescending.Checked = false;
            container.gui.sortOptions.yearAscending.Checked = false;
            container.gui.sortOptions.yearDescending.Checked = false;
            container.gui.sortOptions.genreAscending.Checked = false;
            container.gui.sortOptions.genreDescending.Checked = false;
            container.gui.sortOptions.playsAscending.Checked = false;
            container.gui.sortOptions.playsDescending.Checked = false;
            container.gui.sortOptions.ratingAscending.Checked = false;
            container.gui.sortOptions.ratingDescending.Checked = false;

            GuiControl.sort(container, "Name");
        }

        public static void nameDescending(Mp3_Container container)
        {
            container.gui.sortOptions.nameDescending.Checked = true;

            container.gui.sortOptions.nameAscending.Checked = false;
            container.gui.sortOptions.lengthAscending.Checked = false;
            container.gui.sortOptions.lengthDescending.Checked = false;
            container.gui.sortOptions.artistAscending.Checked = false;
            container.gui.sortOptions.artistDescending.Checked = false;
            container.gui.sortOptions.albumAscending.Checked = false;
            container.gui.sortOptions.albumDescending.Checked = false;
            container.gui.sortOptions.yearAscending.Checked = false;
            container.gui.sortOptions.yearDescending.Checked = false;
            container.gui.sortOptions.genreAscending.Checked = false;
            container.gui.sortOptions.genreDescending.Checked = false;
            container.gui.sortOptions.playsAscending.Checked = false;
            container.gui.sortOptions.playsDescending.Checked = false;
            container.gui.sortOptions.ratingAscending.Checked = false;
            container.gui.sortOptions.ratingDescending.Checked = false;

            GuiControl.sort(container, "Name");
        }

        public static void lengthAscending(Mp3_Container container)
        {
            container.gui.sortOptions.lengthAscending.Checked = true;
            
            container.gui.sortOptions.nameAscending.Checked = false;
            container.gui.sortOptions.nameDescending.Checked = false;
            container.gui.sortOptions.lengthDescending.Checked = false;
            container.gui.sortOptions.artistAscending.Checked = false;
            container.gui.sortOptions.artistDescending.Checked = false;
            container.gui.sortOptions.albumAscending.Checked = false;
            container.gui.sortOptions.albumDescending.Checked = false;
            container.gui.sortOptions.yearAscending.Checked = false;
            container.gui.sortOptions.yearDescending.Checked = false;
            container.gui.sortOptions.genreAscending.Checked = false;
            container.gui.sortOptions.genreDescending.Checked = false;
            container.gui.sortOptions.playsAscending.Checked = false;
            container.gui.sortOptions.playsDescending.Checked = false;
            container.gui.sortOptions.ratingAscending.Checked = false;
            container.gui.sortOptions.ratingDescending.Checked = false;

            GuiControl.sort(container, "Length");
        }

        public static void lengthDescending(Mp3_Container container)
        {
            container.gui.sortOptions.lengthDescending.Checked = true;
           
            container.gui.sortOptions.nameAscending.Checked = false;
            container.gui.sortOptions.nameDescending.Checked = false;
            container.gui.sortOptions.lengthAscending.Checked = false;
            container.gui.sortOptions.artistAscending.Checked = false;
            container.gui.sortOptions.artistDescending.Checked = false;
            container.gui.sortOptions.albumAscending.Checked = false;
            container.gui.sortOptions.albumDescending.Checked = false;
            container.gui.sortOptions.yearAscending.Checked = false;
            container.gui.sortOptions.yearDescending.Checked = false;
            container.gui.sortOptions.genreAscending.Checked = false;
            container.gui.sortOptions.genreDescending.Checked = false;
            container.gui.sortOptions.playsAscending.Checked = false;
            container.gui.sortOptions.playsDescending.Checked = false;
            container.gui.sortOptions.ratingAscending.Checked = false;
            container.gui.sortOptions.ratingDescending.Checked = false;

            GuiControl.sort(container, "Length");
        }

        public static void artistAscending(Mp3_Container container)
        {
            container.gui.sortOptions.artistAscending.Checked = true;
            
            container.gui.sortOptions.nameAscending.Checked = false;
            container.gui.sortOptions.nameDescending.Checked = false;
            container.gui.sortOptions.lengthAscending.Checked = false;
            container.gui.sortOptions.lengthDescending.Checked = false;
            container.gui.sortOptions.artistDescending.Checked = false;
            container.gui.sortOptions.albumAscending.Checked = false;
            container.gui.sortOptions.albumDescending.Checked = false;
            container.gui.sortOptions.yearAscending.Checked = false;
            container.gui.sortOptions.yearDescending.Checked = false;
            container.gui.sortOptions.genreAscending.Checked = false;
            container.gui.sortOptions.genreDescending.Checked = false;
            container.gui.sortOptions.playsAscending.Checked = false;
            container.gui.sortOptions.playsDescending.Checked = false;
            container.gui.sortOptions.ratingAscending.Checked = false;
            container.gui.sortOptions.ratingDescending.Checked = false;

            GuiControl.sort(container, "Artist");
        }

        public static void artistDescending(Mp3_Container container)
        {
            container.gui.sortOptions.artistDescending.Checked = true;
            
            container.gui.sortOptions.nameAscending.Checked = false;
            container.gui.sortOptions.nameDescending.Checked = false;
            container.gui.sortOptions.lengthAscending.Checked = false;
            container.gui.sortOptions.lengthDescending.Checked = false;
            container.gui.sortOptions.artistAscending.Checked = false;
            container.gui.sortOptions.albumAscending.Checked = false;
            container.gui.sortOptions.albumDescending.Checked = false;
            container.gui.sortOptions.yearAscending.Checked = false;
            container.gui.sortOptions.yearDescending.Checked = false;
            container.gui.sortOptions.genreAscending.Checked = false;
            container.gui.sortOptions.genreDescending.Checked = false;
            container.gui.sortOptions.playsAscending.Checked = false;
            container.gui.sortOptions.playsDescending.Checked = false;
            container.gui.sortOptions.ratingAscending.Checked = false;
            container.gui.sortOptions.ratingDescending.Checked = false;

            GuiControl.sort(container, "Artist");
        }

        public static void albumAscending(Mp3_Container container)
        {
            container.gui.sortOptions.albumAscending.Checked = true;
            
            container.gui.sortOptions.nameAscending.Checked = false;
            container.gui.sortOptions.nameDescending.Checked = false;
            container.gui.sortOptions.lengthAscending.Checked = false;
            container.gui.sortOptions.lengthDescending.Checked = false;
            container.gui.sortOptions.artistAscending.Checked = false;
            container.gui.sortOptions.artistDescending.Checked = false;
            container.gui.sortOptions.albumDescending.Checked = false;
            container.gui.sortOptions.yearAscending.Checked = false;
            container.gui.sortOptions.yearDescending.Checked = false;
            container.gui.sortOptions.genreAscending.Checked = false;
            container.gui.sortOptions.genreDescending.Checked = false;
            container.gui.sortOptions.playsAscending.Checked = false;
            container.gui.sortOptions.playsDescending.Checked = false;
            container.gui.sortOptions.ratingAscending.Checked = false;
            container.gui.sortOptions.ratingDescending.Checked = false;

            GuiControl.sort(container, "Album");
        }

        public static void albumDescending(Mp3_Container container)
        {
            container.gui.sortOptions.albumDescending.Checked = true;
            
            container.gui.sortOptions.nameAscending.Checked = false;
            container.gui.sortOptions.nameDescending.Checked = false;
            container.gui.sortOptions.lengthAscending.Checked = false;
            container.gui.sortOptions.lengthDescending.Checked = false;
            container.gui.sortOptions.artistAscending.Checked = false;
            container.gui.sortOptions.artistDescending.Checked = false;
            container.gui.sortOptions.albumAscending.Checked = false;
            container.gui.sortOptions.yearAscending.Checked = false;
            container.gui.sortOptions.yearDescending.Checked = false;
            container.gui.sortOptions.genreAscending.Checked = false;
            container.gui.sortOptions.genreDescending.Checked = false;
            container.gui.sortOptions.playsAscending.Checked = false;
            container.gui.sortOptions.playsDescending.Checked = false;
            container.gui.sortOptions.ratingAscending.Checked = false;
            container.gui.sortOptions.ratingDescending.Checked = false;

            GuiControl.sort(container, "Album");
        }

        public static void yearAscending(Mp3_Container container)
        {
            container.gui.sortOptions.yearAscending.Checked = true;
            
            container.gui.sortOptions.nameAscending.Checked = false;
            container.gui.sortOptions.nameDescending.Checked = false;
            container.gui.sortOptions.lengthAscending.Checked = false;
            container.gui.sortOptions.lengthDescending.Checked = false;
            container.gui.sortOptions.artistAscending.Checked = false;
            container.gui.sortOptions.artistDescending.Checked = false;
            container.gui.sortOptions.albumAscending.Checked = false;
            container.gui.sortOptions.albumDescending.Checked = false;
            container.gui.sortOptions.yearDescending.Checked = false;
            container.gui.sortOptions.genreAscending.Checked = false;
            container.gui.sortOptions.genreDescending.Checked = false;
            container.gui.sortOptions.playsAscending.Checked = false;
            container.gui.sortOptions.playsDescending.Checked = false;
            container.gui.sortOptions.ratingAscending.Checked = false;
            container.gui.sortOptions.ratingDescending.Checked = false;

            GuiControl.sort(container, "Year");
        }

        public static void yearDescending(Mp3_Container container)
        {
            container.gui.sortOptions.yearDescending.Checked = true;
            
            container.gui.sortOptions.nameAscending.Checked = false;
            container.gui.sortOptions.nameDescending.Checked = false;
            container.gui.sortOptions.lengthAscending.Checked = false;
            container.gui.sortOptions.lengthDescending.Checked = false;
            container.gui.sortOptions.artistAscending.Checked = false;
            container.gui.sortOptions.artistDescending.Checked = false;
            container.gui.sortOptions.albumAscending.Checked = false;
            container.gui.sortOptions.albumDescending.Checked = false;
            container.gui.sortOptions.yearAscending.Checked = false;
            container.gui.sortOptions.genreAscending.Checked = false;
            container.gui.sortOptions.genreDescending.Checked = false;
            container.gui.sortOptions.playsAscending.Checked = false;
            container.gui.sortOptions.playsDescending.Checked = false;
            container.gui.sortOptions.ratingAscending.Checked = false;
            container.gui.sortOptions.ratingDescending.Checked = false;

            GuiControl.sort(container, "Year");
        }

        public static void genreAscending(Mp3_Container container)
        {
            container.gui.sortOptions.genreAscending.Checked = true;
            
            container.gui.sortOptions.nameAscending.Checked = false;
            container.gui.sortOptions.nameDescending.Checked = false;
            container.gui.sortOptions.lengthAscending.Checked = false;
            container.gui.sortOptions.lengthDescending.Checked = false;
            container.gui.sortOptions.artistAscending.Checked = false;
            container.gui.sortOptions.artistDescending.Checked = false;
            container.gui.sortOptions.albumAscending.Checked = false;
            container.gui.sortOptions.albumDescending.Checked = false;
            container.gui.sortOptions.yearAscending.Checked = false;
            container.gui.sortOptions.yearDescending.Checked = false;
            container.gui.sortOptions.genreDescending.Checked = false;
            container.gui.sortOptions.playsAscending.Checked = false;
            container.gui.sortOptions.playsDescending.Checked = false;
            container.gui.sortOptions.ratingAscending.Checked = false;
            container.gui.sortOptions.ratingDescending.Checked = false;

            GuiControl.sort(container, "Genre");
        }
        
        public static void genreDescending(Mp3_Container container)
        {
            container.gui.sortOptions.genreDescending.Checked = true;
            
            container.gui.sortOptions.nameAscending.Checked = false;
            container.gui.sortOptions.nameDescending.Checked = false;
            container.gui.sortOptions.lengthAscending.Checked = false;
            container.gui.sortOptions.lengthDescending.Checked = false;
            container.gui.sortOptions.artistAscending.Checked = false;
            container.gui.sortOptions.artistDescending.Checked = false;
            container.gui.sortOptions.albumAscending.Checked = false;
            container.gui.sortOptions.albumDescending.Checked = false;
            container.gui.sortOptions.yearAscending.Checked = false;
            container.gui.sortOptions.yearDescending.Checked = false;
            container.gui.sortOptions.genreAscending.Checked = false;
            container.gui.sortOptions.playsAscending.Checked = false;
            container.gui.sortOptions.playsDescending.Checked = false;
            container.gui.sortOptions.ratingAscending.Checked = false;
            container.gui.sortOptions.ratingDescending.Checked = false;

            GuiControl.sort(container, "Genre");
        }

        public static void playsAscending(Mp3_Container container)
        {
            container.gui.sortOptions.playsAscending.Checked = true;
            
            container.gui.sortOptions.nameAscending.Checked = false;
            container.gui.sortOptions.nameDescending.Checked = false;
            container.gui.sortOptions.lengthAscending.Checked = false;
            container.gui.sortOptions.lengthDescending.Checked = false;
            container.gui.sortOptions.artistAscending.Checked = false;
            container.gui.sortOptions.artistDescending.Checked = false;
            container.gui.sortOptions.albumAscending.Checked = false;
            container.gui.sortOptions.albumDescending.Checked = false;
            container.gui.sortOptions.yearAscending.Checked = false;
            container.gui.sortOptions.yearDescending.Checked = false;
            container.gui.sortOptions.genreAscending.Checked = false;
            container.gui.sortOptions.genreDescending.Checked = false;
            container.gui.sortOptions.playsDescending.Checked = false;
            container.gui.sortOptions.ratingAscending.Checked = false;
            container.gui.sortOptions.ratingDescending.Checked = false;

            GuiControl.sort(container, "Plays");
        }

        public static void playsDescending(Mp3_Container container)
        {
            container.gui.sortOptions.playsDescending.Checked = true;
            
            container.gui.sortOptions.nameAscending.Checked = false;
            container.gui.sortOptions.nameDescending.Checked = false;
            container.gui.sortOptions.lengthAscending.Checked = false;
            container.gui.sortOptions.lengthDescending.Checked = false;
            container.gui.sortOptions.artistAscending.Checked = false;
            container.gui.sortOptions.artistDescending.Checked = false;
            container.gui.sortOptions.albumAscending.Checked = false;
            container.gui.sortOptions.albumDescending.Checked = false;
            container.gui.sortOptions.yearAscending.Checked = false;
            container.gui.sortOptions.yearDescending.Checked = false;
            container.gui.sortOptions.genreAscending.Checked = false;
            container.gui.sortOptions.genreDescending.Checked = false;
            container.gui.sortOptions.playsAscending.Checked = false;
            container.gui.sortOptions.ratingAscending.Checked = false;
            container.gui.sortOptions.ratingDescending.Checked = false;
            
            GuiControl.sort(container, "Plays");
        }

        public static void ratingAscending(Mp3_Container container)
        {
           container.gui.sortOptions.ratingAscending.Checked = true;
           
           container.gui.sortOptions.nameAscending.Checked = false;
           container.gui.sortOptions.nameDescending.Checked = false;
           container.gui.sortOptions.lengthAscending.Checked = false;
           container.gui.sortOptions.lengthDescending.Checked = false;
           container.gui.sortOptions.artistAscending.Checked = false;
           container.gui.sortOptions.artistDescending.Checked = false;
           container.gui.sortOptions.albumAscending.Checked = false;
           container.gui.sortOptions.albumDescending.Checked = false;
           container.gui.sortOptions.yearAscending.Checked = false;
           container.gui.sortOptions.yearDescending.Checked = false;
           container.gui.sortOptions.genreAscending.Checked = false;
           container.gui.sortOptions.genreDescending.Checked = false;
           container.gui.sortOptions.playsAscending.Checked = false;
           container.gui.sortOptions.playsDescending.Checked = false;
           container.gui.sortOptions.ratingDescending.Checked = false;

            GuiControl.sort(container, "Rating");
        }

        public static void ratingDescending(Mp3_Container container)
        {
            container.gui.sortOptions.ratingDescending.Checked = true;
            
            container.gui.sortOptions.nameAscending.Checked = false;
            container.gui.sortOptions.nameDescending.Checked = false;
            container.gui.sortOptions.lengthAscending.Checked = false;
            container.gui.sortOptions.lengthDescending.Checked = false;
            container.gui.sortOptions.artistAscending.Checked = false;
            container.gui.sortOptions.artistDescending.Checked = false;
            container.gui.sortOptions.albumAscending.Checked = false;
            container.gui.sortOptions.albumDescending.Checked = false;
            container.gui.sortOptions.yearAscending.Checked = false;
            container.gui.sortOptions.yearDescending.Checked = false;
            container.gui.sortOptions.genreAscending.Checked = false;
            container.gui.sortOptions.genreDescending.Checked = false;
            container.gui.sortOptions.playsAscending.Checked = false;
            container.gui.sortOptions.playsDescending.Checked = false;
            container.gui.sortOptions.ratingAscending.Checked = false;

            GuiControl.sort(container, "Rating");
        }
    }
}
