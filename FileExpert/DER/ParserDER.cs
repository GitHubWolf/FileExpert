using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Library;
using FileExpert.DER;
using System.IO;

namespace FileExpert.DER
{
    class ParserDER
    {

        public void ParseDER(TreeView treeView, TreeNode parent, DataStore dataStore)
        {
            Int64 bitOffset = 0;
            Int64 maxLeftBits = dataStore.GetLeftBitLength();


            ParseCertificate(treeView, parent, dataStore, ref bitOffset, ref maxLeftBits);

            //ParseMultipleTagLengthContent(treeView, nodeObject, dataStore, ref bitOffset, ref maxLeftBits);
        }

        public Result ParseCertificate(TreeView treeView, TreeNode parent, DataStore dataStore, ref Int64 bitOffset, ref Int64 maxLeftBits)
        {
            Result result = new Result();
            TreeNode newNode = null;
            TreeNode nodeValue = null;
            TreeNode nodeTLV = null;

            Int64 classValue = 0;
            Int64 isConstructed = 0;
            Int64 tagNumber = 0;
            Int64 contentLength = 0;

            Int64 initialBitOffset = 0;
            Int64 finalBitOffset = 0;
            if (result.Fine)
            {
                initialBitOffset = bitOffset;
            }

            //Add one node to indicate this object.
            result = Utility.AddNodeContainer(Position.CHILD, parent, out nodeTLV, "Certificate", ItemType.SECTION2, dataStore, 0, maxLeftBits);


            if (result.Fine)
            {
                //Tag.
                result = ParseTag(dataStore, ref bitOffset, ref maxLeftBits, nodeTLV, ref newNode, ref classValue, ref isConstructed, ref tagNumber);
            }

            if (result.Fine)
            {
                //Length.
                result = ParseLength(dataStore, ref bitOffset, ref maxLeftBits, nodeTLV, ref newNode, ref contentLength);

                //Convert to bits.
                contentLength = contentLength * 8;
            }

            if (result.Fine)
            {
                //Value.
                result = Utility.AddNodeContainerPlus(Position.CHILD, nodeTLV, out nodeValue, "Values", ItemType.DATA, dataStore, bitOffset, contentLength, maxLeftBits);
            }

            finalBitOffset = bitOffset;

            Utility.UpdateNodeLength(nodeTLV, "Certificate", ItemType.ITEM, ((finalBitOffset - initialBitOffset) + contentLength));

            if (result.Fine)
            {
                result = ParseTbsCertificate(treeView, nodeValue, dataStore, ref bitOffset, ref maxLeftBits);
            }

            if (result.Fine)
            {
                result = ParseAlgorithmIdentifier(treeView, nodeValue, dataStore, ref bitOffset, ref contentLength, "signatureAlgorithm");
            }

            if (result.Fine)
            {
                result = ParseSignatureValue(treeView, nodeValue, dataStore, ref bitOffset, ref maxLeftBits);
            }

            if (!result.Fine)
            {
                Utility.UpdateNode(nodeTLV, "Certificate", ItemType.ERROR);
                Utility.ExpandToRoot(nodeTLV, ItemType.WARNING);
            }
            return result;

        }

        public Result ParseTbsCertificate(TreeView treeView, TreeNode parent, DataStore dataStore, ref Int64 bitOffset, ref Int64 maxLeftBits)
        {
            Result result = new Result();
            TreeNode newNode = null;
            TreeNode nodeTLV = null;
            TreeNode nodeValue = null;

            Int64 classValue = 0;
            Int64 isConstructed = 0;
            Int64 tagNumber = 0;
            Int64 contentLength = 0;

            Int64 initialBitOffset = 0;
            Int64 finalBitOffset = 0;
            if (result.Fine)
            {
                initialBitOffset = bitOffset;
            }

            //Add one node to indicate this object.
            result = Utility.AddNodeContainer(Position.CHILD, parent, out nodeTLV, "tbsCertificate", ItemType.SECTION2, dataStore, bitOffset, 0);


            if (result.Fine)
            {
                //Tag.
                result = ParseTag(dataStore, ref bitOffset, ref maxLeftBits, nodeTLV, ref newNode, ref classValue, ref isConstructed, ref tagNumber);
            }

            if (result.Fine)
            {
                //Length.
                result = ParseLength(dataStore, ref bitOffset, ref maxLeftBits, nodeTLV, ref newNode, ref contentLength);

                //Convert to bits.
                contentLength = contentLength * 8;
            }

            finalBitOffset = bitOffset;

            Utility.UpdateNodeLength(nodeTLV, "tbsCertificate", ItemType.ITEM, ((finalBitOffset - initialBitOffset) + contentLength));

            if (result.Fine)
            {
                //Value.
                result = Utility.AddNodeContainerPlus(Position.CHILD, nodeTLV, out nodeValue, "Values", ItemType.DATA, dataStore, bitOffset, contentLength, maxLeftBits);
            }



            if (result.Fine)
            {
                maxLeftBits -= contentLength;
            }

            if (result.Fine)
            {
                result = ParseVersionExplicit(treeView, nodeValue, dataStore, ref bitOffset, ref contentLength);
            }

            if (result.Fine)
            {
                result = ParseSerialNumber(treeView, nodeValue, dataStore, ref bitOffset, ref contentLength);
            }

            if (result.Fine)
            {
                result = ParseAlgorithmIdentifier(treeView, nodeValue, dataStore, ref bitOffset, ref contentLength, "signature");
            }

            if (result.Fine)
            {
                result = ParseName(treeView, nodeValue, dataStore, ref bitOffset, ref contentLength, "issuer");
            }

            if (result.Fine)
            {
                result = ParseValidity(treeView, nodeValue, dataStore, ref bitOffset, ref contentLength);
            }

            if (result.Fine)
            {
                result = ParseName(treeView, nodeValue, dataStore, ref bitOffset, ref contentLength, "subject");
            }

            if (result.Fine)
            {
                result = ParseSubjectPublicKeyInfo(treeView, nodeValue, dataStore, ref bitOffset, ref contentLength);
            }

            if (result.Fine)
            {
                result = ParseTBSCertificateOptionalFields(treeView, nodeValue, dataStore, ref bitOffset, ref contentLength);
            }


            if (!result.Fine)
            {
                Utility.UpdateNode(nodeTLV, "tbsCertificate", ItemType.ERROR);
                Utility.ExpandToRoot(nodeTLV, ItemType.WARNING);
            }
            return result;
        }



        public Result ParseVersionExplicit(TreeView treeView, TreeNode parent, DataStore dataStore, ref Int64 bitOffset, ref Int64 maxLeftBits)
        {
            Result result = new Result();
            TreeNode newNode = null;
            TreeNode nodeTLV = null;
            TreeNode nodeValue = null;

            Int64 classValue = 0;
            Int64 isConstructed = 0;
            Int64 tagNumber = 0;
            Int64 contentLength = 0;

            Int64 initialBitOffset = 0;
            Int64 finalBitOffset = 0;
            if (result.Fine)
            {
                initialBitOffset = bitOffset;
            }

            //Add one node to indicate this object.
            result = Utility.AddNodeContainer(Position.CHILD, parent, out nodeTLV, "version", ItemType.SECTION2, dataStore, bitOffset, 0);


            if (result.Fine)
            {
                //Tag.
                result = ParseTag(dataStore, ref bitOffset, ref maxLeftBits, nodeTLV, ref newNode, ref classValue, ref isConstructed, ref tagNumber);
            }

            if (result.Fine)
            {
                //Length.
                result = ParseLength(dataStore, ref bitOffset, ref maxLeftBits, nodeTLV, ref newNode, ref contentLength);

                //Convert to bits.
                contentLength = contentLength * 8;
            }

            finalBitOffset = bitOffset;

            Utility.UpdateNodeLength(nodeTLV, "version", ItemType.ITEM, ((finalBitOffset - initialBitOffset) + contentLength));

            //version         [0]  EXPLICIT Version DEFAULT v1,
            if (result.Fine)
            {
                if ((classValue != 0x2) || (isConstructed != 0x1) || (tagNumber != 0))
                {
                    result.SetFailure();
                }
            }

            if(result.Fine)
            {
                result = Utility.AddNodeContainerPlus(Position.CHILD, nodeTLV, out nodeValue, "Values", ItemType.DATA, dataStore, bitOffset, contentLength, maxLeftBits);

                if (result.Fine)
                {
                    maxLeftBits -= contentLength;
                }
            }

            if (result.Fine)
            {
                result = ParseVersionImplicit(treeView, nodeValue, dataStore, ref bitOffset, ref contentLength);
            }

            if (!result.Fine)
            {
                Utility.UpdateNode(nodeTLV, "version", ItemType.ERROR);
                Utility.ExpandToRoot(nodeTLV, ItemType.WARNING);
            }
            return result;
        }



