using Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileExpert.MP4.Box
{
    class SampleSizeBox : FullBox
    {
        public SampleSizeBox(TreeView treeView, TreeNode parent, DataStore dataStore, ref Int64 bitOffset) :
            base(treeView, parent, dataStore, ref bitOffset)
        {
            TreeNode nodeBox = null;
            TreeNode newNode = null;

            Int64 firstOffset = bitOffset;

            //Add one node to indicate this box.
            Result result = Utility.AddNodeContainer(Position.CHILD, parent, out nodeBox, "SampleSizeBox_payload", ItemType.SECTION1, dataStore, bitOffset, dataStore.GetLeftBitLength());

            Int64 sampleSize = 0;
            if (result.Fine)
            {
                //unsigned int(32) sample_size;
                result = Utility.AddNodeField(Position.CHILD, nodeBox, out newNode, "sample_size", ItemType.FIELD, dataStore, ref bitOffset, 32, ref sampleSize);
            }

            Int64 sampleCount = 0;
            if (result.Fine)
            {
                //unsigned int(32) sample_count;
                result = Utility.AddNodeField(Position.CHILD, nodeBox, out newNode, "sample_count", ItemType.FIELD, dataStore, ref bitOffset, 32, ref sampleCount);
            }

            if (result.Fine && (0 == sampleSize))
            {
                /*Too many samples, let's add it as a big block.*/
                /*
                for (int i = 0; i < sampleCount; i++)
                {
                    if (result.Fine)
                    {
                        //unsigned int(32) entry_size;
                        result = Utility.AddNodeField(Position.CHILD, nodeBox, out newNode, "entry_size", ItemType.FIELD, dataStore, ref bitOffset, 32, ref fieldValue);
                    }
                }
                 */
                result = Utility.AddNodeData(Position.CHILD, nodeBox, out newNode, "entry_size_list", ItemType.FIELD, dataStore, ref bitOffset, 32*sampleCount);

            }

            if (result.Fine)
            {
                Utility.UpdateNodeLength(nodeBox, "SampleSizeBox_payload", ItemType.SECTION1, bitOffset - firstOffset);
            }
        }
    }
}
