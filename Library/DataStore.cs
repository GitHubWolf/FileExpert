using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class DataStore
    {
        private Int64 repeatTimes = 1;//Once created, the repeatTimes must at least 1.
        private byte[] dataBlock = null;
        private Int64 realDataLength = 0;
        private Int64 finalOffsetInBits = 0;
        private Int64 cutoffSizeInBits = 0;//In some special cases,we will force to cut off some bytes at the end of the buffer. Method GetLength() will be used to get the size with cutoff.
        private Int64 currentOffsetInBit = 0;
        private List<DataSourceMetadata> dataSourceMetadataList = null;

        private bool isNetworkByteOrder = true;

        public DataStore(byte[] buffer)
        {
            dataBlock = buffer;
            realDataLength = buffer.LongLength;
            finalOffsetInBits = realDataLength * 8;
        }

        public DataStore(byte[] buffer, Int64 bitOffset, Int64 dataLength)
        {
            dataBlock = buffer;

            //Convert to bits.
            currentOffsetInBit = bitOffset;
            realDataLength = dataLength;
            finalOffsetInBits = bitOffset +  (dataLength * 8);
        }

        public bool Compare(DataStore newDataStore)
        {
            bool same = false;

            byte[] newData = newDataStore.GetData();

            if (newData.SequenceEqual(dataBlock))
            {
                same = true;
            }

            //If same,increase the repeateTimes.
            if (same)
            {
                repeatTimes++;
            }
            return same;
        }

        public Int64 GetLeftBitLength()
        {
            Int64 leftBitLength = finalOffsetInBits - cutoffSizeInBits - currentOffsetInBit;
            return leftBitLength;
        }

        public Int64 GetRepeatTimes()
        {
            return repeatTimes;
        }

        public byte[] GetData()
        {
            return dataBlock;
        }

        public Int64 GetRealLength()
        {
            return realDataLength;
        }

        //Cut off several bits!
        public Result CutOff(Int64 cutoffBits)
        {
            Result result = new Result();

            if (result.Fine)
            {
                if (GetLeftBitLength() < cutoffBits)
                {
                    //If no enough data left.
                    result.SetResult(ResultCode.INSUFFICIENT_DATA);
                }
            }

            if (result.Fine)
            {
                this.cutoffSizeInBits += cutoffBits;
            }

            return result;
        }

        public void CleanCutoff()
        {
            this.cutoffSizeInBits = 0;
        }

        //Clean up offset and cutoffsize.
        public void Reset()
        {
            this.currentOffsetInBit = 0;
            this.cutoffSizeInBits = 0;
        }


        //To read in bits from the data store. Maximum 64bits is supported.
        public Result ReadBits(ref Int64 bitOffset, Int64 bitLength, ref Int64 fieldValue)
        {
            Result result = new Result();

            Int64 leftBits = GetLeftBitLength();

            //Check whether the length is valid.
            if (bitLength > leftBits) //Use GetLength to get the length with cutoff.
            {
                result.SetResult(ResultCode.INSUFFICIENT_DATA);
            }

            if (result.Fine)
            {
                Int64 dataLeftInBit = GetLeftBitLength(); //Use GetLength to get the length with cutoff.

                result = Utility.ByteArrayReadBits(dataBlock, ref dataLeftInBit, ref bitOffset, bitLength, ref fieldValue, isNetworkByteOrder);
            }

            if (result.Fine)
            {
                currentOffsetInBit += bitLength;
            }
            return result;
        }

        //To rollback bits, i.e. decrease bitOffset
        public Result RollbackBits(ref Int64 bitOffset, Int64 bitLength)
        {
            Result result = new Result();

            if (result.Fine)
            {
                currentOffsetInBit -= bitLength;
                bitOffset -= bitLength;
            }
            return result;
        }


        //Read bits from the end of the buffer.
        public Result ReadEndingBits(ref Int64 bitOffsetForThisField, Int64 bitLength, ref Int64 fieldValue)
        {
            Result result = new Result();

            //Check whether the length is valid.
            if (bitLength > (GetLeftBitLength())) //Use GetLength to get the length with cutoff.
            {
                result.SetResult(ResultCode.INSUFFICIENT_DATA);
            }

            if (result.Fine)
            {
                Int64 dataLeftInBit = GetLeftBitLength(); //To get the length with cutoff.

                //Set the fieldValue so that we can know the offset of the field.
                bitOffsetForThisField = dataBlock.Length * 8 - cutoffSizeInBits - bitLength;

                Int64 bitOffsetTemp = bitOffsetForThisField;

                result = Utility.ByteArrayReadBits(dataBlock, ref dataLeftInBit, ref bitOffsetTemp, bitLength, ref fieldValue, isNetworkByteOrder);
            }

            if (result.Fine)
            {
                //Increase cutoff!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                cutoffSizeInBits += bitLength;
            }

            return result;
        }

        //To read in bits from the data store. Maximum 64bits is supported.
        public Result SkipBits(ref Int64 bitOffset, Int64 bitLength)
        {
            Result result = new Result();

            //Check whether the length is valid.
            if (bitLength > (GetLeftBitLength())) //Use GetLength to get the length with cutoff.
            {
                result.SetResult(ResultCode.INSUFFICIENT_DATA);
            }

            if (result.Fine)
            {
                Int64 dataLeftInBit = GetLeftBitLength(); //Use GetLength to get the length with cutoff.

                result = Utility.ByteArraySkipBits(ref dataLeftInBit, ref bitOffset, bitLength);
            }

            if (result.Fine)
            {
                currentOffsetInBit += bitLength;
            }
            return result;
        }



        public void SetMetadataList(List<DataSourceMetadata> dataSourceMetadataList)
        {
            this.dataSourceMetadataList = dataSourceMetadataList;
        }

        public List<DataSourceMetadata> GetDataSourceMetadataList()
        {
            return this.dataSourceMetadataList;
        }

        public void SetByteOrder(bool readAsNetworkByteOrder)
        {
            isNetworkByteOrder = readAsNetworkByteOrder;
        }
    }
}
