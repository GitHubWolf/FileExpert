using Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileExpert.MP4.Box
{
    class TrackHeaderBox : FullBox
    {
        public TrackHeaderBox(TreeView treeView, TreeNode parent, DataStore dataStore, ref Int64 bitOffset) :
            base(treeView, parent, dataStore, ref bitOffset)
        {
            TreeNode nodeBox = null;
            TreeNode newNode = null;

            Int64 firstOffset = bitOffset;

            //Add one node to indicate this box.
            Result result = Utility.AddNodeContainer(Position.CHILD, parent, out nodeBox, "TrackHeaderBox_payload", ItemType.SECTION1, dataStore, bitOffset, dataStore.GetLeftBitLength());

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
                        //unsigned int(32) track_ID;
                        result = Utility.AddNodeField(Position.CHILD, nodeBox, out newNode, "track_ID", ItemType.FIELD, dataStore, ref bitOffset, 32, ref fieldValue);
                    }

                    if (result.Fine)
                    {
                        //const unsigned int(32) reserved = 0;
                        result = Utility.AddNodeField(Position.CHILD, nodeBox, out newNode, "reserved", ItemType.FIELD, dataStore, ref bitOffset, 32, ref fieldValue);
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
                        //unsigned int(32) track_ID;
                        result = Utility.AddNodeField(Position.CHILD, nodeBox, out newNode, "track_ID", ItemType.FIELD, dataStore, ref bitOffset, 32, ref fieldValue);
                    }

                    if (result.Fine)
                    {
                        //const unsigned int(32) reserved = 0;
                        result = Utility.AddNodeField(Position.CHILD, nodeBox, out newNode, "reserved", ItemType.FIELD, dataStore, ref bitOffset, 32, ref fieldValue);
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
                //const unsigned int(32)[2] reserved = 0;
                result = Utility.AddNodeField(Position.CHILD, nodeBox, out newNode, "reserved", ItemType.FIELD, dataStore, ref bitOffset, 32 * 2, ref fieldValue);
            }

            if (result.Fine)
            {
                //template int(16) layer = 0;
                result = Utility.AddNodeField(Position.CHILD, nodeBox, out newNode, "layer", ItemType.FIELD, dataStore, ref bitOffset, 16, ref fieldValue);
            }

            if (result.Fine)
            {
                //template int(16) alternate_group = 0;
                result = Utility.AddNodeField(Position.CHILD, nodeBox, out newNode, "alternate_group", ItemType.FIELD, dataStore, ref bitOffset, 16, ref fieldValue);
            }

            if (result.Fine)
            {
                //template int(16) volume = {if track_is_audio 0x0100 else 0};
                result = Utility.AddNodeField(Position.CHILD, nodeBox, out newNode, "volume", ItemType.FIELD, dataStore, ref bitOffset, 16, ref fieldValue);
            }

            if (result.Fine)
            {
                //const unsigned int(16) reserved = 0;
                result = Utility.AddNodeField(Position.CHILD, nodeBox, out newNode, "reserved", ItemType.FIELD, dataStore, ref bitOffset, 16, ref fieldValue);
            }

            if (result.Fine)
            {
                //template int(32)[9] matrix={ 0x00010000,0,0,0,0x00010000,0,0,0,0x40000000 };// unity matrix
                result = Utility.AddNodeField(Position.CHILD, nodeBox, out newNode, "matrix", ItemType.FIELD, dataStore, ref bitOffset, 32 * 9, ref fieldValue);
            }
            
            if (result.Fine)
            {
                //unsigned int(32) width;
                result = Utility.AddNodeField(Position.CHILD, nodeBox, out newNode, "width", ItemType.FIELD, dataStore, ref bitOffset, 32, ref fieldValue);
            }

            if (result.Fine)
            {
                //unsigned int(32) height;
                result = Utility.AddNodeField(Position.CHILD, nodeBox, out newNode, "height", ItemType.FIELD, dataStore, ref bitOffset, 32, ref fieldValue);
            }

            if (result.Fine)
            {
                Utility.UpdateNodeLength(nodeBox, "TrackHeaderBox_payload", ItemType.SECTION1, bitOffset - firstOffset);
            }
        }
    }
}
