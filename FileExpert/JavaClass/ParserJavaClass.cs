using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Library;
using FileExpert.JavaClass;
using System.IO;

namespace FileExpert.JavaClass
{
    class ParserJavaClass
    {

        public void ParseJavaClass(TreeView treeView, TreeNode parent, DataStore dataStore)
        {
            TreeNode nodeObject = null;
            Int64 bitOffset = 0;
            Int64 maxLeftBits = dataStore.GetLeftBitLength();

            //Add one node to indicate this object.
            Result result = Utility.AddNodeContainer(Position.CHILD, parent, out nodeObject, "JavaClass", ItemType.SECTION2, dataStore, 0, maxLeftBits);

            
            ParseClassFile(treeView, nodeObject, dataStore, ref bitOffset, ref maxLeftBits);
        }

        public Result ParseClassFile(TreeView treeView, TreeNode parent, DataStore dataStore, ref Int64 bitOffset, ref Int64 maxLeftBits)
        {
            Result result = new Result();
            TreeNode newNode = null;
            Int64 fieldValue = 0;


            if (result.Fine)
            {
                //u4 magic;
                result = Utility.AddNodeField(Position.CHILD, parent, out newNode, "magic", ItemType.FIELD, dataStore, ref bitOffset, 32, ref fieldValue);
            }

            if (result.Fine)
            {
                //u2 minor_version;
                result = Utility.AddNodeField(Position.CHILD, parent, out newNode, "minor_version", ItemType.FIELD, dataStore, ref bitOffset, 16, ref fieldValue);
            }

            if (result.Fine)
            {
                //u2 major_version;
                result = Utility.AddNodeField(Position.CHILD, parent, out newNode, "major_version", ItemType.FIELD, dataStore, ref bitOffset, 16, ref fieldValue);
            }

            Int64 constantPoolCount = 0;
            if (result.Fine)
            {
                //u2 constant_pool_count;
                result = Utility.AddNodeField(Position.CHILD, parent, out newNode, "constant_pool_count", ItemType.FIELD, dataStore, ref bitOffset, 16, ref constantPoolCount);
            }


            TreeNode nodeConstantPool = null;
            Int64 bitOffset1 = bitOffset;
            if (result.Fine)
            {
                //cp_info constant_pool[constant_pool_count-1];
                result = Utility.AddNodeContainer(Position.CHILD, parent, out nodeConstantPool, "constant_pool", ItemType.LOOP, dataStore, bitOffset, 0);
            }

            TreeNode nodeConstantPoolItem = null;
            if (result.Fine)
            {
                //The value of the constant_pool_count item is equal to the number of entries in the constant_pool table plus one.
                for (Int64 i = 1; i < constantPoolCount; ++i)
                {
                    result = Utility.AddNodeContainer(Position.CHILD, nodeConstantPool, out nodeConstantPoolItem, "constant ", ItemType.LOOP, dataStore, bitOffset, 0);

                    if (result.Fine)
                    {
                        result = ParseConstant(treeView, nodeConstantPoolItem, dataStore, ref bitOffset, ref maxLeftBits, ref i);
                    }
                    if (!result.Fine)
                    {
                        break;
                    }

                }
            }

            if (result.Fine)
            {
                Int64 bitOffset2 = bitOffset;
                Utility.UpdateNodeLength(nodeConstantPool, "constant_pool", ItemType.ITEM, (bitOffset2 - bitOffset1));
            }

            Int64 accessFlags = 0;
            if (result.Fine)
            {
                //u2 access_flags;
                result = Utility.AddNodeField(Position.CHILD, parent, out newNode, "access_flags", ItemType.FIELD, dataStore, ref bitOffset, 16, ref accessFlags);
            }

            if (result.Fine)
            {
                //Update description for access_flags.
                String accessFlagsDescription = GetAccessFlagsDescription(accessFlags);

                Utility.UpdateNode(newNode, "access_flags:" + accessFlagsDescription, ItemType.FIELD);

            }

            if (result.Fine)
            {
                //u2 this_class;
                result = Utility.AddNodeField(Position.CHILD, parent, out newNode, "this_class", ItemType.FIELD, dataStore, ref bitOffset, 16, ref fieldValue);
            }

            if (result.Fine)
            {
                //u2 super_class;
                result = Utility.AddNodeField(Position.CHILD, parent, out newNode, "super_class", ItemType.FIELD, dataStore, ref bitOffset, 16, ref fieldValue);
            }

            Int64 interfaceCount = 0;
            if (result.Fine)
            {
                //u2 interfaces_count;
                result = Utility.AddNodeField(Position.CHILD, parent, out newNode, "interfaces_count", ItemType.FIELD, dataStore, ref bitOffset, 16, ref interfaceCount);
            }

            if (result.Fine)
            {
                //interfaces[interfaces_count];
                for (Int64 i = 0; i < interfaceCount; ++i)
                {
                    //u2 interface_index;
                    result = Utility.AddNodeField(Position.CHILD, parent, out newNode, "interface_index: " + i, ItemType.FIELD, dataStore, ref bitOffset, 16, ref fieldValue);

                    if (!result.Fine)
                    {
                        break;
                    }
                }
            }

            Int64 fieldsCount = 0;
            if (result.Fine)
            {
                //u2 fields_count;
                result = Utility.AddNodeField(Position.CHILD, parent, out newNode, "fields_count", ItemType.FIELD, dataStore, ref bitOffset, 16, ref fieldsCount);
            }


            TreeNode nodeFields = null;
            bitOffset1 = bitOffset;
            if (result.Fine)
            {
                //field_info fields[fields_count];
                result = Utility.AddNodeContainer(Position.CHILD, parent, out nodeFields, "fields", ItemType.LOOP, dataStore, bitOffset, 0);
            }

            TreeNode nodeFieldItem = null;
            if (result.Fine)
            {
                for (Int64 i = 0; i < fieldsCount; ++i)
                {
                    result = Utility.AddNodeContainer(Position.CHILD, nodeFields, out nodeFieldItem, "field_info", ItemType.LOOP, dataStore, bitOffset, 0);

                    if (result.Fine)
                    {
                        result = ParseField(treeView, nodeFieldItem, dataStore, ref bitOffset, ref maxLeftBits, i);
                    }
                    if (!result.Fine)
                    {
                        break;
                    }

                }
            }

            if (result.Fine)
            {
                Int64 bitOffset2 = bitOffset;
                Utility.UpdateNodeLength(nodeFields, "fields", ItemType.ITEM, (bitOffset2 - bitOffset1));
            }

            Int64 methodsCount = 0;
            if (result.Fine)
            {
                //u2 methods_count;
                result = Utility.AddNodeField(Position.CHILD, parent, out newNode, "methods_count", ItemType.FIELD, dataStore, ref bitOffset, 16, ref methodsCount);
            }


            TreeNode nodeMethods = null;
            bitOffset1 = bitOffset;
            if (result.Fine)
            {
                //method_info methods[methods_count];
                result = Utility.AddNodeContainer(Position.CHILD, parent, out nodeMethods, "methods", ItemType.LOOP, dataStore, bitOffset, 0);
            }

            TreeNode nodeMethodItem = null;
            if (result.Fine)
            {
                for (Int64 i = 0; i < methodsCount; ++i)
                {
                    result = Utility.AddNodeContainer(Position.CHILD, nodeMethods, out nodeMethodItem, "method_info", ItemType.LOOP, dataStore, bitOffset, 0);

                    if (result.Fine)
                    {
                        result = ParseMethod(treeView, nodeMethodItem, dataStore, ref bitOffset, ref maxLeftBits, i);
                    }
                    if (!result.Fine)
                    {
                        break;
                    }

                }
            }

            if (result.Fine)
            {
                Int64 bitOffset2 = bitOffset;
                Utility.UpdateNodeLength(nodeMethods, "methods", ItemType.ITEM, (bitOffset2 - bitOffset1));
            }


            Int64 attributesCount = 0;
            if (result.Fine)
            {
                //u2 attributes_count;
                result = Utility.AddNodeField(Position.CHILD, parent, out newNode, "attributes_count", ItemType.FIELD, dataStore, ref bitOffset, 16, ref attributesCount);
            }

            //attribute_info attributes[attributes_count];
            TreeNode nodeAttribute = null;
            if (result.Fine)
            {
                for (Int64 i = 0; i < attributesCount; ++i)
                {
                    Int64 bitOffsetA = bitOffset;
                    result = Utility.AddNodeContainer(Position.CHILD, parent, out nodeAttribute, "attribute_info", ItemType.LOOP, dataStore, bitOffset, 0);

                    if (result.Fine)
                    {
                        result = ParseAttribute(treeView, nodeAttribute, dataStore, ref bitOffset, ref maxLeftBits, i);
                    }
                    if (!result.Fine)
                    {
                        break;
                    }

                    //Update the length.
                    if (result.Fine)
                    {
                        Int64 bitOffsetB = bitOffset;
                        Utility.UpdateNodeLength(nodeAttribute, "attribute_info " + i, ItemType.ITEM, (bitOffsetB - bitOffsetA));
                    }

                }
            }


            if (dataStore.GetLeftBitLength() != 0)
            {
                Utility.ExpandToRoot(parent, ItemType.ERROR);
            }
            return result;
        }

