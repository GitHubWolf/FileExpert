using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Library;
using FileExpert.XMR;

namespace FileExpert.XMR
{
    class ParserXMR
    {
        public void ParseXMR(TreeView treeView, TreeNode parent, DataStore dataStore)
        {
            TreeNode nodeObject = null;
            TreeNode newNode = null;

            //Add one node to indicate this object.
            Result result = Utility.AddNodeContainer(Position.CHILD, parent, out nodeObject, "XMR", ItemType.SECTION2, dataStore, 0, dataStore.GetLeftBitLength());

            Int64 bitOffset = 0;
            Int64 fieldValue = 0;

            if (result.Fine)
            {
                result = Utility.AddNodeField(Position.CHILD, nodeObject, out newNode, "Header Constant", ItemType.FIELD, dataStore, ref bitOffset, 32, ref fieldValue);
            }

            if (result.Fine)
            {
                result = Utility.AddNodeField(Position.CHILD, nodeObject, out newNode, "XMR Version", ItemType.FIELD, dataStore, ref bitOffset, 32, ref fieldValue);
            }

            if (result.Fine)
            {
                result = Utility.AddNodeData(Position.CHILD, nodeObject, out newNode, "Rights ID", ItemType.FIELD, dataStore, ref bitOffset, 128);
            }

            if (result.Fine)
            {
                ParseObjects(treeView, nodeObject, dataStore, ref bitOffset);
            }
        }

        private Result ParseMinimumEnvironmentObject(DataStore dataStore, ref Int64 bitOffset, TreeNode nodeBox, ref TreeNode newNode, ref Int64 fieldValue, TreeNode payloadNode, Int64 payloadLengthInBit)
        {
            Result result = new Result();

            result = Utility.UpdateNode(nodeBox, "Minimum Environment Object ", ItemType.ITEM);
            if (result.Fine)
            {
                result = Utility.AddNodeFieldPlus(Position.CHILD, payloadNode, out newNode, "Minimum Security Level", ItemType.FIELD, dataStore, ref bitOffset, 16, ref fieldValue, ref payloadLengthInBit);
            }
            if (result.Fine)
            {
                result = Utility.AddNodeFieldPlus(Position.CHILD, payloadNode, out newNode, "Reserved", ItemType.FIELD, dataStore, ref bitOffset, 32, ref fieldValue, ref payloadLengthInBit);
            }
            if (result.Fine)
            {
                result = Utility.AddNodeFieldPlus(Position.CHILD, payloadNode, out newNode, "Minimum Device Revocation List Version", ItemType.FIELD, dataStore, ref bitOffset, 32, ref fieldValue, ref payloadLengthInBit);
            }

            return result;
        }

        private Result ParseRevocationInformationVersionObject(DataStore dataStore, ref Int64 bitOffset, TreeNode nodeBox, ref TreeNode newNode, ref Int64 fieldValue, TreeNode payloadNode, Int64 payloadLengthInBit)
        {
            Result result = new Result();

            result = Utility.UpdateNode(nodeBox, "Revocation Information Version Object ", ItemType.ITEM);

            if (result.Fine)
            {
                result = Utility.AddNodeFieldPlus(Position.CHILD, payloadNode, out newNode, "RIV", ItemType.FIELD, dataStore, ref bitOffset, 32, ref fieldValue, ref payloadLengthInBit);
            }

            return result;
        }

        private Result ParseInclusionListObject(DataStore dataStore, ref Int64 bitOffset, TreeNode nodeBox, ref TreeNode newNode, ref Int64 fieldValue, TreeNode payloadNode, Int64 payloadLengthInBit)
        {
            Result result = new Result();

            result = Utility.UpdateNode(nodeBox, "Inclusion List Object ", ItemType.ITEM);

            Int64 numItems = 0;
            if (result.Fine)
            {
                result = Utility.AddNodeFieldPlus(Position.CHILD, payloadNode, out newNode, "Num Items", ItemType.FIELD, dataStore, ref bitOffset, 32, ref numItems, ref payloadLengthInBit);
            }

            if (result.Fine)
            {
                result = Utility.AddNodeDataPlus(Position.CHILD, payloadNode, out newNode, "Included Output IDs", ItemType.FIELD, dataStore, ref bitOffset, 128 * numItems, ref payloadLengthInBit);
            }

            return result;
        }

        private Result ParseSerialNumberRestrictionObject(DataStore dataStore, ref Int64 bitOffset, TreeNode nodeBox, ref TreeNode newNode, ref Int64 fieldValue, TreeNode payloadNode, Int64 payloadLengthInBit)
        {
            Result result = new Result();

            result = Utility.UpdateNode(nodeBox, "Serial Number Restriction Object ", ItemType.ITEM);

            if (result.Fine)
            {
                result = Utility.AddNodeDataPlus(Position.CHILD, payloadNode, out newNode, "Serial Number", ItemType.FIELD, dataStore, ref bitOffset, payloadLengthInBit, ref payloadLengthInBit);
            }

            return result;
        }

        private Result ParseRightsSettingsObject(DataStore dataStore, ref Int64 bitOffset, TreeNode nodeBox, ref TreeNode newNode, ref Int64 fieldValue, TreeNode payloadNode, Int64 payloadLengthInBit)
        {
            Result result = new Result();

            result = Utility.UpdateNode(nodeBox, "Rights Settings Object ", ItemType.ITEM);

            if (result.Fine)
            {
                result = Utility.AddNodeFieldPlus(Position.CHILD, payloadNode, out newNode, "Rights", ItemType.FIELD, dataStore, ref bitOffset, 16, ref fieldValue, ref payloadLengthInBit);
            }

            return result;
        }
        private Result ParseExpirationRestrictionObject(DataStore dataStore, ref Int64 bitOffset, TreeNode nodeBox, ref TreeNode newNode, ref Int64 fieldValue, TreeNode payloadNode, Int64 payloadLengthInBit)
        {
            Result result = new Result();

            result = Utility.UpdateNode(nodeBox, "Expiration Restriction Object ", ItemType.ITEM);

            if (result.Fine)
            {
                result = Utility.AddNodeFieldPlus(Position.CHILD, payloadNode, out newNode, "Begin Date", ItemType.FIELD, dataStore, ref bitOffset, 32, ref fieldValue, ref payloadLengthInBit);
            }

            if (result.Fine)
            {
                result = Utility.AddNodeFieldPlus(Position.CHILD, payloadNode, out newNode, "End Date", ItemType.FIELD, dataStore, ref bitOffset, 32, ref fieldValue, ref payloadLengthInBit);
            }


            return result;
        }

