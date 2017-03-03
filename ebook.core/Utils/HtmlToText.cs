using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ebook.core.Utils
{
    public class HtmlToText
    {
        const string TAGWHITESPACE = @"(>|$)(\W|\n|\r)+<";      // matches one or more (white space or line breaks) between '>' and '<'
        const string STRIPFORMATTING = @"<[^>]*(>|$)";          // match any character between '<' and '>', even when end tag is missing
        const string LINEBREAK = @"<(br|BR)\s{0,1}\/{0,1}>";    // matches: <br>,<br/>,<br />,<BR>,<BR/>,<BR />
        readonly Regex _lineBreakRegex;
        readonly Regex _stripFormattingRegex;
        readonly Regex _tagWhiteSpaceRegex;

        public HtmlToText()
        {
            _lineBreakRegex = new Regex(LINEBREAK, RegexOptions.Multiline);
            _stripFormattingRegex = new Regex(STRIPFORMATTING, RegexOptions.Multiline);
            _tagWhiteSpaceRegex = new Regex(TAGWHITESPACE, RegexOptions.Multiline);
        }

        public string Convert(string html)
        {
            if (string.IsNullOrWhiteSpace(html))
                return html;

            var text = html;
            //Decode html specific characters
            text = System.Net.WebUtility.HtmlDecode(text);

            //Remove tag whitespace/line breaks
            text = _tagWhiteSpaceRegex.Replace(text, "><");
            
            //Replace <br /> with line breaks
            text = _lineBreakRegex.Replace(text, Environment.NewLine);
            
            //Strip formatting
            text = _stripFormattingRegex.Replace(text, string.Empty);

            return text;
        }
    }
}
