using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Library
{
    public class Utility
    {
        private static UInt32[] crc32Table = {
	            0x00000000, 0x04C11DB7, 0x09823B6E, 0x0D4326D9, 0x130476DC, 0x17C56B6B,
	            0x1A864DB2, 0x1E475005, 0x2608EDB8, 0x22C9F00F, 0x2F8AD6D6, 0x2B4BCB61,
	            0x350C9B64, 0x31CD86D3, 0x3C8EA00A, 0x384FBDBD, 0x4C11DB70, 0x48D0C6C7,
	            0x4593E01E, 0x4152FDA9, 0x5F15ADAC, 0x5BD4B01B, 0x569796C2, 0x52568B75,
	            0x6A1936C8, 0x6ED82B7F, 0x639B0DA6, 0x675A1011, 0x791D4014, 0x7DDC5DA3,
	            0x709F7B7A, 0x745E66CD, 0x9823B6E0, 0x9CE2AB57, 0x91A18D8E, 0x95609039,
	            0x8B27C03C, 0x8FE6DD8B, 0x82A5FB52, 0x8664E6E5, 0xBE2B5B58, 0xBAEA46EF,
	            0xB7A96036, 0xB3687D81, 0xAD2F2D84, 0xA9EE3033, 0xA4AD16EA, 0xA06C0B5D,
	            0xD4326D90, 0xD0F37027, 0xDDB056FE, 0xD9714B49, 0xC7361B4C, 0xC3F706FB,
	            0xCEB42022, 0xCA753D95, 0xF23A8028, 0xF6FB9D9F, 0xFBB8BB46, 0xFF79A6F1,
	            0xE13EF6F4, 0xE5FFEB43, 0xE8BCCD9A, 0xEC7DD02D, 0x34867077, 0x30476DC0,
	            0x3D044B19, 0x39C556AE, 0x278206AB, 0x23431B1C, 0x2E003DC5, 0x2AC12072,
	            0x128E9DCF, 0x164F8078, 0x1B0CA6A1, 0x1FCDBB16, 0x018AEB13, 0x054BF6A4,
	            0x0808D07D, 0x0CC9CDCA, 0x7897AB07, 0x7C56B6B0, 0x71159069, 0x75D48DDE,
	            0x6B93DDDB, 0x6F52C06C, 0x6211E6B5, 0x66D0FB02, 0x5E9F46BF, 0x5A5E5B08,
	            0x571D7DD1, 0x53DC6066, 0x4D9B3063, 0x495A2DD4, 0x44190B0D, 0x40D816BA,
	            0xACA5C697, 0xA864DB20, 0xA527FDF9, 0xA1E6E04E, 0xBFA1B04B, 0xBB60ADFC,
	            0xB6238B25, 0xB2E29692, 0x8AAD2B2F, 0x8E6C3698, 0x832F1041, 0x87EE0DF6,
	            0x99A95DF3, 0x9D684044, 0x902B669D, 0x94EA7B2A, 0xE0B41DE7, 0xE4750050,
	            0xE9362689, 0xEDF73B3E, 0xF3B06B3B, 0xF771768C, 0xFA325055, 0xFEF34DE2,
	            0xC6BCF05F, 0xC27DEDE8, 0xCF3ECB31, 0xCBFFD686, 0xD5B88683, 0xD1799B34,
	            0xDC3ABDED, 0xD8FBA05A, 0x690CE0EE, 0x6DCDFD59, 0x608EDB80, 0x644FC637,
	            0x7A089632, 0x7EC98B85, 0x738AAD5C, 0x774BB0EB, 0x4F040D56, 0x4BC510E1,
	            0x46863638, 0x42472B8F, 0x5C007B8A, 0x58C1663D, 0x558240E4, 0x51435D53,
	            0x251D3B9E, 0x21DC2629, 0x2C9F00F0, 0x285E1D47, 0x36194D42, 0x32D850F5,
	            0x3F9B762C, 0x3B5A6B9B, 0x0315D626, 0x07D4CB91, 0x0A97ED48, 0x0E56F0FF,
	            0x1011A0FA, 0x14D0BD4D, 0x19939B94, 0x1D528623, 0xF12F560E, 0xF5EE4BB9,
	            0xF8AD6D60, 0xFC6C70D7, 0xE22B20D2, 0xE6EA3D65, 0xEBA91BBC, 0xEF68060B,
	            0xD727BBB6, 0xD3E6A601, 0xDEA580D8, 0xDA649D6F, 0xC423CD6A, 0xC0E2D0DD,
	            0xCDA1F604, 0xC960EBB3, 0xBD3E8D7E, 0xB9FF90C9, 0xB4BCB610, 0xB07DABA7,
	            0xAE3AFBA2, 0xAAFBE615, 0xA7B8C0CC, 0xA379DD7B, 0x9B3660C6, 0x9FF77D71,
	            0x92B45BA8, 0x9675461F, 0x8832161A, 0x8CF30BAD, 0x81B02D74, 0x857130C3,
	            0x5D8A9099, 0x594B8D2E, 0x5408ABF7, 0x50C9B640, 0x4E8EE645, 0x4A4FFBF2,
	            0x470CDD2B, 0x43CDC09C, 0x7B827D21, 0x7F436096, 0x7200464F, 0x76C15BF8,
	            0x68860BFD, 0x6C47164A, 0x61043093, 0x65C52D24, 0x119B4BE9, 0x155A565E,
	            0x18197087, 0x1CD86D30, 0x029F3D35, 0x065E2082, 0x0B1D065B, 0x0FDC1BEC,
	            0x3793A651, 0x3352BBE6, 0x3E119D3F, 0x3AD08088, 0x2497D08D, 0x2056CD3A,
	            0x2D15EBE3, 0x29D4F654, 0xC5A92679, 0xC1683BCE, 0xCC2B1D17, 0xC8EA00A0,
	            0xD6AD50A5, 0xD26C4D12, 0xDF2F6BCB, 0xDBEE767C, 0xE3A1CBC1, 0xE760D676,
	            0xEA23F0AF, 0xEEE2ED18, 0xF0A5BD1D, 0xF464A0AA, 0xF9278673, 0xFDE69BC4,
	            0x89B8FD09, 0x8D79E0BE, 0x803AC667, 0x84FBDBD0, 0x9ABC8BD5, 0x9E7D9662,
	            0x933EB0BB, 0x97FFAD0C, 0xAFB010B1, 0xAB710D06, 0xA6322BDF, 0xA2F33668,
	            0xBCB4666D, 0xB8757BDA, 0xB5365D03, 0xB1F740B4
            };

        public static UInt32 GetCrc32(byte[] data, Int64 offset, Int64 length)
        {
            UInt32 result = 0xFFFFFFFF;

            for (Int64 i = 0; i < length; i++)
                result = (result << 8) ^ crc32Table[((result >> 24) ^ (data[offset + i])) & 0xFF];

            return result;
        }
        //To read in bits from the byte array. Maximum 64bits is supported.
        public static Result ByteArrayReadBits(byte[] data, ref Int64 dataLeftInBit, ref Int64 bitOffset, Int64 bitLength, ref Int64 fieldValue, bool readAsNetworkByteOrder = true)
        {
            Result result = new Result();

            //Check whether there is still data left.
            if (bitLength > dataLeftInBit)
            {
                result.SetResult(ResultCode.INSUFFICIENT_DATA);
            }

            if (result.Fine)
            {
                //Reset the fieldValue.
                fieldValue = 0;

                if ((0 == (bitOffset%8)) && (0 == (bitLength%8)))
                {
                    Int64 lengthInByte = bitLength / 8;
                    Int64 offsetInByte = bitOffset / 8;

                    //Network byte order, i.e. big-endian.
                    if (readAsNetworkByteOrder)
                    {
                        //If it is byte-aligned, copy the bytes directly.
                        for (Int64 i = 0; i < lengthInByte; ++i)
                        {
                            fieldValue = fieldValue << 8;
                            fieldValue = fieldValue | data[offsetInByte + i];
                        }
                    }
                    else
                    {
                        //Little endian.


                        //If it is byte-aligned, copy the bytes directly.
                        for (Int64 i = 0; i < lengthInByte; ++i)
                        {
                            fieldValue = ((Int64)data[offsetInByte + i] << (8 * (int)i)) | fieldValue;

                        }
                    }

                }
                else
                {
                    Int64 currentBit = 0;
                    byte currentByte = 0;

                    Int64 currentBitOffset = 0;

                    Int64 offsetInByte = 0;

                    //Not byte-aligned, get the fieldValue bit by bit.
                    for (Int64 i = 0; i < bitLength; ++i )
                    {
                        currentBitOffset = bitOffset + i;
                        offsetInByte = currentBitOffset / 8;

                        //Get the byte to read in one bit.
                        currentByte = data[offsetInByte];

                        //Read in one bit.
                        currentBit = (currentByte >> (byte)(7 - (currentBitOffset % 8))) & 0x1;

                        if(readAsNetworkByteOrder)
                        {
                            //Write in one bit.
                            fieldValue = fieldValue << 1;
                            fieldValue = fieldValue | currentBit;
                        }
                        else
                        {
                            fieldValue = (currentBit << (int)i) | fieldValue;
                        }

                    }
                }

                //Update the bit offset now.
                bitOffset += bitLength;

                //Decrease the length in bits that are left.
                dataLeftInBit -= bitLength;
            }
            return result;
        }

        public static Result ByteArraySkipBits(ref Int64 dataLeftInBit, ref Int64 bitOffset, Int64 bitLength)
        {
            Result result = new Result();

            //Check whether there is still data left.
            if (bitLength > dataLeftInBit)
            {
                result.SetResult(ResultCode.INSUFFICIENT_DATA);
            }
            else
            {
                //Update the bit offset now.
                bitOffset += bitLength;

                //Decrease the length in bits that are left.
                dataLeftInBit -= bitLength;
            }

            return result;
        }

        public static String GetValueBinaryString(Int64 fieldValue, Int64 bitLength)
        {
            String binaryString = null;

            for (Int64 i = 0; i < bitLength; ++i)
            {
                if ((0 == ((i) % 8)) && (i != 0))
                {
                    binaryString = " " + binaryString;
                }

                if ((fieldValue & 0x1) == 0x1)
                {
                    binaryString = "1" + binaryString;
                }
                else
                {
                    binaryString = "0" + binaryString;
                }
                fieldValue = fieldValue >> 1;
            }
            return binaryString;
        }

        public static String GetValueHexString(Int64 fieldValue, Int64 bitLength)
        {
            String hexString = null;
            Int64 byteLength = 0;

            byteLength = bitLength / 8;
            if ((bitLength % 8) != 0)
            {
                byteLength++;
            }

            switch (byteLength)
            {
                case 1:
                    hexString = String.Format("0x{0,2:X2}", fieldValue);
                    break;
                case 2:
                    hexString = String.Format("0x{0,4:X4}", fieldValue);
                    break;
                case 3:
                    hexString = String.Format("0x{0,6:X6}", fieldValue);
                    break;
                case 4:
                    hexString = String.Format("0x{0,8:X8}", fieldValue);
                    break;
                case 5:
                    hexString = String.Format("0x{0,10:X10}", fieldValue);
                    break;
                case 6:
                    hexString = String.Format("0x{0,12:X12}", fieldValue);
                    break;
                case 7:
                    hexString = String.Format("0x{0,14:X14}", fieldValue);
                    break;
                case 8:
                    hexString = String.Format("0x{0,16:X16}", fieldValue);
                    break;
            }

            return hexString;
        }

        public static String GetValueDecimalString(Int64 fieldValue, Int64 bitLength)
        {
            String decString = null;

            decString = String.Format("{0}", fieldValue);

            return decString;
        }

        public static String GetItemPicture(ItemType itemType)
        {
            String pictureFileName = null;
            switch (itemType)
            {
                case ItemType.ROOT:
                    pictureFileName = "Root.png";
                    break;
                case ItemType.TOP_GROUP1:
                    pictureFileName = "TopGroup1.png";
                    break;
                case ItemType.TOP_GROUP2:
                    pictureFileName = "TopGroup2.png";
                    break;
                case ItemType.STATISTIC:
                    pictureFileName = "Statistic.png";
                    break;
                case ItemType.SECTION1:
                    pictureFileName = "Section1.png";
                    break;
                case ItemType.SECTION2:
                    pictureFileName = "Section2.png";
                    break;
                case ItemType.FIELD:
                    pictureFileName = "Field.png";
                    break;
                case ItemType.DATA:
                    pictureFileName = "Data.png";
                    break;
                case ItemType.LOOP:
                    pictureFileName = "Loop.png";
                    break;
                case ItemType.ITEM:
                    pictureFileName = "Item.png";
                    break;
                case ItemType.WARNING:
                    pictureFileName = "Warning.png";
                    break;
                case ItemType.ERROR:
                    pictureFileName = "Error.png";
                    break;
                case ItemType.SEARCH:
                    pictureFileName = "SearchRequest.png";
                    break;
            }
            return pictureFileName;
        }

        public static void ExpandToRoot(TreeNode newNode, ItemType itemType)
        {
            if ((ItemType.ERROR == itemType) || (ItemType.WARNING == itemType))
            {
                while (null != newNode)
                {
                    newNode.Expand();

                    newNode = newNode.Parent;
                }
            }
        }

        public static TreeNode AddChildNode(TreeView treeView, TreeNode parentNode, String key, String text, ItemType itemType)
        {
            TreeNode resultNode = null;

            if (null == parentNode)
            {
                resultNode = treeView.Nodes.Add(key, text, Utility.GetItemPicture(itemType), Utility.GetItemPicture(itemType));
            }
            else
            {
                resultNode = parentNode.Nodes.Add(key, text, Utility.GetItemPicture(itemType), Utility.GetItemPicture(itemType));
            }

            return resultNode;
            
        }

        public static Result UpdateNode(TreeNode nodeToUpdate, String text, ItemType itemType)
        {
            Result result = new Result();

            if (null != nodeToUpdate)
            {
                if (nodeToUpdate.Tag is FieldItem)
                {
                    FieldItem item = (FieldItem)nodeToUpdate.Tag;
                    nodeToUpdate.ImageKey = Utility.GetItemPicture(itemType);
                    nodeToUpdate.SelectedImageKey = Utility.GetItemPicture(itemType);

                    if (null == item)
                    {
                        nodeToUpdate.Text = text;
                    }
                    else
                    {

                        String nodeText;
                        if ((item.Length <= 64) && ((ItemType.DATA == itemType) || (ItemType.FIELD == itemType)))
                        {
                            nodeText = text + "[" + item.Length + "]" + ":" + Utility.GetValueHexString(item.Value, item.Length);
                        }
                        else
                        {
                            nodeText = text + "[" + item.Length + "]";
                        }

                        //Format sectionDetail.
                        nodeToUpdate.Text = nodeText;// +"[" + item.Length + "]";
                    }

                    //Expand if necessary;
                    ExpandToRoot(nodeToUpdate, itemType);                    
                }

            }


            return result;
        }

        public static Result UpdateNodeLength(TreeNode nodeToUpdate, String text, ItemType itemType, Int64 bitLength)
        {
            Result result = new Result();

            if (null != nodeToUpdate)
            {
                if (nodeToUpdate.Tag is FieldItem)
                {
                    FieldItem item = (FieldItem)nodeToUpdate.Tag;
                    nodeToUpdate.ImageKey = Utility.GetItemPicture(itemType);
                    nodeToUpdate.SelectedImageKey = Utility.GetItemPicture(itemType);

                    if (null == item)
                    {
                        nodeToUpdate.Text = text;
                    }
                    else
                    {
                        //Update the length!!!!!!!!!!!!!!
                        item.Length = bitLength;

                        String nodeText;
                        if( (bitLength <= 64) && ((ItemType.DATA == itemType) || (ItemType.FIELD == itemType)))
                        {
                            nodeText = text + "[" + bitLength + "]" + ":" + Utility.GetValueHexString(item.Value, bitLength);
                        }
                        else
                        {
                            nodeText = text + "[" + bitLength + "]";
                        }

                        //Format sectionDetail.
                        nodeToUpdate.Text = nodeText;// +"[" + item.Length + "]";
                        item.Type = itemType;
                    }

                    //Expand if necessary;
                    ExpandToRoot(nodeToUpdate, itemType);
                }
            }


            return result;
        }

        //Add a node containing fieldValue for specific field. bitOffset will be increased.
        public static Result AddNodeField(Position position, TreeNode referenceNode, out TreeNode newNode, String text, ItemType itemType, DataStore dataStore, ref Int64 bitOffset, Int64 bitLength, ref Int64 fieldValue)
        {
            Result result = new Result();

            newNode = null;

            if (result.Fine)
            {
                //ReadBits will increase the offset!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                result = dataStore.ReadBits(ref bitOffset, bitLength, ref fieldValue);
            }

            if (result.Fine)
            {
                //The fieldValue has been read successfully. Add it into the node.

                String nodeText = null;

                //Give the format sectionDetail.
                if ((bitLength <= 64) && ((ItemType.DATA == itemType) || (ItemType.FIELD == itemType)))
                {
                    nodeText = text + "[" + bitLength + "]" + ":" + Utility.GetValueHexString(fieldValue, bitLength);
                }
                else
                {
                    nodeText = text + "[" + bitLength + "]";
                }
                if (position == Position.CHILD)
                {
                    newNode = referenceNode.Nodes.Add(text, nodeText, Utility.GetItemPicture(itemType), Utility.GetItemPicture(itemType));
                }
                else if (position == Position.PEER)
                {
                    newNode = referenceNode.Parent.Nodes.Insert(referenceNode.Index + 1, text, nodeText, Utility.GetItemPicture(itemType), Utility.GetItemPicture(itemType));
                }

                newNode.Tag = new FieldItem(itemType, dataStore, (bitOffset - bitLength), bitLength, fieldValue);//bitOffset contains the fieldValue after the filed has been read.
            }

            //Expand if necessary;
            ExpandToRoot(newNode, itemType);

            return result;
        }

        //Add a node containing fieldValue for specific field. bitOffset will be increased. Additional length checking will be performed according to maxLeftBits.
        public static Result AddNodeFieldPlus(Position position, TreeNode referenceNode, out TreeNode newNode, String text, ItemType itemType, DataStore dataStore, ref Int64 bitOffset, Int64 bitLength, ref Int64 fieldValue, ref Int64 maxLeftBits)
        {
            Result result = new Result();

            newNode = null;

            if (result.Fine)
            {
                //Check the maxLeftBits agains the length to be read.
                if (bitLength > maxLeftBits)
                {
                    result.SetResult(ResultCode.INSUFFICIENT_DATA);
                }

            }

            if (result.Fine)
            {
                result = AddNodeField(position, referenceNode, out newNode, text, itemType, dataStore, ref bitOffset, bitLength, ref fieldValue);
            }

            if (result.Fine)
            {
                maxLeftBits -= bitLength;//Decrease the length!!!!!!!!!!!!!!
            }

            return result;
        }



        //Add a node pointing to a block of data. bitOffset will be increased.
        public static Result AddNodeData(Position position, TreeNode referenceNode, out TreeNode newNode, String text, ItemType itemType, DataStore dataStore, ref Int64 bitOffset, Int64 bitLength)
        {
            Result result = new Result();

            Int64 fieldValue = 0;

            newNode = null;

            if (bitLength <= 64)
            {
                //ReadBits will increase the offset!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                result = dataStore.ReadBits(ref bitOffset, bitLength, ref fieldValue);
            }
            else
            {
                //ReadBits will increase the offset!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                result = dataStore.SkipBits(ref bitOffset, bitLength);
            }

            if (result.Fine)
            {
                //The fieldValue has been read successfully. Add it into the node.

                String nodeText = null;

                //Give the format sectionDetail.
                nodeText = text + "[" + bitLength + "]";

                if (position == Position.CHILD)
                {
                    newNode = referenceNode.Nodes.Add(text, nodeText, Utility.GetItemPicture(itemType), Utility.GetItemPicture(itemType));
                }
                else if (position == Position.PEER)
                {
                    newNode = referenceNode.Parent.Nodes.Insert(referenceNode.Index + 1, text, nodeText, Utility.GetItemPicture(itemType), Utility.GetItemPicture(itemType));
                }

                newNode.Tag = new FieldItem(itemType, dataStore, (bitOffset - bitLength), bitLength, fieldValue);
            }

            //Expand if necessary;
            ExpandToRoot(newNode, itemType);

            return result;
        }

        //Add a node pointing to a block of data. bitOffset will be increased. Additional length checking will be performed according to maxLeftBits.
        public static Result AddNodeDataPlus(Position position, TreeNode referenceNode, out TreeNode newNode, String text, ItemType itemType, DataStore dataStore, ref Int64 bitOffset, Int64 bitLength, ref Int64 maxLeftBits)
        {
            Result result = new Result();

            newNode = null;

            if (result.Fine)
            {
                //Check the maxLeftBits agains the length to be read.
                if (bitLength > maxLeftBits)
                {
                    result.SetResult(ResultCode.INSUFFICIENT_DATA);
                }

            }

            if (result.Fine)
            {
                result = AddNodeData(position, referenceNode, out newNode, text, itemType, dataStore, ref bitOffset, bitLength);
            }

            if (result.Fine)
            {
                maxLeftBits -= bitLength;//Decrease the length!!!!!!!!!!!!!!
            }

            return result;
        }


        //Add a node pointing to a block of data. bitOffset will NOT be increased!!!!!!!
        public static Result AddNodeContainer(Position position, TreeNode referenceNode, out TreeNode newNode, String text, ItemType itemType, DataStore dataStore, Int64 bitOffset, Int64 bitLength)
        {
            Result result = new Result();

            newNode = null;

            if (result.Fine)
            {
                byte[] data = dataStore.GetData();
                Int64 dataLengthInBit = dataStore.GetLeftBitLength();

                //Check the length.
                if (bitLength > dataLengthInBit)
                {
                    result.SetResult(ResultCode.INSUFFICIENT_DATA);
                }
            }

            if (result.Fine)
            {
                String nodeText = text + "[" + bitLength + "]";

                if (position == Position.CHILD)
                {
                    newNode = referenceNode.Nodes.Add(text, nodeText, Utility.GetItemPicture(itemType), Utility.GetItemPicture(itemType));
                }
                else if (position == Position.PEER)
                {
                    newNode = referenceNode.Parent.Nodes.Insert(referenceNode.Index + 1, text, nodeText, Utility.GetItemPicture(itemType), Utility.GetItemPicture(itemType));
                }
                newNode.Tag = new FieldItem(itemType, dataStore, bitOffset, bitLength, 0);
            }

            //Expand if necessary;
            ExpandToRoot(newNode, itemType);

            return result;
        }

        //Add a node pointing to a block of data. bitOffset will NOT be increased!!!!!!!Additional length checking will be performed according to maxLeftBits.
        public static Result AddNodeContainerPlus(Position position, TreeNode referenceNode, out TreeNode newNode, String text, ItemType itemType, DataStore dataStore, Int64 bitOffset, Int64 bitLength, Int64 maxLeftBits)
        {
            Result result = new Result();

            newNode = null;

            if (result.Fine)
            {
                //Check the maxLeftBits agains the length to be read.
                if (bitLength > maxLeftBits)
                {
                    result.SetResult(ResultCode.INSUFFICIENT_DATA);
                }

            }

            if (result.Fine)
            {
                result = AddNodeContainer(position, referenceNode, out newNode, text, itemType, dataStore, bitOffset, bitLength);
            }


            return result;
        }


        public static Result AddEndingFieldNode(TreeNode parentNode, out TreeNode newNode, String text, ItemType itemType, DataStore dataStore, Int64 bitLength, ref Int64 fieldValue)
        {
            Result result = new Result();

            Int64 bitOffsetForThisField = 0;

            newNode = null;

            if (result.Fine)
            {
                //bitOffsetForThisField will return the offset of this field.
                result = dataStore.ReadEndingBits(ref bitOffsetForThisField, bitLength, ref fieldValue);
            }

            if (result.Fine)
            {
                //The fieldValue has been read successfully. Add it into the node.

                String nodeText = null;

                //Give the format sectionDetail.
                if (bitLength <= 64)
                {
                    nodeText = text + "[" + bitLength + "]" + ":" + Utility.GetValueHexString(fieldValue, bitLength);
                }
                else
                {
                    nodeText = text + "[" + bitLength + "]";
                }
                newNode = parentNode.Nodes.Add(text, nodeText, Utility.GetItemPicture(itemType), Utility.GetItemPicture(itemType));
                newNode.Tag = new FieldItem(itemType, dataStore, bitOffsetForThisField, bitLength, fieldValue);
            }

            //Expand if necessary;
            ExpandToRoot(newNode, itemType);

            return result;
        }

        public static Result AddCrc32Node(TreeNode parentNode, out TreeNode newNode, DataStore dataStore, ref Int64 fieldValue)
        {
            Result result = new Result();

            //Check CRC32.
            if (0 == Utility.GetCrc32(dataStore.GetData(), 0, dataStore.GetData().Length))
            {
                result = AddEndingFieldNode(parentNode, out newNode, "CRC_32", ItemType.FIELD, dataStore, 32, ref fieldValue);
            }
            else
            {
                result = AddEndingFieldNode(parentNode, out newNode, "CRC_32(invalid)", ItemType.ERROR, dataStore, 32, ref fieldValue);
            }

            return result;
        }

        public static Result AddLastSecondNodeData(TreeNode parentNode, TreeNode lastNode, out TreeNode newNode, DataStore dataStore, ItemType itemType, string text, ref Int64 bitOffset, Int64 bitLength)
        {
            Result result = new Result();

            newNode = null;

            if (bitLength > 0)
            {
                Int64 bitOffsetTemp = bitOffset;//SkipBits will increase the offset!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                if (result.Fine)
                {
                    //SkipBits will increase the offset!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    result = dataStore.SkipBits(ref bitOffset, bitLength);
                }

                if (result.Fine)
                {
                    String nodeText = null;

                    //Give the format sectionDetail.
                    nodeText = text + "[" + bitLength + "]";

                    if (lastNode == null)
                    {
                        //We are the last node.
                        newNode = parentNode.Nodes.Add(text, nodeText, Utility.GetItemPicture(itemType), Utility.GetItemPicture(itemType));
                    }
                    else
                    {
                        //We are the last second node. !!!!!
                        newNode = parentNode.Nodes.Insert(lastNode.Index, text, nodeText, Utility.GetItemPicture(itemType), Utility.GetItemPicture(itemType));
                    }


                    newNode.Tag = new FieldItem(itemType, dataStore, bitOffsetTemp, bitLength, 0);
                }

                //Expand if necessary;
                ExpandToRoot(newNode, itemType);

            }

            return result;
        }//AddLastSecondNodeData

        //Get text from the buffer!!!!!!!
        public static Result GetText(out string text, DataStore dataStore, Int64 bitOffset, Int64 bitLength, bool isUTF16 = false)
        {
            Result result = new Result();

            text = null;

            if (result.Fine)
            {
                Int64 byteOffset = bitOffset / 8;
                Int64 byteLength = bitLength / 8;
                byte[] dataBuffer = dataStore.GetData();

                //Try to skip the requested bits.
                if (dataStore.GetLeftBitLength() >= bitLength)
                {
                    //UnicodeEncoding encoding = new UnicodeEncoding();
                    //text = encoding.GetString(dataBuffer, (int)byteOffset, (int)byteLengh);
                    //ASCIIEncoding encoding = new ASCIIEncoding();
                    //text = encoding.GetString(dataBuffer, (int)byteOffset, (int)byteLength);
                    //Encoding ecSelf = Encoding.GetEncoding("iso-8859-1");
                    //text = new string((sbyte *)dataBuffer, (int)byteOffset, (int)byteLength, Encoding.UTF8);
                    //Encoding.Convert(Encoding.GetEncoding(""), Encoding.GetEncoding(""), dataBuffer, (int)byteOffset, (int)byteLength);
                    if (isUTF16)
                    {
                        Encoding encoding = Encoding.GetEncoding("utf-16");
                        text = encoding.GetString(dataBuffer, (int)byteOffset, (int)byteLength);
                    }
                    else
                    {
                        text = System.Text.Encoding.Default.GetString(dataBuffer, (int)byteOffset, (int)byteLength);
                    }
                    
                }
                else
                {
                    result.SetResult(ResultCode.INSUFFICIENT_DATA);
                }
            }

            return result;
        }

        //Get text from the buffer!!!!!!!Additional length checking will be performed according to maxLeftBits.
        public static Result GetTextPlus(out string text, DataStore dataStore, Int64 bitOffset, Int64 bitLength, Int64 maxLeftBits)
        {
            Result result = new Result();

            text = null;

            if (result.Fine)
            {
                //Check the maxLeftBits agains the length to be read.
                if (bitLength > maxLeftBits)
                {
                    result.SetResult(ResultCode.INSUFFICIENT_DATA);
                }

            }

            if (result.Fine)
            {
                result = GetText(out text, dataStore, bitOffset, bitLength);
            }

            return result;
        }

        public static void MjdToYmd(Int64 mjd, ref Int64 year, ref Int64 month, ref Int64 day)
        {
           Int64   y,m,d ,k;

           y = (Int64)((mjd - 15078.2) / 365.25);
           m = (Int64)((mjd - 14956.1 - (Int64)(y * 365.25)) / 30.6001);
           d = (Int64)(mjd - 14956 - (Int64)(y * 365.25) - (Int64)(m * 30.6001));
           k =  (m == 14 || m == 15) ? 1 : 0;
           y = y + k + 1900;
           m = m - 1 - k*12;

           year = y;
           month = m;
           day = d;
        }

        public static string GetLanguageCodeStr(Int64 languageCode)
        {
            return "" + Convert.ToChar((byte)((languageCode >> 16) & 0xFF)) + Convert.ToChar((byte)((languageCode >> 8) & 0xFF)) + Convert.ToChar((byte)((languageCode >> 00) & 0xFF));
        }

        public static string GetCountryCodeStr(Int64 countryCode)
        {
            return "" + Convert.ToChar((byte)((countryCode >> 16) & 0xFF)) + Convert.ToChar((byte)((countryCode >> 8) & 0xFF)) + Convert.ToChar((byte)((countryCode >> 00) & 0xFF));
        }

        public static string GetStrFromBytes(byte[] dataBytes)
        {
            return System.Text.Encoding.Default.GetString(dataBytes);
        }

        public static string GetHexString(byte[] dataBytes, int byteForEachLine)
        {
            StringBuilder myStringBuilder = new StringBuilder();

            for (int i = 0; i < dataBytes.Length; ++i)
            {
                if (0 == ((i + 1) % byteForEachLine))
                {
                    /*To add a new line.*/
                    myStringBuilder.Append(Environment.NewLine);
                }

                /*Show it as a HEX byte.*/
                myStringBuilder.AppendFormat("{0:X2}", dataBytes[i]);
            }

            return myStringBuilder.ToString();
        }

        public static void SetTag1(TreeNode treeNode, object tag1)
        {
            if (treeNode.Tag is FieldItem)
            {
                FieldItem fieldItem = (FieldItem)treeNode.Tag;
                fieldItem.Tag1 = tag1;
            }
        }

        public static object GetTag1(TreeNode treeNode)
        {
            object tag1 = null;
            if (treeNode.Tag is FieldItem)
            {
                FieldItem fieldItem = (FieldItem)treeNode.Tag;
                tag1 = fieldItem.Tag1;
            }

            return tag1;
        }

        public static object FindItem(TreeNode treeNode, Type myType)
        {
            object myObject = null;
            while (null != treeNode)
            {
                if (treeNode.Tag is FieldItem)
                {
                    FieldItem fieldItem = (FieldItem)treeNode.Tag;
                    if (null != fieldItem.Tag1)
                    {
                        if (fieldItem.Tag1.GetType() == myType)
                        {
                            myObject = fieldItem.Tag1;
                            break;
                        }
                    }

                }
                treeNode = treeNode.Parent;
            }

            return myObject;
        }


        
        public static byte[] GetDataBytesOfNode(TreeNode treeNode)
        {
            
            FieldItem fieldItem = (FieldItem)treeNode.Tag;
            Int64 offset = fieldItem.Offset / 8;
            Int64 length = fieldItem.Length / 8;
            byte[] dataBytes = new byte[length];


            Array.Copy(fieldItem.Data, offset, dataBytes, 0, length);
            
            return dataBytes;

        }
    }
}