        private Result ParseIssueDateObject(DataStore dataStore, ref Int64 bitOffset, TreeNode nodeBox, ref TreeNode newNode, ref Int64 fieldValue, TreeNode payloadNode, Int64 payloadLengthInBit)
        {
            Result result = new Result();

            result = Utility.UpdateNode(nodeBox, "Issue Date Object ", ItemType.ITEM);

            if (result.Fine)
            {
                result = Utility.AddNodeFieldPlus(Position.CHILD, payloadNode, out newNode, "Issue Date", ItemType.FIELD, dataStore, ref bitOffset, 32, ref fieldValue, ref payloadLengthInBit);
            }

            return result;
        }

        private Result ParseGracePeriodObject(DataStore dataStore, ref Int64 bitOffset, TreeNode nodeBox, ref TreeNode newNode, ref Int64 fieldValue, TreeNode payloadNode, Int64 payloadLengthInBit)
        {
            Result result = new Result();

            result = Utility.UpdateNode(nodeBox, "Grace Period Object ", ItemType.ITEM);

            if (result.Fine)
            {
                result = Utility.AddNodeFieldPlus(Position.CHILD, payloadNode, out newNode, "Grace Period", ItemType.FIELD, dataStore, ref bitOffset, 32, ref fieldValue, ref payloadLengthInBit);
            }

            return result;
        }

        private Result ParseSourceIDObject(DataStore dataStore, ref Int64 bitOffset, TreeNode nodeBox, ref TreeNode newNode, ref Int64 fieldValue, TreeNode payloadNode, Int64 payloadLengthInBit)
        {
            Result result = new Result();

            result = Utility.UpdateNode(nodeBox, "Source ID Object ", ItemType.ITEM);

            if (result.Fine)
            {
                result = Utility.AddNodeFieldPlus(Position.CHILD, payloadNode, out newNode, "Source ID", ItemType.FIELD, dataStore, ref bitOffset, 32, ref fieldValue, ref payloadLengthInBit);
            }

            return result;
        }

        private Result ParsePriorityObject(DataStore dataStore, ref Int64 bitOffset, TreeNode nodeBox, ref TreeNode newNode, ref Int64 fieldValue, TreeNode payloadNode, Int64 payloadLengthInBit)
        {
            Result result = new Result();

            result = Utility.UpdateNode(nodeBox, "Priority Object ", ItemType.ITEM);

            if (result.Fine)
            {
                result = Utility.AddNodeFieldPlus(Position.CHILD, payloadNode, out newNode, "Priority", ItemType.FIELD, dataStore, ref bitOffset, 32, ref fieldValue, ref payloadLengthInBit);
            }

            return result;
        }
        private Result ParseExpirationAfterFirstPlayRestrictionObject(DataStore dataStore, ref Int64 bitOffset, TreeNode nodeBox, ref TreeNode newNode, ref Int64 fieldValue, TreeNode payloadNode, Int64 payloadLengthInBit)
        {
            Result result = new Result();

            result = Utility.UpdateNode(nodeBox, "Expiration After First Play Restriction Object", ItemType.ITEM);

            if (result.Fine)
            {
                result = Utility.AddNodeFieldPlus(Position.CHILD, payloadNode, out newNode, "Expire After First Play", ItemType.FIELD, dataStore, ref bitOffset, 32, ref fieldValue, ref payloadLengthInBit);
            }

            return result;
        }

        private Result ParseExpirationAfterFirstUseRestrictionObject(DataStore dataStore, ref Int64 bitOffset, TreeNode nodeBox, ref TreeNode newNode, ref Int64 fieldValue, TreeNode payloadNode, Int64 payloadLengthInBit)
        {
            Result result = new Result();

            result = Utility.UpdateNode(nodeBox, "Expiration After First Use Restriction Object ", ItemType.ITEM);

            if (result.Fine)
            {
                result = Utility.AddNodeFieldPlus(Position.CHILD, payloadNode, out newNode, "Expire After First Use", ItemType.FIELD, dataStore, ref bitOffset, 32, ref fieldValue, ref payloadLengthInBit);
            }

            return result;
        }

        private Result ParseExpirationAfterFirstStoreRestrictionObject(DataStore dataStore, ref Int64 bitOffset, TreeNode nodeBox, ref TreeNode newNode, ref Int64 fieldValue, TreeNode payloadNode, Int64 payloadLengthInBit)
        {
            Result result = new Result();

            result = Utility.UpdateNode(nodeBox, "Expiration After First Store Restriction Object ", ItemType.ITEM);

            if (result.Fine)
            {
                result = Utility.AddNodeFieldPlus(Position.CHILD, payloadNode, out newNode, "Expire After First Store", ItemType.FIELD, dataStore, ref bitOffset, 32, ref fieldValue, ref payloadLengthInBit);
            }

            return result;
        }

        private Result ParsePolicyMetadataObject(DataStore dataStore, ref Int64 bitOffset, TreeNode nodeBox, ref TreeNode newNode, ref Int64 fieldValue, TreeNode payloadNode, Int64 payloadLengthInBit)
        {
            Result result = new Result();

            result = Utility.UpdateNode(nodeBox, "Policy Metadata Object ", ItemType.ITEM);

            if (result.Fine)
            {
                result = Utility.AddNodeDataPlus(Position.CHILD, payloadNode, out newNode, "Metadata Type", ItemType.FIELD, dataStore, ref bitOffset, 128, ref payloadLengthInBit);
            } 
            
            if (result.Fine)
            {
                result = Utility.AddNodeDataPlus(Position.CHILD, payloadNode, out newNode, "Policy Data", ItemType.FIELD, dataStore, ref bitOffset, payloadLengthInBit, ref payloadLengthInBit);
            }

            return result;
        }