        public Result ParseVersionImplicit(TreeView treeView, TreeNode parent, DataStore dataStore, ref Int64 bitOffset, ref Int64 maxLeftBits)
        {
            Result result = new Result();
            TreeNode newNode = null;
            TreeNode nodeTLV = null;
            TreeNode nodeValue = null;

            Int64 classValue = 0;
            Int64 isConstructed = 0;
            Int64 tagNumber = 0;
            Int64 contentLength = 0;

            Int64 fieldValue = 0;

            Int64 initialBitOffset = 0;
            Int64 finalBitOffset = 0;
            if (result.Fine)
            {
                initialBitOffset = bitOffset;
            }

            //Add one node to indicate this object.
            result = Utility.AddNodeContainer(Position.CHILD, parent, out nodeTLV, "version", ItemType.SECTION2, dataStore, bitOffset, 0);


            if (result.Fine)
            {
                //Tag.
                result = ParseTag(dataStore, ref bitOffset, ref maxLeftBits, nodeTLV, ref newNode, ref classValue, ref isConstructed, ref tagNumber);
            }

            if (result.Fine)
            {
                //Length.
                result = ParseLength(dataStore, ref bitOffset, ref maxLeftBits, nodeTLV, ref newNode, ref contentLength);

                //Convert to bits.
                contentLength = contentLength * 8;
            }

            finalBitOffset = bitOffset;

            Utility.UpdateNodeLength(nodeTLV, "version", ItemType.ITEM, ((finalBitOffset - initialBitOffset) + contentLength));

            //version         [0]  EXPLICIT Version DEFAULT v1,
            if (result.Fine)
            {
                if ((classValue != 0x0) || (isConstructed != 0x0) || (tagNumber != 2))
                {
                    result.SetFailure();
                }
            }

            if (result.Fine)
            {
                result = Utility.AddNodeContainerPlus(Position.CHILD, nodeTLV, out nodeValue, "Values", ItemType.DATA, dataStore, bitOffset, contentLength, maxLeftBits);

                if (result.Fine)
                {
                    result = Utility.AddNodeFieldPlus(Position.CHILD, nodeValue, out newNode, "version", ItemType.FIELD, dataStore, ref bitOffset,  contentLength, ref fieldValue, ref maxLeftBits);
                }
            }



            if (!result.Fine)
            {
                Utility.UpdateNode(nodeTLV, "version", ItemType.ERROR);
                Utility.ExpandToRoot(nodeTLV, ItemType.WARNING);
            }
            return result;
        }

        public Result ParseSerialNumber(TreeView treeView, TreeNode parent, DataStore dataStore, ref Int64 bitOffset, ref Int64 maxLeftBits)
        {
            Result result = new Result();
            TreeNode newNode = null;
            TreeNode nodeTLV = null;
            TreeNode nodeValue = null;

            Int64 classValue = 0;
            Int64 isConstructed = 0;
            Int64 tagNumber = 0;
            Int64 contentLength = 0;
            Int64 fieldValue = 0;

            Int64 initialBitOffset = 0;
            Int64 finalBitOffset = 0;
            if (result.Fine)
            {
                initialBitOffset = bitOffset;
            }

            //Add one node to indicate this object.
            result = Utility.AddNodeContainer(Position.CHILD, parent, out nodeTLV, "serialNumber", ItemType.SECTION2, dataStore, bitOffset, 0);


            if (result.Fine)
            {
                //Tag.
                result = ParseTag(dataStore, ref bitOffset, ref maxLeftBits, nodeTLV, ref newNode, ref classValue, ref isConstructed, ref tagNumber);
            }

            if (result.Fine)
            {
                //Length.
                result = ParseLength(dataStore, ref bitOffset, ref maxLeftBits, nodeTLV, ref newNode, ref contentLength);

                //Convert to bits.
                contentLength = contentLength * 8;
            }

            finalBitOffset = bitOffset;

            Utility.UpdateNodeLength(nodeTLV, "serialNumber", ItemType.ITEM, ((finalBitOffset - initialBitOffset) + contentLength));

            if (result.Fine)
            {
                if ((classValue != 0x0) || (isConstructed != 0x0) || (tagNumber != 0x2))
                {
                    result.SetFailure();
                }
            }

            if (result.Fine)
            {
                result = Utility.AddNodeContainerPlus(Position.CHILD, nodeTLV, out nodeValue, "Values", ItemType.DATA, dataStore, bitOffset, contentLength, maxLeftBits);

                if (result.Fine)
                {
                    maxLeftBits -= contentLength;
                }

                if (result.Fine)
                {
                    result = Utility.AddNodeFieldPlus(Position.CHILD, nodeValue, out newNode, "serialNumber", ItemType.FIELD, dataStore, ref bitOffset, contentLength, ref fieldValue, ref contentLength);
                }
            }

            if (!result.Fine)
            {
                Utility.UpdateNode(nodeTLV, "serialNumber", ItemType.ERROR);
                Utility.ExpandToRoot(nodeTLV, ItemType.WARNING);
            }
            return result;
        }

        public Result ParseAlgorithmIdentifier(TreeView treeView, TreeNode parent, DataStore dataStore, ref Int64 bitOffset, ref Int64 maxLeftBits, String titleOfNode)
        {
            Result result = new Result();
            TreeNode newNode = null;
            TreeNode nodeTLV = null;
            TreeNode nodeValue = null;

            Int64 classValue = 0;
            Int64 isConstructed = 0;
            Int64 tagNumber = 0;
            Int64 contentLength = 0;

            Int64 initialBitOffset = 0;
            Int64 finalBitOffset = 0;
            if (result.Fine)
            {
                initialBitOffset = bitOffset;
            }

            //Add one node to indicate this object.
            result = Utility.AddNodeContainer(Position.CHILD, parent, out nodeTLV, titleOfNode, ItemType.SECTION2, dataStore, bitOffset, 0);


            if (result.Fine)
            {
                //Tag.
                result = ParseTag(dataStore, ref bitOffset, ref maxLeftBits, nodeTLV, ref newNode, ref classValue, ref isConstructed, ref tagNumber);
            }

            if (result.Fine)
            {
                //Length.
                result = ParseLength(dataStore, ref bitOffset, ref maxLeftBits, nodeTLV, ref newNode, ref contentLength);

                //Convert to bits.
                contentLength = contentLength * 8;
            }

            finalBitOffset = bitOffset;

            Utility.UpdateNodeLength(nodeTLV, titleOfNode, ItemType.ITEM, ((finalBitOffset - initialBitOffset) + contentLength));

            if (result.Fine)
            {
                if ((classValue != 0x0) || (isConstructed != 0x1) || (tagNumber != 0x10))
                {
                    result.SetFailure();
                }
            }

            if (result.Fine)
            {
                result = Utility.AddNodeContainerPlus(Position.CHILD, nodeTLV, out nodeValue, "Values", ItemType.DATA, dataStore, bitOffset, contentLength, maxLeftBits);

                if (result.Fine)
                {
                    maxLeftBits -= contentLength;
                }

                if (result.Fine)
                {
                    result = ParseAlgorithm(treeView, nodeValue, dataStore, ref bitOffset, ref contentLength);
                }

                if (result.Fine && (contentLength != 0))
                {
                    result = ParseAlgorithmParameters(treeView, nodeValue, dataStore, ref bitOffset, ref contentLength);
                }

            }

            if (!result.Fine)
            {
                Utility.UpdateNode(nodeTLV, titleOfNode, ItemType.ERROR);
                Utility.ExpandToRoot(nodeTLV, ItemType.WARNING);
            }
            return result;
        }


