using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace computerdepot
{
    public class ErrorReporter
    {
        public int ReportSqlException(System.Data.SqlClient.SqlException ex)
        {
            string error_message = string.Empty;
            StringBuilder sb = new StringBuilder();

            try
            {
                sb.AppendLine("-----------------------------------------------------------");
                sb.AppendLine("                      ERROR                                ");
                sb.AppendLine("-----------------------------------------------------------");
                sb.AppendLine(ex.Message);
                StackTrace st = new StackTrace(ex, true);

                foreach (System.Diagnostics.StackFrame frame in st.GetFrames())
                {
                    sb.AppendLine(frame.GetFileName() + ": " +
                    frame.GetMethod() + ": " + frame.GetFileLineNumber());
                }
                sb.AppendLine("-----------------------------------------------------------");
                error_message = sb.ToString();
                //error_message = String.Format("Exception Occurred : Line No: {0}", st.GetFrame(0).GetFileLineNumber());
                WriteError(error_message);
            }
            catch (Exception except)
            {
                System.Diagnostics.Debug.WriteLine(except.Message);
            }

            return 0;
        }

        public int ReportException(Exception ex)
        {
            string error_message = string.Empty;
            StringBuilder sb = new StringBuilder(); 

            try
            {
                sb.AppendLine("-----------------------------------------------------------");
                sb.AppendLine("                      ERROR                                ");
                sb.AppendLine("-----------------------------------------------------------");
                sb.AppendLine(ex.Message);
                StackTrace st = new StackTrace(ex, true);
           
                foreach (System.Diagnostics.StackFrame frame in st.GetFrames())
                {
                    sb.AppendLine(frame.GetFileName() + ": " +
                    frame.GetMethod() + ": " + frame.GetFileLineNumber());
                }
                sb.AppendLine("-----------------------------------------------------------");
                error_message = sb.ToString(); 
                //error_message = String.Format("Exception Occurred : Line No: {0}", st.GetFrame(0).GetFileLineNumber());
                WriteError(error_message );
            }
            catch (Exception except)
            {
                System.Diagnostics.Debug.WriteLine(except.Message);   
            }
            
            //Label1.Text = String.Format("Exception Occurred : Line No: {0}", st.GetFrame(0).GetFileLineNumber())
            //Label2.Text = ex.InnerException.Message
            //Label2.Text = ex.Message
            //Label2.Text = ex.StackTrace.ToString()
            return 0;
        }

        //Declare the winDir variable as follows:
        protected void WriteError(string errorMessage)
        {
            try
            {
                string file_name = System.Environment.GetEnvironmentVariable("USERPROFILE") + "\\Desktop";
                
                file_name += "\\Errors.txt";
                StreamWriter writer = new StreamWriter(file_name,true);
                writer.WriteLine(errorMessage);
                writer.Close();
            }
            catch (Exception except)
            {
                System.Diagnostics.Debug.WriteLine(except.Message);   
            }
        }
    }
}


					
