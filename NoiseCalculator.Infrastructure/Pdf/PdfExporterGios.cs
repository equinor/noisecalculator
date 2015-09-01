using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Windows.Documents;
using FluentNHibernate.Cfg.Db;
using Gios.Pdf;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using NHibernate.Hql.Ast.ANTLR;
using NoiseCalculator.Domain.DomainServices;
using NoiseCalculator.Domain.Entities;
using NoiseCalculator.Domain.Enums;
using NoiseCalculator.Infrastructure.Pdf.Resources;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using BorderType = Gios.Pdf.BorderType;
using Color = System.Windows.Media.Color;
using Font = System.Drawing.Font;
using PdfDocument = Gios.Pdf.PdfDocument;
using PdfPage = Gios.Pdf.PdfPage;
using PdfRectangle = Gios.Pdf.PdfRectangle;


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
            myPdfTable.HeadersRow.SetColors(Color.FromRgb(255, 255, 255), Color.FromRgb(0, 0, 255));
            myPdfTable.SetColors(Color.FromRgb(0, 0, 0), Color.FromRgb(255, 255, 255), Color.FromRgb(0, 255, 255));
            myPdfTable.SetBorders(Color.FromRgb(0, 0, 0), 1, BorderType.CompleteGrid);

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
                PdfTextArea reportTitle = new PdfTextArea(new Font("Verdana", 26, FontStyle.Bold), Color.FromRgb(0, 0, 0)
                    , new PdfArea(myPdfDocument, 48, 20, 595, 60), ContentAlignment.TopLeft, ReportResource.ReportTitle);

                // LAKHA - Status
                PdfTextArea statusText = new PdfTextArea(new Font("Verdana", 14, FontStyle.Bold), Color.FromRgb(0, 0, 0)
                    , new PdfArea(myPdfDocument, 48, taskTable.CellArea(taskTable.LastRow, 6 - 1).BottomRightCornerY + 10, 595, 60), ContentAlignment.TopLeft,
                    _noiseLevelService.GetNoiseLevelStatusText(noiseLevelEnum));

                // LAKHA - Total prosent
                PdfRectangle summaryBackground = new PdfArea(myPdfDocument, 635, taskTable.CellArea(taskTable.LastRow, 6 - 1).BottomRightCornerY + 10, 165, 45).ToRectangle(noiseLevelColor, noiseLevelColor);
                PdfTextArea summary = new PdfTextArea(new Font("Verdana", 26, FontStyle.Bold), Color.FromRgb(0, 0, 0)
                    , new PdfArea(myPdfDocument, 640, taskTable.CellArea(taskTable.LastRow, 6 - 1).BottomRightCornerY + 20, 595, 60), ContentAlignment.TopLeft,
                    string.Format(ReportResource.TotalPercentageFormatString, totalNoiseDosage));

                // nice thing: we can put all the objects in the following lines, so we can have
                // a great control of layer sequence... 
                newPdfPage.Add(taskTable);
                newPdfPage.Add(reportTitle);
                newPdfPage.Add(statusText);
                newPdfPage.Add(summaryBackground);
                newPdfPage.Add(summary);

                // Info from report input window
                PdfTextArea reportPlant = new PdfTextArea(new Font("Verdana", 12, FontStyle.Bold), Color.FromRgb(0, 0, 0)
                    , new PdfArea(myPdfDocument, 48, 50, 595, 60), ContentAlignment.TopLeft, string.Format(ReportResource.PlantFormatString, reportInfo.Plant));
                PdfTextArea reportCreatedBy = new PdfTextArea(new Font("Verdana", 12, FontStyle.Bold), Color.FromRgb(0, 0, 0)
                    , new PdfArea(myPdfDocument, 650, 50, 595, 60), ContentAlignment.TopLeft, string.Format(ReportResource.UserFormatString, reportInfo.CreatedBy));

                PdfTextArea reportProfession = new PdfTextArea(new Font("Verdana", 12, FontStyle.Bold), Color.FromRgb(0, 0, 0)
                    , new PdfArea(myPdfDocument, 48, 65, 595, 60), ContentAlignment.TopLeft, string.Format(ReportResource.ProfessionFormatString, reportInfo.Group));
                PdfTextArea reportDate = new PdfTextArea(new Font("Verdana", 12, FontStyle.Bold), Color.FromRgb(0, 0, 0)
                    , new PdfArea(myPdfDocument, 650, 65, 595, 60), ContentAlignment.TopLeft, string.Format(ReportResource.DateFormatString, (reportInfo.Date.HasValue) ? reportInfo.Date.Value.ToString("dd.MM.yyyy") : string.Empty));

                PdfTextArea reportComment = new PdfTextArea(new Font("Verdana", 12, FontStyle.Bold), Color.FromRgb(0, 0, 0)
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

                    if (footNoteText.Length > 380)
                    {
                        heightOfFootnote = heightOfFootnote * 3;
                    }
                    else if (footNoteText.Length > 160)
                    {
                        heightOfFootnote = heightOfFootnote * 2;
                    }

                    PdfArea pdfAreaForText = new PdfArea(myPdfDocument, 48, posY, widthOfFootnote, heightOfFootnote);
                    PdfTextArea footNote = new PdfTextArea(footnoteFont, Color.FromRgb(0, 0, 0), pdfAreaForText, ContentAlignment.TopLeft, string.Format("* {0}", footNoteText));
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


        public Stream GenerateSelectedTasksPDFPdfSharp(IEnumerable<SelectedTask> selectedTasks, ReportInfo reportInfo)
        {
            var enumerable = selectedTasks as SelectedTask[] ?? selectedTasks.ToArray();
            var totalNoiseDosage = enumerable.Sum(x => x.Percentage);
            var noiseLevelEnum = _noiseLevelService.CalculateNoiseLevelEnum(totalNoiseDosage);
            var noiseLevelColor = GetColorForNoiseLevelPdfSharp(noiseLevelEnum);
            var dataTable = GenerateDataTable(enumerable);

            // Starting instantiate the document.
            var myPdfDocument = new PdfSharp.Pdf.PdfDocument();

            var page = myPdfDocument.AddPage();
            page.Orientation = PageOrientation.Landscape;

            var doc = new Document();

            var section = doc.AddSection();
            section.PageSetup.Orientation = Orientation.Landscape;
            section.PageSetup.TopMargin = "0.8cm";

            // ReportTitle
            var reportTitle = section.AddParagraph();
            reportTitle.Format.Font = new MigraDoc.DocumentObjectModel.Font("Verdana", 20)
            {
                Color = MigraDoc.DocumentObjectModel.Color.FromRgbColor(0, Colors.Black)
            };
            reportTitle.AddFormattedText(ReportResource.ReportTitle, TextFormat.Bold);

            var spaceBetween = section.AddTextFrame();
            spaceBetween.Height = "0.2cm";

            // Table to place the text above the table of noise things
            var textTable = section.AddTable();
            textTable.AddColumn("19cm");  // Left side texts here
            textTable.AddColumn("7cm");   // Right side texts here

            var textRow = textTable.AddRow();

            // Left textFrame for placing texts
            var leftText = textRow.Cells[0].AddTextFrame();
            //leftText.FillFormat.Color = MigraDoc.DocumentObjectModel.Color.FromRgbColor(100, Colors.White);
            leftText.Width = "19cm";
            leftText.Height = "1.5cm";

            // reportPlant
            var reportPlantProfCom = leftText.AddParagraph();
            reportPlantProfCom.Format.LineSpacingRule = LineSpacingRule.Exactly;
            reportPlantProfCom.Format.LineSpacing = "0.45cm";
            reportPlantProfCom.Format.Font = new MigraDoc.DocumentObjectModel.Font("Verdana", 9)
            {
                Color = MigraDoc.DocumentObjectModel.Color.FromRgbColor(0, Colors.Black)
            };
            reportPlantProfCom.AddFormattedText(string.Format(ReportResource.PlantFormatString, reportInfo.Plant), TextFormat.Bold);
            reportPlantProfCom.AddLineBreak();

            // reportProfession
            reportPlantProfCom.AddFormattedText(string.Format(ReportResource.ProfessionFormatString, reportInfo.Group), TextFormat.Bold);
            reportPlantProfCom.AddLineBreak();

            // reportComment
            reportPlantProfCom.AddFormattedText(string.Format(ReportResource.CommentFormatString, reportInfo.Comment), TextFormat.Bold);
            reportPlantProfCom.AddLineBreak();

            // Right textFrame for placing texts
            var rightText = textRow.Cells[1].AddTextFrame();
            //rightText.FillFormat.Color = MigraDoc.DocumentObjectModel.Color.FromRgbColor(200, Colors.White);
            rightText.Left = ShapePosition.Right;
            rightText.RelativeHorizontal = RelativeHorizontal.Margin;
            rightText.RelativeVertical = RelativeVertical.Paragraph;
            rightText.Width = "7cm";
            rightText.Height = "1.5cm";

            // reportCreatedBy
            var reportCreaDate = rightText.AddParagraph();
            reportCreaDate.Format.LineSpacing = "0.45cm";
            reportCreaDate.Format.LineSpacingRule = LineSpacingRule.Exactly;
            reportCreaDate.Format.Font = new MigraDoc.DocumentObjectModel.Font("Verdana", 9)
            {
                Color = MigraDoc.DocumentObjectModel.Color.FromRgbColor(0, Colors.Black)
            };
            reportCreaDate.AddFormattedText(string.Format(ReportResource.UserFormatString, reportInfo.CreatedBy), TextFormat.Bold);
            reportCreaDate.AddLineBreak();

            // reportDate
            reportCreaDate.AddFormattedText(string.Format(ReportResource.DateFormatString, (reportInfo.Date.HasValue) ? reportInfo.Date.Value.ToString("dd.MM.yyyy") : string.Empty), TextFormat.Bold);

            var noiseTable = section.AddTable();
            noiseTable.Style = "Table";
            noiseTable.Borders.Width = 0.25;
            noiseTable.Borders.Left.Width = 0.5;
            noiseTable.Borders.Right.Width = 0.5;
            noiseTable.Rows.LeftIndent = 0;
            noiseTable.TopPadding = 0.6;
            noiseTable.BottomPadding = 1;

            for (var i = 0; i < dataTable.Columns.Count; i++)
            {
                switch (i)
                {
                    case 0: // Oppgave
                        noiseTable.AddColumn(Unit.FromCentimeter(7.0));
                        break;
                    case 1: // Rolle
                        noiseTable.AddColumn(Unit.FromCentimeter(3));
                        break;
                    case 2: // Hørselsvern
                        noiseTable.AddColumn(Unit.FromCentimeter(4.1));
                        break;
                    case 3: // Støynivå
                        noiseTable.AddColumn(Unit.FromCentimeter(2.4));
                        break;
                    case 4: // % Knappen inne
                        noiseTable.AddColumn(Unit.FromCentimeter(2.7));
                        break;
                    case 5: // Bakgrunnsstøy
                        noiseTable.AddColumn(Unit.FromCentimeter(2.7));
                        break;
                    case 6: // Arbeidstid
                        noiseTable.AddColumn(Unit.FromCentimeter(2.7));
                        break;
                    case 7: // %
                        noiseTable.AddColumn(Unit.FromCentimeter(1.4));
                        break;
                }
            }

            // Create the header of the table
            var row = noiseTable.AddRow();
            row.HeadingFormat = true;
            row.Format.Alignment = ParagraphAlignment.Center;
            row.Format.Font.Bold = true;
            row.Shading.Color = MigraDoc.DocumentObjectModel.Color.FromRgbColor(200, Colors.Navy);
            row.Format.Font.Color = MigraDoc.DocumentObjectModel.Color.FromRgbColor(200, Colors.White);

            for (var i = 0; i < dataTable.Columns.Count; i++)
            {
                row.Cells[i].AddParagraph(dataTable.Columns[i].ColumnName);
                row.Cells[i].Format.Font.Bold = true;
                row.Cells[i].Format.Alignment = ParagraphAlignment.Center;
                row.Cells[i].VerticalAlignment = VerticalAlignment.Center;
            }

            noiseTable.SetEdge(0, 0, dataTable.Columns.Count, 1, Edge.Box,
                 BorderStyle.Single, 0.75);

            for (var i = 0; i < dataTable.Rows.Count; i++)
            {
                var row1 = noiseTable.AddRow();
                row1.TopPadding = 1.5;

                for (var j = 0; j < dataTable.Columns.Count; j++)
                {
                    row1.Cells[j].VerticalAlignment = VerticalAlignment.Center;

                    row1.Cells[j].Format.Alignment = ParagraphAlignment.Left;
                    row1.Cells[j].Format.FirstLineIndent = 1;

                    // Column 3 and 5 should be output with modifications
                    switch (j)
                    {
                        case 3: // NoiseLevel
                            {
                                bool isMeasured = false, isMeasuredNorw = false;
                                var noiseLevelGuideLine = dataTable.Rows[i][j].ToString();

                                if (noiseLevelGuideLine.Contains("measured"))
                                    isMeasured = true;
                                if (noiseLevelGuideLine.Contains("målt"))
                                    isMeasuredNorw = true;

                                if (noiseLevelGuideLine.IndexOf(",", StringComparison.Ordinal) > 0)
                                    noiseLevelGuideLine = noiseLevelGuideLine.Substring(0,
                                        noiseLevelGuideLine.IndexOf(",", StringComparison.Ordinal));
                                if (noiseLevelGuideLine.IndexOf(".", StringComparison.Ordinal) > 0)
                                    noiseLevelGuideLine = noiseLevelGuideLine.Substring(0,
                                        noiseLevelGuideLine.IndexOf(".", StringComparison.Ordinal));

                                int nLevel;
                                if (int.TryParse(noiseLevelGuideLine, out nLevel))
                                {
                                    if (isMeasured)
                                        row1.Cells[j].AddParagraph(noiseLevelGuideLine + " dBA measured");
                                    else if (isMeasuredNorw)
                                        row1.Cells[j].AddParagraph(noiseLevelGuideLine + " dBA målt");
                                    else
                                        row1.Cells[j].AddParagraph(noiseLevelGuideLine + " dBA");
                                }
                                else
                                    row1.Cells[j].AddParagraph(noiseLevelGuideLine);
                            }
                            break;
                        case 5:
                            row1.Cells[j].AddParagraph(dataTable.Rows[i][j].ToString() == "0 dBA"
                                ? "<80 dBA"
                                : dataTable.Rows[i][j].ToString());
                            break;
                        default:
                            row1.Cells[j].AddParagraph(dataTable.Rows[i][j].ToString());
                            break;
                    }

                    noiseTable.SetEdge(0, noiseTable.Rows.Count - 2, dataTable.Columns.Count, 1,
                         Edge.Box, BorderStyle.Single, 0.75);
                }
            }

            // Table to place the summary below the table of noise things
            var summaryTable = section.AddTable();
            summaryTable.AddColumn("19cm");  // Left side texts here
            summaryTable.AddColumn("5cm");   // Right side texts here
            summaryTable.AddColumn("2cm");   // Percentage here
            summaryTable.TopPadding = "0.3cm";
            summaryTable.BottomPadding = "0.3cm";

            var summaryRow = summaryTable.AddRow();
            summaryRow.Height = "1.2cm";

            // Left textFrame for placing texts
            var leftSummary = summaryRow.Cells[0].AddTextFrame();
            leftSummary.FillFormat.Color = MigraDoc.DocumentObjectModel.Color.FromRgbColor(100, Colors.White);
            leftSummary.Width = "20cm";
            leftSummary.Height = "1.2cm";

            // TotalpercentageText
            //var totalPercentageText = leftSummary.AddParagraph();
            //totalPercentageText.Format.Font = new MigraDoc.DocumentObjectModel.Font("Verdana", 10.5)
            //{
            //    Color = MigraDoc.DocumentObjectModel.Color.FromRgbColor(0, Colors.Black)
            //};
            //totalPercentageText.AddFormattedText(ReportResource.TotalPercentageTextFormatString, TextFormat.Bold);

            // Right textFrame for placing texts

            // Total percentage
            summaryRow.Cells[1].VerticalAlignment = VerticalAlignment.Bottom;
            var totalPercentageTextBox = summaryRow.Cells[1].AddTextFrame();
            totalPercentageTextBox.FillFormat.Color = MigraDoc.DocumentObjectModel.Color.FromRgbColor(200, noiseLevelColor);
            totalPercentageTextBox.Width = "5cm";
            totalPercentageTextBox.Height = "1.2cm";
            var totalPercentageText = totalPercentageTextBox.AddParagraph();
            totalPercentageText.Format.Font = new MigraDoc.DocumentObjectModel.Font("Verdana", 12)
            {
                Color = MigraDoc.DocumentObjectModel.Color.FromRgbColor(0, Colors.Black)
            };
            totalPercentageText.AddFormattedText(string.Format(ReportResource.TotalPercentageTextFormatString), TextFormat.Bold);
            totalPercentageText.Format.Alignment = ParagraphAlignment.Center;

            // percentage
            var totalPercentageBox = summaryRow.Cells[2].AddTextFrame();
            totalPercentageBox.FillFormat.Color = MigraDoc.DocumentObjectModel.Color.FromRgbColor(200, noiseLevelColor);
            totalPercentageBox.Width = "2cm";
            totalPercentageBox.Height = "1.2cm";
            var totalPercentage = totalPercentageBox.AddParagraph();
            totalPercentage.Format.Font = new MigraDoc.DocumentObjectModel.Font("Verdana", 18)
            {
                Color = MigraDoc.DocumentObjectModel.Color.FromRgbColor(0, Colors.Black)
            };
            totalPercentage.AddFormattedText(string.Format(ReportResource.TotalPercentageFormatString, totalNoiseDosage), TextFormat.Bold);
            totalPercentage.Format.Alignment = ParagraphAlignment.Center;


            // Status
            var statusText = section.AddParagraph();
            statusText.Format.Font = new MigraDoc.DocumentObjectModel.Font("Verdana", 10.5)
            {
                Color = MigraDoc.DocumentObjectModel.Color.FromRgbColor(0, Colors.Black)
            };
            statusText.AddFormattedText(_noiseLevelService.GetNoiseLevelStatusText(noiseLevelEnum), TextFormat.Bold);

            foreach (var footNoteText in reportInfo.Footnotes)
            {
                var footNoteHeader = section.AddParagraph();
                var footNote = section.AddParagraph();

                footNote.Format.Font = new MigraDoc.DocumentObjectModel.Font("Verdana", 8)
                {
                    Color = MigraDoc.DocumentObjectModel.Color.FromRgbColor(0, Colors.Black)
                };
                footNoteHeader.Format.Font = new MigraDoc.DocumentObjectModel.Font("Verdana", 8)
                {
                    Color = MigraDoc.DocumentObjectModel.Color.FromRgbColor(0, Colors.Black),
                    Bold = true
                };

                var textHeader = footNoteText.Substring(0, footNoteText.IndexOf("<br/>", StringComparison.Ordinal)).Replace("<b>", "").Replace("</b>", "");
                if (footNoteText.Length > footNoteText.IndexOf("<br/>", StringComparison.Ordinal) + 5)
                {
                    var text = footNoteText.Substring(footNoteText.IndexOf("<br/>", StringComparison.Ordinal) + 5).Replace("<br/><br/>", Environment.NewLine).Replace("<br/>", Environment.NewLine);

                    footNoteHeader.AddFormattedText(string.Format(Environment.NewLine + "* {0}", textHeader));
                    footNote.AddFormattedText(string.Format("{0}", text));
                }
                var spaceBetween1 = section.AddTextFrame();
                spaceBetween1.Height = "0.1cm";
            }

            var pdfRenderer = new PdfDocumentRenderer(false, PdfFontEmbedding.Always) { Document = doc };
            pdfRenderer.RenderDocument();

            myPdfDocument = pdfRenderer.PdfDocument;

            // Finally we save the docuement...
            Stream memoryStream = new MemoryStream();
            myPdfDocument.Save(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);

            return memoryStream;
        }

        private MigraDoc.DocumentObjectModel.Color GetColorForNoiseLevelPdfSharp(NoiseLevelEnum noiseLevelEnum)
        {
            switch (noiseLevelEnum)
            {
                case NoiseLevelEnum.Critical:
                    {
                        return MigraDoc.DocumentObjectModel.Color.FromRgbColor(200, Colors.Red);
                    }
                case NoiseLevelEnum.DangerOfWorkRelatedInjury:
                    {
                        return MigraDoc.DocumentObjectModel.Color.FromRgbColor(200, Colors.Red);
                    }
                case NoiseLevelEnum.Warning:
                    {
                        return MigraDoc.DocumentObjectModel.Color.FromRgbColor(200, Colors.Yellow);
                    }
                case NoiseLevelEnum.MaximumAllowedDosage:
                    {
                        return MigraDoc.DocumentObjectModel.Color.FromRgbColor(200, Colors.Yellow);
                    }
                default:
                    {
                        return MigraDoc.DocumentObjectModel.Color.FromRgbColor(200, Colors.YellowGreen);
                    }
            }
        }

        private Color GetColorForNoiseLevel(NoiseLevelEnum noiseLevelEnum)
        {
            switch (noiseLevelEnum)
            {
                case NoiseLevelEnum.Critical:
                    {
                        return Color.FromRgb(255, 0, 0);
                    }
                case NoiseLevelEnum.DangerOfWorkRelatedInjury:
                    {
                        return Color.FromRgb(255, 0, 0);
                    }
                case NoiseLevelEnum.Warning:
                    {
                        return Color.FromRgb(255, 255, 0);
                    }
                case NoiseLevelEnum.MaximumAllowedDosage:
                    {
                        return Color.FromRgb(255, 255, 0);
                    }
                default:
                    {
                        return Color.FromRgb(160, 255, 0);
                    }
            }
        }

        private DataTable GenerateDataTable(IEnumerable<SelectedTask> selectedTasks)
        {
            string titleHeading = ReportResource.HeadingTitle;
            string roleHeading = ReportResource.HeadingRole;
            string noiseProtectionHeading = ReportResource.HeadingNoiseProtection;
            string noiseLevelHeading = ReportResource.HeadingNoiseLevel;
            string buttonPressedHeading = ReportResource.HeadingButtonPressed;
            string backgroundNoiseHeading = ReportResource.HeadingBackgroundNoise;
            string workTimeHeading = ReportResource.HeadingWorkTime;
            string percentageHeading = ReportResource.HeadingPercentage;

            DataTable dt = new DataTable();
            dt.Columns.Add(titleHeading);
            dt.Columns.Add(roleHeading);
            dt.Columns.Add(noiseProtectionHeading);
            dt.Columns.Add(noiseLevelHeading);
            dt.Columns.Add(buttonPressedHeading);
            dt.Columns.Add(backgroundNoiseHeading);
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

                dr[buttonPressedHeading] = string.Format("{0}%", selectedTask.ButtonPressed);
                dr[backgroundNoiseHeading] = string.Format("{0} dBA", selectedTask.BackgroundNoise);
                dr[workTimeHeading] = string.Format(ReportResource.WorkTimeFormatString, selectedTask.Hours, selectedTask.Minutes);
                dr[percentageHeading] = selectedTask.Percentage;

                dt.Rows.Add(dr);
            }

            return dt;
        }
    }
}