        private Result ParseOutputProtectionLevelRestrictionObject(DataStore dataStore, ref Int64 bitOffset, TreeNode nodeBox, ref TreeNode newNode, ref Int64 fieldValue, TreeNode payloadNode, Int64 payloadLengthInBit)
        {
            Result result = new Result();

            result = Utility.UpdateNode(nodeBox, "Output Protection Level Restriction Object ", ItemType.ITEM);

            if (result.Fine)
            {
                result = Utility.AddNodeFieldPlus(Position.CHILD, payloadNode, out newNode, "Minimum Compressed Digital Video Output Protection Level", ItemType.FIELD, dataStore, ref bitOffset, 16, ref fieldValue, ref payloadLengthInBit);
            }

            if (result.Fine)
            {
                result = Utility.AddNodeFieldPlus(Position.CHILD, payloadNode, out newNode, "Minimum Uncompressed Digital Video Output Protection Level", ItemType.FIELD, dataStore, ref bitOffset, 16, ref fieldValue, ref payloadLengthInBit);
            }

            if (result.Fine)
            {
                result = Utility.AddNodeFieldPlus(Position.CHILD, payloadNode, out newNode, "Minimum Analog Video Output Protection Level", ItemType.FIELD, dataStore, ref bitOffset, 16, ref fieldValue, ref payloadLengthInBit);
            }

            if (result.Fine)
            {
                result = Utility.AddNodeFieldPlus(Position.CHILD, payloadNode, out newNode, "Minimum Digital Compressed Audio Output Protection Level", ItemType.FIELD, dataStore, ref bitOffset, 16, ref fieldValue, ref payloadLengthInBit);
            }

            if (result.Fine)
            {
                result = Utility.AddNodeFieldPlus(Position.CHILD, payloadNode, out newNode, "Minimum Uncompressed Digital Audio Output Protection Level", ItemType.FIELD, dataStore, ref bitOffset, 16, ref fieldValue, ref payloadLengthInBit);
            }

            return result;
        }

        private Result ParseAnalogVideoOutputConfigurationRestrictionObject(DataStore dataStore, ref Int64 bitOffset, TreeNode nodeBox, ref TreeNode newNode, ref Int64 fieldValue, TreeNode payloadNode, Int64 payloadLengthInBit)
        {
            Result result = new Result();

            result = Utility.UpdateNode(nodeBox, "Analog Video Output Configuration Restriction Object ", ItemType.ITEM);

            if (result.Fine)
            {
                result = Utility.AddNodeDataPlus(Position.CHILD, payloadNode, out newNode, "Video Output Protection ID", ItemType.FIELD, dataStore, ref bitOffset, 128, ref payloadLengthInBit);
            }

            if (result.Fine)
            {
                result = Utility.AddNodeDataPlus(Position.CHILD, payloadNode, out newNode, "Binary configuration data", ItemType.FIELD, dataStore, ref bitOffset, payloadLengthInBit, ref payloadLengthInBit);
            }

            return result;
        }

        private Result ParseDigitalAudioOutputConfigurationRestrictionObject(DataStore dataStore, ref Int64 bitOffset, TreeNode nodeBox, ref TreeNode newNode, ref Int64 fieldValue, TreeNode payloadNode, Int64 payloadLengthInBit)
        {
            Result result = new Result();

            result = Utility.UpdateNode(nodeBox, "Digital Audio Output Configuration Restriction Object ", ItemType.ITEM);

            if (result.Fine)
            {
                result = Utility.AddNodeDataPlus(Position.CHILD, payloadNode, out newNode, "Audio Output Protection ID", ItemType.FIELD, dataStore, ref bitOffset, 128, ref payloadLengthInBit);
            }

            if (result.Fine)
            {
                result = Utility.AddNodeDataPlus(Position.CHILD, payloadNode, out newNode, "Binary Configuration Data", ItemType.FIELD, dataStore, ref bitOffset, payloadLengthInBit, ref payloadLengthInBit);
            }

            return result;
        }

        private Result ParseCopyCountRestrictionObject(DataStore dataStore, ref Int64 bitOffset, TreeNode nodeBox, ref TreeNode newNode, ref Int64 fieldValue, TreeNode payloadNode, Int64 payloadLengthInBit)
        {
            Result result = new Result();

            result = Utility.UpdateNode(nodeBox, "Copy Count Restriction Object ", ItemType.ITEM);

            if (result.Fine)
            {
                result = Utility.AddNodeFieldPlus(Position.CHILD, payloadNode, out newNode, "Copy Count", ItemType.FIELD, dataStore, ref bitOffset, 32, ref fieldValue, ref payloadLengthInBit);
            }

            return result;
        }

        private Result ParseCopyProtectionLevelRestrictionObject(DataStore dataStore, ref Int64 bitOffset, TreeNode nodeBox, ref TreeNode newNode, ref Int64 fieldValue, TreeNode payloadNode, Int64 payloadLengthInBit)
        {
            Result result = new Result();

            result = Utility.UpdateNode(nodeBox, "Copy Protection Level Restriction Object ", ItemType.ITEM);

            if (result.Fine)
            {
                result = Utility.AddNodeFieldPlus(Position.CHILD, payloadNode, out newNode, "Minimum Copy Protection Level", ItemType.FIELD, dataStore, ref bitOffset, 16, ref fieldValue, ref payloadLengthInBit);
            }

            return result;
        }

        private Result ParseMoveEnablerObject(DataStore dataStore, ref Int64 bitOffset, TreeNode nodeBox, ref TreeNode newNode, ref Int64 fieldValue, TreeNode payloadNode, Int64 payloadLengthInBit)
        {
            Result result = new Result();

            result = Utility.UpdateNode(nodeBox, "Move Enabler Object ", ItemType.ITEM);

            if (result.Fine)
            {
                result = Utility.AddNodeFieldPlus(Position.CHILD, payloadNode, out newNode, "Minimum Move Protection Level", ItemType.FIELD, dataStore, ref bitOffset, 32, ref fieldValue, ref payloadLengthInBit);
            }

            return result;
        }

