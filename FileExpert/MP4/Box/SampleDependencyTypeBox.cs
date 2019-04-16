using Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileExpert.MP4.Box
{
    class SampleDependencyTypeBox : FullBox
    {
        public SampleDependencyTypeBox(TreeView treeView, TreeNode parent, DataStore dataStore, ref Int64 bitOffset) :
            base(treeView, parent, dataStore, ref bitOffset)
        {
            TreeNode nodeBox = null;
            TreeNode newNode = null;

            Int64 firstOffset = bitOffset;

            //Add one node to indicate this box.
            Result result = Utility.AddNodeContainer(Position.CHILD, parent, out nodeBox, "SampleDependencyTypeBox_payload", ItemType.SECTION1, dataStore, bitOffset, dataStore.GetLeftBitLength());

            Int64 fieldValue = 0;

            Int64 bitOffsetForTheLoop = 0;
            Int64 i = 0;

            // the following are optional fields
            while (result.Fine && (dataStore.GetLeftBitLength() > 0))
            {
                bitOffsetForTheLoop = bitOffset;
                TreeNode nodeLoop = null;
                //Add one node to indicate this box.
                if (result.Fine)
                {
                    result = Utility.AddNodeContainer(Position.CHILD, nodeBox, out nodeLoop, "sample_payload " + i, ItemType.SECTION1, dataStore, bitOffset, 0);
                }

                if (result.Fine)
                {
                    //unsigned int(2) is_leading;
                    result = Utility.AddNodeField(Position.CHILD, nodeLoop, out newNode, "is_leading", ItemType.FIELD, dataStore, ref bitOffset, 2, ref fieldValue);
                }

                if (result.Fine)
                {
                    //unsigned int(2) sample_depends_on;
                    result = Utility.AddNodeField(Position.CHILD, nodeLoop, out newNode, "sample_depends_on", ItemType.FIELD, dataStore, ref bitOffset, 2, ref fieldValue);
                }

                if (result.Fine)
                {
                    //unsigned int(2) sample_is_depended_on;
                    result = Utility.AddNodeField(Position.CHILD, nodeLoop, out newNode, "sample_is_depended_on", ItemType.FIELD, dataStore, ref bitOffset, 2, ref fieldValue);
                }

                if (result.Fine)
                {
                    //unsigned int(2) sample_has_redundancy;
                    result = Utility.AddNodeField(Position.CHILD, nodeLoop, out newNode, "sample_has_redundancy", ItemType.FIELD, dataStore, ref bitOffset, 2, ref fieldValue);
                }

                //Add one node to indicate this box.
                if (result.Fine)
                {
                    Utility.UpdateNodeLength(nodeLoop, "sample_payload " + i, ItemType.SECTION1, bitOffset - bitOffsetForTheLoop);
                    i++;
                }

            }

            if (result.Fine)
            {
                Utility.UpdateNodeLength(nodeBox, "SampleDependencyTypeBox_payload", ItemType.SECTION1, bitOffset - firstOffset);
            }
        }
    }
}
