using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using Gios.Pdf;
using NoiseCalculator.Domain.DomainServices;
using NoiseCalculator.Domain.Entities;
using NoiseCalculator.Infrastructure.Pdf.Resources;


namespace NoiseCalculator.Infrastructure.Pdf
{
    public class PdfExporterGios : IPdfExporter
    {
        private readonly INoiseLevelService _noiseLevelService;

        public PdfExporterGios(INoiseLevelService noiseLevelService)
        {
            _noiseLevelService = noiseLevelService;
        }

        public Stream GenerateSelectedTasksPDF(IEnumerable<SelectedTask> selectedTasks)
        {
            int totalNoiseDosage = selectedTasks.Sum(x => x.Percentage);
            Color noiseLevelColor = GetColorForNoiseLevel(totalNoiseDosage);
            DataTable dataTable = GenerateDataTable(selectedTasks);
            

            // Starting instantiate the document.
		    // Remember to set the Docuement Format. In this case, we specify width and height.
            PdfDocument myPdfDocument = new PdfDocument(PdfDocumentFormat.A4_Horizontal);
		    
            // Now we create a Table with lines likt the number of selected tasks, 6 columns and 4 points of Padding.
            PdfTable myPdfTable = myPdfDocument.NewTable(new Font("Verdana", 12), selectedTasks.Count(), 6, 4);

		    // Importing datas from the datatables... (also column names for the headers!)
            myPdfTable.ImportDataTable(dataTable);

		    // Now we set our Graphic Design: Colors and Borders...
		    myPdfTable.HeadersRow.SetColors(Color.White, Color.Navy);
		    myPdfTable.SetColors(Color.Black, Color.White, Color.Gainsboro);
		    myPdfTable.SetBorders(Color.Black, 1, BorderType.CompleteGrid);

		    // With just one method we can set the proportional width of the columns.
		    // It's a "percentage like" assignment, but the sum can be different from 100.
            myPdfTable.SetColumnsWidth(new int[] { 90, 25, 45, 20, 20, 10 });

		    // Now we set some alignment... for the whole table and then, for a column.
		    myPdfTable.SetContentAlignment(ContentAlignment.MiddleCenter);
            foreach (PdfColumn pdfColumn in myPdfTable.Columns)
            {
                pdfColumn.SetContentAlignment(ContentAlignment.MiddleLeft);
            }
			
		    // Here we start the loop to generate the table...
		    while (!myPdfTable.AllTablePagesCreated)
		    {
			    // we create a new page to put the generation of the new TablePage:
			    PdfPage newPdfPage = myPdfDocument.NewPage();

                // LAKHA
                PdfArea pdfArea = new PdfArea(myPdfDocument, 48, 65, 750, 670);

			    PdfTablePage newPdfTablePage = myPdfTable.CreateTablePage(pdfArea);
				
			    // we also put a Label 
                PdfTextArea pta = new PdfTextArea(new Font("Verdana", 26, FontStyle.Bold), Color.Black
                    , new PdfArea(myPdfDocument, 48, 20, 595, 60), ContentAlignment.TopLeft, ReportResource.ReportTitle);

                PdfRectangle summaryBackground = new PdfArea(myPdfDocument, 635, 10, 165, 45).ToRectangle(noiseLevelColor, noiseLevelColor);
                
                // LAKHA - Total prosent
                PdfTextArea summary = new PdfTextArea(new Font("Verdana", 26, FontStyle.Bold), Color.Black
                    , new PdfArea(myPdfDocument, 650, 20, 595, 60), ContentAlignment.TopLeft, string.Format(ReportResource.TotalPercentageFormatString, totalNoiseDosage));

				
			    // nice thing: we can put all the objects in the following lines, so we can have
			    // a great control of layer sequence... 
			    newPdfPage.Add(newPdfTablePage);
			    newPdfPage.Add(pta);
                newPdfPage.Add(summaryBackground);
                newPdfPage.Add(summary);
			    
                // we save each generated page before start rendering the next.
			    newPdfPage.SaveToDocument();			
		    }
		    
            // Finally we save the docuement...
            Stream memoryStream = new MemoryStream();
            myPdfDocument.SaveToStream(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            
            return memoryStream;
        }

        private Color GetColorForNoiseLevel(int totalNoiseDosage)
        {
            NoiseLevelEnum noiseLevelEnum = _noiseLevelService.CalculateNoiseLevelEnum(totalNoiseDosage);

            switch (noiseLevelEnum)
            {
                case NoiseLevelEnum.Critical:
                    {
                        return Color.Red;
                    }
                case NoiseLevelEnum.Warning:
                    {
                        return Color.Yellow;
                    }
                default:
                    {
                        return Color.GreenYellow;
                    }
            }
        }

        private DataTable GenerateDataTable(IEnumerable<SelectedTask> selectedTasks)
        {
            const string percentageHeading = "%";

            string titleHeading = ReportResource.HeadingTitle;
            string roleHeading = ReportResource.HeadingRole;
            string noiseProtectionHeading = ReportResource.HeadingNoiseProtection;
            string noiseLevelHeading = ReportResource.HeadingNoiseLevel;
            string workTimeHeading = ReportResource.HeadingWorkTime;
            
            DataTable dt = new DataTable();
            dt.Columns.Add(titleHeading);
            dt.Columns.Add(roleHeading);
            dt.Columns.Add(noiseProtectionHeading);
            dt.Columns.Add(noiseLevelHeading);
            dt.Columns.Add(workTimeHeading);
            dt.Columns.Add(percentageHeading);

            foreach (SelectedTask selectedTask in selectedTasks)
            {
                DataRow dr = dt.NewRow();

                dr[titleHeading] = selectedTask.Title;
                dr[roleHeading] = selectedTask.Role;
                dr[noiseProtectionHeading] = selectedTask.NoiseProtection;
                dr[noiseLevelHeading] = string.Format("{0} dBA", selectedTask.NoiseLevel);
                dr[workTimeHeading] = string.Format(ReportResource.WorkTimeFormatString, selectedTask.Hours, selectedTask.Minutes);
                dr[percentageHeading] = selectedTask.Percentage;

                dt.Rows.Add(dr);
            }

            return dt;
        }
	}
}