        private Result ParseCopyCount2RestrictionObject(DataStore dataStore, ref Int64 bitOffset, TreeNode nodeBox, ref TreeNode newNode, ref Int64 fieldValue, TreeNode payloadNode, Int64 payloadLengthInBit)
        {
            Result result = new Result();

            result = Utility.UpdateNode(nodeBox, "Copy Count 2 Restriction Object ", ItemType.ITEM);

            if (result.Fine)
            {
                result = Utility.AddNodeFieldPlus(Position.CHILD, payloadNode, out newNode, "Copy Count", ItemType.FIELD, dataStore, ref bitOffset, 32, ref fieldValue, ref payloadLengthInBit);
            }

            return result;
        }

        private Result ParseCopyEnablerTypeObject(DataStore dataStore, ref Int64 bitOffset, TreeNode nodeBox, ref TreeNode newNode, ref Int64 fieldValue, TreeNode payloadNode, Int64 payloadLengthInBit)
        {
            Result result = new Result();

            result = Utility.UpdateNode(nodeBox, "Copy Enabler Type Object ", ItemType.ITEM);

            if (result.Fine)
            {
                result = Utility.AddNodeDataPlus(Position.CHILD, payloadNode, out newNode, "Copy Enabler Type", ItemType.FIELD, dataStore, ref bitOffset, 128, ref payloadLengthInBit);
            }

            return result;
        }

        private Result ParsePlaylistBurnRestrictionObject(DataStore dataStore, ref Int64 bitOffset, TreeNode nodeBox, ref TreeNode newNode, ref Int64 fieldValue, TreeNode payloadNode, Int64 payloadLengthInBit)
        {
            Result result = new Result();

            result = Utility.UpdateNode(nodeBox, "Playlist Burn Restriction Object ", ItemType.ITEM);

            if (result.Fine)
            {
                result = Utility.AddNodeFieldPlus(Position.CHILD, payloadNode, out newNode, "Maximum Playlist Burn Count", ItemType.FIELD, dataStore, ref bitOffset, 32, ref fieldValue, ref payloadLengthInBit);
            }
            if (result.Fine)
            {
                result = Utility.AddNodeFieldPlus(Position.CHILD, payloadNode, out newNode, "Playlist Burn Track Count", ItemType.FIELD, dataStore, ref bitOffset, 32, ref fieldValue, ref payloadLengthInBit);
            }

            return result;
        }

        private Result ParseLicenseGranterKeyObject(DataStore dataStore, ref Int64 bitOffset, TreeNode nodeBox, ref TreeNode newNode, ref Int64 fieldValue, TreeNode payloadNode, Int64 payloadLengthInBit)
        {
            Result result = new Result();

            result = Utility.UpdateNode(nodeBox, "License Granter Key Object ", ItemType.ITEM);

            if (result.Fine)
            {
                result = Utility.AddNodeFieldPlus(Position.CHILD, payloadNode, out newNode, "Exponent", ItemType.FIELD, dataStore, ref bitOffset, 32, ref fieldValue, ref payloadLengthInBit);
            }
            if (result.Fine)
            {
                result = Utility.AddNodeFieldPlus(Position.CHILD, payloadNode, out newNode, "Modulus Length", ItemType.FIELD, dataStore, ref bitOffset, 16, ref fieldValue, ref payloadLengthInBit);
            }

            if (result.Fine)
            {
                result = Utility.AddNodeDataPlus(Position.CHILD, payloadNode, out newNode, "Modulus", ItemType.FIELD, dataStore, ref bitOffset, payloadLengthInBit, ref payloadLengthInBit);
            }

            return result;
        }

        private Result ParseUserIDObject(DataStore dataStore, ref Int64 bitOffset, TreeNode nodeBox, ref TreeNode newNode, ref Int64 fieldValue, TreeNode payloadNode, Int64 payloadLengthInBit)
        {
            Result result = new Result();

            result = Utility.UpdateNode(nodeBox, "User ID Object ", ItemType.ITEM);

            if (result.Fine)
            {
                result = Utility.AddNodeDataPlus(Position.CHILD, payloadNode, out newNode, "User ID", ItemType.FIELD, dataStore, ref bitOffset, payloadLengthInBit, ref payloadLengthInBit);
            }

            return result;
        }


        private Result ParseContentKeyObject(DataStore dataStore, ref Int64 bitOffset, TreeNode nodeBox, ref TreeNode newNode, ref Int64 fieldValue, TreeNode payloadNode, Int64 payloadLengthInBit)
        {
            Result result = new Result();

            result = Utility.UpdateNode(nodeBox, "Content Key Object ", ItemType.ITEM);

            if (result.Fine)
            {
                result = Utility.AddNodeDataPlus(Position.CHILD, payloadNode, out newNode, "KeyID", ItemType.FIELD, dataStore, ref bitOffset, 128, ref payloadLengthInBit);
            }

            if (result.Fine)
            {
                result = Utility.AddNodeFieldPlus(Position.CHILD, payloadNode, out newNode, "Symmetric Cipher Type", ItemType.FIELD, dataStore, ref bitOffset, 16, ref fieldValue, ref payloadLengthInBit);
            }

            if (result.Fine)
            {
                result = Utility.AddNodeFieldPlus(Position.CHILD, payloadNode, out newNode, "Key Encryption Cipher Type", ItemType.FIELD, dataStore, ref bitOffset, 16, ref fieldValue, ref payloadLengthInBit);
            }

            if (result.Fine)
            {
                result = Utility.AddNodeFieldPlus(Position.CHILD, payloadNode, out newNode, "Encrypted Key Length", ItemType.FIELD, dataStore, ref bitOffset, 16, ref fieldValue, ref payloadLengthInBit);
            }

            if (result.Fine)
            {
                result = Utility.AddNodeDataPlus(Position.CHILD, payloadNode, out newNode, "Encrypted Key Data", ItemType.FIELD, dataStore, ref bitOffset, payloadLengthInBit, ref payloadLengthInBit);
            }

            return result;
        }

