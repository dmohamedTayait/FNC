using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TayaIT.Enterprise.EMadbatah.Model
{
    public class BasicFormat
    {
        public Alignment align { get; set; }
        public TextStyle textStyle { get; set; }
    }
    public enum Alignment
    {
        Right,
        Left,
        Center,
        Justify
    }
    public enum TextStyle
    {
        Normal,
        Bold,
        Italic,
        Underlined,
        BoldItalic,
        BoldUnderlined,
        ItalicUnderLined
    }
    public enum HeadingLevel
    {
        Heading1,
        Heading2,
        Heading3
    }

    public enum TextFont
    {
        Arial,
        TimesNewRoman

    }
    public enum FontColor
    {
        Black,
        Blue,
        Green,
        Red
    }
}
