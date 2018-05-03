﻿namespace MariGold.OpenXHTML
{
    using System;
    using DocumentFormat.OpenXml.Wordprocessing;

    internal sealed class DocxCenter : DocxElement, ITextElement
    {
        internal DocxCenter(IOpenXmlContext context)
            : base(context)
        {
        }

        internal override bool CanConvert(DocxNode node)
        {
            return string.Compare(node.Tag, "center", StringComparison.InvariantCultureIgnoreCase) == 0;
        }

        internal override void Process(DocxNode node, ref Paragraph paragraph)
        {
            if (node.IsNull() || node.Parent == null || IsHidden(node))
            {
                return;
            }

            paragraph = null;
            Paragraph centerParagraph = null;
            node.SetExtentedStyle(DocxAlignment.textAlign, DocxAlignment.center);

            node.SetExtentedStyle(DocxAlignment.textAlign, DocxAlignment.center);

            ProcessBlockElement(node, ref centerParagraph);
        }

        bool ITextElement.CanConvert(DocxNode node)
        {
            return CanConvert(node);
        }

        void ITextElement.Process(DocxNode node)
        {
            if (IsHidden(node))
            {
                return;
            }

            node.SetExtentedStyle(DocxAlignment.textAlign, DocxAlignment.center);
            ProcessTextChild(node);
        }
    }
}