        private Result ParseRSADeviceKeyObject(DataStore dataStore, ref Int64 bitOffset, TreeNode nodeBox, ref TreeNode newNode, ref Int64 fieldValue, TreeNode payloadNode, Int64 payloadLengthInBit)
        {
            Result result = new Result();

            result = Utility.UpdateNode(nodeBox, "RSA Device Key Object ", ItemType.ITEM);


            if (result.Fine)
            {
                result = Utility.AddNodeFieldPlus(Position.CHILD, payloadNode, out newNode, "Exponent", ItemType.FIELD, dataStore, ref bitOffset, 32, ref fieldValue, ref payloadLengthInBit);
            }

            if (result.Fine)
            {
                result = Utility.AddNodeFieldPlus(Position.CHILD, payloadNode, out newNode, "Modulus Length", ItemType.FIELD, dataStore, ref bitOffset, 16, ref fieldValue, ref payloadLengthInBit);
            }

            if (result.Fine)
            {
                result = Utility.AddNodeDataPlus(Position.CHILD, payloadNode, out newNode, "Modulus", ItemType.FIELD, dataStore, ref bitOffset, payloadLengthInBit, ref payloadLengthInBit);
            }

            return result;
        }


        private Result ParseUplinkKIDObject (DataStore dataStore, ref Int64 bitOffset, TreeNode nodeBox, ref TreeNode newNode, ref Int64 fieldValue, TreeNode payloadNode, Int64 payloadLengthInBit)
        {
            Result result = new Result();

            result = Utility.UpdateNode(nodeBox, "Uplink KID Object ", ItemType.ITEM);


            if (result.Fine)
            {
                result = Utility.AddNodeDataPlus(Position.CHILD, payloadNode, out newNode, "Uplink KID", ItemType.FIELD, dataStore, ref bitOffset, 128, ref payloadLengthInBit);
            }

            if (result.Fine)
            {
                result = Utility.AddNodeFieldPlus(Position.CHILD, payloadNode, out newNode, "Chained Checksum Length", ItemType.FIELD, dataStore, ref bitOffset, 16, ref fieldValue, ref payloadLengthInBit);
            }

            if (result.Fine)
            {
                result = Utility.AddNodeDataPlus(Position.CHILD, payloadNode, out newNode, "Chained Checksum", ItemType.FIELD, dataStore, ref bitOffset, payloadLengthInBit, ref payloadLengthInBit);
            }

            return result;
        }

        private Result ParseXMRSignatureObject(DataStore dataStore, ref Int64 bitOffset, TreeNode nodeBox, ref TreeNode newNode, ref Int64 fieldValue, TreeNode payloadNode, Int64 payloadLengthInBit)
        {
            Result result = new Result();

            result = Utility.UpdateNode(nodeBox, "XMR Signature Object", ItemType.ITEM);

            if (result.Fine)
            {
                result = Utility.AddNodeFieldPlus(Position.CHILD, payloadNode, out newNode, "Signature Type", ItemType.FIELD, dataStore, ref bitOffset, 16, ref fieldValue, ref payloadLengthInBit);
            }

            if (result.Fine)
            {
                result = Utility.AddNodeFieldPlus(Position.CHILD, payloadNode, out newNode, "Signature Data Length", ItemType.FIELD, dataStore, ref bitOffset, 16, ref fieldValue, ref payloadLengthInBit);
            }

            if (result.Fine)
            {
                result = Utility.AddNodeDataPlus(Position.CHILD, payloadNode, out newNode, "Signature Data", ItemType.FIELD, dataStore, ref bitOffset, payloadLengthInBit, ref payloadLengthInBit);
            }

            return result;
        }

        private Result ParsePlayReadyRevocationInformationVersionObject(DataStore dataStore, ref Int64 bitOffset, TreeNode nodeBox, ref TreeNode newNode, ref Int64 fieldValue, TreeNode payloadNode, Int64 payloadLengthInBit)
        {
            Result result = new Result();

            result = Utility.UpdateNode(nodeBox, "PlayReady Revocation Information Version Object ", ItemType.ITEM);


            if (result.Fine)
            {
                result = Utility.AddNodeFieldPlus(Position.CHILD, payloadNode, out newNode, "Sequence", ItemType.FIELD, dataStore, ref bitOffset, 32, ref fieldValue, ref payloadLengthInBit);
            }


            return result;
        }

        private Result ParseEmbeddedLicenseSettingsObject(DataStore dataStore, ref Int64 bitOffset, TreeNode nodeBox, ref TreeNode newNode, ref Int64 fieldValue, TreeNode payloadNode, Int64 payloadLengthInBit)
        {
            Result result = new Result();

            result = Utility.UpdateNode(nodeBox, "Embedded License Settings Object ", ItemType.ITEM);


            if (result.Fine)
            {
                result = Utility.AddNodeFieldPlus(Position.CHILD, payloadNode, out newNode, "License Processing Indicator", ItemType.FIELD, dataStore, ref bitOffset, 16, ref fieldValue, ref payloadLengthInBit);
            }


            return result;
        }


        private Result ParseSecurityLevelObject(DataStore dataStore, ref Int64 bitOffset, TreeNode nodeBox, ref TreeNode newNode, ref Int64 fieldValue, TreeNode payloadNode, Int64 payloadLengthInBit)
        {
            Result result = new Result();

            result = Utility.UpdateNode(nodeBox, "Security Level Object ", ItemType.ITEM);


            if (result.Fine)
            {
                result = Utility.AddNodeFieldPlus(Position.CHILD, payloadNode, out newNode, "Minimum Security Level", ItemType.FIELD, dataStore, ref bitOffset, 16, ref fieldValue, ref payloadLengthInBit);
            }


            return result;
        }

        private Result ParsePlayEnablerTypeObject(DataStore dataStore, ref Int64 bitOffset, TreeNode nodeBox, ref TreeNode newNode, ref Int64 fieldValue, TreeNode payloadNode, Int64 payloadLengthInBit)
        {
            Result result = new Result();

            result = Utility.UpdateNode(nodeBox, "Play Enabler Type Object ", ItemType.ITEM);

            if (result.Fine)
            {
                result = Utility.AddNodeDataPlus(Position.CHILD, payloadNode, out newNode, "Modulus", ItemType.FIELD, dataStore, ref bitOffset, 128, ref payloadLengthInBit);
            }

            return result;
        }

