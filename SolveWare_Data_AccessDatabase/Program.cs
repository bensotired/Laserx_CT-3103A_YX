using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolveWare_Data_AccessDatabase.TestDBUI;
using System.Windows.Forms;
 
using System.Data.OleDb;
using System.IO;
using System.Data;

namespace SolveWare_Data_AccessDatabase
{
   

    public class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            FormDataBase debugForm = new FormDataBase();

            Application.Run(debugForm);
            Environment.Exit(0);


            #region
            //ADOX.Catalog catalog = new Catalog();
            //catalog.Create(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=D:\test.mdb;Jet OLEDB:Engine Type = 5");
            //KU();
            //string conStr = @"Provider = Microsoft.Jet.OLEDB.4.0; Data Source = D:\test.mdb";
            //OleDbConnection conn = new OleDbConnection(conStr);
            //string dbstr = "CREATE TABLE NewTable(ID AUTOINCREMENT PRIMARY KEY,P_SN varchar(255) ,P_Name varchar(255),P_Value varchar(255),Strattime DATETIME,Endtime DATETIME)";
            //OleDbCommand oleDbCom = new OleDbCommand(dbstr, conn);
            //conn.Open();
            //oleDbCom.ExecuteNonQuery();
            //string sql = " INSERT INTO NewTable  VALUES ( " + 11+ ", "+22 + ", "+ ")";
            //OleDbCommand ole = new OleDbCommand(sql, conn);
            //ole.ExecuteNonQuery();
            //conn.Close();
            //Table();
            //string conStr = @"Provider = Microsoft.Jet.OLEDB.4.0; Data Source = D:\test.mdb";
            //OleDbConnection conn = new OleDbConnection(conStr);
            //conn.Open();
            //string sql = " INSERT INTO NewTable (P_SN  ,P_Name ,P_Value  ) VALUES ( " + 11 + ", " + 22 + ", " + 33 + ")";
            //OleDbCommand ole = new OleDbCommand(sql, conn);
            //ole.ExecuteNonQuery();
            //conn.Close();

            //string conSt = @"Provider = Microsoft.Jet.OLEDB.4.0; Data Source = D:\test.mdb";
            //OleDbConnection conn = new OleDbConnection(conSt);
            //conn.Open();
            //string sqlAlter = "ALTER TABLE NewTable ADD COLUMN [abc123445] varchar(255)";
            //OleDbCommand oleDbAlter = new OleDbCommand(sqlAlter, conn);

            //oleDbAlter.ExecuteNonQuery();
            //string InSql = $" INSERT INTO NewTable (Batch ,FixtureID ,Station ,Operator ,PartNumber ,SerialNumber ,CarrierID ,WorkOrder ,FailureCode ,Posittion ,IsActive ) VALUES (" + 1 + "," + 1 + "," + 1 + "," + 1 + "," + 1 + "," + 1 + "," + 1 + "," + 1 + "," + 1 + "," + 1 + "," + 1 + ")";
            //OleDbCommand oleDbInsert = new OleDbCommand(InSql, conn);
            //oleDbInsert.ExecuteNonQuery();
            //conn.Close();
            #endregion
        }
        //static void KU()
        //{
        //    ADOX.Catalog catalog = new Catalog();
        //    string path = @"D:\test.mdb";
        //    if (!File.Exists(path))
        //    {
        //        catalog.Create(@"Provider = Microsoft.Jet.OLEDB.4.0;Data Source=D:\test.mdb;Jet OLEDB:Engine Type = 5");
                
        //    }
        //}
        static void Table()
        {
            string conStr = @"Provider = Microsoft.Jet.OLEDB.4.0; Data Source = D:\test.mdb";
            OleDbConnection conn = new OleDbConnection(conStr);
            conn.Open();
            bool bExist = false;
            using (DataTable dt = conn.GetSchema("Tables"))
            {
                foreach (DataRow dr in dt.Rows)
                {
                    if (string.Equals("NewTable", dr[2].ToString()))
                    {
                        bExist = true;
                        break;
                    }
                }
            }
            if (!bExist)
            {
                string dbstr = "CREATE TABLE NewTable(Batch varchar(255) ,FixtureID varchar(255),Station varchar(255),Operator varchar(255),PartNumber varchar(255),SerialNumber varchar(255),CarrierID varchar(255),WorkOrder varchar(255),FailureCode varchar(255),Posittion varchar(255),IsActive bit)";
                OleDbCommand oleDbCom = new OleDbCommand(dbstr, conn);
              
                oleDbCom.ExecuteNonQuery();
                
            }
            conn.Close();
        }
    }
}
