using Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileExpert.MP4.Box
{
    class VideoMediaHeaderBox : FullBox
    {
        public VideoMediaHeaderBox(TreeView treeView, TreeNode parent, DataStore dataStore, ref Int64 bitOffset) :
            base(treeView, parent, dataStore, ref bitOffset)
        {
            TreeNode nodeBox = null;
            TreeNode newNode = null;

            Int64 firstOffset = bitOffset;

            //Add one node to indicate this box.
            Result result = Utility.AddNodeContainer(Position.CHILD, parent, out nodeBox, "VideoMediaHeaderBox_payload", ItemType.SECTION1, dataStore, bitOffset, dataStore.GetLeftBitLength());

            Int64 fieldValue = 0;

            if (result.Fine)
            {
                //template unsigned int(16) graphicsmode = 0; // copy, see below
                result = Utility.AddNodeField(Position.CHILD, nodeBox, out newNode, "graphicsmode", ItemType.FIELD, dataStore, ref bitOffset, 16, ref fieldValue);
            }

            if (result.Fine)
            {
                //template unsigned int(16)[3] opcolor = {0, 0, 0};
                result = Utility.AddNodeField(Position.CHILD, nodeBox, out newNode, "opcolor", ItemType.FIELD, dataStore, ref bitOffset, 16*3, ref fieldValue);
            }


            if (result.Fine)
            {
                Utility.UpdateNodeLength(nodeBox, "VideoMediaHeaderBox_payload", ItemType.SECTION1, bitOffset - firstOffset);
            }
        }
    }
}
