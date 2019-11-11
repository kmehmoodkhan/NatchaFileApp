using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NatchaFileApp
{
    public class NachaFileHeader
    {
        public string DestinationBankRoutingNumber
        {
            get;
            set;
        }

        public string OriginatingCompanyId
        {
            get;
            set;
        }

        public string DestinationBankName
        {
            get;
            set;
        }

        public string OriginatingCompanyName
        {
            get;
            set;
        }

        public string ReferenceCode
        {
            get;
            set;
        }
    }
}
