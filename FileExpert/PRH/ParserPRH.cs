using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Library;
using FileExpert.PRH;

namespace FileExpert.PRH
{
    class ParserPRH
    {
        public void ParsePRH(TreeView treeView, TreeNode parent, DataStore dataStore)
        {
            TreeNode nodeObject = null;
            TreeNode newNode = null;

            //Add one node to indicate this object.
            Result result = Utility.AddNodeContainer(Position.CHILD, parent, out nodeObject, "PlayReadyHeaderObject", ItemType.SECTION2, dataStore, 0, dataStore.GetLeftBitLength());

            Int64 bitOffset = 0;
            Int64 fieldValue = 0;

            if (result.Fine)
            {
                result = Utility.AddNodeField(Position.CHILD, nodeObject, out newNode, "Length", ItemType.FIELD, dataStore, ref bitOffset, 32, ref fieldValue);
            }

            if (result.Fine)
            {
                result = Utility.AddNodeField(Position.CHILD, nodeObject, out newNode, "PlayReady Record Count", ItemType.FIELD, dataStore, ref bitOffset, 16, ref fieldValue);
            }

            if (result.Fine)
            {
                ParseRecords(treeView, nodeObject, dataStore, ref bitOffset);
            }
        }

        private Result ParseRightsManagementHeader(DataStore dataStore, ref Int64 bitOffset, TreeNode nodeBox, ref TreeNode newNode, ref Int64 fieldValue, TreeNode payloadNode, long maxBitLength)
        {
            Result result = new Result();

            result = Utility.UpdateNode(nodeBox, "rights management header", ItemType.ITEM);

            if (result.Fine)
            {
                result = Utility.AddNodeDataPlus(Position.CHILD, payloadNode, out newNode, "rights management header data", ItemType.FIELD, dataStore, ref bitOffset, maxBitLength, ref maxBitLength);
            }

            return result;
        }

        private Result ParseEmbeddedLicenseStore(DataStore dataStore, ref Int64 bitOffset, TreeNode nodeBox, ref TreeNode newNode, ref Int64 fieldValue, TreeNode payloadNode, long maxBitLength)
        {
            Result result = new Result();

            result = Utility.UpdateNode(nodeBox, "embedded license store", ItemType.ITEM);


            if (result.Fine)
            {
                result = Utility.AddNodeFieldPlus(Position.CHILD, payloadNode, out newNode, "Header constants", ItemType.FIELD, dataStore, ref bitOffset, 32, ref fieldValue, ref maxBitLength);
            }

            if (result.Fine)
            {
                result = Utility.AddNodeFieldPlus(Position.CHILD, payloadNode, out newNode, "Store version", ItemType.FIELD, dataStore, ref bitOffset, 16, ref fieldValue, ref maxBitLength);
            }

            if (result.Fine)
            {
                result = Utility.AddNodeFieldPlus(Position.CHILD, payloadNode, out newNode, "Minimum compatibility version", ItemType.FIELD, dataStore, ref bitOffset, 16, ref fieldValue, ref maxBitLength);
            }

            if (result.Fine)
            {
                result = Utility.AddNodeFieldPlus(Position.CHILD, payloadNode, out newNode, "Store size", ItemType.FIELD, dataStore, ref bitOffset, 32, ref fieldValue, ref maxBitLength);
            }


            //Add one node to indicate License slots.
            TreeNode licenseSlotsNode = null;
            result = Utility.AddNodeContainer(Position.CHILD, payloadNode, out licenseSlotsNode, "License slots", ItemType.ITEM, dataStore, bitOffset, 0);

            while ((maxBitLength > 0) && result.Fine)
            {
                TreeNode licenseSlotNode = null;
                //Add one node to indicate this license slot.
                result = Utility.AddNodeContainer(Position.CHILD, licenseSlotsNode, out licenseSlotNode, "License slot", ItemType.ITEM, dataStore, bitOffset, 0);

                Int64 slotSize = 0;
                if (result.Fine)
                {
                    result = Utility.AddNodeFieldPlus(Position.CHILD, licenseSlotNode, out newNode, "Slot size", ItemType.FIELD, dataStore, ref bitOffset, 32, ref slotSize, ref maxBitLength);
                }

                if (result.Fine)
                {
                    result = Utility.AddNodeDataPlus(Position.CHILD, licenseSlotNode, out newNode, "Key ID", ItemType.FIELD, dataStore, ref bitOffset, 128, ref maxBitLength);
                }

                if (result.Fine)
                {
                    result = Utility.AddNodeDataPlus(Position.CHILD, licenseSlotNode, out newNode, "License ID", ItemType.FIELD, dataStore, ref bitOffset, 128, ref maxBitLength);
                }

                if (result.Fine)
                {
                    result = Utility.AddNodeDataPlus(Position.CHILD, licenseSlotNode, out newNode, "License priority", ItemType.FIELD, dataStore, ref bitOffset, 32, ref maxBitLength);
                }

                if (result.Fine)
                {
                    result = Utility.AddNodeDataPlus(Position.CHILD, licenseSlotNode, out newNode, "License data", ItemType.FIELD, dataStore, ref bitOffset, (slotSize * 8 - 32 - 128 - 128 - 32), ref maxBitLength);
                }


                if (result.Fine)
                {
                    Utility.UpdateNodeLength(licenseSlotNode, "License slot", ItemType.ITEM, (slotSize) * 8);
                }
            }


            return result;
        }