        public Result ParseConstant(TreeView treeView, TreeNode parent, DataStore dataStore, ref Int64 bitOffset, ref Int64 maxLeftBits, ref Int64 index)
        {

            Result result = new Result();
            TreeNode newNode = null;

            Int64 bitOffset1 = bitOffset;

            Int64 tag = 0;
            if (result.Fine)
            {
                //u1 tag;
                result = Utility.AddNodeField(Position.CHILD, parent, out newNode, "tag", ItemType.FIELD, dataStore, ref bitOffset, 8, ref tag);
            }

            string constantName = null;
            if (result.Fine)
            {
                switch(tag)
                {
                    case 0:
                        constantName = "stuffing";
                        result = ParseConstant_stuffing(treeView, parent, dataStore, ref bitOffset, ref maxLeftBits);
                        break;
                    case 1:
                        constantName = "CONSTANT_Utf8_info";                        
                        result = ParseConstant_CONSTANT_Utf8_info(treeView, parent, dataStore, ref bitOffset, ref maxLeftBits);
                        break;
                    case 3:
                        constantName = "CONSTANT_Integer_info";
                        result = ParseConstant_CONSTANT_Integer_info(treeView, parent, dataStore, ref bitOffset, ref maxLeftBits);
                        break;
                    case 4:
                        constantName = "CONSTANT_Float_info";
                        result = ParseConstant_CONSTANT_Float_info(treeView, parent, dataStore, ref bitOffset, ref maxLeftBits);
                        break;
                    case 5:
                        constantName = "CONSTANT_Long_info";
                        result = ParseConstant_CONSTANT_Long_info(treeView, parent, dataStore, ref bitOffset, ref maxLeftBits);
                        break;
                    case 6:
                        constantName = "CONSTANT_Double_info";
                        result = ParseConstant_CONSTANT_Double_info(treeView, parent, dataStore, ref bitOffset, ref maxLeftBits);
                        break;
                    case 7:
                        constantName = "CONSTANT_Class_info";
                        result = ParseConstant_CONSTANT_Class_info(treeView, parent, dataStore, ref bitOffset, ref maxLeftBits);
                        break;
                    case 8:
                        constantName = "CONSTANT_String_info";
                        result = ParseConstant_CONSTANT_String_info(treeView, parent, dataStore, ref bitOffset, ref maxLeftBits);
                        break;
                    case 9:
                        constantName = "CONSTANT_Fieldref_info";
                        result = ParseConstant_CONSTANT_Fieldref_info(treeView, parent, dataStore, ref bitOffset, ref maxLeftBits);
                        break;
                    case 10:
                        constantName = "CONSTANT_Methodref_info";
                        result = ParseConstant_CONSTANT_Methodref_info(treeView, parent, dataStore, ref bitOffset, ref maxLeftBits);
                        break;
                    case 11:
                        constantName = "CONSTANT_InterfaceMethodref_info";
                        result = ParseConstant_CONSTANT_InterfaceMethodref_info(treeView, parent, dataStore, ref bitOffset, ref maxLeftBits);
                        break;
                    case 12:
                        constantName = "CONSTANT_NameAndType_info";
                        result = ParseConstant_CONSTANT_NameAndType_info(treeView, parent, dataStore, ref bitOffset, ref maxLeftBits);
                        break;
                    case 15:
                        constantName = "CONSTANT_MethodHandle_info";
                        result = ParseConstant_CONSTANT_MethodHandle_info(treeView, parent, dataStore, ref bitOffset, ref maxLeftBits);
                        break;
                    case 16:
                        constantName = "CONSTANT_MethodType_info";
                        result = ParseConstant_CONSTANT_MethodType_info(treeView, parent, dataStore, ref bitOffset, ref maxLeftBits);
                        break;
                    case 17:
                        constantName = "CONSTANT_Dynamic_info";
                        result = ParseConstant_CONSTANT_Dynamic_info(treeView, parent, dataStore, ref bitOffset, ref maxLeftBits);
                        break;
                    case 18:
                        constantName = "CONSTANT_InvokeDynamic_info";
                        result = ParseConstant_CONSTANT_InvokeDynamic_info(treeView, parent, dataStore, ref bitOffset, ref maxLeftBits);
                        break;
                    case 19:
                        constantName = "CONSTANT_Module_info";
                        result = ParseConstant_CONSTANT_Module_info(treeView, parent, dataStore, ref bitOffset, ref maxLeftBits);
                        break;
                    case 20:
                        constantName = "CONSTANT_Package_info";
                        result = ParseConstant_CONSTANT_Package_info(treeView, parent, dataStore, ref bitOffset, ref maxLeftBits);
                        break;
                }
            }

            //Update the length of parent node.
            if (result.Fine)
            {
                Int64 bitOffset2 = bitOffset;
                Utility.UpdateNodeLength(parent, "constant " + index + ":" + constantName, ItemType.ITEM, (bitOffset2 - bitOffset1));
            }

            if ((tag == 5) || (tag == 6))
            {
                /*All 8-byte constants take up two entries in the constant_pool table of the class file.
                 * If a CONSTANT_Long_info or CONSTANT_Double_info structure is the entry at index n in the constant_pool table, 
                 * then the next usable entry in the table is located at index n+2. The constant_pool index n+1 must be valid but is considered unusable.
                 */

                index++;
            }
            return result;
        }


