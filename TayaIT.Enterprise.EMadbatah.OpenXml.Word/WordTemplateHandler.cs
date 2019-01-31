using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DocumentFormat.OpenXml.Packaging;
using System.IO;
using System.Xml.Linq;
using System.Xml;

namespace TayaIT.Enterprise.EMadbatah.OpenXml.Word
{
    //xml for madbatah cover
    /*<root>
      <Name>name</Name>
      <Season>Season</Season>
      <StageType>StageType</StageType>
      <Stage>Stage</Stage>
      <Subject>Subject</Subject>
      <DateHijri>DateHijri</DateHijri>
      <DateMilady>DateMilady</DateMilady>
      <Footer>Footer</Footer>
    </root>
     */
    //xml for attachment cover
    /*
     <root>
    <Number>Number</Number>
    <Title>Number</Title>
    </root>
     */

    /*
     <root>
  <SessionNum>SessionNum</SessionNum>
  <HijriDate>HijriDate</HijriDate>
  <GeorgianDate>GeorgianDate</GeorgianDate>
  <sessionTime>sessionTime</sessionTime>
  <Table Name="AgendaItem">
    <Field Name="AgendaItem" />
  </Table>
<Header>
<SessionNum>SessionNum</SessionNum>
<sessionType>sessionType</sessionType>
<sessionStage>sessionStage</sessionStage>
<sessionTime>sessionTime</sessionTime>
  <HijriDate>HijriDate</HijriDate>
  <GeorgianDate>GeorgianDate</GeorgianDate>
<sessionPresident>sessionPresident</sessionPresident>
</Header>
  <Table Name="ApologiesAtt">
    <Field Name="ApologiesAtt1" />
    <Field Name="ApologiesAtt2" />
  </Table>
  <Table Name="AbsentAtt">
    <Field Name="AbsentAtt1" />
    <Field Name="AbsentAtt2" />
  </Table>

  <Table Name="attFromGov">
    <Field Name="attFromGov" />
    <Field Name="attFromGovTitle" />
  </Table>
<attendingOutOfMajles>attendingOutOfMajles</attendingOutOfMajles>
<secertairs>secertairs</secertairs>

</root>
     * */
    public static class AssembleDocumentLocalExtensions
    {
        public static string StringConcatenate<T>(this IEnumerable<T> source,
            Func<T, string> func)
        {
            StringBuilder sb = new StringBuilder();
            foreach (T item in source)
                sb.Append(func(item));
            return sb.ToString();
        }

