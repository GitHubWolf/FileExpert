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
    class HandlerBox : FullBox
    {
        protected string handlerTypeName = null;
        protected Int64 handlerType = 0;
 
        public HandlerBox(TreeView treeView, TreeNode parent, DataStore dataStore, ref Int64 bitOffset) :
            base(treeView, parent, dataStore, ref bitOffset)
        {
            TreeNode nodeBox = null;
            TreeNode newNode = null;

            Int64 firstOffset = bitOffset;

            //Add one node to indicate this box.
            Result result = Utility.AddNodeContainer(Position.CHILD, parent, out nodeBox, "HandlerBox_payload", ItemType.SECTION1, dataStore, bitOffset, dataStore.GetLeftBitLength());

            Int64 fieldValue = 0;


            if (result.Fine)
            {
                //unsigned int(32) pre_defined = 0;
                result = Utility.AddNodeField(Position.CHILD, nodeBox, out newNode, "pre_defined", ItemType.FIELD, dataStore, ref bitOffset, 32, ref fieldValue);
            }

            
            if (result.Fine)
            {
                //unsigned int(32) handler_type;
                result = Utility.GetText(out handlerTypeName, dataStore, bitOffset, 32);
            }

            if (result.Fine)
            {
                //unsigned int(32) handler_type;
                result = Utility.AddNodeField(Position.CHILD, nodeBox, out newNode, "handler_type:" + handlerTypeName, ItemType.FIELD, dataStore, ref bitOffset, 32, ref handlerType);

                //Save the info so that we can know the handler type which will be used to judge the payload type.
                MediaBoxInfo mediaBoxInfo = (MediaBoxInfo)Utility.FindItem(nodeBox, typeof(MediaBoxInfo));
                if (mediaBoxInfo != null)
                {
                    mediaBoxInfo.HandlerTypeName = handlerTypeName;
                    mediaBoxInfo.HandlerType = handlerType;
                }
            }


            if (result.Fine)
            {
                //const unsigned int(32)[3] reserved = 0;
                result = Utility.AddNodeField(Position.CHILD, nodeBox, out newNode, "reserved", ItemType.FIELD, dataStore, ref bitOffset, 32 * 3, ref fieldValue);
            }

            string name = null;
            if (result.Fine)
            {
                //string name;
                result = Utility.GetText(out name, dataStore, bitOffset, dataStore.GetLeftBitLength());
            }

            if (result.Fine)
            {
                //string name;
                result = Utility.AddNodeField(Position.CHILD, nodeBox, out newNode, "name:" + name, ItemType.FIELD, dataStore, ref bitOffset, dataStore.GetLeftBitLength(), ref fieldValue);
            }

            if (result.Fine)
            {
                Utility.UpdateNodeLength(nodeBox, "HandlerBox_payload", ItemType.SECTION1, bitOffset - firstOffset);
            }
        }
    }
}