        public Result ParseConstant_stuffing(TreeView treeView, TreeNode parent, DataStore dataStore, ref Int64 bitOffset, ref Int64 maxLeftBits)
        {

            Result result = new Result();

            return result;
        }

        public Result ParseConstant_CONSTANT_Utf8_info(TreeView treeView, TreeNode parent, DataStore dataStore, ref Int64 bitOffset, ref Int64 maxLeftBits)
        {

            Result result = new Result();
            TreeNode newNode = null;

            Int64 length = 0;
            if (result.Fine)
            {
                //u2 length;
                result = Utility.AddNodeField(Position.CHILD, parent, out newNode, "length", ItemType.FIELD, dataStore, ref bitOffset, 16, ref length);
            }

            string utf8Info = null;
            if (result.Fine)
            {
                //u1 bytes[length]; Read it as text.
                result = Utility.GetText(out utf8Info, dataStore, bitOffset, length * 8);
            }

            if (result.Fine)
            {
                //u1 bytes[length];
                result = Utility.AddNodeData(Position.CHILD, parent, out newNode, "bytes:" + utf8Info, ItemType.FIELD, dataStore, ref bitOffset, length * 8);
            }
            return result;
        }


        public Result ParseConstant_CONSTANT_Integer_info(TreeView treeView, TreeNode parent, DataStore dataStore, ref Int64 bitOffset, ref Int64 maxLeftBits)
        {

            Result result = new Result();
            TreeNode newNode = null;
            Int64 fieldValue = 0;

            if (result.Fine)
            {
                //u4 bytes;
                result = Utility.AddNodeField(Position.CHILD, parent, out newNode, "bytes", ItemType.FIELD, dataStore, ref bitOffset, 32, ref fieldValue);
            }

            return result;
        }


