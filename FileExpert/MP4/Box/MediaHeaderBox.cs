using Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileExpert.MP4.Box
{
    class MediaHeaderBox : FullBox
    {
        public MediaHeaderBox(TreeView treeView, TreeNode parent, DataStore dataStore, ref Int64 bitOffset) :
            base(treeView, parent, dataStore, ref bitOffset)
        {
            TreeNode nodeBox = null;
            TreeNode newNode = null;

            Int64 firstOffset = bitOffset;

            //Add one node to indicate this box.
            Result result = Utility.AddNodeContainer(Position.CHILD, parent, out nodeBox, "MediaHeaderBox_payload", ItemType.SECTION1, dataStore, bitOffset, dataStore.GetLeftBitLength());

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
                //bit(1) pad = 0;
                result = Utility.AddNodeField(Position.CHILD, nodeBox, out newNode, "pad", ItemType.FIELD, dataStore, ref bitOffset, 1, ref fieldValue);
            }

            if (result.Fine)
            {
                //unsigned int(5)[3] language; // ISO-639-2/T language code
                result = Utility.AddNodeField(Position.CHILD, nodeBox, out newNode, "language", ItemType.FIELD, dataStore, ref bitOffset, 5*3, ref fieldValue);
            }

            if (result.Fine)
            {
                //unsigned int(16) pre_defined = 0;
                result = Utility.AddNodeField(Position.CHILD, nodeBox, out newNode, "pre_defined", ItemType.FIELD, dataStore, ref bitOffset, 16, ref fieldValue);
            }

            if (result.Fine)
            {
                Utility.UpdateNodeLength(nodeBox, "MediaHeaderBox_payload", ItemType.SECTION1, bitOffset - firstOffset);
            }
        }
    }
}
