using System;
using System.Collections.Generic;
using System.Text;

namespace Library
{
    //Callback to notify all kinds of messages(from StreamParser to owner of the StreamParser, like StreamParserForm.)
    public delegate void MessageHandlerDelegate(MessageId messageId, object message);

    public enum DataSource
    {
        SOURCE_FILE = 1,
        SOURCE_NETWORK = 2
    };

    public enum MessageId
    {
        MESSAGE_PARSING_START = 0,
        MESSAGE_DATA = 1, //Packet data.
        MESSAGE_PROGRESS = 2, //Progress update.
        MESSAGE_PARSING_DONE = 3
    }

    public enum ItemType
    {
        ROOT = 0,
        TOP_GROUP1 = 1,
        TOP_GROUP2 = 2,
        STATISTIC = 3,
        SECTION1 = 4,
        SECTION2 = 5,
        FIELD = 6,
        DATA = 7,
        LOOP = 8,
        ITEM = 9,
        WARNING = 10,
        ERROR = 11,
        SEARCH = 12
    }


    public enum Position
    {
        CHILD = 0,
        PEER = 1
    }
}