        public Result ParseConstant_CONSTANT_Float_info(TreeView treeView, TreeNode parent, DataStore dataStore, ref Int64 bitOffset, ref Int64 maxLeftBits)
        {

            Result result = new Result();
            TreeNode newNode = null;
            Int64 fieldValue = 0;

            if (result.Fine)
            {
                //u4 bytes;
                result = Utility.AddNodeField(Position.CHILD, parent, out newNode, "bytes", ItemType.FIELD, dataStore, ref bitOffset, 32, ref fieldValue);
            }

            return result;
        }


        public Result ParseConstant_CONSTANT_Long_info(TreeView treeView, TreeNode parent, DataStore dataStore, ref Int64 bitOffset, ref Int64 maxLeftBits)
        {

            Result result = new Result();
            TreeNode newNode = null;
            Int64 fieldValue = 0;

            if (result.Fine)
            {
                //u4 high_bytes;
                result = Utility.AddNodeField(Position.CHILD, parent, out newNode, "high_bytes", ItemType.FIELD, dataStore, ref bitOffset, 32, ref fieldValue);
            }

            if (result.Fine)
            {
                //u4 low_bytes;
                result = Utility.AddNodeField(Position.CHILD, parent, out newNode, "low_bytes", ItemType.FIELD, dataStore, ref bitOffset, 32, ref fieldValue);
            }

            return result;
        }



