using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NatchaFileApp
{
    public static class ApplicationSettings
    {
        public static string SFTPHost
        {
            get
            {
                return ConfigurationManager.AppSettings["SFTPHost"].ToString();
            }
        }

        public static string SFTPUsername
        {
            get
            {
                return ConfigurationManager.AppSettings["SFTPUsername"].ToString();
            }
        }

        public static string SFTPUserPassword
        {
            get
            {
                return ConfigurationManager.AppSettings["SFTPUserPassword"].ToString();
            }
        }

        public static string SFTPDirPath
        {
            get
            {
                return ConfigurationManager.AppSettings["SFTPDirPath"].ToString();
            }
        }

        public static string SFTUploadDirPath
        {
            get
            {
                return ConfigurationManager.AppSettings["SFTUploadDirPath"].ToString();
            }
        }


        public static string SFTPDefaultPort
        {
            get
            {
                return ConfigurationManager.AppSettings["SFTPDefaultPort"].ToString();
            }
        }
    }
}
