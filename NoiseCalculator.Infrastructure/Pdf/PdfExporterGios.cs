using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using Gios.Pdf;
using NoiseCalculator.Domain.Entities;

namespace NoiseCalculator.Infrastructure.Pdf
{
    public class PdfExporterGios : IPdfExporter
    {
        public Stream GenerateSelectedTasksPDF(IEnumerable<SelectedTask> selectedTasks)
        {
            DataTable dataTable = GenerateDataTable(selectedTasks);

            // Starting instantiate the document.
		    // Remember to set the Docuement Format. In this case, we specify width and height.
            PdfDocument myPdfDocument = new PdfDocument(PdfDocumentFormat.A4_Horizontal);
		    
            // Now we create a Table of 100 lines, 6 columns and 4 points of Padding.
            PdfTable myPdfTable = myPdfDocument.NewTable(new Font("Verdana", 12), selectedTasks.Count(), 6, 4);

		    // Importing datas from the datatables... (also column names for the headers!)
            myPdfTable.ImportDataTable(dataTable);

		    // Sets the format for correct date-time representation
		    //myPdfTable.Columns[2].SetContentFormat("{0:dd/MM/yyyy}");

		    // Now we set our Graphic Design: Colors and Borders...
		    myPdfTable.HeadersRow.SetColors(Color.White, Color.Navy);
		    myPdfTable.SetColors(Color.Black, Color.White, Color.Gainsboro);
		    myPdfTable.SetBorders(Color.Black, 1, BorderType.CompleteGrid);

		    // With just one method we can set the proportional width of the columns.
		    // It's a "percentage like" assignment, but the sum can be different from 100.
            myPdfTable.SetColumnsWidth(new int[] { 100, 20, 40, 20, 20, 10 });

		    // Now we set some alignment... for the whole table and then, for a column.
		    myPdfTable.SetContentAlignment(ContentAlignment.MiddleCenter);
		    myPdfTable.Columns[1].SetContentAlignment(ContentAlignment.MiddleLeft);
			
		    // Here we start the loop to generate the table...
		    while (!myPdfTable.AllTablePagesCreated)
		    {
			    // we create a new page to put the generation of the new TablePage:
			    PdfPage newPdfPage = myPdfDocument.NewPage();

                // LAKHA
                PdfArea pdfArea = new PdfArea(myPdfDocument, 48, 60, 750, 670);

			    PdfTablePage newPdfTablePage = myPdfTable.CreateTablePage(pdfArea);
				
			    // we also put a Label 
                PdfTextArea pta = new PdfTextArea(new Font("Verdana", 26, FontStyle.Bold), Color.Red
                    , new PdfArea(myPdfDocument, 48, 20, 595, 60), ContentAlignment.TopLeft, "Mine Oppgaver");
				
			    // nice thing: we can put all the objects in the following lines, so we can have
			    // a great control of layer sequence... 
			    newPdfPage.Add(newPdfTablePage);
			    newPdfPage.Add(pta);
			    
                // we save each generated page before start rendering the next.
			    newPdfPage.SaveToDocument();			
		    }
		    
            // Finally we save the docuement...
            Stream memoryStream = new MemoryStream();
            myPdfDocument.SaveToStream(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            
            return memoryStream;
        }



        private static DataTable GenerateDataTable(IEnumerable<SelectedTask> selectedTasks)
        {
            const string titleHeading = "Title";
            const string roleHeading = "Role";
            const string noiseProtectionHeading = "Noise Protection";
            const string noiseLevelHeading = "Noise Level";
            const string workTimeHeading = "Work Time";
            const string percentageHeading = "%";
            const string workTimeFormatString = "{0} t {1} min";
            
            DataTable dt = new DataTable();
            dt.Columns.Add(titleHeading);
            dt.Columns.Add(roleHeading);
            dt.Columns.Add(noiseProtectionHeading);
            dt.Columns.Add(noiseLevelHeading, typeof(int));
            dt.Columns.Add(workTimeHeading);
            dt.Columns.Add(percentageHeading);

            //for (int x = 0; x <= 2000; x++)
            foreach (SelectedTask selectedTask in selectedTasks)
            {
                DataRow dr = dt.NewRow();

                dr[titleHeading] = selectedTask.Title;
                dr[roleHeading] = selectedTask.Role;
                dr[noiseProtectionHeading] = selectedTask.NoiseProtection;
                dr[noiseLevelHeading] = selectedTask.NoiseLevel;
                dr[workTimeHeading] = string.Format(workTimeFormatString, selectedTask.Hours, selectedTask.Minutes);
                dr[percentageHeading] = selectedTask.Percentage;

                dt.Rows.Add(dr);
            }


            return dt;
        }



        static Random r = new Random();

        static string GetAName
	    {
		    get
		    {
			    ArrayList al = new ArrayList();
			    al.Add("John Doe");
			    al.Add("Perry White");
			    al.Add("Jackson");
			    al.Add("Henry James Junior Ford");
			    al.Add("Bill Norton");
			    al.Add("Michal Johnathan Stewart ");
			    al.Add("George Wilson");
			    al.Add("Steven Edwards");
			    
                return al[r.Next(0,al.Count)].ToString();
		    }
	    }

	    static DataTable Table
	    {
		    get
		    {
			    DataTable dt = new DataTable();
			    dt.Columns.Add("ID");
			    dt.Columns.Add("Name");
			    dt.Columns.Add("Date of Birth",typeof(DateTime));
			    dt.Columns.Add("Phone Number");
			    dt.Columns.Add("Mobile Phone");
			    dt.Columns.Add("Password");
				
			    for (int x=0; x <= 2000; x++)
			    {
				    DataRow dr = dt.NewRow();

				    dr["ID"] = x.ToString();
				    dr["Name"] = GetAName;
				    dr["Date of Birth"] = new DateTime(r.Next(1940, 1984), r.Next(1, 12), r.Next(1, 28));
				    dr["Phone Number"]="555-"+r.Next(100000, 999999).ToString();
				    dr["Mobile Phone"]="444-"+r.Next(100000, 999999).ToString();
				    dr["Password"]=r.Next(10000000, 99999999).ToString();
				    
                    dt.Rows.Add(dr);
			    }

			    return dt;
		    }
        }

	}
}