        private Result ParseRemovalDateObject(DataStore dataStore, ref Int64 bitOffset, TreeNode nodeBox, ref TreeNode newNode, ref Int64 fieldValue, TreeNode payloadNode, Int64 payloadLengthInBit)
        {
            Result result = new Result();

            result = Utility.UpdateNode(nodeBox, "Removal Date Object ", ItemType.ITEM);

            if (result.Fine)
            {
                result = Utility.AddNodeFieldPlus(Position.CHILD, payloadNode, out newNode, "Removal Date", ItemType.FIELD, dataStore, ref bitOffset, 32, ref fieldValue, ref payloadLengthInBit);
                DateTime dateTime1970 = Convert.ToDateTime("1970-01-01");
                DateTime removalDateTime = dateTime1970.AddSeconds(fieldValue);
                Utility.UpdateNode(newNode, "RemovalDate:" + removalDateTime.ToString(), ItemType.FIELD);
            }

            return result;
        }

        private Result ParseAuxiliaryKeyObject(DataStore dataStore, ref Int64 bitOffset, TreeNode nodeBox, ref TreeNode newNode, ref Int64 fieldValue, TreeNode payloadNode, Int64 payloadLengthInBit)
        {
            Result result = new Result();

            result = Utility.UpdateNode(nodeBox, "Auxiliary Key Object ", ItemType.ITEM);

            Int64 count = 0;
            if (result.Fine)
            {
                result = Utility.AddNodeFieldPlus(Position.CHILD, payloadNode, out newNode, "Count", ItemType.FIELD, dataStore, ref bitOffset, 16, ref count, ref payloadLengthInBit);
            }

            for (Int64 i = 0; i < count; ++i)
            {
                if (result.Fine)
                {
                    result = Utility.AddNodeFieldPlus(Position.CHILD, payloadNode, out newNode, "Location", ItemType.FIELD, dataStore, ref bitOffset, 32, ref fieldValue, ref payloadLengthInBit);
                }

                if (result.Fine)
                {
                    result = Utility.AddNodeDataPlus(Position.CHILD, payloadNode, out newNode, "Key", ItemType.FIELD, dataStore, ref bitOffset, 128, ref payloadLengthInBit);
                }

                if (!result.Fine)
                {
                    break;
                }

            }

            return result;
        }
        

        private Result ParseUplinkKIDObject3(DataStore dataStore, ref Int64 bitOffset, TreeNode nodeBox, ref TreeNode newNode, ref Int64 fieldValue, TreeNode payloadNode, Int64 payloadLengthInBit)
        {
            Result result = new Result();

            result = Utility.UpdateNode(nodeBox, "Uplink KID Object 3", ItemType.ITEM);

            if (result.Fine)
            {
                result = Utility.AddNodeDataPlus(Position.CHILD, payloadNode, out newNode, "Uplink KID", ItemType.FIELD, dataStore, ref bitOffset, 128, ref payloadLengthInBit);
            }

            Int64 checksumLength = 0;
            if (result.Fine)
            {
                result = Utility.AddNodeFieldPlus(Position.CHILD, payloadNode, out newNode, "Checksum Length", ItemType.FIELD, dataStore, ref bitOffset, 16, ref checksumLength, ref payloadLengthInBit);
            }

            if (result.Fine)
            {
                result = Utility.AddNodeDataPlus(Position.CHILD, payloadNode, out newNode, "Checksum", ItemType.FIELD, dataStore, ref bitOffset, checksumLength * 8, ref payloadLengthInBit);
            }

            Int64 count = 0;
            if (result.Fine)
            {
                result = Utility.AddNodeFieldPlus(Position.CHILD, payloadNode, out newNode, "Count", ItemType.FIELD, dataStore, ref bitOffset, 16, ref count, ref payloadLengthInBit);
            }

            for (Int64 i = 0; i < count; ++i)
            {
                if (result.Fine)
                {
                    result = Utility.AddNodeFieldPlus(Position.CHILD, payloadNode, out newNode, "Location", ItemType.FIELD, dataStore, ref bitOffset, 32, ref fieldValue, ref payloadLengthInBit);
                }
            }


            return result;
        }

        private Result ParseECCKeyObject(DataStore dataStore, ref Int64 bitOffset, TreeNode nodeBox, ref TreeNode newNode, ref Int64 fieldValue, TreeNode payloadNode, Int64 payloadLengthInBit)
        {
            Result result = new Result();

            result = Utility.UpdateNode(nodeBox, "ECC Key Object", ItemType.ITEM);

            if (result.Fine)
            {
                result = Utility.AddNodeFieldPlus(Position.CHILD, payloadNode, out newNode, "Curve type", ItemType.FIELD, dataStore, ref bitOffset, 16, ref fieldValue, ref payloadLengthInBit);
            }

            Int64 keyLength = 0;
            if (result.Fine)
            {
                result = Utility.AddNodeFieldPlus(Position.CHILD, payloadNode, out newNode, "Key Length", ItemType.FIELD, dataStore, ref bitOffset, 16, ref keyLength, ref payloadLengthInBit);
            }

            if (result.Fine)
            {
                result = Utility.AddNodeDataPlus(Position.CHILD, payloadNode, out newNode, "Key", ItemType.FIELD, dataStore, ref bitOffset, keyLength*8, ref payloadLengthInBit);
            }

            return result;
        }

