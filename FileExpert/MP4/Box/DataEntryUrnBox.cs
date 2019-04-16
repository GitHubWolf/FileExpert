using Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileExpert.MP4.Box
{
    class DataEntryUrnBox : FullBox
    {
        public DataEntryUrnBox(TreeView treeView, TreeNode parent, DataStore dataStore, ref Int64 bitOffset) :
            base(treeView, parent, dataStore, ref bitOffset)
        {
            TreeNode nodeBox = null;
            TreeNode newNode = null;

            Int64 firstOffset = bitOffset;

            //Add one node to indicate this box.
            Result result = Utility.AddNodeContainer(Position.CHILD, parent, out nodeBox, "DataEntryUrnBox_payload", ItemType.SECTION1, dataStore, bitOffset, dataStore.GetLeftBitLength());

            Int64 fieldValue = 0;

            string name = null;
            if (result.Fine)
            {
                //string name;
                result = Utility.GetText(out name, dataStore, bitOffset, dataStore.GetLeftBitLength());
            }

            if (result.Fine)
            {
                //string name;
                result = Utility.AddNodeField(Position.CHILD, nodeBox, out newNode, "name:" + name, ItemType.FIELD, dataStore, ref bitOffset, dataStore.GetLeftBitLength(), ref fieldValue);
            }

            string location = null;
            if (result.Fine)
            {
                //string location;
                result = Utility.GetText(out location, dataStore, bitOffset, dataStore.GetLeftBitLength());
            }

            if (result.Fine)
            {
                //string location;
                result = Utility.AddNodeField(Position.CHILD, nodeBox, out newNode, "location:" + location, ItemType.FIELD, dataStore, ref bitOffset, dataStore.GetLeftBitLength(), ref fieldValue);
            }

            if (result.Fine)
            {
                Utility.UpdateNodeLength(nodeBox, "DataEntryUrnBox_payload", ItemType.SECTION1, bitOffset - firstOffset);
            }
        }
    }
}