        public Result ParseConstant_CONSTANT_Double_info(TreeView treeView, TreeNode parent, DataStore dataStore, ref Int64 bitOffset, ref Int64 maxLeftBits)
        {

            Result result = new Result();
            TreeNode newNode = null;
            Int64 fieldValue = 0;


            if (result.Fine)
            {
                //u4 high_bytes;
                result = Utility.AddNodeField(Position.CHILD, parent, out newNode, "high_bytes", ItemType.FIELD, dataStore, ref bitOffset, 32, ref fieldValue);
            }

            if (result.Fine)
            {
                //u4 low_bytes;
                result = Utility.AddNodeField(Position.CHILD, parent, out newNode, "low_bytes", ItemType.FIELD, dataStore, ref bitOffset, 32, ref fieldValue);
            }


            return result;
        }

        public Result ParseConstant_CONSTANT_Class_info(TreeView treeView, TreeNode parent, DataStore dataStore, ref Int64 bitOffset, ref Int64 maxLeftBits)
        {

            Result result = new Result();
            TreeNode newNode = null;
            Int64 fieldValue = 0;

            if (result.Fine)
            {
                //u2 name_index;
                result = Utility.AddNodeField(Position.CHILD, parent, out newNode, "name_index", ItemType.FIELD, dataStore, ref bitOffset, 16, ref fieldValue);
            }

            return result;
        }

        public Result ParseConstant_CONSTANT_String_info(TreeView treeView, TreeNode parent, DataStore dataStore, ref Int64 bitOffset, ref Int64 maxLeftBits)
        {

            Result result = new Result();
            TreeNode newNode = null;
            Int64 fieldValue = 0;

            if (result.Fine)
            {
                //u2 string_index;
                result = Utility.AddNodeField(Position.CHILD, parent, out newNode, "string_index", ItemType.FIELD, dataStore, ref bitOffset, 16, ref fieldValue);
            }

            return result;
        }


        public Result ParseConstant_CONSTANT_Fieldref_info(TreeView treeView, TreeNode parent, DataStore dataStore, ref Int64 bitOffset, ref Int64 maxLeftBits)
        {

            Result result = new Result();
            TreeNode newNode = null;
            Int64 fieldValue = 0;

            if (result.Fine)
            {
                //u2 class_index;
                result = Utility.AddNodeField(Position.CHILD, parent, out newNode, "class_index", ItemType.FIELD, dataStore, ref bitOffset, 16, ref fieldValue);
            }

            if (result.Fine)
            {
                //u2 name_and_type_index;;
                result = Utility.AddNodeField(Position.CHILD, parent, out newNode, "name_and_type_index", ItemType.FIELD, dataStore, ref bitOffset, 16, ref fieldValue);
            }

            return result;
        }


        public Result ParseConstant_CONSTANT_Methodref_info(TreeView treeView, TreeNode parent, DataStore dataStore, ref Int64 bitOffset, ref Int64 maxLeftBits)
        {

            Result result = new Result();
            TreeNode newNode = null;
            Int64 fieldValue = 0;

            if (result.Fine)
            {
                //u2 class_index;
                result = Utility.AddNodeField(Position.CHILD, parent, out newNode, "class_index", ItemType.FIELD, dataStore, ref bitOffset, 16, ref fieldValue);
            }

            if (result.Fine)
            {
                //u2 name_and_type_index;
                result = Utility.AddNodeField(Position.CHILD, parent, out newNode, "name_and_type_index", ItemType.FIELD, dataStore, ref bitOffset, 16, ref fieldValue);
            }

            return result;
        }


        public Result ParseConstant_CONSTANT_InterfaceMethodref_info(TreeView treeView, TreeNode parent, DataStore dataStore, ref Int64 bitOffset, ref Int64 maxLeftBits)
        {

            Result result = new Result();
            TreeNode newNode = null;
            Int64 fieldValue = 0;

            if (result.Fine)
            {
                //u2 class_index;
                result = Utility.AddNodeField(Position.CHILD, parent, out newNode, "class_index", ItemType.FIELD, dataStore, ref bitOffset, 16, ref fieldValue);
            }

            if (result.Fine)
            {
                //u2 name_and_type_index;;
                result = Utility.AddNodeField(Position.CHILD, parent, out newNode, "name_and_type_index", ItemType.FIELD, dataStore, ref bitOffset, 16, ref fieldValue);
            }

            return result;
        }

