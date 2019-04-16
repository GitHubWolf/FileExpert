using Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileExpert.MP4.Box
{
    class TrackFragmentHeaderBox : FullBox
    {
        public TrackFragmentHeaderBox(TreeView treeView, TreeNode parent, DataStore dataStore, ref Int64 bitOffset) :
            base(treeView, parent, dataStore, ref bitOffset)
        {
            TreeNode nodeBox = null;
            TreeNode newNode = null;

            //Add one node to indicate this box.
            Result result = Utility.AddNodeContainer(Position.CHILD, parent, out nodeBox, "TrackFragmentHeaderBox_payload", ItemType.SECTION1, dataStore, bitOffset, dataStore.GetLeftBitLength());

            Int64 fieldValue = 0;

            if (result.Fine)
            {
                //unsigned int(32) track_ID;
                result = Utility.AddNodeField(Position.CHILD, nodeBox, out newNode, "track_ID", ItemType.FIELD, dataStore, ref bitOffset, 32, ref fieldValue);
            }

            // all the following are optional fields
            if (result.Fine && (dataStore.GetLeftBitLength() > 0))
            {
                //unsigned int(64) base_data_offset;
                result = Utility.AddNodeField(Position.CHILD, nodeBox, out newNode, "base_data_offset", ItemType.FIELD, dataStore, ref bitOffset, 64, ref fieldValue);

                if (result.Fine)
                {
                    //unsigned int(32) sample_description_index;
                    result = Utility.AddNodeField(Position.CHILD, nodeBox, out newNode, "sample_description_index", ItemType.FIELD, dataStore, ref bitOffset, 32, ref fieldValue);
                }

                if (result.Fine)
                {
                    //unsigned int(32) default_sample_duration;
                    result = Utility.AddNodeField(Position.CHILD, nodeBox, out newNode, "default_sample_duration", ItemType.FIELD, dataStore, ref bitOffset, 32, ref fieldValue);
                }

                if (result.Fine)
                {
                    //unsigned int(32) default_sample_size;
                    result = Utility.AddNodeField(Position.CHILD, nodeBox, out newNode, "default_sample_size", ItemType.FIELD, dataStore, ref bitOffset, 32, ref fieldValue);
                }

                if (result.Fine)
                {
                    //unsigned int(32) default_sample_flags
                    result = Utility.AddNodeField(Position.CHILD, nodeBox, out newNode, "default_sample_flags", ItemType.FIELD, dataStore, ref bitOffset, 32, ref fieldValue);
                }

            }

            if (result.Fine)
            {
                Utility.UpdateNode(nodeBox, "TrackFragmentHeaderBox_payload", ItemType.SECTION1);
            }
        }
    }
}
