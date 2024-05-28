using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringContainsTest
{
    public interface ICanContainsLookup
    {
        public string ContainsSearchText { get; }
    }

    public class StringContainsHelper<T> where T: ICanContainsLookup
    {
        public StringContainsHelper(List<T> data)
        {
            PrepareLookupData(data);
        }

        private List<T> _data = [];
        private string _dataString = string.Empty;
        private readonly Dictionary<int, LookupItem<T>> _lookupItems = [];
        private readonly Dictionary<int, int> _indexLookup = [];

        private void PrepareLookupData(List<T> data)
        {
            _data = data;
            var sb = new StringBuilder();
            var id = 0;
            foreach (var item in _data)
            {
                var firstIndex = sb.Length;
                sb.Append(item.ContainsSearchText);
                var lastIndex = sb.Length - 1;
                _lookupItems.Add(id, new LookupItem<T> { Item = item, LastIndex = lastIndex });
                for (var i = firstIndex; i <= lastIndex; i++)
                {
                    _indexLookup.Add(i, id);
                }

                id++;
            }

            _dataString = sb.ToString();
        }

        public List<T> ContainsSearch(string input)
        {
            var toReturn = new List<T>();
            if (string.IsNullOrEmpty(input))
                return toReturn;

            var character = input[0];
            var matchingIndexes = FirstCharSearch(character, input.Length);

            for (var i = 1; i < input.Length; i++)
            {
                matchingIndexes = NextCharSearch(input[i], input.Length - i, matchingIndexes);
            }

            foreach (var index in matchingIndexes)
            {
                toReturn.Add(_lookupItems[_indexLookup[index]].Item);
            }

            return toReturn;
        }

        private HashSet<int> FirstCharSearch(char character, int inputLength)
        {
            var toReturn = new HashSet<int>();
            var index = _dataString.IndexOf(character);
            while (index > -1)
            {
                var id = _indexLookup[index];
                var item = _lookupItems[id];
                if (item.LastIndex - index + 1 >= inputLength)
                {
                    toReturn.Add(index);
                }

                if (index >= _dataString.Length - 1)
                    break;
                index = _dataString.IndexOf(character, index + 1);
            }

            return toReturn;
        }

        private HashSet<int> NextCharSearch(char character, int inputRemainingLength, HashSet<int> matchingLastIndexes)
        {
            var toReturn = new HashSet<int>();
            foreach (var index in matchingLastIndexes)
            {
                if (index + inputRemainingLength < _dataString.Length && _dataString[index + 1] == character)
                {
                    var id = _indexLookup[index];
                    var item = _lookupItems[id];
                    if (id == _indexLookup[index + 1] && item.LastIndex - index >= inputRemainingLength)
                    {
                        toReturn.Add(index + 1);
                    }
                }
            }

            return toReturn;
        }

        private class LookupItem<TItem>
        {
            public int LastIndex { get; set; }
            public TItem Item { get; set; }
        }
    }
}