        public Result ParseConstant_CONSTANT_NameAndType_info(TreeView treeView, TreeNode parent, DataStore dataStore, ref Int64 bitOffset, ref Int64 maxLeftBits)
        {

            Result result = new Result();
            TreeNode newNode = null;
            Int64 fieldValue = 0;

            if (result.Fine)
            {
                //u2 name_index;
                result = Utility.AddNodeField(Position.CHILD, parent, out newNode, "name_index", ItemType.FIELD, dataStore, ref bitOffset, 16, ref fieldValue);
            }

            if (result.Fine)
            {
                //u2 descriptor_index;;
                result = Utility.AddNodeField(Position.CHILD, parent, out newNode, "descriptor_index", ItemType.FIELD, dataStore, ref bitOffset, 16, ref fieldValue);
            }

            return result;
        }


        public Result ParseConstant_CONSTANT_MethodHandle_info(TreeView treeView, TreeNode parent, DataStore dataStore, ref Int64 bitOffset, ref Int64 maxLeftBits)
        {

            Result result = new Result();
            TreeNode newNode = null;
            Int64 fieldValue = 0;

            if (result.Fine)
            {
                //u1 reference_kind;
                result = Utility.AddNodeField(Position.CHILD, parent, out newNode, "reference_kind", ItemType.FIELD, dataStore, ref bitOffset, 8, ref fieldValue);
            }

            if (result.Fine)
            {
                //u2 reference_index;
                result = Utility.AddNodeField(Position.CHILD, parent, out newNode, "reference_index", ItemType.FIELD, dataStore, ref bitOffset, 16, ref fieldValue);
            }

            return result;
        }

        public Result ParseConstant_CONSTANT_MethodType_info(TreeView treeView, TreeNode parent, DataStore dataStore, ref Int64 bitOffset, ref Int64 maxLeftBits)
        {

            Result result = new Result();
            TreeNode newNode = null;
            Int64 fieldValue = 0;

            if (result.Fine)
            {
                //u2 descriptor_index;
                result = Utility.AddNodeField(Position.CHILD, parent, out newNode, "descriptor_index", ItemType.FIELD, dataStore, ref bitOffset, 16, ref fieldValue);
            }

            return result;
        }

        public Result ParseConstant_CONSTANT_Dynamic_info(TreeView treeView, TreeNode parent, DataStore dataStore, ref Int64 bitOffset, ref Int64 maxLeftBits)
        {

            Result result = new Result();
            TreeNode newNode = null;
            Int64 fieldValue = 0;

            if (result.Fine)
            {
                //u2 bootstrap_method_attr_index;
                result = Utility.AddNodeField(Position.CHILD, parent, out newNode, "bootstrap_method_attr_index", ItemType.FIELD, dataStore, ref bitOffset, 16, ref fieldValue);
            }

            if (result.Fine)
            {
                //u2 name_and_type_index;
                result = Utility.AddNodeField(Position.CHILD, parent, out newNode, "name_and_type_index", ItemType.FIELD, dataStore, ref bitOffset, 16, ref fieldValue);
            }

            return result;
        }

        public Result ParseConstant_CONSTANT_InvokeDynamic_info(TreeView treeView, TreeNode parent, DataStore dataStore, ref Int64 bitOffset, ref Int64 maxLeftBits)
        {

            Result result = new Result();
            TreeNode newNode = null;
            Int64 fieldValue = 0;

            if (result.Fine)
            {
                //u2 bootstrap_method_attr_index;
                result = Utility.AddNodeField(Position.CHILD, parent, out newNode, "bootstrap_method_attr_index", ItemType.FIELD, dataStore, ref bitOffset, 16, ref fieldValue);
            }

            if (result.Fine)
            {
                //u2 name_and_type_index;
                result = Utility.AddNodeField(Position.CHILD, parent, out newNode, "name_and_type_index", ItemType.FIELD, dataStore, ref bitOffset, 16, ref fieldValue);
            }

            return result;
        }


        public Result ParseConstant_CONSTANT_Module_info(TreeView treeView, TreeNode parent, DataStore dataStore, ref Int64 bitOffset, ref Int64 maxLeftBits)
        {

            Result result = new Result();
            TreeNode newNode = null;
            Int64 fieldValue = 0;

            if (result.Fine)
            {
                //u2 name_index;
                result = Utility.AddNodeField(Position.CHILD, parent, out newNode, "name_index", ItemType.FIELD, dataStore, ref bitOffset, 16, ref fieldValue);
            }

            return result;
        }


