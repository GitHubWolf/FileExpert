using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Library;
using FileExpert.PRE;

namespace FileExpert.PRE
{
    class ParserPRE
    {
        public void ParsePRE(TreeView treeView, TreeNode parent, DataStore dataStore)
        {
            TreeNode nodeObject = null;
            TreeNode newNode = null;

            //Add one node to indicate this node.
            Result result = Utility.AddNodeContainer(Position.CHILD, parent, out nodeObject, "PRE", ItemType.SECTION2, dataStore, 0, dataStore.GetLeftBitLength());

            Int64 bitOffset = 0;
            Int64 fieldValue = 0;

            if (result.Fine)
            {
                result = Utility.AddNodeField(Position.CHILD, nodeObject, out newNode, "File Signature", ItemType.FIELD, dataStore, ref bitOffset, 32, ref fieldValue);
            }

            if (result.Fine)
            {
                result = Utility.AddNodeField(Position.CHILD, nodeObject, out newNode, "Total Header Size", ItemType.FIELD, dataStore, ref bitOffset, 32, ref fieldValue);
            }

            if (result.Fine)
            {
                result = Utility.AddNodeField(Position.CHILD, nodeObject, out newNode, "Offset to Encrypted Data", ItemType.FIELD, dataStore, ref bitOffset, 32, ref fieldValue);
            }

            Int64 formatVersion = 0;
            if (result.Fine)
            {
                result = Utility.AddNodeField(Position.CHILD, nodeObject, out newNode, "Format Version", ItemType.FIELD, dataStore, ref bitOffset, 16, ref formatVersion);
            }

            if (result.Fine)
            {
                if (formatVersion == 0x1)
                {
                    result = parseV1PRE(dataStore, nodeObject, ref newNode, ref bitOffset, ref fieldValue);
                }
                else if (formatVersion == 0x2)
                {
                    result = parseV2PRE(dataStore, nodeObject, ref newNode, ref bitOffset, ref fieldValue);
                }
            }

        }

        private Result parseV1PRE(DataStore dataStore, TreeNode nodeObject, ref TreeNode newNode, ref Int64 bitOffset, ref Int64 fieldValue)
        {
            Result result = new Result();

            if (result.Fine)
            {
                result = Utility.AddNodeField(Position.CHILD, nodeObject, out newNode, "Minimum Compatibility Version", ItemType.FIELD, dataStore, ref bitOffset, 16, ref fieldValue);
            }


            if (result.Fine)
            {
                result = Utility.AddNodeField(Position.CHILD, nodeObject, out newNode, "Cipher Type", ItemType.FIELD, dataStore, ref bitOffset, 32, ref fieldValue);
            }

            if (result.Fine)
            {
                result = Utility.AddNodeData(Position.CHILD, nodeObject, out newNode, "Cipher Specific Data", ItemType.FIELD, dataStore, ref bitOffset, 192);
            }

            Int64 originalFilenameLength = 0;
            if (result.Fine)
            {
                result = Utility.AddNodeField(Position.CHILD, nodeObject, out newNode, "Original Filename Length", ItemType.FIELD, dataStore, ref bitOffset, 16, ref originalFilenameLength);
            }

            Int64 playReadyObjectLength = 0;
            if (result.Fine)
            {
                result = Utility.AddNodeField(Position.CHILD, nodeObject, out newNode, "PlayReady Object Length", ItemType.FIELD, dataStore, ref bitOffset, 32, ref playReadyObjectLength);
            }

            string originalFilename = null;
            if (result.Fine)
            {
                result = Utility.GetText(out originalFilename, dataStore, bitOffset, originalFilenameLength * 8, true);
            }

            if (result.Fine)
            {
                result = Utility.AddNodeData(Position.CHILD, nodeObject, out newNode, "Original Filename:" + originalFilename, ItemType.FIELD, dataStore, ref bitOffset, originalFilenameLength * 8);
            }

            if (result.Fine)
            {
                TreeNode playReadyObjectListNode = null;
                result = Utility.AddNodeContainer(Position.CHILD, nodeObject, out playReadyObjectListNode, "PlayReady Object", ItemType.FIELD, dataStore, bitOffset, playReadyObjectLength * 8);
                if (result.Fine)
                {
                    result = ParsePlayReadyObjects(dataStore, playReadyObjectListNode, ref bitOffset, playReadyObjectLength * 8);
                }
            }


            if (result.Fine)
            {
                result = Utility.AddNodeData(Position.CHILD, nodeObject, out newNode, "Encrypted File data(incomplete)", ItemType.FIELD, dataStore, ref bitOffset, dataStore.GetLeftBitLength());
            }


            return result;
        }


