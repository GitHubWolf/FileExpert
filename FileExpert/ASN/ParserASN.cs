using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Library;
using FileExpert.ASN;
using System.IO;

namespace FileExpert.ASN
{
    class ParserASN
    {

        public void ParseASN(TreeView treeView, TreeNode parent, DataStore dataStore)
        {
            TreeNode nodeObject = null;
            Int64 bitOffset = 0;
            Int64 maxLeftBits = dataStore.GetLeftBitLength();

            //Add one node to indicate this object.
            Result result = Utility.AddNodeContainer(Position.CHILD, parent, out nodeObject, "ASN", ItemType.SECTION2, dataStore, 0, maxLeftBits);

            ParseOneTagLengthValue(treeView, nodeObject, dataStore, ref bitOffset, ref maxLeftBits);

            //ParseMultipleTagLengthContent(treeView, nodeObject, dataStore, ref bitOffset, ref maxLeftBits);
        }

        public Result ParseMultipleTagLengthValue(TreeView treeView, TreeNode parent, DataStore dataStore, ref Int64 bitOffset, ref Int64 maxLeftBits)
        {
            Result result = new Result();
    
            while (maxLeftBits > 0 && result.Fine)
            {
                result = ParseOneTagLengthValue(treeView, parent, dataStore, ref bitOffset, ref maxLeftBits);
            }

            return result;
        }

        public Result ParsePrintableStringType(TreeView treeView, TreeNode parent, DataStore dataStore, ref Int64 bitOffset, ref Int64 maxLeftBits)
        {
            Result result = new Result();
            TreeNode newNode = null;


            result = Utility.AddNodeDataPlus(Position.CHILD, parent, out newNode, "UTF8String", ItemType.DATA, dataStore, ref bitOffset, maxLeftBits, ref maxLeftBits);


            return result;

        }

        public Result ParseUTCTimeType(TreeView treeView, TreeNode parent, DataStore dataStore, ref Int64 bitOffset, ref Int64 maxLeftBits)
        {
            Result result = new Result();
            TreeNode newNode = null;


            result = Utility.AddNodeDataPlus(Position.CHILD, parent, out newNode, "UTCTime", ItemType.DATA, dataStore, ref bitOffset, maxLeftBits, ref maxLeftBits);


            return result;

        }