        public void ParseObjects(TreeView treeView, TreeNode parent, DataStore dataStore, ref Int64 bitOffset)
        {
            TreeNode nodeBox = null;
            TreeNode newNode = null;

            Result result = new Result();
            Int64 fieldValue = 0;


            while (0 != dataStore.GetLeftBitLength() && result.Fine)
            {

                //Add one node to indicate this object.
                result = Utility.AddNodeContainer(Position.CHILD, parent, out nodeBox, "Object", ItemType.ITEM, dataStore, bitOffset, 0);

                Int64 objectFlags = 0;
                if (result.Fine)
                {
                    result = Utility.AddNodeField(Position.CHILD, nodeBox, out newNode, "Flags", ItemType.FIELD, dataStore, ref bitOffset, 16, ref objectFlags);
                }

                Int64 objectType = 0;
                if (result.Fine)
                {
                    result = Utility.AddNodeField(Position.CHILD, nodeBox, out newNode, "Object Type", ItemType.FIELD, dataStore, ref bitOffset, 16, ref objectType);
                }

                Int64 objectLength = 0;
                if (result.Fine)
                {
                    result = Utility.AddNodeField(Position.CHILD, nodeBox, out newNode, "Object Length", ItemType.FIELD, dataStore, ref bitOffset, 32, ref objectLength);
                }


                if (result.Fine)
                {
                    Utility.UpdateNodeLength(nodeBox, "Object", ItemType.ITEM, (objectLength) * 8);
                }

                TreeNode payloadNode = null;
                Int64 payloadLengthInBit = 0;
                if (result.Fine)
                {
                    payloadLengthInBit = (objectLength - 8) * 8;
                    result = Utility.AddNodeContainer(Position.CHILD, nodeBox, out payloadNode, "Object Payload", ItemType.FIELD, dataStore, bitOffset, payloadLengthInBit);
                }

                if (result.Fine)
                {
                    //Not a container. Let's parse the object according to the type.
                    switch (objectType)
                    {
                        case 0x0001:
                            result = Utility.UpdateNode(nodeBox, "Outer XMR Container Object ", ItemType.ITEM);
                            break;
                        case 0x0002:
                            result = Utility.UpdateNode(nodeBox, "Global Policy Container Object ", ItemType.ITEM);
                            break;
                        case 0x0003:
                            result = ParseMinimumEnvironmentObject(dataStore, ref bitOffset, nodeBox, ref newNode, ref fieldValue, payloadNode, payloadLengthInBit);
                            break;
                        case 0x0004:
                            result = Utility.UpdateNode(nodeBox, "Play Policy Container Object", ItemType.ITEM);
                            break;
                        case 0x0005:
                            result = ParseOutputProtectionLevelRestrictionObject(dataStore, ref bitOffset, nodeBox, ref newNode, ref fieldValue, payloadNode, payloadLengthInBit);
                            break;
                        case 0x0006:
                            result = ParseUplinkKIDObject(dataStore, ref bitOffset, nodeBox, ref newNode, ref fieldValue, payloadNode, payloadLengthInBit);
                            break;
                        case 0x0007:
                            result = Utility.UpdateNode(nodeBox, "Explicit Analog Video Output Protection Container Object ", ItemType.ITEM);
                            break;
                        case 0x0008:
                            result = ParseAnalogVideoOutputConfigurationRestrictionObject(dataStore, ref bitOffset, nodeBox, ref newNode, ref fieldValue, payloadNode, payloadLengthInBit);
                            break;
                        case 0x0009:
                            result = Utility.UpdateNode(nodeBox, "Key Material Container Object ", ItemType.ITEM);
                            break;
                        case 0x000A:
                            result = ParseContentKeyObject(dataStore, ref bitOffset, nodeBox, ref newNode, ref fieldValue, payloadNode, payloadLengthInBit);
                            break;
                        case 0x000B:
                            result = ParseXMRSignatureObject(dataStore, ref bitOffset, nodeBox, ref newNode, ref fieldValue, payloadNode, payloadLengthInBit);
                            break;
                        case 0x000C:
                            result = ParseSerialNumberRestrictionObject(dataStore, ref bitOffset, nodeBox, ref newNode, ref fieldValue, payloadNode, payloadLengthInBit);
                            break;
                        case 0x000D:
                            result = ParseRightsSettingsObject(dataStore, ref bitOffset, nodeBox, ref newNode, ref fieldValue, payloadNode, payloadLengthInBit);
                            break;
                        case 0x000E:
                            result = Utility.UpdateNode(nodeBox, "Copy Policy Container Object ", ItemType.ITEM);
                            break;
                        case 0x000F:
                            result = Utility.UpdateNode(nodeBox, "Allow Playlist Burn Right Container Object ", ItemType.ITEM);
                            break;
                        case 0x0010:
                            result = ParseInclusionListObject(dataStore, ref bitOffset, nodeBox, ref newNode, ref fieldValue, payloadNode, payloadLengthInBit);
                            break;
                        case 0x0011:
                            result = ParsePriorityObject(dataStore, ref bitOffset, nodeBox, ref newNode, ref fieldValue, payloadNode, payloadLengthInBit);
                            break;
                        case 0x0012:
                            result = ParseExpirationRestrictionObject(dataStore, ref bitOffset, nodeBox, ref newNode, ref fieldValue, payloadNode, payloadLengthInBit);
                            break;
                        case 0x0013:
                            result = ParseIssueDateObject(dataStore, ref bitOffset, nodeBox, ref newNode, ref fieldValue, payloadNode, payloadLengthInBit);
                            break;
                        case 0x0014:
                            result = ParseExpirationAfterFirstUseRestrictionObject(dataStore, ref bitOffset, nodeBox, ref newNode, ref fieldValue, payloadNode, payloadLengthInBit);
                            break;
                        case 0x0015:
                            result = ParseExpirationAfterFirstStoreRestrictionObject(dataStore, ref bitOffset, nodeBox, ref newNode, ref fieldValue, payloadNode, payloadLengthInBit);
                            break;
                        case 0x001A:
                            result = ParseGracePeriodObject(dataStore, ref bitOffset, nodeBox, ref newNode, ref fieldValue, payloadNode, payloadLengthInBit);
                            break;
                        case 0x001B:
                            result = ParseCopyCountRestrictionObject(dataStore, ref bitOffset, nodeBox, ref newNode, ref fieldValue, payloadNode, payloadLengthInBit);
                            break;
                        case 0x001C:
                            result = ParseCopyProtectionLevelRestrictionObject(dataStore, ref bitOffset, nodeBox, ref newNode, ref fieldValue, payloadNode, payloadLengthInBit);
                            break;
                        case 0x001F:
                            result = ParsePlaylistBurnRestrictionObject(dataStore, ref bitOffset, nodeBox, ref newNode, ref fieldValue, payloadNode, payloadLengthInBit);
                            break;
                        case 0x0020:
                            result = ParseRevocationInformationVersionObject(dataStore, ref bitOffset, nodeBox, ref newNode, ref fieldValue, payloadNode, payloadLengthInBit);
                            break;
                        case 0x0021:
                            result = ParseRSADeviceKeyObject(dataStore, ref bitOffset, nodeBox, ref newNode, ref fieldValue, payloadNode, payloadLengthInBit);
                            break;
                        case 0x0022:
                            result = ParseSourceIDObject(dataStore, ref bitOffset, nodeBox, ref newNode, ref fieldValue, payloadNode, payloadLengthInBit);
                            break;
                        case 0x0025:
                            result = Utility.UpdateNode(nodeBox, "Revocation Container Object ", ItemType.ITEM);
                            break;
                        case 0x0026:
                            result = ParseLicenseGranterKeyObject(dataStore, ref bitOffset, nodeBox, ref newNode, ref fieldValue, payloadNode, payloadLengthInBit);
                            break;
                        case 0x0027:
                            result = ParseUserIDObject(dataStore, ref bitOffset, nodeBox, ref newNode, ref fieldValue, payloadNode, payloadLengthInBit);
                            break;
                        case 0x0028:
                            result = Utility.UpdateNode(nodeBox, "Restricted Source ID Object ", ItemType.ITEM);
                            break;
                        case 0x002A:
                            result = ParseECCKeyObject(dataStore, ref bitOffset, nodeBox, ref newNode, ref fieldValue, payloadNode, payloadLengthInBit);
                            break;
                        case 0x002C:
                            result = ParsePolicyMetadataObject(dataStore, ref bitOffset, nodeBox, ref newNode, ref fieldValue, payloadNode, payloadLengthInBit);
                            break;
                        case 0x002E:
                            result = Utility.UpdateNode(nodeBox, "Explicit Digital Audio Output Protection Container Object ", ItemType.ITEM);
                            break;
                        case 0x0030:
                            result = ParseExpirationAfterFirstPlayRestrictionObject(dataStore, ref bitOffset, nodeBox, ref newNode, ref fieldValue, payloadNode, payloadLengthInBit);
                            break;
                        case 0x0031:
                            result = ParseDigitalAudioOutputConfigurationRestrictionObject(dataStore, ref bitOffset, nodeBox, ref newNode, ref fieldValue, payloadNode, payloadLengthInBit);
                            break;
                        case 0x0032:
                            result = ParsePlayReadyRevocationInformationVersionObject(dataStore, ref bitOffset, nodeBox, ref newNode, ref fieldValue, payloadNode, payloadLengthInBit);
                            break;
                        case 0x0033:
                            result = ParseEmbeddedLicenseSettingsObject(dataStore, ref bitOffset, nodeBox, ref newNode, ref fieldValue, payloadNode, payloadLengthInBit);
                            break;
                        case 0x0034:
                            result = ParseSecurityLevelObject(dataStore, ref bitOffset, nodeBox, ref newNode, ref fieldValue, payloadNode, payloadLengthInBit);
                            break;
                        case 0x0036:
                            result = Utility.UpdateNode(nodeBox, "Play Enabler Container Object ", ItemType.ITEM);
                            break;
                        case 0x0037:
                            result = ParseMoveEnablerObject(dataStore, ref bitOffset, nodeBox, ref newNode, ref fieldValue, payloadNode, payloadLengthInBit);
                            break;
                        case 0x0038:
                            result = Utility.UpdateNode(nodeBox, "Copy Enabler Container Object ", ItemType.ITEM);
                            break;
                        case 0x0039:
                            result = ParsePlayEnablerTypeObject(dataStore, ref bitOffset, nodeBox, ref newNode, ref fieldValue, payloadNode, payloadLengthInBit);
                            break;
                        case 0x003A:
                            result = ParseCopyEnablerTypeObject(dataStore, ref bitOffset, nodeBox, ref newNode, ref fieldValue, payloadNode, payloadLengthInBit);
                            break;
                        case 0x003C: 
                            result = Utility.UpdateNode(nodeBox, "Copy Policy 2 Container Object ", ItemType.ITEM);
                            break;
                        case 0x003D:
                            result = ParseCopyCount2RestrictionObject(dataStore, ref bitOffset, nodeBox, ref newNode, ref fieldValue, payloadNode, payloadLengthInBit);
                            break;
                        case 0x0050:
                            result = ParseRemovalDateObject(dataStore, ref bitOffset, nodeBox, ref newNode, ref fieldValue, payloadNode, payloadLengthInBit);
                            break;
                        case 0x0051:
                            result = ParseAuxiliaryKeyObject(dataStore, ref bitOffset, nodeBox, ref newNode, ref fieldValue, payloadNode, payloadLengthInBit);
                            break;
                        case 0x0052:
                            result = ParseUplinkKIDObject3(dataStore, ref bitOffset, nodeBox, ref newNode, ref fieldValue, payloadNode, payloadLengthInBit);
                            break;
                        default:
                            if ((result.Fine) && (((objectFlags >> 1) & 0x1) == 0x0))
                            {
                                result = Utility.AddNodeData(Position.CHILD, payloadNode, out newNode, "Unknown Payload", ItemType.FIELD, dataStore, ref bitOffset, payloadLengthInBit);
                            }

                            Console.WriteLine("unknown object " + String.Format("0x{0,4:X2}", objectType));
                            break;
                    }
                }

                if (result.Fine && (dataStore.GetLeftBitLength() > 0))
                {
                    //Check if it is a container.
                    if (((objectFlags >> 1) & 0x1) == 0x1)
                    {
                        //The container bit is set. The payload contains the nested objects.
                        Int64 innerObjectBitOffset = bitOffset;
                        if (result.Fine)
                        {
                            //Create a new data store containing the payload for this object.
                            DataStore innerBoxDataStore = new DataStore(dataStore.GetData(), innerObjectBitOffset, (objectLength - 8));
                            innerBoxDataStore.SetMetadataList(dataStore.GetDataSourceMetadataList());

                            //Parse box payload which may also contains objects.
                            ParseObjects(treeView, payloadNode, innerBoxDataStore, ref innerObjectBitOffset);

                            //Skip the payload.
                            dataStore.SkipBits(ref bitOffset, (objectLength - 8) * 8);

                        }
                    }
                }

            }
        }


    }
}