        public static string StringConcatenate(this IEnumerable<string> source)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string item in source)
                sb.Append(item);
            return sb.ToString();
        }

        public static XDocument GetXDocument(this OpenXmlPart part)
        {
            XDocument xdoc = part.Annotation<XDocument>();
            if (xdoc != null)
                return xdoc;
            using (Stream str = part.GetStream())
            using (StreamReader streamReader = new StreamReader(str))
            using (XmlReader xr = XmlReader.Create(streamReader))
                xdoc = XDocument.Load(xr);
            part.AddAnnotation(xdoc);
            return xdoc;
        }

        public static void PutXDocument(this OpenXmlPart part)
        {
            XDocument xdoc = part.GetXDocument();
            if (xdoc != null)
            {
                // Serialize the XDocument object back to the package.
                using (XmlWriter xw =
                    XmlWriter.Create(part.GetStream
                   (FileMode.Create, FileAccess.Write)))
                {
                    xdoc.Save(xw);
                }
            }
        }
    }

    public static class W
    {
        public static XNamespace w =
            "http://schemas.openxmlformats.org/wordprocessingml/2006/main";

        public static XName body = w + "body";
        public static XName sdt = w + "sdt";
        public static XName sdtPr = w + "sdtPr";
        public static XName tag = w + "tag";
        public static XName val = w + "val";
        public static XName sdtContent = w + "sdtContent";
        public static XName tbl = w + "tbl";
        public static XName tr = w + "tr";
        public static XName tc = w + "tc";
        public static XName p = w + "p";
        public static XName r = w + "r";
        public static XName t = w + "t";
        public static XName rPr = w + "rPr";
        public static XName highlight = w + "highlight";
        public static XName pPr = w + "pPr";
        public static XName color = w + "color";
        public static XName sz = w + "sz";
        public static XName szCs = w + "szCs";
    }

    public static class ContentControlManager
    {
        public static XElement GetContentControls(WordprocessingDocument document)
        {
            XElement contentControls = new XElement("ContentControls",
                document
                    .MainDocumentPart
                    .GetXDocument()
                    .Root
                    .Element(W.body)
                    .Elements(W.sdt)
                    .Select(tableContentControl =>
                        new XElement("Table",
                            new XAttribute("Name", (string)tableContentControl
                                .Element(W.sdtPr).Element(W.tag).Attribute(W.val)),
                            tableContentControl
                                .Descendants(W.sdt)
                                .Select(fieldContentControl =>
                                    new XElement("Field",
                                        new XAttribute("Name",
                                            (string)fieldContentControl
                                                .Element(W.sdtPr)
                                                .Element(W.tag)
                                                .Attribute(W.val)
                                        )
                                    )
                                )
                        )
                    )
            );
            return contentControls;
        }

        public static void SetContentControls(WordprocessingDocument document,
             XElement contentControlData)
        {
            List<string> errors = new List<string>();

            // Iterate through the tables in the source document.
            foreach (XElement table in contentControlData.Elements("Table"))
            {
                // Find the content control that contains the table.
                string listName = (string)table.Attribute("Name");
                XElement tableContentControl = document
                    .MainDocumentPart
                    .GetXDocument()
                    .Root
                    .Element(W.body)
                    .Elements(W.sdt)
                    .Where(sdt =>
                        listName == (string)sdt
                            .Element(W.sdtPr)
                            .Element(W.tag)
                            .Attribute(W.val))
                    .FirstOrDefault();

                // If there isn't a table with that name, add an error to the error string,
                // and continue.
                if (tableContentControl == null)
                {
                    errors.Add(String.Format("Table Content Control '{0}' not found.",
                        listName));
                    continue;
                }

                // If the table doesn't contain content controls in cells, then error.
                XElement cellContentControl = tableContentControl
                    .Descendants(W.sdt)
                    .FirstOrDefault();
                if (cellContentControl == null)
                {
                    errors.Add(String.Format(
            "Table Content Control '{0}' doesn't contain content controls in cells.",
                        listName));
                    continue;
                }

                // Determine the element for the row that contains the content controls.
                // This is the prototype for the rows that the code will generate from data.
                XElement prototypeRow = tableContentControl
                    .Descendants(W.sdt)
                    .Ancestors(W.tr)
                    .FirstOrDefault();

                // Create a list of new rows to be inserted into the document.  Because this
                // is a document centric transform, this is written in a non-functional
                // style, using tree modification.
                List<XElement> newRows = new List<XElement>();
                foreach (XElement row in table.Elements("Row"))
                {
                    // Clone the prototypeRow into newRow.
                    XElement newRow = new XElement(prototypeRow);

                    // Create new rows that will contain the data that was passed in to this
                    // method in the XML tree.
                    foreach (XElement sdt in newRow.Descendants(W.sdt).ToList())
                    {
                        // Get fieldName from the content control tag.
                        string fieldName = (string)sdt
                            .Element(W.sdtPr)
                            .Element(W.tag)
                            .Attribute(W.val);

                        // Get the new value out of contentControlValues.
                        XElement newValueElement = row
                            .Elements("Field")
                            .Where(f => (string)f.Attribute("Name") == fieldName)
                            .FirstOrDefault();

                        // Generate error message if the new value doesn't exist.
                        if (newValueElement == null)
                        {
                            errors.Add(String.Format(
                                "Table '{0}', Field '{1}' value isn't specified.",
                                listName, fieldName));
                            continue;
                        }

                        // Get the string value of the Value attribute of the element
                        string newValue = newValueElement.Attribute("Value").Value;

                        // Get run properties if there are any.  If there are no run
                        // properties, runProperties will be set to null.
                        XElement runProperties = sdt.Element(W.sdtContent)
                            .Descendants(W.r)
                            .Descendants(W.rPr)
                            .FirstOrDefault();

                        // Replace the run in the structured data type with the new value.
                        sdt.Element(W.sdtContent)
                            .Descendants(W.r)
                            .FirstOrDefault()
                            .ReplaceWith(
                                new XElement(W.r,
                                    runProperties,  // It is OK for runProperties to be null.
                                    new XElement(W.t, newValue)));

                        // Remove the content control, and replace it with its contents.
                        XElement replacementElement =
                            new XElement(sdt.Element(W.sdtContent).Elements().First());
                        sdt.ReplaceWith(replacementElement);
                    }

                    // Add the newRow to the list of rows that will be placed in the newly
                    // generated table.
                    newRows.Add(newRow);
                }

                // Remove the prototype row and add all of the newly constructed rows.
                XElement tableElement = prototypeRow.Ancestors(W.tbl).First();
                prototypeRow.Remove();
                tableElement.Add(newRows);

                // Remove the content control for the table and replace it with its contents.
                XElement tableClone = new XElement(tableElement);
                tableContentControl.ReplaceWith(tableClone);
            }

            // Add any errors as red text on yellow at the beginning of the document.
            if (errors.Any())
                document.MainDocumentPart
                    .GetXDocument()
                    .Root
                    .Element(W.body)
                    .AddFirst(errors.Select(s =>
                        new XElement(W.p,
                            new XElement(W.r,
                                new XElement(W.rPr,
                                    new XElement(W.color,
                                        new XAttribute(W.val, "red")),
                                    new XElement(W.sz,
                                        new XAttribute(W.val, "28")),
                                    new XElement(W.szCs,
                                        new XAttribute(W.val, "28")),
                                    new XElement(W.highlight,
                                        new XAttribute(W.val, "yellow"))),
                                new XElement(W.t, s)))));

            document.MainDocumentPart.PutXDocument();
        }
    }

    public class WordTemplateHandler
    {
        /// <summary>
        /// Replaces the custom XML part inside a docx file with the specified customXML, overwrite the sent file
        /// </summary>
        /// <param name="docxTemplate">Docx file to modify</param>
        /// <param name="customXML">Custom XML part with the 
        /// data to insert in the docx document</param>
        public static void replaceCustomXML(string docxTemplate, string customXML)
        {
            try
            {
                using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(
                    docxTemplate, true))
                {
                    MainDocumentPart mainPart = wordDoc.MainDocumentPart;

                    mainPart.DeleteParts<CustomXmlPart>(mainPart.CustomXmlParts);
                    //Add a new customXML part and then add content
                    CustomXmlPart customXmlPart = mainPart.AddCustomXmlPart(
                         CustomXmlPartType.CustomXml);
                    //copy the XML into the new part...
                    using (StreamWriter ts = new StreamWriter(customXmlPart.GetStream()))
                        ts.Write(customXML);
                }
            }
            catch (Exception ex)
            {
                //throw new FaultException("WCF error!\r\n" + ex.Message);
            }
        }
        public static void replaceTableXmlContents(string docxTemplate, XElement contentControlValues)
        {
            using (WordprocessingDocument doc = WordprocessingDocument.Open(docxTemplate, true))
            {
                XElement contentControlStructure =
                    ContentControlManager.GetContentControls(doc);

                Console.WriteLine(contentControlStructure);
                Environment.Exit(0);
                /*
                XElement contentControlValues = XElement.Parse(
                    @"<ContentControls>
                  <Table Name='Team Members'>
                    <Row>
                      <Field Name='Name' Value='Eric'/>
                      <Field Name='Title' Value='Program Manager'/>
                    </Row>
                    <Row>
                      <Field Name='Name' Value='Bob'/>
                      <Field Name='Title' Value='Developer'/>
                    </Row>
                  </Table>
                  <Table Name='Work Items'>
                    <Row>
                      <Field Name='Item Number' Value='100'/>
                      <Field Name='Name' Value='Learn SP2010'/>
                      <Field Name='Description' Value='It will be fun'/>
                      <Field Name='Estimated Hours' Value='80'/>
                    </Row>
                    <Row>
                      <Field Name='Item Number' Value='110'/>
                      <Field Name='Name' Value='Morale Event'/>
                      <Field Name='Description' Value='This will be work.'/>
                      <Field Name='Estimated Hours' Value='4'/>
                    </Row>
                    <Row>
                      <Field Name='Item Number' Value='120'/>
                      <Field Name='Name' Value='Scrum Meeting'/>
                      <Field Name='Description' Value='Dave better attend this time.  This is a very long value that should wrap properly.  Hope so...'/>
                      <Field Name='Estimated Hours' Value='.50'/>
                    </Row>
                  </Table>
                </ContentControls>
                ");
                */
                ContentControlManager.SetContentControls(doc, contentControlValues);
            }
        }
    }
}


