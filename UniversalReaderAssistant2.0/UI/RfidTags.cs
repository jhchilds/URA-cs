using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ThingMagic.URA2
{
    class RfidTags
    {
        //Getter and Setters
        public int databaseID { get; set; }
        public string epcID { get; set; } //HEX: 56414f54000000000001 Binary:01010110010000010100111101010100000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000001
        public string rfidManufactureDate { get; set; }
        public string rfidInstallationDate { get; set; }
        public int rfidAssetID { get; set; }

        static string myconnstrng = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;


        public DataTable Select()
        {
            ///Database Connection
            SqlConnection conn = new SqlConnection(myconnstrng);
            DataTable dt = new DataTable();
            try
            {
                //SQL Query to select from database
                string sql = "SELECT * FROM tbl_rfid";
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                conn.Open();
                adapter.Fill(dt);
            }
            catch (Exception ex)
            {

            }
            finally
            {
                conn.Close();
            }
            return dt;
        }

        //Insert Data into database
        public bool Insert(RfidTags c)
        {


            //Creating Default return type and setting value to false
            bool isSuccess = false;

            //Connect to databse
            SqlConnection conn = new SqlConnection(myconnstrng);
            try
            {
                //Create SQL Query for inserting data
                string sql = "INSERT INTO tbl_rfid (epcID, rfidManufactureDate, rfidInstallationDate, assetID) VALUES (@epcID, @rfidManufactureDate, @rfidInstallationDate, @assetID)";


                SqlCommand cmd = new SqlCommand(sql, conn);
                //Parameters for adding data to databse
                cmd.Parameters.AddWithValue("@epcID", c.epcID);
                cmd.Parameters.AddWithValue("@rfidManufactureDate", c.rfidManufactureDate);
                cmd.Parameters.AddWithValue("@rfidInstallationDate", c.rfidInstallationDate);
                cmd.Parameters.AddWithValue("@assetID", c.rfidAssetID);



                //Connection Open Here
                conn.Open();
                int rows = cmd.ExecuteNonQuery();
                //If query is successful then value of rows will be > 0 else value will be 0
                if (rows > 0)
                {
                    isSuccess = true;
                }
                else
                {
                    isSuccess = false;
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                conn.Close();
            }
            return isSuccess;
        }

        //Update Method 
        public bool Update(RfidTags c)
        {
            //Create default 
            bool isSuccess = false;

            //Create sql connection
            SqlConnection conn = new SqlConnection(myconnstrng);

            try
            {
                //Update database values
                string sql = "UPDATE tbl_rfid SET epcID=@epcID, rfidManufactureDate=@rfidManufactureDate, rfidInstallationDate=@rfidInstallationDate, assetID=@assetID WHERE databaseID=@databaseID";

                //SQL Command
                SqlCommand cmd = new SqlCommand(sql, conn);
                //Parameters to add value
                cmd.Parameters.AddWithValue("epcID", c.epcID);
                cmd.Parameters.AddWithValue("rfidManufactureDate", c.rfidManufactureDate);
                cmd.Parameters.AddWithValue("rfidInstallationDate", c.rfidInstallationDate);
                cmd.Parameters.AddWithValue("assetID", c.rfidAssetID);
                cmd.Parameters.AddWithValue("databaseID", c.databaseID);
                //Open Connection
                conn.Open();

                int rows = cmd.ExecuteNonQuery();
                //If rows greater than zero than successful query

                if (rows > 0)
                {
                    isSuccess = true;
                }
                else
                {
                    isSuccess = false;
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                conn.Close();
            }
            return isSuccess;
        }

        /// <summary>
        /// Delete data from the database
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public bool Delete(RfidTags c)
        {
            //Create default return value
            bool isSuccess = false;

            //SQL Connection
            SqlConnection conn = new SqlConnection(myconnstrng);

            try
            {
                //Delete from database
                string sql = "DELETE FROM tbl_rfid WHERE databaseID = @databaseID";
                //Sql Command
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@databaseID", c.databaseID);
                //Open Connection to server
                conn.Open();
                int rows = cmd.ExecuteNonQuery();
                //If query is successful then rows > 0
                if (rows > 0)
                {
                    isSuccess = true;
                }
                else
                {
                    isSuccess = false;
                }

            }
            catch (Exception ex)
            {

            }
            finally
            {
                conn.Close();
            }

            return isSuccess;
        }

    }
}