        public Result ParseConstant_CONSTANT_Package_info(TreeView treeView, TreeNode parent, DataStore dataStore, ref Int64 bitOffset, ref Int64 maxLeftBits)
        {

            Result result = new Result();
            TreeNode newNode = null;
            Int64 fieldValue = 0;

            if (result.Fine)
            {
                //u2 name_index;
                result = Utility.AddNodeField(Position.CHILD, parent, out newNode, "name_index", ItemType.FIELD, dataStore, ref bitOffset, 16, ref fieldValue);
            }

            return result;
        }

        String GetAccessFlagsDescription(Int64 accessFlags)
        {
            String description = null;

            if ((accessFlags & 0x0001) != 0)
            {
                description += " ACC_PUBLIC ";
            }

            if ((accessFlags & 0x0010) != 0)
            {
                description += " ACC_FINAL ";
            }

            if ((accessFlags & 0x0020) != 0)
            {
                description += " ACC_SUPER ";
            }

            if ((accessFlags & 0x0200) != 0)
            {
                description += " ACC_INTERFACE ";
            }

            if ((accessFlags & 0x0400) != 0)
            {
                description += " ACC_ABSTRACT ";
            }

            if ((accessFlags & 0x1000) != 0)
            {
                description += " ACC_SYNTHETIC ";
            }

            if ((accessFlags & 0x2000) != 0)
            {
                description += " ACC_ANNOTATION ";
            }

            if ((accessFlags & 0x4000) != 0)
            {
                description += " ACC_ENUM ";
            }

            if ((accessFlags & 0x8000) != 0)
            {
                description += " ACC_MODULE ";
            }

            return description;
        }


        public Result ParseField(TreeView treeView, TreeNode parent, DataStore dataStore, ref Int64 bitOffset, ref Int64 maxLeftBits, Int64 index)
        {

            Result result = new Result();
            TreeNode newNode = null;
            Int64 fieldValue = 0;

            Int64 bitOffset1 = bitOffset;

            Int64 accessFlags = 0;
            if (result.Fine)
            {
                //u2 access_flags;
                result = Utility.AddNodeField(Position.CHILD, parent, out newNode, "access_flags", ItemType.FIELD, dataStore, ref bitOffset, 16, ref accessFlags);
            }

            if (result.Fine)
            {
                //Update description for access_flags.
                String accessFlagsDescription = GetAccessFlagsDescription(accessFlags);

                Utility.UpdateNode(newNode, "access_flags:" + accessFlagsDescription, ItemType.FIELD);

            }

            if (result.Fine)
            {
                //u2 name_index;
                result = Utility.AddNodeField(Position.CHILD, parent, out newNode, "name_index", ItemType.FIELD, dataStore, ref bitOffset, 16, ref fieldValue);
            }

            if (result.Fine)
            {
                //u2 descriptor_index;
                result = Utility.AddNodeField(Position.CHILD, parent, out newNode, "descriptor_index", ItemType.FIELD, dataStore, ref bitOffset, 16, ref fieldValue);
            }

            Int64 attributesCount = 0;
            if (result.Fine)
            {
                //u2 attributes_count;
                result = Utility.AddNodeField(Position.CHILD, parent, out newNode, "attributes_count", ItemType.FIELD, dataStore, ref bitOffset, 16, ref attributesCount);
            }

            //attribute_info attributes[attributes_count];
            TreeNode nodeAttribute = null;
            if (result.Fine)
            {
                for (Int64 i = 0; i < attributesCount; ++i)
                {
                    Int64 bitOffsetA = bitOffset;
                    result = Utility.AddNodeContainer(Position.CHILD, parent, out nodeAttribute, "attribute_info", ItemType.LOOP, dataStore, bitOffset, 0);

                    if (result.Fine)
                    {
                        result = ParseAttribute(treeView, nodeAttribute, dataStore, ref bitOffset, ref maxLeftBits, i);
                    }
                    if (!result.Fine)
                    {
                        break;
                    }

                    //Update the length.
                    if (result.Fine)
                    {
                        Int64 bitOffsetB = bitOffset;
                        Utility.UpdateNodeLength(nodeAttribute, "attribute_info " + index, ItemType.ITEM, (bitOffsetB - bitOffsetA));
                    }

                }
            }

            //Update the length of parent node.
            if (result.Fine)
            {
                Int64 bitOffset2 = bitOffset;
                Utility.UpdateNodeLength(parent, "field_info " + index, ItemType.ITEM, (bitOffset2 - bitOffset1));
            }

            return result;
        }


