using Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileExpert.MP4.Box
{
    class Box
    {
        public Box(TreeView treeView, TreeNode parent, DataStore dataStore, ref Int64 bitOffset)
        {
            /*
            TreeNode nodeBox = null;
            TreeNode newNode = null;

            //Add one node to indicate this box.
            Result result = Utility.AddNodeContainer(Position.CHILD, parent, out nodeBox, "Box", ItemType.SECTION1, dataStore, bitOffset, dataStore.GetLeftBitLength());

            Int64 fieldValue = 0;

            //unsigned int(32) size; 
            Int64 size = 0;
            if (result.Fine)
            {
                result = Utility.AddNodeField(Position.CHILD, nodeBox, out newNode, "size", ItemType.FIELD, dataStore, ref bitOffset, 32, ref size);
            }

            string boxTypeName = null;
            if (result.Fine)
            {
                //unsigned int(32) type = boxtype;
                result = Utility.GetText(out boxTypeName, dataStore, bitOffset, 32);
            }

            if (result.Fine)
            {
                result = Utility.AddNodeData(Position.CHILD, nodeBox, out newNode, "type: " + boxTypeName, ItemType.FIELD, dataStore, ref bitOffset, 32);
            }

            if (result.Fine && (1 == size))
            {
                //unsigned int(64) largesize;
                result = Utility.AddNodeField(Position.CHILD, nodeBox, out newNode, "largesize", ItemType.FIELD, dataStore, ref bitOffset, 64, ref fieldValue);
            }
             */
        }
    }
}
