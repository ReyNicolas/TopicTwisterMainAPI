namespace ApiTopicTwisterQuark.Entities
{
    [Serializable]
    public class Category
    {
        public string Name;
        public List<string> words;

        public Category(string name, List<string> words)
        {
            Name = name;
            this.words = words;
        }

        public bool ContainsThisWordAndThatMustStartWithThisLetter(string word, char letter)
        {
            return WordsThatStartWithThisLetter(letter).Contains(word);
        }            
       

        private List<string> WordsThatStartWithThisLetter(char letter)
        {
            return words.Where(w => w.StartsWith(letter.ToString())).ToList();
        }
    }
    
    
}