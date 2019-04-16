using Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileExpert.MP4.Box
{
    class FileTypeBox : Box
    {
        public FileTypeBox(TreeView treeView, TreeNode parent, DataStore dataStore, ref Int64 bitOffset) :
            base(treeView, parent, dataStore, ref bitOffset)
        {
            TreeNode nodeBox = null;
            TreeNode newNode = null;

            //Add one node to indicate this box.
            Result result = Utility.AddNodeContainer(Position.CHILD, parent, out nodeBox, "FileTypeBox_payload", ItemType.SECTION1, dataStore, bitOffset, dataStore.GetLeftBitLength());

            Int64 fieldValue = 0;

            if (result.Fine)
            {
                //unsigned int(32)  major_brand;
                result = Utility.AddNodeField(Position.CHILD, nodeBox, out newNode, "major_brand", ItemType.FIELD, dataStore, ref bitOffset, 32, ref fieldValue);
            }

            if (result.Fine)
            {
                //unsigned int(32)  minor_version;
                result = Utility.AddNodeField(Position.CHILD, nodeBox, out newNode, "minor_version", ItemType.FIELD, dataStore, ref bitOffset, 32, ref fieldValue);
            }

            if (result.Fine)
            {
                //unsigned int(32)  compatible_brands[];  // to end of the box
                result = Utility.AddNodeData(Position.CHILD, nodeBox, out newNode, "compatible_brands", ItemType.DATA, dataStore, ref bitOffset, dataStore.GetLeftBitLength());
            }
            
            if (result.Fine)
            {
                Utility.UpdateNode(nodeBox, "FileTypeBox_payload", ItemType.SECTION1);
            }
        }
    }
}
