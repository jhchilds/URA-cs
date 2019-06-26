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
        public int id { get; set; }
        public string epc { get; set; } //HEX: 56414f54000000000000000000000001 Binary:01010110010000010100111101010100000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000001
        public string manufacture_date { get; set; }
        public string installation_date { get; set; }
        public int asset_id { get; set; }
        public string comments { get; set; }

        static string myconnstrng = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;

        /// <summary>
        /// Selects all information from Assets and Rfid table basded on the epc recovered from Reader
        /// </summary>
        /// <param name="rfid"></param>
        /// <returns></returns>
        public DataTable Select(RfidTags rfid)
        {
            ///Database Connection
            SqlConnection conn = new SqlConnection(myconnstrng);
            DataTable dt = new DataTable();
            try
            {
                //SQL Query to select from database
                string sql = "SELECT rfid.id, rfid.epc, rfid.manufacture_date," +
                    " rfid.installation_date, rfid.asset_id, rfid.created_at, rfid.comments," +
                    " asset.id, asset.lane_direction, asset.position_code," +
                    " asset.route_suffix, asset.marker, asset.city," +
                    " asset.county, asset.district, asset.streetname," +
                    " asset.mutcd_code, asset.retired, asset.replaced," +
                    " asset.sign_age, asset.twn_tid, asset.twn_mi," +
                    " asset.qc_flag, asset.min_twn_fm, asset.max_twn_tm," +
                    " asset.sr_sid, asset.sign_height," +
                    " asset.sign_width FROM asset INNER JOIN rfid ON asset.id = rfid.asset_id WHERE epc = @epc";
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@epc", rfid.epc);

               

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                conn.Open();
                adapter.Fill(dt);

                //Testing datatable fill
                foreach (DataRow dataRow in dt.Rows)
                {
                    foreach(var item in dataRow.ItemArray)
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
        public bool Insert(RfidTags c)
        {


            //Creating Default return type and setting value to false
            bool isSuccess = false;

            //Connect to databse
            SqlConnection conn = new SqlConnection(myconnstrng);
            try
            {
                //Create SQL Query for inserting data
                string sql = "INSERT INTO rfid (epc, manufacture_date, installation_date, asset_id, comments) VALUES (@epc, @manufacture_date, @installation_date, @asset_id, @comments)";


                SqlCommand cmd = new SqlCommand(sql, conn);
                //Parameters for adding data to databse
                cmd.Parameters.AddWithValue("@epc", c.epc);
                cmd.Parameters.AddWithValue("@manufacture_date", c.manufacture_date);
                cmd.Parameters.AddWithValue("@installation_date", c.installation_date);
                cmd.Parameters.AddWithValue("@asset_id", c.asset_id);
                cmd.Parameters.AddWithValue("@comments", c.comments);



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
                string sql = "UPDATE rfid SET epc=@epc, manufacture_date=@manufacture_date," +
                    " installation_date=@installation_date, asset_id=@asset_id," +
                    " comments=@comments WHERE id=@id";

                //SQL Command
                SqlCommand cmd = new SqlCommand(sql, conn);
                //Parameters to add value
                cmd.Parameters.AddWithValue("epc", c.epc);
                cmd.Parameters.AddWithValue("manufacture_date", c.manufacture_date);
                cmd.Parameters.AddWithValue("installation_date", c.installation_date);
                cmd.Parameters.AddWithValue("asset_id", c.asset_id);
                cmd.Parameters.AddWithValue("id", c.id);
                cmd.Parameters.AddWithValue("comments", c.comments);
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
                string sql = "DELETE FROM rfid WHERE id = @id";
                //Sql Command
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", c.id);
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
