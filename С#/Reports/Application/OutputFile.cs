using System.Data;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml;

namespace Reports
{
    public class OutputFile
    {
        int rowcount, columncount;
        DataSet content;
        string StartingString;

        // Creates a WordprocessingDocument.
        public void CreatePackage(string filePath, int rowcount, int columncount, DataSet content, string StartingString)
        {
            this.rowcount = rowcount;
            this.columncount = columncount;
            this.content = content;
            this.StartingString = StartingString;

            using (WordprocessingDocument package = WordprocessingDocument.Create(filePath, WordprocessingDocumentType.Document))
            {
                CreateParts(package);
            }
        }

        // Adds child parts and generates content of the specified part.
        private void CreateParts(WordprocessingDocument document)
        {
            MainDocumentPart mainDocumentPart1 = document.AddMainDocumentPart();
            GenerateMainDocumentPart1Content(mainDocumentPart1);

            NumberingDefinitionsPart numberingDefinitionsPart1 = mainDocumentPart1.AddNewPart<NumberingDefinitionsPart>("docRId0");
            GenerateNumberingDefinitionsPart1Content(numberingDefinitionsPart1);

            StyleDefinitionsPart styleDefinitionsPart1 = mainDocumentPart1.AddNewPart<StyleDefinitionsPart>("docRId1");
            GenerateStyleDefinitionsPart1Content(styleDefinitionsPart1);
        }

        // Generates content of mainDocumentPart1.
        private void GenerateMainDocumentPart1Content(MainDocumentPart mainDocumentPart1)
        {
            Document document1 = new Document();
            document1.AddNamespaceDeclaration("w", "http://schemas.openxmlformats.org/wordprocessingml/2006/main");

            Body body1 = new Body();

            Paragraph paragraph1 = new Paragraph();

            ParagraphProperties paragraphProperties1 = new ParagraphProperties();
            SpacingBetweenLines spacingBetweenLines1 = new SpacingBetweenLines() { Before = "0", After = "200", Line = "276" };
            Indentation indentation1 = new Indentation() { Left = "0", Right = "0", FirstLine = "0" };
            Justification justification1 = new Justification() { Val = JustificationValues.Left };

            ParagraphMarkRunProperties paragraphMarkRunProperties1 = new ParagraphMarkRunProperties();
            RunFonts runFonts1 = new RunFonts() { Ascii = "Calibri", HighAnsi = "Calibri", EastAsia = "Calibri", ComplexScript = "Calibri" };
            Color color1 = new Color() { Val = "auto" };
            Spacing spacing1 = new Spacing() { Val = 0 };
            Position position1 = new Position() { Val = "0" };
            FontSize fontSize1 = new FontSize() { Val = "22" };
            Shading shading1 = new Shading() { Val = ShadingPatternValues.Clear, Fill = "auto" };

            paragraphMarkRunProperties1.Append(runFonts1);
            paragraphMarkRunProperties1.Append(color1);
            paragraphMarkRunProperties1.Append(spacing1);
            paragraphMarkRunProperties1.Append(position1);
            paragraphMarkRunProperties1.Append(fontSize1);
            paragraphMarkRunProperties1.Append(shading1);

            paragraphProperties1.Append(spacingBetweenLines1);
            paragraphProperties1.Append(indentation1);
            paragraphProperties1.Append(justification1);
            paragraphProperties1.Append(paragraphMarkRunProperties1);

            Run run1 = new Run();

            RunProperties runProperties1 = new RunProperties();
            RunFonts runFonts2 = new RunFonts() { Ascii = "Calibri", HighAnsi = "Calibri", EastAsia = "Calibri", ComplexScript = "Calibri" };
            Color color2 = new Color() { Val = "auto" };
            Spacing spacing2 = new Spacing() { Val = 0 };
            Position position2 = new Position() { Val = "0" };
            FontSize fontSize2 = new FontSize() { Val = "22" };
            Shading shading2 = new Shading() { Val = ShadingPatternValues.Clear, Fill = "auto" };

            runProperties1.Append(runFonts2);
            runProperties1.Append(color2);
            runProperties1.Append(spacing2);
            runProperties1.Append(position2);
            runProperties1.Append(fontSize2);
            runProperties1.Append(shading2);
            Text text1 = new Text() { Space = SpaceProcessingModeValues.Preserve };
            text1.Text = StartingString;

            run1.Append(runProperties1);
            run1.Append(text1);

            paragraph1.Append(paragraphProperties1);
            paragraph1.Append(run1);

            body1.Append(paragraph1);
            document1.Append(body1);
            Table table = CreateTable();
            document1.Body.Append(table);

            mainDocumentPart1.Document = document1;
        }

        // Generates content of numberingDefinitionsPart1.
        private void GenerateNumberingDefinitionsPart1Content(NumberingDefinitionsPart numberingDefinitionsPart1)
        {
            Numbering numbering1 = new Numbering();
            numbering1.AddNamespaceDeclaration("w", "http://schemas.openxmlformats.org/wordprocessingml/2006/main");

            numberingDefinitionsPart1.Numbering = numbering1;
        }

        // Generates content of styleDefinitionsPart1.
        private void GenerateStyleDefinitionsPart1Content(StyleDefinitionsPart styleDefinitionsPart1)
        {
            Styles styles1 = new Styles();
            styles1.AddNamespaceDeclaration("w", "http://schemas.openxmlformats.org/wordprocessingml/2006/main");

            styleDefinitionsPart1.Styles = styles1;
        }

        public Table CreateTable()
        {
            // Create an empty table.
            Table table = new Table();
            uint BorderSize = 8;
            EnumValue<BorderValues> BorderVal = new EnumValue<BorderValues>(BorderValues.Single);
            // Create a TableProperties object and specify its border information.
            TableProperties tblProp = new TableProperties(
                new TableBorders(
                    new TopBorder()
                    {
                        Val = BorderVal,
                        Size = BorderSize
                    },
                    new BottomBorder()
                    {
                        Val = BorderVal,
                        Size = BorderSize
                    },
                    new LeftBorder()
                    {
                        Val = BorderVal,
                        Size = BorderSize
                    },
                    new RightBorder()
                    {
                        Val = BorderVal,
                        Size = BorderSize
                    },
                    new InsideHorizontalBorder()
                    {
                        Val = BorderVal,
                        Size = BorderSize
                    },
                    new InsideVerticalBorder()
                    {
                        Val = BorderVal,
                        Size = BorderSize
                    }
                )
            );

            // Append the TableProperties object to the empty table.
            table.AppendChild<TableProperties>(tblProp);
            for (int i = 0; i < rowcount; i++)
            {
                // Create a row.
                TableRow tr = new TableRow();
                for (int j = 0; j < columncount; j++)
                {
                    // Create a cell.
                    TableCell tc1 = new TableCell();

                    // Specify the width property of the table cell.
                    tc1.Append(new TableCellProperties(
                        new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "2400" }));

                    // Specify the table cell content.
                    tc1.Append(new Paragraph(new Run(new Text(content.Tables[0].Rows[i].ItemArray[j].ToString()))));

                    // Append the table cell to the table row.
                    tr.Append(tc1);

                }
                // Append the table row to the table.
                table.Append(tr);
            }

            return table;
            // Append the table to the document.
            //doc.MainDocumentPart.Document.Body.Append(table);
        }


    }
}
