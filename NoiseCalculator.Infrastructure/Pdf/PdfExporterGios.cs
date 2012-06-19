using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using Gios.Pdf;
using NoiseCalculator.Domain.DomainServices;
using NoiseCalculator.Domain.Entities;
using NoiseCalculator.Domain.Enums;
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

        public Stream GenerateSelectedTasksPDF(IEnumerable<SelectedTask> selectedTasks, ReportInfo reportInfo)
        {
            int countSelectedTasks = selectedTasks.Count();
            int totalNoiseDosage = selectedTasks.Sum(x => x.Percentage);
            NoiseLevelEnum noiseLevelEnum = _noiseLevelService.CalculateNoiseLevelEnum(totalNoiseDosage);
            Color noiseLevelColor = GetColorForNoiseLevel(noiseLevelEnum);
            DataTable dataTable = GenerateDataTable(selectedTasks);
            

            // Starting instantiate the document.
		    // Remember to set the Docuement Format. In this case, we specify width and height.
            PdfDocument myPdfDocument = new PdfDocument(PdfDocumentFormat.A4_Horizontal);
		    
            // Now we create a Table with lines likt the number of selected tasks, 6 columns and 4 points of Padding.
            PdfTable myPdfTable = myPdfDocument.NewTable(new Font("Verdana", 12), countSelectedTasks, 6, 4);

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
                PdfArea pdfArea = new PdfArea(myPdfDocument, 48, 95, 750, 670);
			    PdfTablePage taskTable = myPdfTable.CreateTablePage(pdfArea);
				
			    // we also put a Label 
                PdfTextArea reportTitle = new PdfTextArea(new Font("Verdana", 26, FontStyle.Bold), Color.Black
                    , new PdfArea(myPdfDocument, 48, 20, 595, 60), ContentAlignment.TopLeft, ReportResource.ReportTitle);
                
                // LAKHA - Status
                PdfTextArea statusText = new PdfTextArea(new Font("Verdana", 14, FontStyle.Bold), Color.Black
                    , new PdfArea(myPdfDocument, 48, taskTable.CellArea(taskTable.LastRow, 6 - 1).BottomRightCornerY + 10, 595, 60), ContentAlignment.TopLeft,
                    _noiseLevelService.GetNoiseLevelStatusText(noiseLevelEnum));
   
                // LAKHA - Total prosent
                PdfRectangle summaryBackground = new PdfArea(myPdfDocument, 635, taskTable.CellArea(taskTable.LastRow, 6-1).BottomRightCornerY + 10, 165, 45).ToRectangle(noiseLevelColor, noiseLevelColor);
                PdfTextArea summary = new PdfTextArea(new Font("Verdana", 26, FontStyle.Bold), Color.Black
                    , new PdfArea(myPdfDocument, 640, taskTable.CellArea(taskTable.LastRow, 6-1).BottomRightCornerY + 20, 595, 60), ContentAlignment.TopLeft,
                    string.Format(ReportResource.TotalPercentageFormatString, totalNoiseDosage));
				
			    // nice thing: we can put all the objects in the following lines, so we can have
			    // a great control of layer sequence... 
			    newPdfPage.Add(taskTable);
			    newPdfPage.Add(reportTitle);
                newPdfPage.Add(statusText);
                newPdfPage.Add(summaryBackground);
                newPdfPage.Add(summary);

                // Info from report input window
                PdfTextArea reportPlant = new PdfTextArea(new Font("Verdana", 12, FontStyle.Bold), Color.Black
                    , new PdfArea(myPdfDocument, 48, 50, 595, 60), ContentAlignment.TopLeft, string.Format(ReportResource.PlantFormatString, reportInfo.Plant));
                PdfTextArea reportCreatedBy = new PdfTextArea(new Font("Verdana", 12, FontStyle.Bold), Color.Black
                    , new PdfArea(myPdfDocument, 650, 50, 595, 60), ContentAlignment.TopLeft, string.Format(ReportResource.UserFormatString, reportInfo.CreatedBy));

                PdfTextArea reportProfession = new PdfTextArea(new Font("Verdana", 12, FontStyle.Bold), Color.Black
                    , new PdfArea(myPdfDocument, 48, 65, 595, 60), ContentAlignment.TopLeft, string.Format(ReportResource.ProfessionFormatString, reportInfo.Group));
                PdfTextArea reportDate = new PdfTextArea(new Font("Verdana", 12, FontStyle.Bold), Color.Black
                    , new PdfArea(myPdfDocument, 650, 65, 595, 60), ContentAlignment.TopLeft, string.Format(ReportResource.DateFormatString, (reportInfo.Date.HasValue) ? reportInfo.Date.Value.ToString("dd.MM.yyyy") : string.Empty));

                PdfTextArea reportComment = new PdfTextArea(new Font("Verdana", 12, FontStyle.Bold), Color.Black
                    , new PdfArea(myPdfDocument, 48, 80, 700, 60), ContentAlignment.TopLeft, string.Format(ReportResource.CommentFormatString, reportInfo.Comment));

                newPdfPage.Add(reportPlant);
                newPdfPage.Add(reportCreatedBy);
                newPdfPage.Add(reportProfession);
                newPdfPage.Add(reportDate);
                newPdfPage.Add(reportComment);
                
                
                // LAKHA - Add footnotes...
		        const int widthOfFootnote = 750;
                Font footnoteFont = new Font("Verdana", 9, FontStyle.Regular);
                double posY = statusText.PdfArea.BottomRightCornerY + 3;

                foreach (string footNoteText in reportInfo.Footnotes)
                {
                    int heightOfFootnote = 10;

                    if(footNoteText.Length > 380)
                    {
                        heightOfFootnote = heightOfFootnote*3;
                    }
                    else if(footNoteText.Length > 160)
                    {
                        heightOfFootnote = heightOfFootnote * 2;
                    }

                    PdfArea pdfAreaForText = new PdfArea(myPdfDocument, 48, posY, widthOfFootnote, heightOfFootnote);
                    PdfTextArea footNote = new PdfTextArea(footnoteFont, Color.Black, pdfAreaForText, ContentAlignment.TopLeft, string.Format("* {0}", footNoteText));
                    newPdfPage.Add(footNote);

                    posY = footNote.PdfArea.BottomRightCornerY + 2;
                }
			    
                // we save each generated page before start rendering the next.
			    newPdfPage.SaveToDocument();			
		    }
		    
            // Finally we save the docuement...
            Stream memoryStream = new MemoryStream();
            myPdfDocument.SaveToStream(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);

            return memoryStream;
        }

        private Color GetColorForNoiseLevel(NoiseLevelEnum noiseLevelEnum)
        {
            switch (noiseLevelEnum)
            {
                case NoiseLevelEnum.Critical:
                    {
                        return Color.Red;
                    }
                case NoiseLevelEnum.DangerOfWorkRelatedInjury:
                    {
                        return Color.Red;
                    }
                case NoiseLevelEnum.Warning:
                    {
                        return Color.Yellow;
                    }
                case NoiseLevelEnum.MaximumAllowedDosage:
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
                /*
                if (selectedTask.HelicopterTaskId > 0)
                {
                    NoiseLevel = TaskResources.SelectedTaskNoiseLevelNotApplicable;
                }
                else
                {
                    if (selectedTask.IsNoiseMeassured)
                    {
                        NoiseLevel = string.Format("{0} dBA {1}", selectedTask.NoiseLevel, TaskResources.SelectedTaskNoiseMeasured);
                    }
                    else
                    {
                        NoiseLevel = string.Format("{0} dBA", selectedTask.NoiseLevel.ToString());
                    }
                }
                */
                
                DataRow dr = dt.NewRow();

                dr[titleHeading] = selectedTask.Title;
                dr[roleHeading] = selectedTask.Role;
                dr[noiseProtectionHeading] = selectedTask.NoiseProtection;

                if (selectedTask.HelicopterTaskId > 0)
                {
                    dr[noiseLevelHeading] = ReportResource.SelectedTaskNoiseLevelNotApplicable;
                }
                else
                {
                    if (selectedTask.IsNoiseMeassured)
                    {
                        dr[noiseLevelHeading] = string.Format("{0} dBA {1}", selectedTask.NoiseLevel, ReportResource.NoiseMeassured);
                    }
                    else
                    {
                        dr[noiseLevelHeading] = string.Format("{0} dBA", selectedTask.NoiseLevel);
                    }
                }
                
                dr[workTimeHeading] = string.Format(ReportResource.WorkTimeFormatString, selectedTask.Hours, selectedTask.Minutes);
                dr[percentageHeading] = selectedTask.Percentage;

                dt.Rows.Add(dr);
            }

            return dt;
        }
    }
}