        public Result ParseAttribute(TreeView treeView, TreeNode parent, DataStore dataStore, ref Int64 bitOffset, ref Int64 maxLeftBits, Int64 index)
        {

            Result result = new Result();
            TreeNode newNode = null;
            Int64 fieldValue = 0;

            Int64 bitOffset1 = bitOffset;

            if (result.Fine)
            {
                //u2 attribute_name_index;
                result = Utility.AddNodeField(Position.CHILD, parent, out newNode, "attribute_name_index", ItemType.FIELD, dataStore, ref bitOffset, 16, ref fieldValue);
            }

            Int64 attributeLength = 0;
            if (result.Fine)
            {
                //u4 attribute_length;
                result = Utility.AddNodeField(Position.CHILD, parent, out newNode, "attribute_length", ItemType.FIELD, dataStore, ref bitOffset, 32, ref attributeLength);
            }

            if (result.Fine)
            {
                //u1 info[attribute_length];
                result = Utility.AddNodeData(Position.CHILD, parent, out newNode, "info", ItemType.FIELD, dataStore, ref bitOffset, attributeLength*8);
            }
            return result;
        }


        public Result ParseMethod(TreeView treeView, TreeNode parent, DataStore dataStore, ref Int64 bitOffset, ref Int64 maxLeftBits, Int64 index)
        {

            Result result = new Result();
            TreeNode newNode = null;
            Int64 fieldValue = 0;

            Int64 bitOffset1 = bitOffset;

            Int64 accessFlags = 0;
            if (result.Fine)
            {
                //u2 access_flags;
                result = Utility.AddNodeField(Position.CHILD, parent, out newNode, "access_flags", ItemType.FIELD, dataStore, ref bitOffset, 16, ref accessFlags);
            }

            if (result.Fine)
            {
                //Update description for access_flags.
                String accessFlagsDescription = GetAccessFlagsDescription(accessFlags);

                Utility.UpdateNode(newNode, "access_flags:" + accessFlagsDescription, ItemType.FIELD);

            }

            if (result.Fine)
            {
                //u2 name_index;
                result = Utility.AddNodeField(Position.CHILD, parent, out newNode, "name_index", ItemType.FIELD, dataStore, ref bitOffset, 16, ref fieldValue);
            }

            if (result.Fine)
            {
                //u2 descriptor_index;
                result = Utility.AddNodeField(Position.CHILD, parent, out newNode, "descriptor_index", ItemType.FIELD, dataStore, ref bitOffset, 16, ref fieldValue);
            }

            Int64 attributesCount = 0;
            if (result.Fine)
            {
                //u2 attributes_count;
                result = Utility.AddNodeField(Position.CHILD, parent, out newNode, "attributes_count", ItemType.FIELD, dataStore, ref bitOffset, 16, ref attributesCount);
            }

            //attribute_info attributes[attributes_count];
            TreeNode nodeAttribute = null;
            if (result.Fine)
            {
                for (Int64 i = 0; i < attributesCount; ++i)
                {
                    Int64 bitOffsetA = bitOffset;
                    result = Utility.AddNodeContainer(Position.CHILD, parent, out nodeAttribute, "attribute_info", ItemType.LOOP, dataStore, bitOffset, 0);

                    if (result.Fine)
                    {
                        result = ParseAttribute(treeView, nodeAttribute, dataStore, ref bitOffset, ref maxLeftBits, i);
                    }
                    if (!result.Fine)
                    {
                        break;
                    }

                    //Update the length.
                    if (result.Fine)
                    {
                        Int64 bitOffsetB = bitOffset;
                        Utility.UpdateNodeLength(nodeAttribute, "attribute_info " + index, ItemType.ITEM, (bitOffsetB - bitOffsetA));
                    }

                }
            }

            //Update the length of parent node.
            if (result.Fine)
            {
                Int64 bitOffset2 = bitOffset;
                Utility.UpdateNodeLength(parent, "method_info " + index, ItemType.ITEM, (bitOffset2 - bitOffset1));
            }

            return result;
        }


    }
}
