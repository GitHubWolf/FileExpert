using Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileExpert.MP4.Box
{
    class FreeSpaceBox : Box
    {
        public FreeSpaceBox(TreeView treeView, TreeNode parent, DataStore dataStore, ref Int64 bitOffset) :
            base(treeView, parent, dataStore, ref bitOffset)
        {

            TreeNode nodeBoxes = null;

            //Add one node to indicate the payload.
            Result result = Utility.AddNodeData(Position.CHILD, parent, out nodeBoxes, "FreeSpaceBox_payload", ItemType.DATA, dataStore, ref bitOffset, dataStore.GetLeftBitLength());

            if (result.Fine)
            {
                Utility.UpdateNode(nodeBoxes, "FreeSpaceBox_payload", ItemType.SECTION1);
            }
        }
    }
}
