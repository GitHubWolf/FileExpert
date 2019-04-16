using FileExpert.MP4.Info;
using Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileExpert.MP4.Box
{
    class SampleDescriptionBox : FullBox
    {
        public SampleDescriptionBox(TreeView treeView, TreeNode parent, DataStore dataStore, ref Int64 bitOffset) :
            base(treeView, parent, dataStore, ref bitOffset)
        {
            TreeNode nodeBox = null;
            TreeNode newNode = null;

            Int64 firstOffset = bitOffset;

            //Add one node to indicate this box.
            Result result = Utility.AddNodeContainer(Position.CHILD, parent, out nodeBox, "SampleDescriptionBox_payload", ItemType.SECTION1, dataStore, bitOffset, dataStore.GetLeftBitLength());

            Int64 entryCount = 0;
            if (result.Fine)
            {
                //unsigned int(32) entry_count;
                result = Utility.AddNodeField(Position.CHILD, nodeBox, out newNode, "entry_count", ItemType.FIELD, dataStore, ref bitOffset, 32, ref entryCount);
            }

            string handlerTypeName = null;
            if (result.Fine)
            {
                //Save the info so that we can know the handler type which will be used to judge the payload type.
                MediaBoxInfo mediaBoxInfo = (MediaBoxInfo)Utility.FindItem(parent, typeof(MediaBoxInfo));
                if (mediaBoxInfo != null)
                {
                    handlerTypeName = mediaBoxInfo.HandlerTypeName;
                }

            }

            ParseMp4 parserMp4 = new ParseMp4();

            parserMp4.ParseInnerBox(treeView, nodeBox, dataStore, ref bitOffset);

            /*
            for (int i = 0; i < entryCount; i++)
            {
                if (result.Fine)
                {
                    if (handlerTypeName.Equals("soun", StringComparison.OrdinalIgnoreCase))
                    {
                        new AudioSampleEntry(treeView, nodeBox, dataStore, ref bitOffset);
                    }
                    else if (handlerTypeName.Equals("vide", StringComparison.OrdinalIgnoreCase))
                    {

                    }
                    else if (handlerTypeName.Equals("hint", StringComparison.OrdinalIgnoreCase))
                    {

                    }
                    else if (handlerTypeName.Equals("meta", StringComparison.OrdinalIgnoreCase))
                    {

                    }
                }
            }*/

            if (result.Fine)
            {
                Utility.UpdateNodeLength(nodeBox, "SampleDescriptionBox_payload", ItemType.SECTION1, bitOffset - firstOffset);
            }
        }
    }
}