        public Result ParseAlgorithm(TreeView treeView, TreeNode parent, DataStore dataStore, ref Int64 bitOffset, ref Int64 maxLeftBits)
        {
            Result result = new Result();
            TreeNode newNode = null;
            TreeNode nodeTLV = null;
            TreeNode nodeValue = null;

            Int64 classValue = 0;
            Int64 isConstructed = 0;
            Int64 tagNumber = 0;
            Int64 contentLength = 0;

            Int64 initialBitOffset = 0;
            Int64 finalBitOffset = 0;
            if (result.Fine)
            {
                initialBitOffset = bitOffset;
            }

            //Add one node to indicate this object.
            result = Utility.AddNodeContainer(Position.CHILD, parent, out nodeTLV, "algorithm", ItemType.SECTION2, dataStore, bitOffset, 0);


            if (result.Fine)
            {
                //Tag.
                result = ParseTag(dataStore, ref bitOffset, ref maxLeftBits, nodeTLV, ref newNode, ref classValue, ref isConstructed, ref tagNumber);
            }

            if (result.Fine)
            {
                //Length.
                result = ParseLength(dataStore, ref bitOffset, ref maxLeftBits, nodeTLV, ref newNode, ref contentLength);

                //Convert to bits.
                contentLength = contentLength * 8;
            }

            finalBitOffset = bitOffset;

            Utility.UpdateNodeLength(nodeTLV, "algorithm", ItemType.ITEM, ((finalBitOffset - initialBitOffset) + contentLength));

            if (result.Fine)
            {
                if ((classValue != 0x0) || (isConstructed != 0x0) || (tagNumber != 0x6))
                {
                    result.SetFailure();
                }
            }

            if (result.Fine)
            {
                result = Utility.AddNodeContainerPlus(Position.CHILD, nodeTLV, out nodeValue, "Values", ItemType.DATA, dataStore, bitOffset, contentLength, maxLeftBits);

                if (result.Fine)
                {
                    result = Utility.AddNodeDataPlus(Position.CHILD, nodeValue, out newNode, "algorithm", ItemType.FIELD, dataStore, ref bitOffset, contentLength, ref maxLeftBits);
                }

                if (result.Fine)
                {
                    byte[] dataBytes = Utility.GetDataBytesOfNode(newNode);
                    MemoryStream ms = new MemoryStream(dataBytes);
                    String oid = OIDParser.OID2String(ms);
                    String oidName = OIDParser.GetOIDName(oid);
                    Utility.UpdateNode(newNode, "algorithm:" + oid +" name: "+oidName, ItemType.FIELD);
                }
            }



            if (!result.Fine)
            {
                Utility.UpdateNode(nodeTLV, "algorithm", ItemType.ERROR);
                Utility.ExpandToRoot(nodeTLV, ItemType.WARNING);
            }
            return result;
        }

        public Result ParseAlgorithmParameters(TreeView treeView, TreeNode parent, DataStore dataStore, ref Int64 bitOffset, ref Int64 maxLeftBits)
        {
            Result result = new Result();
            TreeNode newNode = null;
            TreeNode nodeTLV = null;
            TreeNode nodeValue = null;

            Int64 classValue = 0;
            Int64 isConstructed = 0;
            Int64 tagNumber = 0;
            Int64 contentLength = 0;

            Int64 initialBitOffset = 0;
            Int64 finalBitOffset = 0;
            if (result.Fine)
            {
                initialBitOffset = bitOffset;
            }

            //Add one node to indicate this object.
            result = Utility.AddNodeContainer(Position.CHILD, parent, out nodeTLV, "parameters", ItemType.SECTION2, dataStore, bitOffset, 0);


            if (result.Fine)
            {
                //Tag.
                result = ParseTag(dataStore, ref bitOffset, ref maxLeftBits, nodeTLV, ref newNode, ref classValue, ref isConstructed, ref tagNumber);
            }

            if (result.Fine)
            {
                //Length.
                result = ParseLength(dataStore, ref bitOffset, ref maxLeftBits, nodeTLV, ref newNode, ref contentLength);

                //Convert to bits.
                contentLength = contentLength * 8;
            }

            finalBitOffset = bitOffset;

            Utility.UpdateNodeLength(nodeTLV, "parameters", ItemType.ITEM, ((finalBitOffset - initialBitOffset) + contentLength));



            if (result.Fine)
            {
                result = Utility.AddNodeDataPlus(Position.CHILD, nodeTLV, out nodeValue, "Values", ItemType.DATA, dataStore, ref bitOffset, contentLength, ref maxLeftBits);

            }

            if (!result.Fine)
            {
                Utility.UpdateNode(nodeTLV, "parameters", ItemType.ERROR);
                Utility.ExpandToRoot(nodeTLV, ItemType.WARNING);
            }
            return result;
        }

        public Result ParseName(TreeView treeView, TreeNode parent, DataStore dataStore, ref Int64 bitOffset, ref Int64 maxLeftBits, String titleOfNode)
        {
            Result result = new Result();
            TreeNode newNode = null;
            TreeNode nodeTLV = null;
            TreeNode nodeValue = null;

            Int64 classValue = 0;
            Int64 isConstructed = 0;
            Int64 tagNumber = 0;
            Int64 contentLength = 0;

            Int64 initialBitOffset = 0;
            Int64 finalBitOffset = 0;
            if (result.Fine)
            {
                initialBitOffset = bitOffset;
            }

            //Add one node to indicate this object.
            result = Utility.AddNodeContainer(Position.CHILD, parent, out nodeTLV, titleOfNode, ItemType.SECTION2, dataStore, bitOffset, 0);


            if (result.Fine)
            {
                //Tag.
                result = ParseTag(dataStore, ref bitOffset, ref maxLeftBits, nodeTLV, ref newNode, ref classValue, ref isConstructed, ref tagNumber);
            }

            if (result.Fine)
            {
                //Length.
                result = ParseLength(dataStore, ref bitOffset, ref maxLeftBits, nodeTLV, ref newNode, ref contentLength);

                //Convert to bits.
                contentLength = contentLength * 8;
            }

            finalBitOffset = bitOffset;

            Utility.UpdateNodeLength(nodeTLV, titleOfNode, ItemType.ITEM, ((finalBitOffset - initialBitOffset) + contentLength));

            if (result.Fine)
            {
                if ((classValue != 0x0) || (isConstructed != 0x1) || (tagNumber != 0x10))
                {
                    result.SetFailure();
                }
            }

            if (result.Fine)
            {
                result = Utility.AddNodeContainerPlus(Position.CHILD, nodeTLV, out nodeValue, "Values", ItemType.DATA, dataStore, bitOffset, contentLength, maxLeftBits);

                if (result.Fine)
                {
                    maxLeftBits -= contentLength;
                }

                while (result.Fine && (contentLength > 0))
                {
                    result = ParseRelativeDistinguishedName(treeView, nodeValue, dataStore, ref bitOffset, ref contentLength);
                }


            }

            if (!result.Fine)
            {
                Utility.UpdateNode(nodeTLV, titleOfNode, ItemType.ERROR);
                Utility.ExpandToRoot(nodeTLV, ItemType.WARNING);
            }
            return result;
        }

        public Result ParseValidity(TreeView treeView, TreeNode parent, DataStore dataStore, ref Int64 bitOffset, ref Int64 maxLeftBits)
        {
            Result result = new Result();
            TreeNode newNode = null;
            TreeNode nodeTLV = null;
            TreeNode nodeValue = null;

            Int64 classValue = 0;
            Int64 isConstructed = 0;
            Int64 tagNumber = 0;
            Int64 contentLength = 0;

            Int64 initialBitOffset = 0;
            Int64 finalBitOffset = 0;
            if (result.Fine)
            {
                initialBitOffset = bitOffset;
            }

            //Add one node to indicate this object.
            result = Utility.AddNodeContainer(Position.CHILD, parent, out nodeTLV, "validity", ItemType.SECTION2, dataStore, bitOffset, 0);


            if (result.Fine)
            {
                //Tag.
                result = ParseTag(dataStore, ref bitOffset, ref maxLeftBits, nodeTLV, ref newNode, ref classValue, ref isConstructed, ref tagNumber);
            }

            if (result.Fine)
            {
                //Length.
                result = ParseLength(dataStore, ref bitOffset, ref maxLeftBits, nodeTLV, ref newNode, ref contentLength);

                //Convert to bits.
                contentLength = contentLength * 8;
            }

            finalBitOffset = bitOffset;

            Utility.UpdateNodeLength(nodeTLV, "validity", ItemType.ITEM, ((finalBitOffset - initialBitOffset) + contentLength));

            if (result.Fine)
            {
                if ((classValue != 0x0) || (isConstructed != 0x1) || (tagNumber != 0x10))
                {
                    result.SetFailure();
                }
            }

            if (result.Fine)
            {
                result = Utility.AddNodeContainerPlus(Position.CHILD, nodeTLV, out nodeValue, "Values", ItemType.DATA, dataStore, bitOffset, contentLength, maxLeftBits);

                if (result.Fine)
                {
                    maxLeftBits -= contentLength;
                }

                if (result.Fine)
                {
                    result = ParseTime(treeView, nodeValue, dataStore, ref bitOffset, ref contentLength, "notBefore");
                }

                if (result.Fine && (contentLength != 0))
                {
                    result = ParseTime(treeView, nodeValue, dataStore, ref bitOffset, ref contentLength, "notAfter");
                }


            }

            if (!result.Fine)
            {
                Utility.UpdateNode(nodeTLV, "validity", ItemType.ERROR);
                Utility.ExpandToRoot(nodeTLV, ItemType.WARNING);
            }
            return result;
        }

