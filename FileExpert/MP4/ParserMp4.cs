using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Library;
using FileExpert.MP4;
using FileExpert.MP4.Box;
using FileExpert.MP4.Info;


namespace FileExpert.MP4
{
    class ParseMp4
    {
        public void ParseFirstLevelBox(TreeView treeView, TreeNode parent, DataStore dataStore)
        {
            TreeNode nodeBox = null;
            TreeNode newNode = null;

            //Add one node to indicate this box.
            Result result = Utility.AddNodeContainer(Position.CHILD, parent, out nodeBox, "Box", ItemType.SECTION2, dataStore, 0, dataStore.GetLeftBitLength());

            Int64 bitOffset = 0;
            Int64 fieldValue = 0;

            //unsigned int(32) size; 
            Int64 size = 0;
            if (result.Fine)
            {
                result = Utility.AddNodeField(Position.CHILD, nodeBox, out newNode, "size", ItemType.FIELD, dataStore, ref bitOffset, 32, ref size);
            }

            string boxTypeName = null;
            if (result.Fine)
            {
                //unsigned int(32) type = boxtype;
                result = Utility.GetText(out boxTypeName, dataStore, bitOffset, 32);
            }

            if (result.Fine)
            {
                result = Utility.AddNodeData(Position.CHILD, nodeBox, out newNode, "type: " + boxTypeName, ItemType.FIELD, dataStore, ref bitOffset, 32);
            }

            if (result.Fine && (1 == size))
            {
                //unsigned int(64) largesize;
                result = Utility.AddNodeField(Position.CHILD, nodeBox, out newNode, "largesize", ItemType.FIELD, dataStore, ref bitOffset, 64, ref fieldValue);
            }

            if (result.Fine)
            {
                ParseBoxPayload(treeView, dataStore, nodeBox, bitOffset, boxTypeName);
            }
            else
            {
                result = Utility.AddNodeData(Position.CHILD, nodeBox, out newNode, "Unknown Data", ItemType.ERROR, dataStore, ref bitOffset, dataStore.GetLeftBitLength());
            }
        }

        public void ParseInnerBox(TreeView treeView, TreeNode parent, DataStore dataStore, ref Int64 bitOffset)
        {
            TreeNode nodeBox = null;
            TreeNode newNode = null;

            Result result = new Result();
            //Int64 fieldValue = 0;
            Int64 size = 0;
            Int64 realBoxSize = 0;
            Int64 boxHeaderSize = 0;

            while (0 != dataStore.GetLeftBitLength() && result.Fine)
            {
                realBoxSize = 0;
                boxHeaderSize = 0;

                //Add one node to indicate this box.
                result = Utility.AddNodeContainer(Position.CHILD, parent, out nodeBox, "Box", ItemType.ITEM, dataStore, bitOffset, 0);

                //unsigned int(32) size; 
                if (result.Fine)
                {
                    result = Utility.AddNodeField(Position.CHILD, nodeBox, out newNode, "size", ItemType.FIELD, dataStore, ref bitOffset, 32, ref size);
                }

                string boxTypeName = null;
                if (result.Fine)
                {
                    //unsigned int(32) type = boxtype;
                    result = Utility.GetText(out boxTypeName, dataStore, bitOffset, 32);
                }

                if (result.Fine)
                {
                    result = Utility.AddNodeData(Position.CHILD, nodeBox, out newNode, "type: " + boxTypeName, ItemType.FIELD, dataStore, ref bitOffset, 32);
                }

                if (result.Fine)
                {
                    if (1 == size)
                    {
                        //unsigned int(64) largesize;
                        result = Utility.AddNodeField(Position.CHILD, nodeBox, out newNode, "largesize", ItemType.FIELD, dataStore, ref bitOffset, 64, ref realBoxSize);
                        boxHeaderSize = 16;
                    }
                    else
                    {
                        realBoxSize = size;
                        boxHeaderSize = 8;
                    }
                }

                if (result.Fine)
                {
                    Utility.UpdateNodeLength(nodeBox, "box " + boxTypeName, ItemType.ITEM, realBoxSize * 8);
                }

                Int64 innerBoxBitOffset = bitOffset;
                if (result.Fine)
                {
                    //Create a new data store containing the payload for this box.
                    DataStore innerBoxDataStore = new DataStore(dataStore.GetData(), innerBoxBitOffset, realBoxSize - boxHeaderSize);
                    innerBoxDataStore.SetMetadataList(dataStore.GetDataSourceMetadataList());

                    //Parse box payload which may also contains boxes.
                    ParseBoxPayload(treeView, innerBoxDataStore, nodeBox, innerBoxBitOffset, boxTypeName);

                    //Skip the payload.
                    dataStore.SkipBits(ref bitOffset, (realBoxSize - boxHeaderSize) * 8);

                }
                else
                {
                    result = Utility.AddNodeData(Position.CHILD, nodeBox, out newNode, "Unknown Data", ItemType.ERROR, dataStore, ref bitOffset, dataStore.GetLeftBitLength());
                }
            }
        }