        public Result ParseGeneralizedTimeType(TreeView treeView, TreeNode parent, DataStore dataStore, ref Int64 bitOffset, ref Int64 maxLeftBits)
        {
            Result result = new Result();
            TreeNode newNode = null;


            result = Utility.AddNodeDataPlus(Position.CHILD, parent, out newNode, "GeneralizedTime", ItemType.DATA, dataStore, ref bitOffset, maxLeftBits, ref maxLeftBits);


            return result;

        }
        private Result ParseTag(DataStore dataStore, ref Int64 bitOffset, ref Int64 maxLeftBits, TreeNode nodeParent, ref TreeNode nodeForTag, ref Int64 isConstructed, ref Int64 tagNumber)
        {
            Result result = new Result();
            TreeNode newNode = null;
            Int64 fieldValue = 0;
            Int64 tagValue = 0;

            if (result.Fine)
            {
                result = Utility.AddNodeFieldPlus(Position.CHILD, nodeParent, out nodeForTag, "Tag", ItemType.ITEM, dataStore, ref bitOffset, 8, ref tagValue, ref maxLeftBits);
            }

            if (result.Fine)
            {
                //Rollback 8 bits so that we can parse the details.
                result = dataStore.RollbackBits(ref bitOffset, 8);
                maxLeftBits += 8;
            }

            Int64 classValue = 0;
            if (result.Fine)
            {
                result = Utility.AddNodeFieldPlus(Position.CHILD, nodeForTag, out newNode, "Class", ItemType.FIELD, dataStore, ref bitOffset, 2, ref classValue, ref maxLeftBits);
            }

            if (result.Fine)
            {
                Utility.UpdateNode(newNode, getClassName(classValue), ItemType.FIELD);
            }


            if (result.Fine)
            {
                /*
                0 = Primitive 
                1 = Constructed
                 */
                result = Utility.AddNodeFieldPlus(Position.CHILD, nodeForTag, out newNode, "P/C", ItemType.FIELD, dataStore, ref bitOffset, 1, ref isConstructed, ref maxLeftBits);
            }

            if (result.Fine)
            {
                Utility.UpdateNode(newNode, getTypeName(fieldValue), ItemType.FIELD);
            }

            if (result.Fine)
            {
                result = Utility.AddNodeFieldPlus(Position.CHILD, nodeForTag, out newNode, "Tag number", ItemType.FIELD, dataStore, ref bitOffset, 5, ref tagNumber, ref maxLeftBits);
            }

            if (result.Fine)
            {
                Utility.UpdateNode(newNode, getTagNumberName(tagNumber), ItemType.FIELD);
            }

            if (result.Fine)
            {
                if (tagNumber > 30)
                {
                    //For tags with a number greater than or equal to 31, the identifier shall comprise a leading octet followed by one or more subsequent octets.

                    TreeNode nodeForSubsequentOctets = null;
                    //Add one container node for Subsequent octets.
                    result = Utility.AddNodeContainerPlus(Position.CHILD, nodeForTag, out nodeForSubsequentOctets, "subsequent octets", ItemType.ITEM, dataStore, bitOffset, 0,  maxLeftBits);
                    bool isLastOctet = false;
                    Int64 octet = 0;
                    Int64 totalSubsequentOctetLength = 0;
                    while (!isLastOctet && result.Fine)
                    {
                        result = Utility.AddNodeFieldPlus(Position.CHILD, nodeForSubsequentOctets, out newNode, "octet", ItemType.ITEM, dataStore, ref bitOffset, 8, ref octet, ref maxLeftBits);

                        if (result.Fine)
                        {
                            //One additional byte.
                            totalSubsequentOctetLength++;

                            //bit 8 of each octet shall be set to one unless it is the last octet of the identifier octets
                            if ((octet & 0x80) == 0)
                            {
                                //It is the last octet.
                                isLastOctet = true;
                            }
                            else
                            {
                                //It is NOT the last octet.
                            }


                            //The final tag number for this special case is the result of: 7 bits | ... | n bits.
                            /* bits 7 to 1 of the first subsequent octet, followed by bits 7 to 1 of the second subsequent octet, followed
                                in turn by bits 7 to 1 of each further octet, up to and including the last subsequent octet in the identifier
                                octets shall be the encoding of an unsigned binary integer equal to the tag number, with bit 7 of the first
                                subsequent octet as the most significant bit;*/
                        }
                    }

                    Utility.UpdateNodeLength(nodeForSubsequentOctets, "subsequent octets", ItemType.ITEM, totalSubsequentOctetLength * 8);
                }
                else
                { 
                    //For tags with a number ranging from zero to 30 (inclusive), the identifier octets shall comprise a single octet
                }
            }



            return result;

        }

        private Result ParseLength(DataStore dataStore, ref Int64 bitOffset, ref Int64 maxLeftBits, TreeNode nodeParent, ref TreeNode nodeForLength, ref Int64 contentLength)
        {
            Result result = new Result();
            TreeNode newNode = null;

            /*
             * In the short form, the length octets shall consist of a single octet in which bit 8 is zero and bits 7 to 1
                encode the number of octets in the contents octets (which may be zero), as an unsigned binary integer with bit 7 as the
                most significant bit.
             */

            /*
             * In the long form, the length octets shall consist of an initial octet and one or more subsequent octets. The
                initial octet shall be encoded as follows:
                a) bit 8 shall be one;
                b) bits 7 to 1 shall encode the number of subsequent octets in the length octets, as an unsigned binary
                integer with bit 7 as the most significant bit;
                c) the value 11111111 shall not be used.
              */

            /*
             * For the indefinite form, the length octets indicate that the contents octets are terminated by end-of-contents octets (see 8.1.5), and shall consist of a single octet.
                8.1.3.6.1 The single octet shall have bit 8 set to one, and bits 7 to 1 set to zero.
                8.1.3.6.2 If this form of length is used, then end-of-contents octets (see 8.1.5) shall be present in the encoding following the contents octets.
             */
            if (result.Fine)
            {
                result = Utility.AddNodeContainerPlus(Position.CHILD, nodeParent, out nodeForLength, "Length", ItemType.ITEM, dataStore, bitOffset, 0, 0);
            }

            Int64 initialBitOffset = bitOffset;

            Int64 lengthByte1 = 0;
            Int64 lengthInByte = 0;
            if (result.Fine)
            {
                result = Utility.AddNodeFieldPlus(Position.CHILD, nodeForLength, out newNode, "LengthByte1", ItemType.FIELD, dataStore, ref bitOffset, 8, ref lengthByte1, ref maxLeftBits);
            }

            if (result.Fine)
            {
                if (lengthByte1 < 128)
                {
                    //Short form.
                    contentLength = lengthByte1;

                    Utility.UpdateNodeLength(nodeForLength, "Length(ShortForm)", ItemType.ITEM, 8);
                }
                else if (lengthByte1 == 128)
                {
                    //Indefinite form.
                    Utility.UpdateNode(nodeForLength, "Length(IndefiniteForm)", ItemType.ITEM);
                }
                else
                {
                    //Long form.
                    lengthInByte = (lengthByte1 & 0x7F);

                    //Read in the bytes  as the content length. Here I don't expect that it will be very long.
                    result = Utility.AddNodeFieldPlus(Position.CHILD, nodeForLength, out newNode, "LengthByteN", ItemType.FIELD, dataStore, ref bitOffset, 8 * lengthInByte, ref contentLength, ref maxLeftBits);

                    if (result.Fine)
                    {
                        Utility.UpdateNodeLength(nodeForLength, "Length(LongForm)", ItemType.ITEM, 8*(1 + contentLength));
                    }
                }
            }


            Int64 finalBitOffset = bitOffset;

            Utility.UpdateNodeLength(nodeForLength, "Length", ItemType.ITEM, finalBitOffset - initialBitOffset);
            
            return result;

        }

