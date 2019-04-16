using Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileExpert.MP4.Box
{
    class TrackFragmentRandomAccessBox : FullBox
    {
        public TrackFragmentRandomAccessBox(TreeView treeView, TreeNode parent, DataStore dataStore, ref Int64 bitOffset) :
            base(treeView, parent, dataStore, ref bitOffset)
        {
            TreeNode nodeBox = null;
            TreeNode newNode = null;

            Int64 firstOffset = bitOffset;

            //Add one node to indicate this box.
            Result result = Utility.AddNodeContainer(Position.CHILD, parent, out nodeBox, "TrackFragmentRandomAccessBox_payload", ItemType.SECTION1, dataStore, bitOffset, dataStore.GetLeftBitLength());

            Int64 fieldValue = 0;

            if (result.Fine)
            {
                //unsigned int(32) track_ID;
                result = Utility.AddNodeField(Position.CHILD, nodeBox, out newNode, "track_ID", ItemType.FIELD, dataStore, ref bitOffset, 32, ref fieldValue);
            }

            if (result.Fine)
            {
                //const unsigned int(26) reserved = 0;
                result = Utility.AddNodeField(Position.CHILD, nodeBox, out newNode, "reserved", ItemType.FIELD, dataStore, ref bitOffset, 26, ref fieldValue);
            }

            Int64 lengthSizeOfTrafNum = 0;
            if (result.Fine)
            {
                //unsigned int(2) length_size_of_traf_num;
                result = Utility.AddNodeField(Position.CHILD, nodeBox, out newNode, "length_size_of_traf_num", ItemType.FIELD, dataStore, ref bitOffset, 2, ref lengthSizeOfTrafNum);
            }

            Int64 lengthSizeOfTrunNum = 0;
            if (result.Fine)
            {
                //unsigned int(2) length_size_of_trun_num;
                result = Utility.AddNodeField(Position.CHILD, nodeBox, out newNode, "length_size_of_trun_num", ItemType.FIELD, dataStore, ref bitOffset, 2, ref lengthSizeOfTrunNum);
            }

            Int64 lengthSizeOfSampleNum = 0;
            if (result.Fine)
            {
                //unsigned int(2) length_size_of_sample_num;
                result = Utility.AddNodeField(Position.CHILD, nodeBox, out newNode, "length_size_of_sample_num", ItemType.FIELD, dataStore, ref bitOffset, 2, ref lengthSizeOfSampleNum);
            }

            Int64 numberOfEntry = 0;
            if (result.Fine)
            {
                //unsigned int(32) number_of_entry;
                result = Utility.AddNodeField(Position.CHILD, nodeBox, out newNode, "number_of_entry", ItemType.FIELD, dataStore, ref bitOffset, 32, ref numberOfEntry);
            }


            if (result.Fine)
            {
                Int64 bitOffsetForTheLoop = 0;
                for (Int64 i = 0; i < numberOfEntry; ++i)
                {
                    bitOffsetForTheLoop = bitOffset;
                    TreeNode nodeLoop = null;
                    //Add one node to indicate this box.
                    if (result.Fine)
                    {
                        result = Utility.AddNodeContainer(Position.CHILD, nodeBox, out nodeLoop, "entry_payload " + i, ItemType.SECTION1, dataStore, bitOffset, 0);
                    }

                    if (1 == version)
                    {
                        if (result.Fine)
                        {
                            //unsigned int(64) time;
                            result = Utility.AddNodeField(Position.CHILD, nodeLoop, out newNode, "time", ItemType.FIELD, dataStore, ref bitOffset, 64, ref fieldValue);
                        }

                        if (result.Fine)
                        {
                            //unsigned int(64) moof_offset;
                            result = Utility.AddNodeField(Position.CHILD, nodeLoop, out newNode, "moof_offset", ItemType.FIELD, dataStore, ref bitOffset, 64, ref fieldValue);
                        }
                    }
                    else
                    {
                        if (result.Fine)
                        {
                            //unsigned int(64) time;
                            result = Utility.AddNodeField(Position.CHILD, nodeLoop, out newNode, "time", ItemType.FIELD, dataStore, ref bitOffset, 32, ref fieldValue);
                        }

                        if (result.Fine)
                        {
                            //unsigned int(64) moof_offset;
                            result = Utility.AddNodeField(Position.CHILD, nodeLoop, out newNode, "moof_offset", ItemType.FIELD, dataStore, ref bitOffset, 32, ref fieldValue);
                        }
                    }

                    if (result.Fine)
                    {
                        //unsigned int((length_size_of_traf_num+1) * 8) traf_number;
                        result = Utility.AddNodeField(Position.CHILD, nodeLoop, out newNode, "traf_number", ItemType.FIELD, dataStore, ref bitOffset, (lengthSizeOfTrafNum + 1) * 8, ref fieldValue);
                    }

                    if (result.Fine)
                    {
                        //unsigned int((length_size_of_trun_num+1) * 8) trun_number;
                        result = Utility.AddNodeField(Position.CHILD, nodeLoop, out newNode, "trun_number", ItemType.FIELD, dataStore, ref bitOffset, (lengthSizeOfTrunNum + 1) * 8, ref fieldValue);
                    }

                    if (result.Fine)
                    {
                        //unsigned int((length_size_of_sample_num+1) * 8) sample_number;
                        result = Utility.AddNodeField(Position.CHILD, nodeLoop, out newNode, "sample_number", ItemType.FIELD, dataStore, ref bitOffset, (lengthSizeOfSampleNum + 1) * 8, ref fieldValue);
                    }

                    //Add one node to indicate this box.
                    if (result.Fine)
                    {
                        Utility.UpdateNodeLength(nodeLoop, "entry_payload " + i, ItemType.SECTION1, bitOffset - bitOffsetForTheLoop);
                    }
                }
            }

            if (result.Fine)
            {
                Utility.UpdateNodeLength(nodeBox, "TrackFragmentRandomAccessBox_payload", ItemType.SECTION1, bitOffset - firstOffset);
            }
        }
    }
}
