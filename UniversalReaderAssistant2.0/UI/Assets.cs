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
    class Assets
    {
        //Getter and Setters
        public int assetAssetID { get; set; }
        public string assetDescription { get; set;}
        public string assetComments { get; set; } 
      

        static string myconnstrng = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;

        /// <summary>
        /// Selects all information for a Specific Asset (for future use)
        /// </summary>
        /// <param name="asset"></param>
        /// <returns></returns>
        public DataTable Select(Assets asset)
        {
            ///Database Connection
            SqlConnection conn = new SqlConnection(myconnstrng);
            DataTable dt = new DataTable();
            try
            {
                //SQL Query to select from database
                string sql = "SELECT * FROM tbl_asset WHERE assetID = @assetID";
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@assetID", asset.assetAssetID);



                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                conn.Open();
                adapter.Fill(dt);

                //Testing datatable fill
                foreach (DataRow dataRow in dt.Rows)
                {
                    foreach (var item in dataRow.ItemArray)
                    {
                        Console.WriteLine(item);
                    }
                }

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
        public bool Insert(Assets asset)
        {


            //Creating Default return type and setting value to false
            bool isSuccess = false;

            //Connect to databse
            SqlConnection conn = new SqlConnection(myconnstrng);
            try
            {
                //Create SQL Query for inserting data
                string sql = "INSERT INTO tbl_asset (assetDescription, assetComments) VALUES (@assetDescription, @assetComments)";


                SqlCommand cmd = new SqlCommand(sql, conn);
                //Parameters for adding data to databse
                cmd.Parameters.AddWithValue("@assetDescription", asset.assetDescription);
                cmd.Parameters.AddWithValue("@assetComments", asset.assetComments);
             



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
        public bool Update(Assets asset)
        {
            //Create default 
            bool isSuccess = false;

            //Create sql connection
            SqlConnection conn = new SqlConnection(myconnstrng);

            try
            {
                //Update database values
                string sql = "UPDATE tbl_asset SET assetDescription=@assetDescription, assetComments=@assetComments WHERE assetID=@assetID";

                //SQL Command
                SqlCommand cmd = new SqlCommand(sql, conn);
                //Parameters to add value
                cmd.Parameters.AddWithValue("assetID", asset.assetAssetID);
                cmd.Parameters.AddWithValue("assetDescription", asset.assetDescription);
                cmd.Parameters.AddWithValue("assetComments", asset.assetComments);
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
        public bool Delete(Assets asset)
        {
            //Create default return value
            bool isSuccess = false;

            //SQL Connection
            SqlConnection conn = new SqlConnection(myconnstrng);

            try
            {
                //Delete from database
                string sql = "DELETE FROM tbl_asset WHERE assetID = @assetID";
                //Sql Command
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@assetID", asset.assetAssetID);
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