        public Result ParseSubjectPublicKeyInfo(TreeView treeView, TreeNode parent, DataStore dataStore, ref Int64 bitOffset, ref Int64 maxLeftBits)
        {
            Result result = new Result();
            TreeNode newNode = null;
            TreeNode nodeTLV = null;
            TreeNode nodeValue = null;

            Int64 classValue = 0;
            Int64 isConstructed = 0;
            Int64 tagNumber = 0;
            Int64 contentLength = 0;

            Int64 initialBitOffset = 0;
            Int64 finalBitOffset = 0;
            if (result.Fine)
            {
                initialBitOffset = bitOffset;
            }

            //Add one node to indicate this object.
            result = Utility.AddNodeContainer(Position.CHILD, parent, out nodeTLV, "subjectPublicKeyInfo", ItemType.SECTION2, dataStore, bitOffset, 0);


            if (result.Fine)
            {
                //Tag.
                result = ParseTag(dataStore, ref bitOffset, ref maxLeftBits, nodeTLV, ref newNode, ref classValue, ref isConstructed, ref tagNumber);
            }

            if (result.Fine)
            {
                //Length.
                result = ParseLength(dataStore, ref bitOffset, ref maxLeftBits, nodeTLV, ref newNode, ref contentLength);

                //Convert to bits.
                contentLength = contentLength * 8;
            }

            finalBitOffset = bitOffset;

            Utility.UpdateNodeLength(nodeTLV, "subjectPublicKeyInfo", ItemType.ITEM, ((finalBitOffset - initialBitOffset) + contentLength));

            if (result.Fine)
            {
                if ((classValue != 0x0) || (isConstructed != 0x1) || (tagNumber != 0x10))
                {
                    result.SetFailure();
                }
            }

            if (result.Fine)
            {
                result = Utility.AddNodeContainerPlus(Position.CHILD, nodeTLV, out nodeValue, "Values", ItemType.DATA, dataStore, bitOffset, contentLength, maxLeftBits);

                if (result.Fine)
                {
                    maxLeftBits -= contentLength;
                }

                if (result.Fine)
                {
                    result = ParseAlgorithmIdentifier(treeView, nodeValue, dataStore, ref bitOffset, ref contentLength, "algorithm");
                }

                if (result.Fine && (contentLength != 0))
                {
                    result = ParseBitString(treeView, nodeValue, dataStore, ref bitOffset, ref contentLength, "subjectPublicKey");
                }


            }

            if (!result.Fine)
            {
                Utility.UpdateNode(nodeTLV, "subjectPublicKeyInfo", ItemType.ERROR);
                Utility.ExpandToRoot(nodeTLV, ItemType.WARNING);
            }
            return result;
        }

        public Result ParseRelativeDistinguishedName(TreeView treeView, TreeNode parent, DataStore dataStore, ref Int64 bitOffset, ref Int64 maxLeftBits)
        {
            Result result = new Result();
            TreeNode newNode = null;
            TreeNode nodeTLV = null;
            TreeNode nodeValue = null;

            Int64 classValue = 0;
            Int64 isConstructed = 0;
            Int64 tagNumber = 0;
            Int64 contentLength = 0;

            Int64 initialBitOffset = 0;
            Int64 finalBitOffset = 0;
            if (result.Fine)
            {
                initialBitOffset = bitOffset;
            }

            //Add one node to indicate this object.
            result = Utility.AddNodeContainer(Position.CHILD, parent, out nodeTLV, "RelativeDistinguishedName", ItemType.SECTION2, dataStore, bitOffset, 0);


            if (result.Fine)
            {
                //Tag.
                result = ParseTag(dataStore, ref bitOffset, ref maxLeftBits, nodeTLV, ref newNode, ref classValue, ref isConstructed, ref tagNumber);
            }

            if (result.Fine)
            {
                //Length.
                result = ParseLength(dataStore, ref bitOffset, ref maxLeftBits, nodeTLV, ref newNode, ref contentLength);

                //Convert to bits.
                contentLength = contentLength * 8;
            }

            finalBitOffset = bitOffset;

            Utility.UpdateNodeLength(nodeTLV, "RelativeDistinguishedName", ItemType.ITEM, ((finalBitOffset - initialBitOffset) + contentLength));

            if (result.Fine)
            {
                if ((classValue != 0x0) || (isConstructed != 0x1) || (tagNumber != 0x11))
                {
                    result.SetFailure();
                }
            }



            if (result.Fine)
            {
                result = Utility.AddNodeContainerPlus(Position.CHILD, nodeTLV, out nodeValue, "Values", ItemType.DATA, dataStore, bitOffset, contentLength, maxLeftBits);

                if (result.Fine)
                {
                    maxLeftBits -= contentLength;
                }

                while (result.Fine && (contentLength > 0))
                {
                    result = ParseAttributeTypeAndValue(treeView, nodeValue, dataStore, ref bitOffset, ref contentLength);
                }

            }


            if (!result.Fine)
            {
                Utility.UpdateNode(nodeTLV, "RelativeDistinguishedName", ItemType.ERROR);
                Utility.ExpandToRoot(nodeTLV, ItemType.WARNING);
            }
            return result;
        }



        public Result ParseAttributeTypeAndValue(TreeView treeView, TreeNode parent, DataStore dataStore, ref Int64 bitOffset, ref Int64 maxLeftBits)
        {
            Result result = new Result();
            TreeNode newNode = null;
            TreeNode nodeTLV = null;
            TreeNode nodeValue = null;

            Int64 classValue = 0;
            Int64 isConstructed = 0;
            Int64 tagNumber = 0;
            Int64 contentLength = 0;

            Int64 initialBitOffset = 0;
            Int64 finalBitOffset = 0;
            if (result.Fine)
            {
                initialBitOffset = bitOffset;
            }

            //Add one node to indicate this object.
            result = Utility.AddNodeContainer(Position.CHILD, parent, out nodeTLV, "AttributeTypeAndValue", ItemType.SECTION2, dataStore, bitOffset, 0);


            if (result.Fine)
            {
                //Tag.
                result = ParseTag(dataStore, ref bitOffset, ref maxLeftBits, nodeTLV, ref newNode, ref classValue, ref isConstructed, ref tagNumber);
            }

            if (result.Fine)
            {
                //Length.
                result = ParseLength(dataStore, ref bitOffset, ref maxLeftBits, nodeTLV, ref newNode, ref contentLength);

                //Convert to bits.
                contentLength = contentLength * 8;
            }

            finalBitOffset = bitOffset;

            Utility.UpdateNodeLength(nodeTLV, "AttributeTypeAndValue", ItemType.ITEM, ((finalBitOffset - initialBitOffset) + contentLength));

            if (result.Fine)
            {
                if ((classValue != 0x0) || (isConstructed != 0x1) || (tagNumber != 0x10))
                {
                    result.SetFailure();
                }
            }



            if (result.Fine)
            {
                result = Utility.AddNodeContainerPlus(Position.CHILD, nodeTLV, out nodeValue, "Values", ItemType.DATA, dataStore, bitOffset, contentLength, maxLeftBits);

                if (result.Fine)
                {
                    maxLeftBits -= contentLength;
                }

                while (result.Fine && (contentLength > 0))
                {
                    if (result.Fine)
                    {
                        result = ParseAttributeType(treeView, nodeValue, dataStore, ref bitOffset, ref contentLength);
                    }

                    if (result.Fine && (contentLength != 0))
                    {
                        result = ParseAttributeValue(treeView, nodeValue, dataStore, ref bitOffset, ref contentLength);
                    }
                }

            }


            if (!result.Fine)
            {
                Utility.UpdateNode(nodeTLV, "AttributeTypeAndValue", ItemType.ERROR);
                Utility.ExpandToRoot(nodeTLV, ItemType.WARNING);
            }
            return result;
        }


