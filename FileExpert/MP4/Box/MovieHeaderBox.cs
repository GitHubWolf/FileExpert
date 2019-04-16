using Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileExpert.MP4.Box
{
    class MovieHeaderBox : FullBox
    {
        public MovieHeaderBox(TreeView treeView, TreeNode parent, DataStore dataStore, ref Int64 bitOffset) :
            base(treeView, parent, dataStore, ref bitOffset)
        {
            TreeNode nodeBox = null;
            TreeNode newNode = null;

            Int64 firstOffset = bitOffset;

            //Add one node to indicate this box.
            Result result = Utility.AddNodeContainer(Position.CHILD, parent, out nodeBox, "MovieHeaderBox_payload", ItemType.SECTION1, dataStore, bitOffset, dataStore.GetLeftBitLength());

            Int64 fieldValue = 0;

            if (result.Fine)
            {
                if (1 == version)
                {
                    if (result.Fine)
                    {
                        //unsigned int(64) creation_time;
                        result = Utility.AddNodeField(Position.CHILD, nodeBox, out newNode, "creation_time", ItemType.FIELD, dataStore, ref bitOffset, 64, ref fieldValue);
                    }

                    if (result.Fine)
                    {
                        //unsigned int(64) modification_time;
                        result = Utility.AddNodeField(Position.CHILD, nodeBox, out newNode, "modification_time", ItemType.FIELD, dataStore, ref bitOffset, 64, ref fieldValue);
                    }

                    if (result.Fine)
                    {
                        //unsigned int(32) timescale;
                        result = Utility.AddNodeField(Position.CHILD, nodeBox, out newNode, "timescale", ItemType.FIELD, dataStore, ref bitOffset, 32, ref fieldValue);
                    }

                    if (result.Fine)
                    {
                        //unsigned int(64) duration;
                        result = Utility.AddNodeField(Position.CHILD, nodeBox, out newNode, "duration", ItemType.FIELD, dataStore, ref bitOffset, 64, ref fieldValue);
                    }
                }
                else
                {
                    if (result.Fine)
                    {
                        //unsigned int(32) creation_time;
                        result = Utility.AddNodeField(Position.CHILD, nodeBox, out newNode, "creation_time", ItemType.FIELD, dataStore, ref bitOffset, 32, ref fieldValue);
                    }

                    if (result.Fine)
                    {
                        //unsigned int(32) modification_time;
                        result = Utility.AddNodeField(Position.CHILD, nodeBox, out newNode, "modification_time", ItemType.FIELD, dataStore, ref bitOffset, 32, ref fieldValue);
                    }

                    if (result.Fine)
                    {
                        //unsigned int(32) timescale;
                        result = Utility.AddNodeField(Position.CHILD, nodeBox, out newNode, "timescale", ItemType.FIELD, dataStore, ref bitOffset, 32, ref fieldValue);
                    }

                    if (result.Fine)
                    {
                        //unsigned int(32) duration;
                        result = Utility.AddNodeField(Position.CHILD, nodeBox, out newNode, "duration", ItemType.FIELD, dataStore, ref bitOffset, 32, ref fieldValue);
                    }
                }
            }

            if (result.Fine)
            {
                //template int(32) rate = 0x00010000; // typically 1.0
                result = Utility.AddNodeField(Position.CHILD, nodeBox, out newNode, "rate", ItemType.FIELD, dataStore, ref bitOffset, 32, ref fieldValue);
            }

            if (result.Fine)
            {
                //template int(16) volume = 0x0100; // typically, full volume
                result = Utility.AddNodeField(Position.CHILD, nodeBox, out newNode, "volume", ItemType.FIELD, dataStore, ref bitOffset, 16, ref fieldValue);
            }

            if (result.Fine)
            {
                //const bit(16) reserved = 0;
                result = Utility.AddNodeField(Position.CHILD, nodeBox, out newNode, "reserved", ItemType.FIELD, dataStore, ref bitOffset, 16, ref fieldValue);
            }

            if (result.Fine)
            {
                //const unsigned int(32)[2] reserved = 0;
                result = Utility.AddNodeField(Position.CHILD, nodeBox, out newNode, "reserved", ItemType.FIELD, dataStore, ref bitOffset, 32*2, ref fieldValue);
            }

            if (result.Fine)
            {
                //template int(32)[9] matrix ={ 0x00010000,0,0,0,0x00010000,0,0,0,0x40000000 };
                result = Utility.AddNodeField(Position.CHILD, nodeBox, out newNode, "matrix", ItemType.FIELD, dataStore, ref bitOffset, 32 * 9, ref fieldValue);
            }

            if (result.Fine)
            {
                //bit(32)[6] pre_defined = 0;
                result = Utility.AddNodeField(Position.CHILD, nodeBox, out newNode, "pre_defined", ItemType.FIELD, dataStore, ref bitOffset, 32 * 6, ref fieldValue);
            }

            if (result.Fine)
            {
                //unsigned int(32) next_track_ID;
                result = Utility.AddNodeField(Position.CHILD, nodeBox, out newNode, "next_track_ID", ItemType.FIELD, dataStore, ref bitOffset, 32, ref fieldValue);
            }

            if (result.Fine)
            {
                Utility.UpdateNodeLength(nodeBox, "MovieHeaderBox_payload", ItemType.SECTION1, bitOffset - firstOffset);
            }
        }
    }
}