        public Result ParseBooleanType(TreeView treeView, TreeNode parent, DataStore dataStore, ref Int64 bitOffset, ref Int64 maxLeftBits)
        {
            Result result = new Result();
            Int64 fieldValue = 0;
            TreeNode newNode = null;


            result = Utility.AddNodeFieldPlus(Position.CHILD, parent, out newNode, "Boolean", ItemType.FIELD, dataStore, ref bitOffset, maxLeftBits, ref fieldValue, ref maxLeftBits);


            return result;

        }

        public Result ParseIntegerType(TreeView treeView, TreeNode parent, DataStore dataStore, ref Int64 bitOffset, ref Int64 maxLeftBits)
        {
            Result result = new Result();
            Int64 fieldValue = 0;
            TreeNode newNode = null;


            result = Utility.AddNodeFieldPlus(Position.CHILD, parent, out newNode, "Integer", ItemType.FIELD, dataStore, ref bitOffset, maxLeftBits, ref fieldValue, ref maxLeftBits);

            
            return result;

        }

        public Result ParseBitStringType(TreeView treeView, TreeNode parent, DataStore dataStore, ref Int64 bitOffset, ref Int64 maxLeftBits)
        {
            Result result = new Result();
            TreeNode newNode = null;


            result = Utility.AddNodeDataPlus(Position.CHILD, parent, out newNode, "BitString", ItemType.FIELD, dataStore, ref bitOffset, maxLeftBits, ref maxLeftBits);


            return result;

        }

        public Result ParseOctetStringType(TreeView treeView, TreeNode parent, DataStore dataStore, ref Int64 bitOffset, ref Int64 maxLeftBits)
        {
            Result result = new Result();
            TreeNode newNode = null;


            result = Utility.AddNodeDataPlus(Position.CHILD, parent, out newNode, "OctetString", ItemType.FIELD, dataStore, ref bitOffset, maxLeftBits, ref maxLeftBits);


            return result;

        }
        public Result ParseNullType(TreeView treeView, TreeNode parent, DataStore dataStore, ref Int64 bitOffset, ref Int64 maxLeftBits)
        {
            Result result = new Result();
            TreeNode newNode = null;


            result = Utility.AddNodeDataPlus(Position.CHILD, parent, out newNode, "Null", ItemType.DATA, dataStore, ref bitOffset, maxLeftBits, ref maxLeftBits);


            return result;

        }


        public Result ParseObjectIdentifierType(TreeView treeView, TreeNode parent, DataStore dataStore, ref Int64 bitOffset, ref Int64 maxLeftBits)
        {
            Result result = new Result();
            TreeNode newNode = null;


            result = Utility.AddNodeDataPlus(Position.CHILD, parent, out newNode, "ObjectIdentifier", ItemType.DATA, dataStore, ref bitOffset, maxLeftBits, ref maxLeftBits);

            if (result.Fine)
            {
                byte[] dataBytes = Utility.GetDataBytesOfNode(newNode);
                MemoryStream ms = new MemoryStream(dataBytes);
                String oid = OIDParser.OID2String(ms);
                String oidName = OIDParser.GetOIDName(oid);
                Utility.UpdateNode(newNode, "algorithm:" + oid + " name: " + oidName, ItemType.FIELD);
            }

            return result;

        }

