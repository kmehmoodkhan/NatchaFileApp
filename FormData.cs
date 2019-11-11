using ChoETL.NACHA;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinSCP;

namespace NatchaFileApp
{
    public partial class FormData : Form
    {
        private string ApplicationTitle
        {
            get
            {
                var appTitle = string.Empty;
                appTitle = ConfigurationManager.AppSettings["AppTitle"];
                return appTitle;
            }
        }

        private DataSet DataSource
        {
            get
            {
                bool isTest = true;

                if (!isTest)
                {
                    DataAccess dataAccess = new DataAccess();
                    var data = dataAccess.GetNachaData();
                    return data;
                }
                else
                {
                    TestData testData = new TestData();
                    var data = testData.GetData();
                    return data;
                }
            }
        }
        public FormData()
        {
            InitializeComponent();

            this.Text = ApplicationTitle;

            this.dataGridViewData.DataSource = DataSource.Tables[0];

        }


        private void Button1_Click(object sender, EventArgs e)
        {
            
            var data = this.DataSource;
            string filePath = "";
            try
            {
                filePath = GenerateFile(data.Tables[0]);
            }
            catch(Exception ex)
            {
                MessageBox.Show(this, "System encountered an error while generating NACHA file.", "File Upload", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            try
            {
                var result = UploadFile(filePath);
                if (result)
                {
                    File.Delete(filePath);
                    MessageBox.Show(this, "NACHA file uploaded successfully.", "File Upload", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(this, "Application failed to upload NACHA file.", "File Upload", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(this, "System encountered an error while uploading NACHA file.", "File Upload", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region GenerateNacha
        private static string GenerateFile(DataTable dt)
        {
            string tempPath = Path.GetTempPath();
            string fileName = DateTime.Now.ToString("yyyyddMM_HHMMss")+"_ACH.txt";

            if (!tempPath.EndsWith("\\"))
            {
                fileName = tempPath + "\\" + fileName;
            }
            else
            {
                fileName = tempPath + fileName;
            }

            ChoNACHAConfiguration config = new ChoNACHAConfiguration();
            config.DestinationBankRoutingNumber = "123456789";
            config.OriginatingCompanyId = "123456789";
            config.DestinationBankName = "PNC Bank";
            config.OriginatingCompanyName = "Microsoft Inc.";
            config.ReferenceCode = "Internal Use Only.";
            using (var nachaWriter = new ChoNACHAWriter(fileName, config))
            {
                using (var bw1 = nachaWriter.CreateBatch(200))
                {
                    foreach (DataRow dataRow in dt.Rows)
                    {
                        string transactionCode = "";
                        string routingNo = "";
                        string accountNo = "";
                        string amount = "";
                        string individualIDNumber = "";
                        string individualName = "";

                        string recordType = "";
                        string discreteDictionary = "";

                        recordType = dataRow["RecordType"].ToString();
                        transactionCode = dataRow["TransactionCode"].ToString();

                        string firstTwoFields = recordType + transactionCode;

                        routingNo = dataRow["RoutingTransitNumber"].ToString();
                        accountNo = dataRow["AccountNo"].ToString();
                        amount = dataRow["Amount"].ToString();
                        individualIDNumber = dataRow["IdentificationNo"].ToString();
                        individualName = dataRow["Company"].ToString();
                        discreteDictionary = dataRow["DiscData"].ToString();

                        amount = Regex.Match(amount, @"\d+.+\d").Value;

                        bw1.CreateDebitEntryDetail(Convert.ToInt32(firstTwoFields), routingNo, accountNo, Convert.ToDecimal(amount), individualIDNumber, individualName, discreteDictionary);
                    }

                }
            }

            return fileName;
        }
        #endregion

        #region Upload
        public static bool UploadFile(string filePath)
        {
            SessionOptions sessionOptions = new SessionOptions
            {
                Protocol = Protocol.Sftp,
                HostName = ApplicationSettings.SFTPHost,
                UserName = ApplicationSettings.SFTPUsername,
                Password = ApplicationSettings.SFTPUserPassword,
                GiveUpSecurityAndAcceptAnySshHostKey = true
            };


            TransferOptions transferOptions = new TransferOptions();
            transferOptions.TransferMode = TransferMode.Binary;
            transferOptions.OverwriteMode = OverwriteMode.Overwrite;


            TransferOperationResult transferResult = null;

            try
            {
                using (Session session = new Session())
                {
                    session.Open(sessionOptions);

                    //transferResult = session.PutFiles(filePath, $"/home/{ApplicationSettings.SFTPUsername}/{ApplicationSettings.SFTUploadDirPath}/", false, transferOptions);
                    transferResult = session.PutFiles(filePath, $"/{ApplicationSettings.SFTUploadDirPath}/", false, transferOptions);

                    transferResult.Check();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return transferResult.IsSuccess;
        } 
        #endregion

    }
}
