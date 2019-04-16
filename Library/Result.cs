using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{

    public enum ResultCode
    {
        SUCCESS = 1,
        FAILURE = 2,
        INVALID_STREAM = 3,
        FAILED_TO_OPEN_FILE = 4,
        INSUFFICIENT_DATA = 5,
        INVALID_DATA = 6,
        ID_MISMATCH = 7,
        DATA_MISMATCH = 8,
        INVALID_PLUGIN = 9
    };

    public class Result
    {
        private ResultCode resultCode = ResultCode.SUCCESS;

        public bool Fine
        {
            get
            {
                return (resultCode == ResultCode.SUCCESS);
            }
        }

        public void SetResult(ResultCode resultCode)
        {
            this.resultCode = resultCode;
        }

        public void SetFailure()
        {
            this.resultCode = ResultCode.FAILURE;
        }
    }
}
