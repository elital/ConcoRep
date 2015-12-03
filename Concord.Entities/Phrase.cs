using System.Collections.Generic;
using System.Linq;

namespace Concord.Entities
{
    public class Phrase
    {
        public int PhraseNumber { get; set; }
        public List<PhraseWord> Words { get; set; }

        // TODO : Move to read (in Song to)

        private string _text = string.Empty;
        public string Text
        {
            get
            {
                if (!string.IsNullOrEmpty(_text))
                    return _text;

                foreach (var word in Words.OrderBy(w => w.WordSequence))
                {
                    _text = word.WordSequence == 1
                        ? word.Word.Text
                        : $"{_text} {word.Word.Text}";
                }

                return _text;
            }
        }

        public Phrase()
        {
            Words = new List<PhraseWord>();
        }

    }

    public class PhraseWord
    {
        public int Id { get; set; }
        public int WordSequence { get; set; }
        public Word Word { get; set; }
    }
}