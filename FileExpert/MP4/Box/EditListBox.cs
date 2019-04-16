using Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileExpert.MP4.Box
{
    class EditListBox : FullBox
    {
        public EditListBox(TreeView treeView, TreeNode parent, DataStore dataStore, ref Int64 bitOffset) :
            base(treeView, parent, dataStore, ref bitOffset)
        {
            TreeNode nodeBox = null;
            TreeNode newNode = null;

            Int64 firstOffset = bitOffset;

            //Add one node to indicate this box.
            Result result = Utility.AddNodeContainer(Position.CHILD, parent, out nodeBox, "EditListBox_payload", ItemType.SECTION1, dataStore, bitOffset, dataStore.GetLeftBitLength());

            Int64 fieldValue = 0;

            Int64 entryCount = 0;
            if (result.Fine)
            {
                //unsigned int(32) entry_count;
                result = Utility.AddNodeField(Position.CHILD, nodeBox, out newNode, "entry_count", ItemType.FIELD, dataStore, ref bitOffset, 32, ref entryCount);
            }

            Int64 bitOffsetForTheLoop = 0;
            for (Int64 i = 0; i < entryCount; ++i)
            {
                bitOffsetForTheLoop = bitOffset;
                TreeNode nodeLoop = null;
                //Add one node to indicate this box.
                if (result.Fine)
                {
                    result = Utility.AddNodeContainer(Position.CHILD, nodeBox, out nodeLoop, "entry_payload " + i, ItemType.SECTION1, dataStore, bitOffset, 0);
                }


                if (result.Fine)
                {
                    if (1 == version)
                    {
                        if (result.Fine)
                        {
                            //unsigned int(64) segment_duration;
                            result = Utility.AddNodeField(Position.CHILD, nodeLoop, out newNode, "segment_duration", ItemType.FIELD, dataStore, ref bitOffset, 64, ref fieldValue);
                        }

                        if (result.Fine)
                        {
                            //int(64) media_time;
                            result = Utility.AddNodeField(Position.CHILD, nodeLoop, out newNode, "media_time", ItemType.FIELD, dataStore, ref bitOffset, 64, ref fieldValue);
                        }
                    }
                    else
                    {
                        if (result.Fine)
                        {
                            //unsigned int(32) segment_duration;
                            result = Utility.AddNodeField(Position.CHILD, nodeLoop, out newNode, "segment_duration", ItemType.FIELD, dataStore, ref bitOffset, 32, ref fieldValue);
                        }

                        if (result.Fine)
                        {
                            //int(32) media_time;
                            result = Utility.AddNodeField(Position.CHILD, nodeLoop, out newNode, "media_time", ItemType.FIELD, dataStore, ref bitOffset, 32, ref fieldValue);
                        }
                    }
                }
                if (result.Fine)
                {
                    //int(16) media_rate_integer;
                    result = Utility.AddNodeField(Position.CHILD, nodeLoop, out newNode, "media_rate_integer", ItemType.FIELD, dataStore, ref bitOffset, 16, ref fieldValue);
                }

                if (result.Fine)
                {
                    //int(16) media_rate_fraction = 0;
                    result = Utility.AddNodeField(Position.CHILD, nodeLoop, out newNode, "media_rate_fraction", ItemType.FIELD, dataStore, ref bitOffset, 16, ref fieldValue);
                }

                //Add one node to indicate this box.
                if (result.Fine)
                {
                    Utility.UpdateNodeLength(nodeLoop, "entry_payload " + i, ItemType.SECTION1, bitOffset - bitOffsetForTheLoop);
                }

            }

            if (result.Fine)
            {
                Utility.UpdateNodeLength(nodeBox, "EditListBox_payload", ItemType.SECTION1, bitOffset - firstOffset);
            }
        }
    }
}