        public Result ParseAttributeType(TreeView treeView, TreeNode parent, DataStore dataStore, ref Int64 bitOffset, ref Int64 maxLeftBits)
        {
            Result result = new Result();
            TreeNode newNode = null;
            TreeNode nodeTLV = null;
            TreeNode nodeValue = null;

            Int64 classValue = 0;
            Int64 isConstructed = 0;
            Int64 tagNumber = 0;
            Int64 contentLength = 0;

            Int64 initialBitOffset = 0;
            Int64 finalBitOffset = 0;
            if (result.Fine)
            {
                initialBitOffset = bitOffset;
            }

            //Add one node to indicate this object.
            result = Utility.AddNodeContainer(Position.CHILD, parent, out nodeTLV, "AttributeType", ItemType.SECTION2, dataStore, bitOffset, 0);


            if (result.Fine)
            {
                //Tag.
                result = ParseTag(dataStore, ref bitOffset, ref maxLeftBits, nodeTLV, ref newNode, ref classValue, ref isConstructed, ref tagNumber);
            }

            if (result.Fine)
            {
                //Length.
                result = ParseLength(dataStore, ref bitOffset, ref maxLeftBits, nodeTLV, ref newNode, ref contentLength);

                //Convert to bits.
                contentLength = contentLength * 8;
            }

            finalBitOffset = bitOffset;

            Utility.UpdateNodeLength(nodeTLV, "AttributeType", ItemType.ITEM, ((finalBitOffset - initialBitOffset) + contentLength));

            if (result.Fine)
            {
                if ((classValue != 0x0) || (isConstructed != 0x0) || (tagNumber != 0x6))
                {
                    result.SetFailure();
                }
            }

            if (result.Fine)
            {
                result = Utility.AddNodeContainerPlus(Position.CHILD, nodeTLV, out nodeValue, "Values", ItemType.DATA, dataStore, bitOffset, contentLength, maxLeftBits);

                if (result.Fine)
                {
                    result = Utility.AddNodeDataPlus(Position.CHILD, nodeValue, out newNode, "AttributeTypeIdentifier", ItemType.FIELD, dataStore, ref bitOffset, contentLength, ref maxLeftBits);
                }
            }



            if (!result.Fine)
            {
                Utility.UpdateNode(nodeTLV, "AttributeType", ItemType.ERROR);
                Utility.ExpandToRoot(nodeTLV, ItemType.WARNING);
            }
            return result;
        }

        public Result ParseAttributeValue(TreeView treeView, TreeNode parent, DataStore dataStore, ref Int64 bitOffset, ref Int64 maxLeftBits)
        {
            Result result = new Result();
            TreeNode newNode = null;
            TreeNode nodeTLV = null;
            TreeNode nodeValue = null;

            Int64 classValue = 0;
            Int64 isConstructed = 0;
            Int64 tagNumber = 0;
            Int64 contentLength = 0;

            Int64 initialBitOffset = 0;
            Int64 finalBitOffset = 0;
            if (result.Fine)
            {
                initialBitOffset = bitOffset;
            }

            //Add one node to indicate this object.
            result = Utility.AddNodeContainer(Position.CHILD, parent, out nodeTLV, "AttributeValue", ItemType.SECTION2, dataStore, bitOffset, 0);


            if (result.Fine)
            {
                //Tag.
                result = ParseTag(dataStore, ref bitOffset, ref maxLeftBits, nodeTLV, ref newNode, ref classValue, ref isConstructed, ref tagNumber);
            }

            if (result.Fine)
            {
                //Length.
                result = ParseLength(dataStore, ref bitOffset, ref maxLeftBits, nodeTLV, ref newNode, ref contentLength);

                //Convert to bits.
                contentLength = contentLength * 8;
            }

            finalBitOffset = bitOffset;

            Utility.UpdateNodeLength(nodeTLV, "AttributeValue", ItemType.ITEM, ((finalBitOffset - initialBitOffset) + contentLength));



            if (result.Fine)
            {
                result = Utility.AddNodeDataPlus(Position.CHILD, nodeTLV, out nodeValue, "Values", ItemType.DATA, dataStore, ref bitOffset, contentLength, ref maxLeftBits);

            }

            if (!result.Fine)
            {
                Utility.UpdateNode(nodeTLV, "AttributeValue", ItemType.ERROR);
                Utility.ExpandToRoot(nodeTLV, ItemType.WARNING);
            }
            return result;
        }

