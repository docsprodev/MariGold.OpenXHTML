﻿namespace MariGold.OpenXHTML
{
    using System;
    using DocumentFormat.OpenXml.Wordprocessing;
    using System.Text.RegularExpressions;

    internal sealed class DocxHeading : DocxElement
    {
        private Regex isValid;

        private int GetHeaderNumber(DocxNode node)
        {
            int value = -1;
            Regex regex = new Regex("[1-6]{1}$");

            Match match = regex.Match(node.Tag);

            if (match != null)
            {
                Int32.TryParse(match.Value, out value);
            }

            return value;
        }

        private string CalculateFontSize(int headerSize)
        {
            string fontSize = string.Empty;

            switch (headerSize)
            {
                case 1:
                    fontSize = "2em";
                    break;

                case 2:
                    fontSize = "1.5em";
                    break;

                case 3:
                    fontSize = "1.17em";
                    break;

                case 4:
                    fontSize = "1em";
                    break;

                case 5:
                    fontSize = ".83em";
                    break;

                case 6:
                    fontSize = ".67em";
                    break;
            }

            return fontSize;
        }

        private void ApplyStyle(DocxNode node)
        {
            string fontSizeValue = node.ExtractOwnStyleValue(DocxFontStyle.fontSize);
            string fontWeightValue = node.ExtractOwnStyleValue(DocxFontStyle.fontWeight);

            if (string.IsNullOrEmpty(fontSizeValue))
            {
                string headingFontSize = CalculateFontSize(GetHeaderNumber(node));
                string inheritedStyle = node.ExtractInheritedStyleValue(DocxFontStyle.fontSize);

                if (!string.IsNullOrEmpty(inheritedStyle))
                {
                    fontSizeValue = string.Concat(
                        context.Parser.CalculateRelativeChildFontSize(
                        inheritedStyle, headingFontSize).ToString("G29"), "px");
                }
                else
                {
                    fontSizeValue = headingFontSize;
                }
            }

            if (string.IsNullOrEmpty(fontWeightValue))
            {
                fontWeightValue = DocxFontStyle.bold;
            }

            node.SetExtentedStyle(DocxFontStyle.fontSize, fontSizeValue);
            node.SetExtentedStyle(DocxFontStyle.fontWeight, fontWeightValue);
        }

        internal DocxHeading(IOpenXmlContext context)
            : base(context)
        {
            isValid = new Regex(@"^[hH][1-6]{1}$");
        }

        internal override bool CanConvert(DocxNode node)
        {
            return isValid.IsMatch(node.Tag);
        }

        internal override void Process(DocxNode node, ref Paragraph paragraph)
        {
            if (node.IsNull() || node.Parent == null || IsHidden(node))
            {
                return;
            }

            paragraph = null;
            Paragraph headerParagraph = null;
            ApplyStyle(node);

            ProcessBlockElement(node, ref headerParagraph);
        }
    }
}