        private static void ParseBoxPayload(TreeView treeView, DataStore dataStore, TreeNode nodeBox, Int64 bitOffset, string boxTypeName)
        {
            System.Console.WriteLine("-------------------" + boxTypeName);
            if (0 == boxTypeName.CompareTo("ftyp"))
            {
                Utility.UpdateNode(nodeBox, "FileTypeBox", ItemType.SECTION1);
                new FileTypeBox(treeView, nodeBox, dataStore, ref bitOffset);
            }
            else if (0 == boxTypeName.CompareTo("moov"))
            {
                Utility.UpdateNode(nodeBox, "MovieBox", ItemType.SECTION1);
                new MovieBox(treeView, nodeBox, dataStore, ref bitOffset);
            }
            else if (0 == boxTypeName.CompareTo("moof"))
            {
                Utility.UpdateNode(nodeBox, "MovieFragmentBox", ItemType.SECTION1);
                new MovieFragmentBox(treeView, nodeBox, dataStore, ref bitOffset);
            }
            else if (0 == boxTypeName.CompareTo("mdat"))
            {
                Utility.UpdateNode(nodeBox, "MediaDataBox", ItemType.SECTION1);
                new MediaDataBox(treeView, nodeBox, dataStore, ref bitOffset);
            }
            else if (0 == boxTypeName.CompareTo("mfra"))
            {
                Utility.UpdateNode(nodeBox, "MovieFragmentRandomAccessBox", ItemType.SECTION1);
                new MovieFragmentRandomAccessBox(treeView, nodeBox, dataStore, ref bitOffset);
            }
            else if (0 == boxTypeName.CompareTo("mvhd"))
            {
                Utility.UpdateNode(nodeBox, "MovieHeaderBox", ItemType.SECTION1);
                new MovieHeaderBox(treeView, nodeBox, dataStore, ref bitOffset);
            }
            else if (0 == boxTypeName.CompareTo("trak"))
            {
                Utility.UpdateNode(nodeBox, "TrackBox", ItemType.SECTION1);
                new TrackBox(treeView, nodeBox, dataStore, ref bitOffset);
            }
            else if (0 == boxTypeName.CompareTo("tkhd"))
            {
                Utility.UpdateNode(nodeBox, "TrackHeaderBox", ItemType.SECTION1);
                new TrackHeaderBox(treeView, nodeBox, dataStore, ref bitOffset);
            }
            else if (0 == boxTypeName.CompareTo("mdia"))
            {
                Utility.UpdateNode(nodeBox, "MediaBox", ItemType.SECTION1);
                //Special point: create the class to save the useful info.
                Utility.SetTag1(nodeBox, new MediaBoxInfo());
                new MediaBox(treeView, nodeBox, dataStore, ref bitOffset);
            }
            else if (0 == boxTypeName.CompareTo("mdhd"))
            {
                Utility.UpdateNode(nodeBox, "MediaHeaderBox", ItemType.SECTION1);
                new MediaHeaderBox(treeView, nodeBox, dataStore, ref bitOffset);
            }
            else if (0 == boxTypeName.CompareTo("hdlr"))
            {
                Utility.UpdateNode(nodeBox, "HandlerBox", ItemType.SECTION1);
                new HandlerBox(treeView, nodeBox, dataStore, ref bitOffset);
            }
            else if (0 == boxTypeName.CompareTo("minf"))
            {
                Utility.UpdateNode(nodeBox, "MediaInformationBox", ItemType.SECTION1);
                new MediaInformationBox(treeView, nodeBox, dataStore, ref bitOffset);
            }
            else if (0 == boxTypeName.CompareTo("smhd"))
            {
                Utility.UpdateNode(nodeBox, "SoundMediaHeaderBox", ItemType.SECTION1);
                new SoundMediaHeaderBox(treeView, nodeBox, dataStore, ref bitOffset);
            }
            else if (0 == boxTypeName.CompareTo("dinf"))
            {
                Utility.UpdateNode(nodeBox, "DataInformationBox", ItemType.SECTION1);
                new DataInformationBox(treeView, nodeBox, dataStore, ref bitOffset);
            }
            else if (0 == boxTypeName.CompareTo("dref"))
            {
                Utility.UpdateNode(nodeBox, "DataReferenceBox", ItemType.SECTION1);
                new DataReferenceBox(treeView, nodeBox, dataStore, ref bitOffset);
            }
            else if (0 == boxTypeName.CompareTo("url "))//A pending space!
            {
                Utility.UpdateNode(nodeBox, "DataEntryUrlBox", ItemType.SECTION1);
                new DataEntryUrlBox(treeView, nodeBox, dataStore, ref bitOffset);
            }
            else if (0 == boxTypeName.CompareTo("urn "))//A pending space!
            {
                Utility.UpdateNode(nodeBox, "DataEntryUrlBox", ItemType.SECTION1);
                new DataEntryUrnBox(treeView, nodeBox, dataStore, ref bitOffset);
            }
            else if (0 == boxTypeName.CompareTo("stbl"))
            {
                Utility.UpdateNode(nodeBox, "SampleTableBox", ItemType.SECTION1);
                new SampleTableBox(treeView, nodeBox, dataStore, ref bitOffset);
            }
            else if (0 == boxTypeName.CompareTo("stts"))
            {
                Utility.UpdateNode(nodeBox, "TimeToSampleBox", ItemType.SECTION1);
                new TimeToSampleBox(treeView, nodeBox, dataStore, ref bitOffset);
            }
            else if (0 == boxTypeName.CompareTo("ctts"))
            {
                Utility.UpdateNode(nodeBox, "CompositionOffsetBox", ItemType.SECTION1);
                new CompositionOffsetBox(treeView, nodeBox, dataStore, ref bitOffset);
            }
            else if (0 == boxTypeName.CompareTo("stsc"))
            {
                Utility.UpdateNode(nodeBox, "SampleToChunkBox", ItemType.SECTION1);
                new SampleToChunkBox(treeView, nodeBox, dataStore, ref bitOffset);
            }
            else if (0 == boxTypeName.CompareTo("stsz"))
            {
                Utility.UpdateNode(nodeBox, "SampleSizeBox", ItemType.SECTION1);
                new SampleSizeBox(treeView, nodeBox, dataStore, ref bitOffset);
            }
            else if (0 == boxTypeName.CompareTo("stco"))
            {
                Utility.UpdateNode(nodeBox, "ChunkOffsetBox", ItemType.SECTION1);
                new ChunkOffsetBox(treeView, nodeBox, dataStore, ref bitOffset);
            }
            else if (0 == boxTypeName.CompareTo("stsd"))
            {
                Utility.UpdateNode(nodeBox, "SampleDescriptionBox", ItemType.SECTION1);
                new SampleDescriptionBox(treeView, nodeBox, dataStore, ref bitOffset);
            }
            else if (0 == boxTypeName.CompareTo("mvex"))
            {
                Utility.UpdateNode(nodeBox, "MovieExtendsBox", ItemType.SECTION1);
                new MovieExtendsBox(treeView, nodeBox, dataStore, ref bitOffset);
            }
            else if (0 == boxTypeName.CompareTo("mehd"))
            {
                Utility.UpdateNode(nodeBox, "MovieExtendsHeaderBox", ItemType.SECTION1);
                new MovieExtendsHeaderBox(treeView, nodeBox, dataStore, ref bitOffset);
            }
            else if (0 == boxTypeName.CompareTo("trex"))
            {
                Utility.UpdateNode(nodeBox, "TrackExtendsBox", ItemType.SECTION1);
                new TrackExtendsBox(treeView, nodeBox, dataStore, ref bitOffset);
            }
            else if (0 == boxTypeName.CompareTo("mfhd"))
            {
                Utility.UpdateNode(nodeBox, "MovieFragmentHeaderBox", ItemType.SECTION1);
                new MovieFragmentHeaderBox(treeView, nodeBox, dataStore, ref bitOffset);
            }
            else if (0 == boxTypeName.CompareTo("traf"))
            {
                Utility.UpdateNode(nodeBox, "TrackFragmentBox", ItemType.SECTION1);
                new TrackFragmentBox(treeView, nodeBox, dataStore, ref bitOffset);
            }
            else if (0 == boxTypeName.CompareTo("tfhd"))
            {
                Utility.UpdateNode(nodeBox, "TrackFragmentHeaderBox", ItemType.SECTION1);
                new TrackFragmentHeaderBox(treeView, nodeBox, dataStore, ref bitOffset);
            }
            else if (0 == boxTypeName.CompareTo("trun"))
            {
                Utility.UpdateNode(nodeBox, "TrackRunBox", ItemType.SECTION1);
                new TrackRunBox(treeView, nodeBox, dataStore, ref bitOffset);
            }
            else if (0 == boxTypeName.CompareTo("tfra"))
            {
                Utility.UpdateNode(nodeBox, "TrackFragmentRandomAccessBox", ItemType.SECTION1);
                new TrackFragmentRandomAccessBox(treeView, nodeBox, dataStore, ref bitOffset);
            }
            else if (0 == boxTypeName.CompareTo("mfro"))
            {
                Utility.UpdateNode(nodeBox, "MovieFragmentRandomAccessOffsetBox", ItemType.SECTION1);
                new MovieFragmentRandomAccessOffsetBox(treeView, nodeBox, dataStore, ref bitOffset);
            }
            else if (0 == boxTypeName.CompareTo("vmhd"))
            {
                Utility.UpdateNode(nodeBox, "VideoMediaHeaderBox", ItemType.SECTION1);
                new VideoMediaHeaderBox(treeView, nodeBox, dataStore, ref bitOffset);
            }
            else if (0 == boxTypeName.CompareTo("sdtp"))
            {
                Utility.UpdateNode(nodeBox, "SampleDependencyTypeBox", ItemType.SECTION1);
                new SampleDependencyTypeBox(treeView, nodeBox, dataStore, ref bitOffset);
            }
            else if (0 == boxTypeName.CompareTo("stss"))
            {
                Utility.UpdateNode(nodeBox, "SyncSampleBox", ItemType.SECTION1);
                new SyncSampleBox(treeView, nodeBox, dataStore, ref bitOffset);                
            }
            else if (0 == boxTypeName.CompareTo("udta"))
            {
                Utility.UpdateNode(nodeBox, "UserDataBox", ItemType.SECTION1);
                new UserDataBox(treeView, nodeBox, dataStore, ref bitOffset);
            }
            else if (0 == boxTypeName.CompareTo("edts"))
            {
                Utility.UpdateNode(nodeBox, "EditBox", ItemType.SECTION1);
                new EditBox(treeView, nodeBox, dataStore, ref bitOffset);
            }
            else if (0 == boxTypeName.CompareTo("elst"))
            {
                Utility.UpdateNode(nodeBox, "EditListBox", ItemType.SECTION1);
                new EditListBox(treeView, nodeBox, dataStore, ref bitOffset);
            }
            else if (0 == boxTypeName.CompareTo("meta"))
            {
                Utility.UpdateNode(nodeBox, "MetaBox", ItemType.SECTION1);
                new MetaBox(treeView, nodeBox, dataStore, ref bitOffset);
            }
            else if (0 == boxTypeName.CompareTo("free"))
            {
                Utility.UpdateNode(nodeBox, "FreeSpaceBox", ItemType.SECTION1);
                new FreeSpaceBox(treeView, nodeBox, dataStore, ref bitOffset);
                Utility.ExpandToRoot(nodeBox, ItemType.WARNING);
            }
            else if (0 == boxTypeName.CompareTo("uuid"))
            {
                TreeNode newNode = null;
                //payload which is still unknown to us.
                Utility.AddNodeData(Position.CHILD, nodeBox, out newNode, "box_payload", ItemType.DATA, dataStore, ref bitOffset, dataStore.GetLeftBitLength());

            }
            else if (0 == boxTypeName.CompareTo("encv"))
            {
                TreeNode newNode = null;
                //payload which is still unknown to us.
                Utility.AddNodeData(Position.CHILD, nodeBox, out newNode, "box_payload", ItemType.DATA, dataStore, ref bitOffset, dataStore.GetLeftBitLength());

            }
            else if (0 == boxTypeName.CompareTo("enca"))
            {
                TreeNode newNode = null;
                //payload which is still unknown to us.
                Utility.AddNodeData(Position.CHILD, nodeBox, out newNode, "box_payload", ItemType.DATA, dataStore, ref bitOffset, dataStore.GetLeftBitLength());

            }
            else
            {

                TreeNode newNode = null;
                //payload which is still unknown to us.
                Utility.AddNodeData(Position.CHILD, nodeBox, out newNode, "box_payload", ItemType.DATA, dataStore, ref bitOffset, dataStore.GetLeftBitLength());

                //Utility.ExpandToRoot(newNode, ItemType.WARNING);
            }
        }


    }
}
