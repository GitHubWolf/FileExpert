using Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileExpert.MP4.Box
{
    class SampleEntry : Box
    {
        public SampleEntry(TreeView treeView, TreeNode parent, DataStore dataStore, ref Int64 bitOffset) :
            base(treeView, parent, dataStore, ref bitOffset)
        {
            TreeNode nodeBox = null;
            TreeNode newNode = null;

            //Add one node to indicate this box.
            Result result = Utility.AddNodeContainer(Position.CHILD, parent, out nodeBox, "SampleEntry_payload", ItemType.SECTION1, dataStore, bitOffset, dataStore.GetLeftBitLength());

            Int64 fieldValue = 0;

            if (result.Fine)
            {
                //const unsigned int(8)[6] reserved = 0;
                result = Utility.AddNodeField(Position.CHILD, nodeBox, out newNode, "reserved", ItemType.FIELD, dataStore, ref bitOffset, 8*6, ref fieldValue);
            }

            if (result.Fine)
            {
                //unsigned int(16) data_reference_index;
                result = Utility.AddNodeField(Position.CHILD, nodeBox, out newNode, "data_reference_index", ItemType.FIELD, dataStore, ref bitOffset, 16, ref fieldValue);
            }
            
            if (result.Fine)
            {
                Utility.UpdateNode(nodeBox, "SampleEntry_payload", ItemType.SECTION1);
            }
        }
    }
}
