using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Office.Core;
using Microsoft.Office.Interop;

using System.Reflection;
using Word = Microsoft.Office.Interop.Word;
using Excel = Microsoft.Office.Interop.Excel;

namespace computerdepot
{
    public class WordReport
    {
        protected ErrorReporter errs = null;
        protected Report m_report = null;
        protected List<Report> m_reports = null;

        public WordReport(List<Report> reports)
        {
            m_reports = reports;
            errs = new ErrorReporter();
        }

        public int DemoWordAndExcel()
        {
            Word.Application word = null;
            Word.Document doc = null;
            Word.Chart wdChart = null;
            Excel.Workbook dataWorkbook = null;

            try
            {
                object missing = Missing.Value;
                object oMissing = System.Reflection.Missing.Value;

                word = new Word.Application();
                word.Visible = true;

                doc = word.Documents.Add(ref oMissing, ref oMissing, ref oMissing, ref oMissing);

                wdChart = doc.InlineShapes.AddChart(Microsoft.Office.Core.XlChartType.xl3DColumn, ref missing).Chart;

                var chartData = wdChart.ChartData;

                dataWorkbook = (Excel.Workbook)chartData.Workbook;
                Excel.Worksheet dataSheet = (Excel.Worksheet)dataWorkbook.Worksheets[1];
                Excel.ListObject tbl1 = dataSheet.ListObjects["Table1"];

                string row_col1 = "A";
                string row_col2 = "B";
                int start_row = 2;

                string end_range = "B" + m_reports.Count.ToString();

                Excel.Range tRange = dataSheet.Cells.get_Range("A1", end_range);
                tbl1.Resize(tRange);

                for (int index = 0; index < m_reports.Count; index++)
                {
                    string range1 = row_col1 + start_row.ToString();
                    string range2 = row_col2 + start_row.ToString();

                    ((Excel.Range)dataSheet.Cells.get_Range(range1, missing)).FormulaR1C1 = m_reports[index].Description;
                    ((Excel.Range)dataSheet.Cells.get_Range(range2, missing)).FormulaR1C1 = m_reports[index].ID.ToString();

                    start_row++;
                }

                
                wdChart.ChartTitle.Font.Italic = true;
                wdChart.ChartTitle.Font.Size = 18;
                wdChart.ChartTitle.Font.Color = System.Drawing.Color.Black.ToArgb();
                wdChart.ChartTitle.Text = "2007 Sales";
                wdChart.ChartTitle.Format.Line.Visible = Microsoft.Office.Core.MsoTriState.msoTrue;
                wdChart.ChartTitle.Format.Line.ForeColor.RGB = System.Drawing.Color.Black.ToArgb();

                wdChart.ApplyDataLabels(Word.XlDataLabelsType.xlDataLabelsShowLabel, missing, missing, missing, missing, missing, missing, missing, missing, missing);

                dataWorkbook.Application.Quit();
            }
            catch (Exception except)
            {
                errs.ReportException(except);
            }
            finally
            {
                string filename = System.Environment.GetEnvironmentVariable("USERPROFILE") + "\\Desktop";
                filename += "\\OrderReport.docx";

                try
                {
                    doc.SaveAs(filename);
                    doc.Close();
                    //doc = null;

                    //((_Application)wordApplication)
                    word.Quit();
                    word = null;
                }
                catch (Exception)
                {

                }
                dataWorkbook = null;
            }
            return 0;
        }
    }
}
