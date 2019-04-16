using Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileExpert.MP4.Box
{
    class TrackRunBox : FullBox
    {
        public TrackRunBox(TreeView treeView, TreeNode parent, DataStore dataStore, ref Int64 bitOffset) :
            base(treeView, parent, dataStore, ref bitOffset)
        {
            TreeNode nodeBox = null;
            TreeNode newNode = null;

            Int64 firstOffset = bitOffset;

            //Add one node to indicate this box.
            Result result = Utility.AddNodeContainer(Position.CHILD, parent, out nodeBox, "TrackRunBox_payload", ItemType.SECTION1, dataStore, bitOffset, dataStore.GetLeftBitLength());

            Int64 fieldValue = 0;

            Int64 sampleCount = 0;
            if (result.Fine)
            {
                //unsigned int(32) sample_count;
                result = Utility.AddNodeField(Position.CHILD, nodeBox, out newNode, "sample_count", ItemType.FIELD, dataStore, ref bitOffset, 32, ref sampleCount);
            }

            // the following are optional fields
            if (result.Fine && (dataStore.GetLeftBitLength() > 0))
            {
                if (result.Fine)
                {
                    //signed int(32) data_offset;
                    result = Utility.AddNodeField(Position.CHILD, nodeBox, out newNode, "data_offset", ItemType.FIELD, dataStore, ref bitOffset, 32, ref fieldValue);
                }

                if (result.Fine)
                {
                    //unsigned int(32) first_sample_flags;
                    result = Utility.AddNodeField(Position.CHILD, nodeBox, out newNode, "first_sample_flags", ItemType.FIELD, dataStore, ref bitOffset, 32, ref fieldValue);
                }

                if (result.Fine)
                {
                    Int64 bitOffsetForTheLoop = 0;
                    for (Int64 i = 0; i < sampleCount; ++i)
                    {
                        bitOffsetForTheLoop = bitOffset;
                        TreeNode nodeLoop = null;
                        //Add one node to indicate this box.
                        if (result.Fine)
                        {
                            result = Utility.AddNodeContainer(Position.CHILD, nodeBox, out nodeLoop, "sample_payload " + i, ItemType.SECTION1, dataStore, bitOffset, 0);
                        }


                        // all fields in the following array are optional
                        if (result.Fine)
                        {
                            //unsigned int(32) sample_duration;
                            result = Utility.AddNodeField(Position.CHILD, nodeLoop, out newNode, "sample_duration", ItemType.FIELD, dataStore, ref bitOffset, 32, ref fieldValue);
                        }

                        if (result.Fine)
                        {
                            //unsigned int(32) sample_size;
                            result = Utility.AddNodeField(Position.CHILD, nodeLoop, out newNode, "sample_size", ItemType.FIELD, dataStore, ref bitOffset, 32, ref fieldValue);
                        }

                        if (result.Fine)
                        {
                            //unsigned int(32) sample_flags
                            result = Utility.AddNodeField(Position.CHILD, nodeLoop, out newNode, "sample_flags", ItemType.FIELD, dataStore, ref bitOffset, 32, ref fieldValue);
                        }

                        if (result.Fine)
                        {
                            //unsigned int(32) sample_composition_time_offset; or signed int(32) sample_composition_time_offset;
                            result = Utility.AddNodeField(Position.CHILD, nodeLoop, out newNode, "sample_composition_time_offset", ItemType.FIELD, dataStore, ref bitOffset, 32, ref fieldValue);
                        }

                        //Add one node to indicate this box.
                        if (result.Fine)
                        {
                            Utility.UpdateNodeLength(nodeLoop, "sample_payload " + i, ItemType.SECTION1, bitOffset - bitOffsetForTheLoop);
                        }

                    }
                }
            }

            if (result.Fine)
            {
                Utility.UpdateNodeLength(nodeBox, "TrackRunBox_payload", ItemType.SECTION1, bitOffset - firstOffset);
            }
        }
    }
}