        public Result ParseEnumeratedType(TreeView treeView, TreeNode parent, DataStore dataStore, ref Int64 bitOffset, ref Int64 maxLeftBits)
        {
            Result result = new Result();
            TreeNode newNode = null;


            result = Utility.AddNodeDataPlus(Position.CHILD, parent, out newNode, "Enumerated", ItemType.DATA, dataStore, ref bitOffset, maxLeftBits, ref maxLeftBits);


            return result;

        }

        public Result ParseUTF8StringType(TreeView treeView, TreeNode parent, DataStore dataStore, ref Int64 bitOffset, ref Int64 maxLeftBits)
        {
            Result result = new Result();
            TreeNode newNode = null;
            string text = null;

            result = Utility.GetTextPlus(out text, dataStore, bitOffset, maxLeftBits, maxLeftBits);

            if (result.Fine)
            {
                result = Utility.AddNodeDataPlus(Position.CHILD, parent, out newNode, "UTF8String", ItemType.DATA, dataStore, ref bitOffset, maxLeftBits, ref maxLeftBits);

                if (result.Fine)
                {
                    Utility.UpdateNode(newNode, "UTF8String:" + text, ItemType.DATA);
                }
            }
            else 
            {
                result = Utility.AddNodeDataPlus(Position.CHILD, parent, out newNode, "UTF8String", ItemType.DATA, dataStore, ref bitOffset, maxLeftBits, ref maxLeftBits);
            }
            


            return result;

        }
        public Result ParseOneTagLengthValue(TreeView treeView, TreeNode parent, DataStore dataStore, ref Int64 bitOffset, ref Int64 maxLeftBits)
        {
            Result result = new Result();
            TreeNode newNode = null;
            TreeNode nodeTLV = null;
            TreeNode nodeValue = null;


            Int64 isConstructed = 0;
            Int64 tagNumber = 0;
            Int64 contentLength = 0;

            Int64 initialBitOffset = 0;
            Int64 finalBitOffset = 0;
            if (result.Fine)
            {
                initialBitOffset = bitOffset;
                result = Utility.AddNodeContainerPlus(Position.CHILD, parent, out nodeTLV, "TLV", ItemType.ITEM, dataStore, bitOffset, 0, maxLeftBits);
            }
            if (result.Fine)
            {
                //Tag.
                result = ParseTag(dataStore, ref bitOffset, ref maxLeftBits, nodeTLV, ref newNode, ref isConstructed, ref tagNumber);
            }

            if (result.Fine)
            {
                //Length.
                result = ParseLength(dataStore, ref bitOffset, ref maxLeftBits, nodeTLV, ref newNode, ref contentLength);

                //Convert to bits.
                contentLength = contentLength * 8;
            }

            finalBitOffset = bitOffset;

            Utility.UpdateNodeLength(nodeTLV, "TLV", ItemType.ITEM, ((finalBitOffset - initialBitOffset) + contentLength));

            if (result.Fine)
            {
                //Value.
                result = Utility.AddNodeContainerPlus(Position.CHILD, nodeTLV, out nodeValue, "Values", ItemType.DATA, dataStore, bitOffset, contentLength, maxLeftBits);
            }

            if (result.Fine)
            {
                //Important! Remember to reduce maxLeftBits.
                maxLeftBits -= contentLength;

                if (isConstructed == 1)
                {
                    //Constructed.
                    result = ParseMultipleTagLengthValue(treeView, nodeValue, dataStore, ref bitOffset, ref contentLength);
                }
                else
                {
                    switch (tagNumber)
                    {
                        case 0x01:
                            //Boolean
                            result = ParseBooleanType(treeView, nodeValue, dataStore, ref bitOffset, ref contentLength);
                            
                            break;
                        case 0x02:
                            //Integer
                            result = ParseIntegerType(treeView, nodeValue, dataStore, ref bitOffset, ref contentLength);
                            break;
                        case 0x03:
                            //BitString
                            result = ParseBitStringType(treeView, nodeValue, dataStore, ref bitOffset, ref contentLength);
                            break;
                        case 0x04:
                            //OctetString
                            result = ParseOctetStringType(treeView, nodeValue, dataStore, ref bitOffset, ref contentLength);
                            //result = ParseMultipleTagLengthValue(treeView, nodeValue, dataStore, ref bitOffset, ref contentLength);
                            break;
                        case 0x05:
                            //Integer
                            result = ParseNullType(treeView, nodeValue, dataStore, ref bitOffset, ref contentLength);
                            break;
                        case 0x06:
                            //ObjectIdentifier
                            result = ParseObjectIdentifierType(treeView, nodeValue, dataStore, ref bitOffset, ref contentLength);
                            break;
                        case 0x0A:
                            //Enumerated
                            result = ParseEnumeratedType(treeView, nodeValue, dataStore, ref bitOffset, ref contentLength);
                            break;
                        case 0x0C:
                            //UTF8String
                            result = ParseUTF8StringType(treeView, nodeValue, dataStore, ref bitOffset, ref contentLength);
                            break;
                        case 0x10:
                        //case 0x30:
                            //sequence, sequence of. The content in the sequence/sequence-of will be in order.
                            result = ParseMultipleTagLengthValue(treeView, nodeValue, dataStore, ref bitOffset, ref contentLength);
                            break;
                        case 0x11:
                        //case 0x31:
                            //set, set of. The content in the set/set-of is not in order.
                            result = ParseMultipleTagLengthValue(treeView, nodeValue, dataStore, ref bitOffset, ref contentLength);
                            
                            break;

                        case 0x13:
                            //PrintableString.
                            result = result = ParsePrintableStringType(treeView, nodeValue, dataStore, ref bitOffset, ref contentLength);
                            
                            break;
                        case 0x17:
                            //UTCTime.
                            result = ParseUTCTimeType(treeView, nodeValue, dataStore, ref bitOffset, ref contentLength);

                            break;
                        case 0x18:
                            //GeneralizedTime.
                            result = ParseGeneralizedTimeType(treeView, nodeValue, dataStore, ref bitOffset, ref contentLength);
                            

                            break;
                        default:
                            Console.WriteLine("tag Value:" + String.Format("0x{0:X2}", tagNumber));
                            result = Utility.AddNodeDataPlus(Position.CHILD, nodeValue, out newNode, "Values", ItemType.DATA, dataStore, ref bitOffset, contentLength, ref contentLength);
                            
                            break;
                    }
                }
                
            }


            if (!result.Fine)
            {
                Utility.UpdateNode(nodeTLV, "TLV", ItemType.ERROR);
                Utility.ExpandToRoot(nodeTLV, ItemType.WARNING);
            }
            return result;

        }


