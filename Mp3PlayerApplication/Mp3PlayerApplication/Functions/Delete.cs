using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Mp3.Containers;

namespace Mp3.Functions
{
    class Delete
    {
        public static void deleteSong(Mp3_Container container)
        {
            if (MessageBox.Show("Are you sure you want to delete " + container.gui.songList.SelectedRows[0].Cells[1].Value.ToString() + " by " +
                                                         container.gui.songList.SelectedRows[0].Cells[3].Value.ToString() + "?", "Delete Song",
                                                         MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                                                         MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                if (container.xmlFile.xml.ChildNodes[1].ChildNodes[1].HasChildNodes)
                {
                    foreach (XmlNode xn in container.xmlFile.xml.ChildNodes[1].ChildNodes[1])
                    {
                        if (xn.ChildNodes[9].InnerText.Equals(container.gui.songList.SelectedRows[0].Cells[10].Value.ToString()))
                        {
                            container.xmlFile.xml.ChildNodes[1].ChildNodes[1].RemoveChild(xn);
                            int rowIndex = container.gui.songList.SelectedRows[0].Index;
                            container.gui.songList.SelectedRows[0].Selected = false;
                            container.gui.songList.Rows.RemoveAt(rowIndex);
                            break;
                        }
                    }
                }
                container.xmlFile.xml.Save(container.xmlFile.userPath);
            }
        }

        public static void deletePlaylist(Mp3_Container container)
        {
            if (container.gui.playlistList.SelectedNode != null)
            {
                if (container.gui.playlistList.SelectedNode.Text.Equals("Master Library"))
                {
                    MessageBox.Show("You cannot delete your Master Library", "Cannot Delete", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    return;
                }

                bool canDelete = false;
                if (container.gui.playlistList.SelectedNode.Nodes.Count > 0)
                {
                    if (MessageBox.Show("Are you sure you want to delete " + container.gui.playlistList.SelectedNode.Text +
                                        "? This will Also delete all childern in this group!", "Delete",
                                        MessageBoxButtons.OKCancel, MessageBoxIcon.Question,
                                        MessageBoxDefaultButton.Button1) == DialogResult.OK)
                    {
                        canDelete = true;
                    }
                }
                else
                {
                    if (MessageBox.Show("Are you sure you want to delete " + container.gui.playlistList.SelectedNode.Text + "?",
                                        "Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Question,
                                        MessageBoxDefaultButton.Button1) == DialogResult.OK)
                    {
                        canDelete = true;
                    }
                }
                if (!canDelete)
                {
                    return;
                }
                else
                {
                    string playlist = container.gui.playlistList.SelectedNode.Text;
                    foreach (TreeNode xn in container.gui.playlistList.Nodes)
                    {
                        if (xn.Text.Equals(playlist))
                        {
                            container.gui.playlistList.Nodes.Remove(xn);
                            break;
                        }
                        else if (xn.Nodes.Count > 0)
                        {
                            bool broke = false;
                            foreach (TreeNode xcn in xn.Nodes)
                            {
                                if (xcn.Text.Equals(playlist))
                                {
                                    xn.Nodes.Remove(xcn);
                                    broke = true;
                                    break;
                                }
                            }
                            if (broke)
                            {
                                break;
                            }
                        }
                    }

                    foreach (XmlNode xn in container.xmlFile.xml.ChildNodes[1].ChildNodes[0])
                    {
                        if (xn.Attributes["name"].Value.Equals(playlist))
                        {
                            container.xmlFile.xml.ChildNodes[1].ChildNodes[0].RemoveChild(xn);
                            break;
                        }
                        else if (xn.HasChildNodes)
                        {
                            bool broke = false;
                            foreach (XmlNode xcn in xn)
                            {
                                if (xcn.Name.Equals("Song"))
                                {
                                    continue;
                                }
                                if (xcn.Attributes["name"].Value.Equals(playlist))
                                {
                                    xn.RemoveChild(xcn);
                                    broke = true;
                                    break;
                                }
                            }
                            if (broke)
                            {
                                break;
                            }
                        }
                    }
                    container.xmlFile.xml.Save(container.xmlFile.userPath);
                }
            }
        }
    }
}
