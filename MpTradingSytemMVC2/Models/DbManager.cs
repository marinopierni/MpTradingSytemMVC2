using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace MpTradingSytemMVC2.Models
{
    public class DbManager
    {
        private SqlConnection con;
        private string constring;
        private DataSet ds;

        public DataSet ExecSql(String query)
        {
            try
            {
                constring = System.Configuration.ConfigurationManager.ConnectionStrings["MPTradingSystemConnection"].ConnectionString;
                con = new SqlConnection(constring);

                ds = new DataSet();
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, con);
                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);
                con.Open();
                dataAdapter.Fill(ds, "table");

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                // if (con.State == System.Data.ConnectionState.Open)
                // con.Close();

            }
            return ds;
        }

        //Guida http://www.c-sharpcorner.com/code/2910/mvc-web-grid-using-dynamic-data-table.aspx     

        public CurrentReport getDataTableForGridView(String sql, DataTable mDt = null)
        {
            CurrentReport rp = new CurrentReport();
            DataTable dt = new DataTable();

            if (mDt == null)
            {
                dt = ExecSql(sql).Tables[0];
            }
            else
            {
                dt = mDt;
            }

            List<WebGridColumn> columns = new List<WebGridColumn>();
            foreach (DataColumn col in dt.Columns)
            {
                if (col.ColumnName.ToString().Contains("ID_OP") || col.ColumnName.ToString().Contains("DESCR") || col.ColumnName.ToString().Contains("CDC"))
                {
                    columns.Add(new WebGridColumn()
                    {
                        ColumnName = col.ColumnName,
                        Header = col.ColumnName,
                        //per rendere le colonne visualizzabili solo nel mobile
                        Style = "mobile"
                        //if (col.ColumnName.ToString().Contains("ID_OP"))
                        // {
                        //
                        //Style = "mobile"
                        //  }
                    });
                }
                else
                {
                    columns.Add(new WebGridColumn()
                    {
                        ColumnName = col.ColumnName,
                        Header = col.ColumnName,
                        Style = "no-mobile"
                    });
                }
            }


            columns.Add(new WebGridColumn()
            {
                Format = (item) =>
                {
                    return new HtmlString(string.Format("<a href=/Operazioni/Details?id={0}>Dettagli</a>", item.ID_OPERAZIONE));
                }
            });



            columns.Add(new WebGridColumn()
            {
                Format = (item) =>
                {
                    return new HtmlString(string.Format("<a href=/Operazioni/Edit?id={0}>Modifica</a>", item.ID_OPERAZIONE));
                }
            });



            columns.Add(new WebGridColumn()
            {
                Format = (item) =>
                {
                    return new HtmlString(string.Format("<a href=/Operazioni/Delete?id={0}>Elimina</a>", item.ID_OPERAZIONE));
                }
            });




            //columns.Add(new WebGridColumn()
            //{
            //    Format = (item) =>
            //    {
            //        return new HtmlString(string.Format("<a href= {0}>Edit</a>", url.Action("Edit", "Operazioni", new
            //        {
            //            Id = item.ID_OPERAZIONE
            //        })));
            //    }
            //});
            rp.oCampiTabella = columns;
            //Converting datatable to dynamic list     
            var dns = new List<dynamic>();
            rp.oDatiTabella = ConvertDtToList(dt);

            return rp;
        }

        public List<dynamic> ConvertDtToList(DataTable dt)
        {
            var data = new List<dynamic>();
            foreach (var item in dt.AsEnumerable())
            {
                // Expando objects are IDictionary<string, object>  
                IDictionary<string, object> dn = new ExpandoObject();

                foreach (var column in dt.Columns.Cast<DataColumn>())
                {
                    dn[column.ColumnName] = item[column];
                }

                data.Add(dn);
            }
            return data;
        }

    }
}