        private String getClassName(Int64 value)
        {
            switch (value)
            {
                case 0x0:
                    return "Class:Universal";
                case 0x01:
                    return "Class:Application";
                case 0x02:
                    return "Class:Context-specific";
                case 0x03:
                    return "Class:Private";
                default:
                    return "Class:Unknown";
            }
        }

        private String getTypeName(Int64 value)
        {
            switch (value)
            {
                case 0x0:
                    return "P/C:Primitive";
                case 0x01:
                    return "P/C:Primitive";
                default:
                    return "P/C:Unknown";
            }
        }
        private String getTagNumberName(Int64 value)
        {
            switch (value)
            {
                case 0:
                    return "TagNumber:Reserved type";
                case 1:
                    return "TagNumber:Boolean type";
                case 2:
                    return "TagNumber:Integer type";
                case 3:
                    return "TagNumber:Bitstring type";
                case 4:
                    return "TagNumber:Octetstring type";
                case 5:
                    return "TagNumber:Null type";
                case 6:
                    return "TagNumber:Object identifier type";
                case 7:
                    return "TagNumber:Object descriptor type";
                case 8:
                    return "TagNumber:External type and Instance-of type";
                case 9:
                    return "TagNumber:Real type";
                case 10:
                    return "TagNumber:Enumerated type";
                case 11:
                    return "TagNumber:Embedded-pdv type";
                case 12:
                    return "TagNumber:UTF8String type";
                case 13:
                    return "TagNumber:Relative object identifier type";
                case 14:
                    return "TagNumber:The time type";
                case 15:
                    return "TagNumber:Reserved type";
                case 16:
                    return "TagNumber:Sequence and Sequence-of types";
                case 17:
                    return "TagNumber:Set and Set-of types";
                case 18:
                    return "TagNumber:NumericString type";
                case 19:
                    return "TagNumber:PrintableString type";
                case 20:
                    return "TagNumber:TeletexString/T61String type";
                case 21:
                    return "TagNumber:VideotexString type";
                case 22:
                    return "TagNumber:IA5String type";
                case 23:
                    return "TagNumber:UTCTime type";
                case 24:
                    return "TagNumber:GeneralizedTime type";
                case 25:
                    return "TagNumber:GraphicString type";
                case 26:
                    return "TagNumber:VisibleString/ISO646String type";
                case 27:
                    return "TagNumber:GeneralString type";
                case 28:
                    return "TagNumber:UniversalString type";
                case 29:
                    return "TagNumber:CharacterString type";
                case 30:
                    return "TagNumber:BMPString type";
                default:
                    return "PTagNumber:Reserved type";
            }
        }
    }
}