        public void ParseRecords(TreeView treeView, TreeNode parent, DataStore dataStore, ref Int64 bitOffset)
        {
            TreeNode nodeBox = null;
            TreeNode newNode = null;

            Result result = new Result();
            Int64 fieldValue = 0;


            while (0 != dataStore.GetLeftBitLength() && result.Fine)
            {

                //Add one node to indicate this record.
                result = Utility.AddNodeContainer(Position.CHILD, parent, out nodeBox, "Record", ItemType.ITEM, dataStore, bitOffset, 0);

                Int64 recordType = 0;
                if (result.Fine)
                {
                    result = Utility.AddNodeField(Position.CHILD, nodeBox, out newNode, "Record Type", ItemType.FIELD, dataStore, ref bitOffset, 16, ref recordType);
                }

                Int64 recordLength = 0;
                if (result.Fine)
                {
                    result = Utility.AddNodeField(Position.CHILD, nodeBox, out newNode, "Record Length", ItemType.FIELD, dataStore, ref bitOffset, 16, ref recordLength);
                }


                if (result.Fine)
                {
                    Utility.UpdateNodeLength(nodeBox, "Record", ItemType.ITEM, (recordLength) * 8);
                }

                TreeNode payloadNode = null;
                if (result.Fine)
                {
                    result = Utility.AddNodeContainer(Position.CHILD, nodeBox, out payloadNode, "Record Value", ItemType.FIELD, dataStore, bitOffset, (recordLength) * 8);

                }

                //The records needs to be parsed as network byte order.
                dataStore.SetByteOrder(true);
                if (result.Fine)
                {
                    //Not a container. Let's parse the record according to the type.
                    switch (recordType)
                    {
                        case 0x0001:
                            result = ParseRightsManagementHeader(dataStore, ref bitOffset, nodeBox, ref newNode, ref fieldValue, payloadNode, (recordLength) * 8);
                            break;
                        case 0x0003:
                            result = ParseEmbeddedLicenseStore(dataStore, ref bitOffset, nodeBox, ref newNode, ref fieldValue, payloadNode, (recordLength) * 8);
                            break;

                        default:
                            if (result.Fine) 
                            {
                                result = Utility.AddNodeData(Position.CHILD, payloadNode, out newNode, "Unknown Record", ItemType.FIELD, dataStore, ref bitOffset, (recordLength) * 8);
                            }

                            Console.WriteLine("unknown record " + String.Format("0x{0,4:X2}", recordType));
                            break;
                    }
                }

                dataStore.SetByteOrder(false);

            }
        }


    }
}
