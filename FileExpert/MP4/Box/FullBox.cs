using Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileExpert.MP4.Box
{
    class FullBox : Box
    {
        protected Int64 version = 0;
        protected Int64 flags = 0;


        public FullBox(TreeView treeView, TreeNode parent, DataStore dataStore, ref Int64 bitOffset) :
            base(treeView, parent, dataStore, ref bitOffset)
        {
            TreeNode nodeBox = null;
            TreeNode newNode = null;

            Int64 firstOffset = bitOffset;
            //Add one node to indicate this box.
            Result result = Utility.AddNodeContainer(Position.CHILD, parent, out nodeBox, "FullBox_payload", ItemType.SECTION1, dataStore, bitOffset, dataStore.GetLeftBitLength());

            if (result.Fine)
            {
                //unsigned int(8)  version
                result = Utility.AddNodeField(Position.CHILD, nodeBox, out newNode, "version", ItemType.FIELD, dataStore, ref bitOffset, 8, ref version);
            }

            if (result.Fine)
            {
                //bit(24)        flags
                result = Utility.AddNodeField(Position.CHILD, nodeBox, out newNode, "flags", ItemType.FIELD, dataStore, ref bitOffset, 24, ref flags);
            }

            if (result.Fine)
            {
                Utility.UpdateNodeLength(nodeBox, "FullBox_payload", ItemType.SECTION1, bitOffset - firstOffset);
            }
        }
    }
}
