using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class FieldItem
    {
        private DataStore dataStore = null;
        private Int64 fieldOffsetInBit = 0;
        private Int64 fieldLengthInBit = 0;
        private Int64 fieldValue = 0;
        private ItemType itemType = ItemType.FIELD;

        public byte[] Data
        {
            get
            {
                return dataStore.GetData();
            }
        }

        //Offset in bit.
        public Int64 Offset
        {
            get
            {
                return fieldOffsetInBit;
            }
        }

        //Length in bit.
        public Int64 Length
        {
            get
            {
                return fieldLengthInBit;
            }
            set
            {
                fieldLengthInBit = value;
            }
        }

        //Value.
        public Int64 Value
        {
            get
            {
                return fieldValue;
            }
        }

        //ItemType.
        public ItemType Type
        {
            get 
            {
                return itemType; 
            }
            set
            {
                itemType = value;
            }
        }

        public object Tag1
        {
            get;
            set;
        }

        public DataStore GetDataStore()
        {
            return dataStore ;
        }


        public FieldItem(ItemType nodeType, DataStore dataStore, Int64 fieldOffsetInBit, Int64 fieldLengthInBit, Int64 fieldValue)
        {
            this.dataStore = dataStore;
            this.fieldOffsetInBit = fieldOffsetInBit;
            this.fieldLengthInBit = fieldLengthInBit;
            this.fieldValue = fieldValue;
            this.itemType = nodeType;
        }

        public String GetDataStoreHexString()
        {
            byte[] dataBlock = dataStore.GetData();
            Int64 dataLength = dataBlock.Length;
            Int64 blockCount = dataLength / 8;
            Int64 byteCount = dataLength % 8;
            Int64 i = 0;
            StringBuilder strBuilder = new StringBuilder();

            while (i < blockCount)
            {
                strBuilder.AppendFormat("{0,2:X2} {1,2:X2} {2,2:X2} {3,2:X2} {4,2:X2} {5,2:X2} {6,2:X2} {7,2:X2} " + Environment.NewLine,
                    dataBlock[i * 8 + 0],
                    dataBlock[i * 8 + 1],
                    dataBlock[i * 8 + 2],
                    dataBlock[i * 8 + 3],
                    dataBlock[i * 8 + 4],
                    dataBlock[i * 8 + 5],
                    dataBlock[i * 8 + 6],
                    dataBlock[i * 8 + 7]);

                i++;
            }

            for (i = 0; i < byteCount; ++i)
            {
                strBuilder.AppendFormat("{0,2:X2} ", dataBlock[8 * blockCount + i]);
            }

            return strBuilder.ToString();
        }
    }
}