        public Result ParseTime(TreeView treeView, TreeNode parent, DataStore dataStore, ref Int64 bitOffset, ref Int64 maxLeftBits, String titleOfNode)
        {
            Result result = new Result();
            TreeNode newNode = null;
            TreeNode nodeTLV = null;
            TreeNode nodeValue = null;


            Int64 classValue = 0;
            Int64 isConstructed = 0;
            Int64 tagNumber = 0;
            Int64 contentLength = 0;

            Int64 initialBitOffset = 0;
            Int64 finalBitOffset = 0;
            if (result.Fine)
            {
                initialBitOffset = bitOffset;
                result = Utility.AddNodeContainerPlus(Position.CHILD, parent, out nodeTLV, titleOfNode, ItemType.ITEM, dataStore, bitOffset, 0, maxLeftBits);
            }
            if (result.Fine)
            {
                //Tag.
                result = ParseTag(dataStore, ref bitOffset, ref maxLeftBits, nodeTLV, ref newNode, ref classValue, ref isConstructed, ref tagNumber);
            }

            if (result.Fine)
            {
                //Length.
                result = ParseLength(dataStore, ref bitOffset, ref maxLeftBits, nodeTLV, ref newNode, ref contentLength);

                //Convert to bits.
                contentLength = contentLength * 8;
            }

            finalBitOffset = bitOffset;

            Utility.UpdateNodeLength(nodeTLV, titleOfNode, ItemType.ITEM, ((finalBitOffset - initialBitOffset) + contentLength));

            if (result.Fine)
            {
                //Value.
                result = Utility.AddNodeContainerPlus(Position.CHILD, nodeTLV, out nodeValue, "Values", ItemType.DATA, dataStore, bitOffset, contentLength, maxLeftBits);
            }

            if (result.Fine)
            {
                //Important! Remember to reduce maxLeftBits.
                maxLeftBits -= contentLength;

                switch (tagNumber)
                {
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


            if (!result.Fine)
            {
                Utility.UpdateNode(nodeTLV, titleOfNode, ItemType.ERROR);
                Utility.ExpandToRoot(nodeTLV, ItemType.WARNING);
            }
            return result;

        }


        public Result ParseBitString(TreeView treeView, TreeNode parent, DataStore dataStore, ref Int64 bitOffset, ref Int64 maxLeftBits, String titleOfString)
        {
            Result result = new Result();
            TreeNode newNode = null;
            TreeNode nodeTLV = null;
            TreeNode nodeValue = null;

            Int64 classValue = 0;
            Int64 isConstructed = 0;
            Int64 tagNumber = 0;
            Int64 contentLength = 0;

            Int64 initialBitOffset = 0;
            Int64 finalBitOffset = 0;
            if (result.Fine)
            {
                initialBitOffset = bitOffset;
            }

            //Add one node to indicate this object.
            result = Utility.AddNodeContainer(Position.CHILD, parent, out nodeTLV, titleOfString, ItemType.SECTION2, dataStore, bitOffset, 0);


            if (result.Fine)
            {
                //Tag.
                result = ParseTag(dataStore, ref bitOffset, ref maxLeftBits, nodeTLV, ref newNode, ref classValue, ref isConstructed, ref tagNumber);
            }

            if (result.Fine)
            {
                //Length.
                result = ParseLength(dataStore, ref bitOffset, ref maxLeftBits, nodeTLV, ref newNode, ref contentLength);

                //Convert to bits.
                contentLength = contentLength * 8;
            }

            finalBitOffset = bitOffset;

            Utility.UpdateNodeLength(nodeTLV, titleOfString, ItemType.ITEM, ((finalBitOffset - initialBitOffset) + contentLength));

            if (result.Fine)
            {
                if ((classValue != 0x0) || (isConstructed != 0x0) || (tagNumber != 0x03))
                {
                    result.SetFailure();
                }
            }

            if (result.Fine)
            {
                result = Utility.AddNodeContainerPlus(Position.CHILD, nodeTLV, out nodeValue, "Values", ItemType.DATA, dataStore, bitOffset, contentLength, maxLeftBits);

                if (result.Fine)
                {
                    result = Utility.AddNodeDataPlus(Position.CHILD, nodeValue, out newNode, "BitString", ItemType.FIELD, dataStore, ref bitOffset, contentLength, ref maxLeftBits);
                }
            }



            if (!result.Fine)
            {
                Utility.UpdateNode(nodeTLV, titleOfString, ItemType.ERROR);
                Utility.ExpandToRoot(nodeTLV, ItemType.WARNING);
            }
            return result;
        }



        public Result ParseTBSCertificateOptionalFields(TreeView treeView, TreeNode parent, DataStore dataStore, ref Int64 bitOffset, ref Int64 maxLeftBits)
        {
            Result result = new Result();
            TreeNode newNode = null;
            TreeNode nodeTLV = null;
            TreeNode nodeValue = null;


            Int64 classValue = 0;
            Int64 isConstructed = 0;
            Int64 tagNumber = 0;
            Int64 contentLength = 0;

            Int64 initialBitOffset = 0;
            Int64 finalBitOffset = 0;

            while (result.Fine && maxLeftBits > 0)
            {
                if (result.Fine)
                {
                    initialBitOffset = bitOffset;
                    result = Utility.AddNodeContainerPlus(Position.CHILD, parent, out nodeTLV, "TBSCertificateOptionalFields", ItemType.ITEM, dataStore, bitOffset, 0, maxLeftBits);
                }
                if (result.Fine)
                {
                    //Tag.
                    result = ParseTag(dataStore, ref bitOffset, ref maxLeftBits, nodeTLV, ref newNode, ref classValue, ref isConstructed, ref tagNumber);
                }

                if (result.Fine)
                {
                    //Length.
                    result = ParseLength(dataStore, ref bitOffset, ref maxLeftBits, nodeTLV, ref newNode, ref contentLength);

                    //Convert to bits.
                    contentLength = contentLength * 8;
                }

                finalBitOffset = bitOffset;
                

                if (result.Fine)
                {
                    //Value.
                    result = Utility.AddNodeContainerPlus(Position.CHILD, nodeTLV, out nodeValue, "Values", ItemType.DATA, dataStore, bitOffset, contentLength, maxLeftBits);
                }

                if (result.Fine)
                {
                    //Important! Remember to reduce maxLeftBits.
                    maxLeftBits -= contentLength;
                }

                if ((classValue == 0x2) && (isConstructed == 0x1) && (tagNumber == 0x01))
                {
                    Utility.UpdateNodeLength(nodeTLV, "issuerUniqueID", ItemType.ITEM, ((finalBitOffset - initialBitOffset) + contentLength));

                    result = ParseBitString(treeView, nodeValue, dataStore, ref bitOffset, ref contentLength, "issuerUniqueID");
                }
                else if ((classValue == 0x2) && (isConstructed == 0x1) && (tagNumber == 0x02))
                {
                    Utility.UpdateNodeLength(nodeTLV, "subjectUniqueID", ItemType.ITEM, ((finalBitOffset - initialBitOffset) + contentLength));
                    result = ParseBitString(treeView, nodeValue, dataStore, ref bitOffset, ref contentLength, "subjectUniqueID");
                }
                if ((classValue == 0x2) && (isConstructed == 0x1) && (tagNumber == 0x03))
                {
                    Utility.UpdateNodeLength(nodeTLV, "extensions", ItemType.ITEM, ((finalBitOffset - initialBitOffset) + contentLength));
                    result = ParseSequenceOfExtensions(treeView, nodeValue, dataStore, ref bitOffset, ref contentLength);
                }
                else
                {
                    Utility.UpdateNodeLength(nodeTLV, "unknown context specific extensions", ItemType.ITEM, ((finalBitOffset - initialBitOffset) + contentLength));
                    Console.WriteLine("tag Value:" + String.Format("0x{0:X2}", tagNumber));
                    result = Utility.AddNodeDataPlus(Position.CHILD, nodeValue, out newNode, "Values", ItemType.DATA, dataStore, ref bitOffset, contentLength, ref contentLength);

                }
                   
                if (!result.Fine)
                {
                    Utility.UpdateNode(nodeTLV, "TBSCertificateOptionalFields", ItemType.ERROR);
                    Utility.ExpandToRoot(nodeTLV, ItemType.WARNING);
                }
            }
            

            return result;
        }


        public Result ParseSequenceOfExtensions(TreeView treeView, TreeNode parent, DataStore dataStore, ref Int64 bitOffset, ref Int64 maxLeftBits)
        {
            Result result = new Result();

            TreeNode newNode = null;
            TreeNode nodeTLV = null;
            TreeNode nodeValue = null;

            Int64 classValue = 0;
            Int64 isConstructed = 0;
            Int64 tagNumber = 0;
            Int64 contentLength = 0;

            Int64 initialBitOffset = 0;
            Int64 finalBitOffset = 0;

            while (result.Fine && (maxLeftBits > 0))
            {
                if (result.Fine)
                {
                    initialBitOffset = bitOffset;
                }

                if (result.Fine)
                {
                    //Add one node to indicate this object.
                    result = Utility.AddNodeContainer(Position.CHILD, parent, out nodeTLV, "extensions", ItemType.SECTION2, dataStore, bitOffset, 0);
                }
                
                if (result.Fine)
                {
                    //Tag.
                    result = ParseTag(dataStore, ref bitOffset, ref maxLeftBits, nodeTLV, ref newNode, ref classValue, ref isConstructed, ref tagNumber);
                }

                if (result.Fine)
                {
                    //Length.
                    result = ParseLength(dataStore, ref bitOffset, ref maxLeftBits, nodeTLV, ref newNode, ref contentLength);

                    //Convert to bits.
                    contentLength = contentLength * 8;
                }


                if (result.Fine)
                {
                    //Value.
                    result = Utility.AddNodeContainerPlus(Position.CHILD, nodeTLV, out nodeValue, "Values", ItemType.DATA, dataStore, bitOffset, contentLength, maxLeftBits);
                }


                if (result.Fine)
                {
                    finalBitOffset = bitOffset;

                    Utility.UpdateNodeLength(nodeTLV, "extensions", ItemType.ITEM, ((finalBitOffset - initialBitOffset) + contentLength));

                    //Important!
                    maxLeftBits -= contentLength;
                }

                if (result.Fine)
                {
                    if ((classValue != 0x0) || (isConstructed != 0x1) || (tagNumber != 0x10))
                    {
                        result.SetFailure();
                    }
                }

                while((result.Fine) &&(contentLength >0 ))
                {
                    result = ParseExtensions(treeView, nodeValue, dataStore, ref bitOffset, ref contentLength);
                }
                
            }

            return result;
        }

        public Result ParseExtensions(TreeView treeView, TreeNode parent, DataStore dataStore, ref Int64 bitOffset, ref Int64 maxLeftBits)
        {
            Result result = new Result();
            TreeNode newNode = null;
            TreeNode nodeTLV = null;
            TreeNode nodeValue = null;

            Int64 classValue = 0;
            Int64 isConstructed = 0;
            Int64 tagNumber = 0;
            Int64 contentLength = 0;

            Int64 initialBitOffset = 0;
            Int64 finalBitOffset = 0;
            if (result.Fine)
            {
                initialBitOffset = bitOffset;
            }

            //Add one node to indicate this object.
            result = Utility.AddNodeContainer(Position.CHILD, parent, out nodeTLV, "extension", ItemType.SECTION2, dataStore, bitOffset, 0);


            if (result.Fine)
            {
                //Tag.
                result = ParseTag(dataStore, ref bitOffset, ref maxLeftBits, nodeTLV, ref newNode, ref classValue, ref isConstructed, ref tagNumber);
            }

            if (result.Fine)
            {
                //Length.
                result = ParseLength(dataStore, ref bitOffset, ref maxLeftBits, nodeTLV, ref newNode, ref contentLength);

                //Convert to bits.
                contentLength = contentLength * 8;
            }

            finalBitOffset = bitOffset;

            Utility.UpdateNodeLength(nodeTLV, "extension", ItemType.ITEM, ((finalBitOffset - initialBitOffset) + contentLength));

            if (result.Fine)
            {
                if ((classValue != 0x0) || (isConstructed != 0x1) || (tagNumber != 0x10))
                {
                    result.SetFailure();
                }
            }



            if (result.Fine)
            {
                result = Utility.AddNodeContainerPlus(Position.CHILD, nodeTLV, out nodeValue, "Values", ItemType.DATA, dataStore, bitOffset, contentLength, maxLeftBits);

                if (result.Fine)
                {
                    maxLeftBits -= contentLength;
                }

                while (result.Fine && (contentLength > 0))
                {
                    result = ParseExtension(treeView, nodeValue, dataStore, ref bitOffset, ref contentLength);
                }

            }


            if (!result.Fine)
            {
                Utility.UpdateNode(nodeTLV, "extension", ItemType.ERROR);
                Utility.ExpandToRoot(nodeTLV, ItemType.WARNING);
            }
            return result;
        }



        public Result ParseExtension(TreeView treeView, TreeNode parent, DataStore dataStore, ref Int64 bitOffset, ref Int64 maxLeftBits)
        {
            Result result = new Result();
            TreeNode newNode = null;
            TreeNode nodeTLV = null;
            TreeNode nodeValue = null;

            Int64 classValue = 0;
            Int64 isConstructed = 0;
            Int64 tagNumber = 0;
            Int64 contentLength = 0;
            Int64 fieldValue = 0;

            Int64 initialBitOffset = 0;
            Int64 finalBitOffset = 0;
            if (result.Fine)
            {
                initialBitOffset = bitOffset;
            }

            //Add one node to indicate this object.
            result = Utility.AddNodeContainer(Position.CHILD, parent, out nodeTLV, "unknown extension", ItemType.SECTION2, dataStore, bitOffset, 0);


            if (result.Fine)
            {
                //Tag.
                result = ParseTag(dataStore, ref bitOffset, ref maxLeftBits, nodeTLV, ref newNode, ref classValue, ref isConstructed, ref tagNumber);
            }

            if (result.Fine)
            {
                //Length.
                result = ParseLength(dataStore, ref bitOffset, ref maxLeftBits, nodeTLV, ref newNode, ref contentLength);

                //Convert to bits.
                contentLength = contentLength * 8;
            }

            finalBitOffset = bitOffset;

            if (result.Fine)
            {
                switch (tagNumber)
                {
                    case 0x06:
                        //extnID
                        Utility.UpdateNodeLength(nodeTLV, "extnID", ItemType.ITEM, ((finalBitOffset - initialBitOffset) + contentLength));
                        break;
                    case 0x01:
                        //critical
                        Utility.UpdateNodeLength(nodeTLV, "critical", ItemType.ITEM, ((finalBitOffset - initialBitOffset) + contentLength));
                        break;
                    case 0x04:
                        //extnValue
                        Utility.UpdateNodeLength(nodeTLV, "extnValue", ItemType.ITEM, ((finalBitOffset - initialBitOffset) + contentLength));
                        break;
                    default:
                        result.SetFailure();
                        break;
                }
            }



            if (result.Fine)
            {
                result = Utility.AddNodeContainerPlus(Position.CHILD, nodeTLV, out nodeValue, "Values", ItemType.DATA, dataStore, bitOffset, contentLength, maxLeftBits);

                if (result.Fine)
                {
                    maxLeftBits -= contentLength;
                }

                switch (tagNumber)
                {
                    case 0x06:
                        //extnID
                        Utility.UpdateNodeLength(nodeTLV, "extnID", ItemType.ITEM, ((finalBitOffset - initialBitOffset) + contentLength));
                        result = Utility.AddNodeDataPlus(Position.CHILD, nodeValue, out newNode, "extnID", ItemType.DATA, dataStore, ref bitOffset, contentLength, ref contentLength);
                        break;
                    case 0x01:
                        //critical
                        Utility.UpdateNodeLength(nodeTLV, "critical", ItemType.ITEM, ((finalBitOffset - initialBitOffset) + contentLength));
                        //This field has default value. It means that this field indeed is optional.
                        result = Utility.AddNodeFieldPlus(Position.CHILD, nodeValue, out newNode, "critical", ItemType.DATA, dataStore, ref bitOffset, contentLength, ref fieldValue, ref contentLength);
                        break;
                    case 0x04:
                        //extnValue
                        Utility.UpdateNodeLength(nodeTLV, "extnValue", ItemType.ITEM, ((finalBitOffset - initialBitOffset) + contentLength));
                        result = Utility.AddNodeDataPlus(Position.CHILD, nodeValue, out newNode, "extnValue", ItemType.DATA, dataStore, ref bitOffset, contentLength, ref contentLength);
                        break;
                    default:
                        result.SetFailure();
                        break;
                }
                
            }


            if (!result.Fine)
            {
                Utility.UpdateNode(nodeTLV, "unknown extension", ItemType.ERROR);
                Utility.ExpandToRoot(nodeTLV, ItemType.WARNING);
            }
            return result;
        }

        public Result ParseExtnID(TreeView treeView, TreeNode parent, DataStore dataStore, ref Int64 bitOffset, ref Int64 maxLeftBits)
        {
            Result result = new Result();
            TreeNode newNode = null;
            TreeNode nodeTLV = null;
            TreeNode nodeValue = null;


            Int64 classValue = 0;
            Int64 isConstructed = 0;
            Int64 tagNumber = 0;
            Int64 contentLength = 0;

            Int64 initialBitOffset = 0;
            Int64 finalBitOffset = 0;
            if (result.Fine)
            {
                initialBitOffset = bitOffset;
                result = Utility.AddNodeContainerPlus(Position.CHILD, parent, out nodeTLV, "extnID", ItemType.ITEM, dataStore, bitOffset, 0, maxLeftBits);
            }
            if (result.Fine)
            {
                //Tag.
                result = ParseTag(dataStore, ref bitOffset, ref maxLeftBits, nodeTLV, ref newNode, ref classValue, ref isConstructed, ref tagNumber);
            }

            if (result.Fine)
            {
                //Length.
                result = ParseLength(dataStore, ref bitOffset, ref maxLeftBits, nodeTLV, ref newNode, ref contentLength);

                //Convert to bits.
                contentLength = contentLength * 8;
            }

            finalBitOffset = bitOffset;

            Utility.UpdateNodeLength(nodeTLV, "extnID", ItemType.ITEM, ((finalBitOffset - initialBitOffset) + contentLength));

            if (result.Fine)
            {
                //Value.
                result = Utility.AddNodeContainerPlus(Position.CHILD, nodeTLV, out nodeValue, "Values", ItemType.DATA, dataStore, bitOffset, contentLength, maxLeftBits);
            }

            if ((classValue != 0x0) || (isConstructed != 0x0) || (tagNumber != 0x6))
            {
                result.SetFailure();
            }

            if (result.Fine)
            {
                //Important! Remember to reduce maxLeftBits.
                maxLeftBits -= contentLength;

            }


            if (result.Fine)
            {
                result = Utility.AddNodeDataPlus(Position.CHILD, nodeValue, out newNode, "extnID", ItemType.DATA, dataStore, ref bitOffset, contentLength, ref contentLength);
            }
            

            if (!result.Fine)
            {
                Utility.UpdateNode(nodeTLV, "extnID", ItemType.ERROR);
                Utility.ExpandToRoot(nodeTLV, ItemType.WARNING);
            }
            return result;

        }


        public Result ParseCritical(TreeView treeView, TreeNode parent, DataStore dataStore, ref Int64 bitOffset, ref Int64 maxLeftBits)
        {
            Result result = new Result();
            TreeNode newNode = null;
            TreeNode nodeTLV = null;
            TreeNode nodeValue = null;
            Int64 fieldValue = 0;


            Int64 classValue = 0;
            Int64 isConstructed = 0;
            Int64 tagNumber = 0;
            Int64 contentLength = 0;

            Int64 initialBitOffset = 0;
            Int64 finalBitOffset = 0;
            if (result.Fine)
            {
                initialBitOffset = bitOffset;
                result = Utility.AddNodeContainerPlus(Position.CHILD, parent, out nodeTLV, "critical", ItemType.ITEM, dataStore, bitOffset, 0, maxLeftBits);
            }
            if (result.Fine)
            {
                //Tag.
                result = ParseTag(dataStore, ref bitOffset, ref maxLeftBits, nodeTLV, ref newNode, ref classValue, ref isConstructed, ref tagNumber);
            }

            if (result.Fine)
            {
                //Length.
                result = ParseLength(dataStore, ref bitOffset, ref maxLeftBits, nodeTLV, ref newNode, ref contentLength);

                //Convert to bits.
                contentLength = contentLength * 8;
            }

            finalBitOffset = bitOffset;

            Utility.UpdateNodeLength(nodeTLV, "critical", ItemType.ITEM, ((finalBitOffset - initialBitOffset) + contentLength));

            if (result.Fine)
            {
                //Value.
                result = Utility.AddNodeContainerPlus(Position.CHILD, nodeTLV, out nodeValue, "critical", ItemType.DATA, dataStore, bitOffset, contentLength, maxLeftBits);
            }

            if ((classValue != 0x0) || (isConstructed != 0x0) || (tagNumber != 0x1))
            {
                result.SetFailure();
            }

            if (result.Fine)
            {
                //Important! Remember to reduce maxLeftBits.
                maxLeftBits -= contentLength;

            }

            if (result.Fine)
            {
                result = Utility.AddNodeFieldPlus(Position.CHILD, nodeValue, out newNode, "critical", ItemType.DATA, dataStore, ref bitOffset, contentLength, ref fieldValue, ref contentLength);
            }

            if (!result.Fine)
            {
                Utility.UpdateNode(nodeTLV, "critical", ItemType.ERROR);
                Utility.ExpandToRoot(nodeTLV, ItemType.WARNING);
            }
            return result;

        }



        public Result ParseExtnValue(TreeView treeView, TreeNode parent, DataStore dataStore, ref Int64 bitOffset, ref Int64 maxLeftBits)
        {
            Result result = new Result();
            TreeNode newNode = null;
            TreeNode nodeTLV = null;
            TreeNode nodeValue = null;


            Int64 classValue = 0;
            Int64 isConstructed = 0;
            Int64 tagNumber = 0;
            Int64 contentLength = 0;

            Int64 initialBitOffset = 0;
            Int64 finalBitOffset = 0;
            if (result.Fine)
            {
                initialBitOffset = bitOffset;
                result = Utility.AddNodeContainerPlus(Position.CHILD, parent, out nodeTLV, "extnValue", ItemType.ITEM, dataStore, bitOffset, 0, maxLeftBits);
            }
            if (result.Fine)
            {
                //Tag.
                result = ParseTag(dataStore, ref bitOffset, ref maxLeftBits, nodeTLV, ref newNode, ref classValue, ref isConstructed, ref tagNumber);
            }

            if (result.Fine)
            {
                //Length.
                result = ParseLength(dataStore, ref bitOffset, ref maxLeftBits, nodeTLV, ref newNode, ref contentLength);

                //Convert to bits.
                contentLength = contentLength * 8;
            }

            finalBitOffset = bitOffset;

            Utility.UpdateNodeLength(nodeTLV, "extnValue", ItemType.ITEM, ((finalBitOffset - initialBitOffset) + contentLength));

            if (result.Fine)
            {
                //Value.
                result = Utility.AddNodeContainerPlus(Position.CHILD, nodeTLV, out nodeValue, "Values", ItemType.DATA, dataStore, bitOffset, contentLength, maxLeftBits);
            }

            if ((classValue != 0x0) || (isConstructed != 0x0) || (tagNumber != 0x4))
            {
                result.SetFailure();
            }

            if (result.Fine)
            {
                //Important! Remember to reduce maxLeftBits.
                maxLeftBits -= contentLength;

            }

            if (result.Fine)
            {
                result = Utility.AddNodeDataPlus(Position.CHILD, nodeValue, out newNode, "extnValue", ItemType.DATA, dataStore, ref bitOffset, contentLength, ref contentLength);
            }

            if (!result.Fine)
            {
                Utility.UpdateNode(nodeTLV, "extnValue", ItemType.ERROR);
                Utility.ExpandToRoot(nodeTLV, ItemType.WARNING);
            }
            return result;

        }



        public Result ParseSignatureAlgorithm(TreeView treeView, TreeNode parent, DataStore dataStore, ref Int64 bitOffset, ref Int64 maxLeftBits)
        {
            Result result = new Result();
            TreeNode newNode = null;
            TreeNode nodeTLV = null;

            Int64 classValue = 0;
            Int64 isConstructed = 0;
            Int64 tagNumber = 0;
            Int64 contentLength = 0;

            Int64 initialBitOffset = 0;
            Int64 finalBitOffset = 0;
            if (result.Fine)
            {
                initialBitOffset = bitOffset;
            }

            //Add one node to indicate this object.
            result = Utility.AddNodeContainer(Position.CHILD, parent, out nodeTLV, "signatureAlgorithm", ItemType.SECTION2, dataStore, bitOffset, 0);


            if (result.Fine)
            {
                //Tag.
                result = ParseTag(dataStore, ref bitOffset, ref maxLeftBits, nodeTLV, ref newNode, ref classValue, ref isConstructed, ref tagNumber);
            }

            if (result.Fine)
            {
                //Length.
                result = ParseLength(dataStore, ref bitOffset, ref maxLeftBits, nodeTLV, ref newNode, ref contentLength);

                //Convert to bits.
                contentLength = contentLength * 8;
            }

            finalBitOffset = bitOffset;

            Utility.UpdateNodeLength(nodeTLV, "signatureAlgorithm", ItemType.ITEM, ((finalBitOffset - initialBitOffset) + contentLength));

            if (result.Fine)
            {
                result = Utility.AddNodeDataPlus(Position.CHILD, nodeTLV, out newNode, "Values", ItemType.DATA, dataStore, ref bitOffset, contentLength, ref contentLength);

                if (result.Fine)
                {
                    maxLeftBits -= contentLength;
                }
            }

            if (!result.Fine)
            {
                Utility.UpdateNode(nodeTLV, "signatureAlgorithm", ItemType.ERROR);
                Utility.ExpandToRoot(nodeTLV, ItemType.WARNING);
            }
            return result;
        }



        public Result ParseSignatureValue(TreeView treeView, TreeNode parent, DataStore dataStore, ref Int64 bitOffset, ref Int64 maxLeftBits)
        {
            Result result = new Result();
            TreeNode newNode = null;
            TreeNode nodeTLV = null;

            Int64 classValue = 0;
            Int64 isConstructed = 0;
            Int64 tagNumber = 0;
            Int64 contentLength = 0;

            Int64 initialBitOffset = 0;
            Int64 finalBitOffset = 0;
            if (result.Fine)
            {
                initialBitOffset = bitOffset;
            }

            //Add one node to indicate this object.
            result = Utility.AddNodeContainer(Position.CHILD, parent, out nodeTLV, "signatureValue", ItemType.SECTION2, dataStore, bitOffset, 0);


            if (result.Fine)
            {
                //Tag.
                result = ParseTag(dataStore, ref bitOffset, ref maxLeftBits, nodeTLV, ref newNode, ref classValue, ref isConstructed, ref tagNumber);
            }

            if (result.Fine)
            {
                //Length.
                result = ParseLength(dataStore, ref bitOffset, ref maxLeftBits, nodeTLV, ref newNode, ref contentLength);

                //Convert to bits.
                contentLength = contentLength * 8;
            }

            finalBitOffset = bitOffset;

            Utility.UpdateNodeLength(nodeTLV, "signatureValue", ItemType.ITEM, ((finalBitOffset - initialBitOffset) + contentLength));

            if (result.Fine)
            {
                result = Utility.AddNodeDataPlus(Position.CHILD, nodeTLV, out newNode, "Values", ItemType.DATA, dataStore, ref bitOffset, contentLength, ref contentLength);


                if (result.Fine)
                {
                    maxLeftBits -= contentLength;
                }
            }

            if (!result.Fine)
            {
                Utility.UpdateNode(nodeTLV, "signatureValue", ItemType.ERROR);
                Utility.ExpandToRoot(nodeTLV, ItemType.WARNING);
            }
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
        private Result ParseTag(DataStore dataStore, ref Int64 bitOffset, ref Int64 maxLeftBits, TreeNode nodeParent, ref TreeNode nodeForTag, ref Int64 classValue, ref Int64 isConstructed, ref Int64 tagNumber)
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


            return result;

        }

        public Result ParseEnumeratedType(TreeView treeView, TreeNode parent, DataStore dataStore, ref Int64 bitOffset, ref Int64 maxLeftBits)
        {
            Result result = new Result();
            TreeNode newNode = null;


            result = Utility.AddNodeDataPlus(Position.CHILD, parent, out newNode, "Enumerated", ItemType.DATA, dataStore, ref bitOffset, maxLeftBits, ref maxLeftBits);


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
