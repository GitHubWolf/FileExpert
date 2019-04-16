using Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileExpert.MP4.Box
{
    class AudioSampleEntry : SampleEntry
    {
        public AudioSampleEntry(TreeView treeView, TreeNode parent, DataStore dataStore, ref Int64 bitOffset) :
            base(treeView, parent, dataStore, ref bitOffset)
        {
            TreeNode nodeBox = null;
            TreeNode newNode = null;

            //Add one node to indicate this box.
            Result result = Utility.AddNodeContainer(Position.CHILD, parent, out nodeBox, "AudioSampleEntry_payload", ItemType.SECTION1, dataStore, bitOffset, dataStore.GetLeftBitLength());

            Int64 fieldValue = 0;

            if (result.Fine)
            {
                //const unsigned int(32)[2] reserved = 0;
                result = Utility.AddNodeField(Position.CHILD, nodeBox, out newNode, "reserved", ItemType.FIELD, dataStore, ref bitOffset, 32*2, ref fieldValue);
            }

            if (result.Fine)
            {
                //template unsigned int(16) channelcount = 2;
                result = Utility.AddNodeField(Position.CHILD, nodeBox, out newNode, "channelcount", ItemType.FIELD, dataStore, ref bitOffset, 16, ref fieldValue);
            }
            
            if (result.Fine)
            {
                //template unsigned int(16) samplesize = 2;
                result = Utility.AddNodeField(Position.CHILD, nodeBox, out newNode, "samplesize", ItemType.FIELD, dataStore, ref bitOffset, 16, ref fieldValue);
            }

            if (result.Fine)
            {
                //unsigned int(16) pre_defined = 0;
                result = Utility.AddNodeField(Position.CHILD, nodeBox, out newNode, "pre_defined", ItemType.FIELD, dataStore, ref bitOffset, 16, ref fieldValue);
            }

            if (result.Fine)
            {
                //const unsigned int(16) reserved = 0 ;
                result = Utility.AddNodeField(Position.CHILD, nodeBox, out newNode, "reserved", ItemType.FIELD, dataStore, ref bitOffset, 16, ref fieldValue);
            }

            if (result.Fine)
            {
                //template unsigned int(32) samplerate = { default samplerate of media}<<16;
                result = Utility.AddNodeField(Position.CHILD, nodeBox, out newNode, "samplerate", ItemType.FIELD, dataStore, ref bitOffset, 32, ref fieldValue);
            }


            if (result.Fine)
            {
                Utility.UpdateNode(nodeBox, "AudioSampleEntry_payload", ItemType.SECTION1);
            }
        }
    }
}