        private Result parseV2PRE(DataStore dataStore, TreeNode nodeObject, ref TreeNode newNode, ref Int64 bitOffset, ref Int64 fieldValue)
        {
            Result result = new Result();

            if (result.Fine)
            {
                result = Utility.AddNodeField(Position.CHILD, nodeObject, out newNode, "Minimum Compatibility Version", ItemType.FIELD, dataStore, ref bitOffset, 16, ref fieldValue);
            }


            if (result.Fine)
            {
                result = Utility.AddNodeField(Position.CHILD, nodeObject, out newNode, "Cipher Type", ItemType.FIELD, dataStore, ref bitOffset, 32, ref fieldValue);
            }

            if (result.Fine)
            {
                result = Utility.AddNodeData(Position.CHILD, nodeObject, out newNode, "Cipher Specific Data", ItemType.FIELD, dataStore, ref bitOffset, 192);
            }

            Int64 originalFilenameLength = 0;
            if (result.Fine)
            {
                result = Utility.AddNodeField(Position.CHILD, nodeObject, out newNode, "Original Filename Length", ItemType.FIELD, dataStore, ref bitOffset, 16, ref originalFilenameLength);
            }

            Int64 playReadyObjectLength = 0;
            if (result.Fine)
            {
                result = Utility.AddNodeField(Position.CHILD, nodeObject, out newNode, "PlayReady Object Length", ItemType.FIELD, dataStore, ref bitOffset, 32, ref playReadyObjectLength);
            }

            string originalFilename = null;
            if (result.Fine)
            {
                result = Utility.GetText(out originalFilename, dataStore, bitOffset, originalFilenameLength * 8, true);
            }

            if (result.Fine)
            {
                result = Utility.AddNodeData(Position.CHILD, nodeObject, out newNode, "Original Filename:" + originalFilename, ItemType.FIELD, dataStore, ref bitOffset, originalFilenameLength * 8);
            }

            if (result.Fine)
            {
                TreeNode playReadyObjectListNode = null;
                result = Utility.AddNodeContainer(Position.CHILD, nodeObject, out playReadyObjectListNode, "PlayReady Object", ItemType.FIELD, dataStore, bitOffset, playReadyObjectLength * 8);
                if (result.Fine)
                {
                    result = ParsePlayReadyObjects(dataStore, playReadyObjectListNode, ref bitOffset, playReadyObjectLength * 8);
                }
            }

            Int64 customAttributesLength = 0;
            if (result.Fine)
            {
                result = Utility.AddNodeField(Position.CHILD, nodeObject, out newNode, "Custom Attributes Length", ItemType.FIELD, dataStore, ref bitOffset, 32, ref customAttributesLength);
            }

            if (result.Fine)
            {
                result = Utility.AddNodeField(Position.CHILD, nodeObject, out newNode, "Offset to Custom Attributes", ItemType.FIELD, dataStore, ref bitOffset, 32, ref fieldValue);
            }

            if (result.Fine)
            {
                result = Utility.AddNodeData(Position.CHILD, nodeObject, out newNode, "Custom Attributes", ItemType.FIELD, dataStore, ref bitOffset, customAttributesLength * 8);
            }


            if (result.Fine)
            {
                result = Utility.AddNodeData(Position.CHILD, nodeObject, out newNode, "Encrypted File data", ItemType.FIELD, dataStore, ref bitOffset, dataStore.GetLeftBitLength());
            }


            return result;
        }

        private Result ParsePlayReadyObjects(DataStore dataStore, TreeNode playReadyObjectListNode, ref long bitOffset, long maxBitLength)
        {
            Result result = new Result();
            TreeNode newNode = null;
            TreeNode objectNode = null;
            Int64 fieldValue = 0;

            if (result.Fine)
            {
                result = Utility.AddNodeFieldPlus(Position.CHILD, playReadyObjectListNode, out newNode, "Length", ItemType.FIELD, dataStore, ref bitOffset, 32, ref fieldValue, ref maxBitLength);
            }

            Int64 playReadyRecordCount = 0;
            if (result.Fine)
            {
                result = Utility.AddNodeFieldPlus(Position.CHILD, playReadyObjectListNode, out newNode, "PlayReady Record Count", ItemType.FIELD, dataStore, ref bitOffset, 16, ref playReadyRecordCount, ref maxBitLength);
            }



            while ((maxBitLength > 0) && result.Fine)
            {
                //Add one node to indicate this record.
                result = Utility.AddNodeContainer(Position.CHILD, playReadyObjectListNode, out objectNode, "Record", ItemType.ITEM, dataStore, bitOffset, 0);

                Int64 recordType = 0;
                if (result.Fine)
                {
                    result = Utility.AddNodeFieldPlus(Position.CHILD, objectNode, out newNode, "Record Type", ItemType.FIELD, dataStore, ref bitOffset, 16, ref recordType, ref maxBitLength);
                }

                Int64 recordLength = 0;
                if (result.Fine)
                {
                    result = Utility.AddNodeFieldPlus(Position.CHILD, objectNode, out newNode, "Record Length", ItemType.FIELD, dataStore, ref bitOffset, 16, ref recordLength, ref maxBitLength);
                }

                if (result.Fine)
                {
                    result = Utility.AddNodeDataPlus(Position.CHILD, objectNode, out newNode, "Record Value", ItemType.FIELD, dataStore, ref bitOffset, (recordLength)*8, ref maxBitLength);
                }              


                if (result.Fine)
                {
                    Utility.UpdateNodeLength(objectNode, "Record", ItemType.ITEM, (recordLength+4) * 8);
                }
            }

            return result;
        }


    }
}
