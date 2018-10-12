using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceiptLotteryCheck
{
    class LotteryNumberJudgment
    {
        #region Vriable
        Dictionary<string, int> prizeResult;
        List<string> userData;
        #endregion

        #region Constructor
        public LotteryNumberJudgment()
        {
            prizeResult = new Dictionary<string, int>();
        }
        public LotteryNumberJudgment(List<string> ouserData) 
        {
            prizeResult = new Dictionary<string, int>();
            userData = new List<string>(ouserData.Select(x => x));
        }
        #endregion

        #region Property
        public Dictionary<string, int> decidePrizeType(List<string>prizeNumerList)
        {
            List<string> prizeList = new List<string>();
            addToPrizeResult(searchSpecialPrize(prizeNumerList[0]), PrizeTypeDefinition.specialPrize);
            addToPrizeResult(searchGrandPrize(prizeNumerList[1]), PrizeTypeDefinition.grandPrize);
            prizeList = InserttoFirstPrize(prizeNumerList);
            addToPrizeResult(searchFisrtPrize(prizeList), PrizeTypeDefinition.firstPrize);
            addToPrizeResult(searchTwoPrize(prizeList), PrizeTypeDefinition.twoPrize);
            addToPrizeResult(searchThreePrize(prizeList), PrizeTypeDefinition.threePrize);
            addToPrizeResult(searchFourPrize(prizeList), PrizeTypeDefinition.fourPrize);
            addToPrizeResult(searchFivePrize(prizeList), PrizeTypeDefinition.fivePrize);
            addToPrizeResult(searchSixPrize(prizeList), PrizeTypeDefinition.sixPrize);
            prizeList = InserttoExtraPrize(prizeNumerList);
            addToPrizeResult(searchExtraPrize(prizeList), PrizeTypeDefinition.sixPrize);
            return prizeResult;
        }

        private List<string> searchSpecialPrize(string prizeNumer)
        {
            List<string> result = new List<string>();
            result = userData.FindAll(x => x == prizeNumer);
            removeWonNumber(result);
            return result;
        }

        private List<string> searchGrandPrize(string prizeNumer)
        {
            List<string> result = new List<string>();
            result = userData.FindAll(x => x == prizeNumer);
            removeWonNumber(result);
            return result;
        }

        private List<string> InserttoFirstPrize(List<string> prizeNumerList)
        {
            List<string> result = new List<string>();
            for (int i = 2; i < 5; i++)
            {
                result.Add(prizeNumerList[i]);
            }
            return result;
        }

        private List<List<string>> searchFisrtPrize(List<string> prizeNumer)
        {
            List<string> wonPrize = new List<string>();
            List<List<string>> result = new List<List<string>>();
            foreach (string number in prizeNumer)
            {
                wonPrize = userData.FindAll(x => x == number);
                result.Add(wonPrize);
                removeWonNumber(wonPrize);
            }
            return result;
        }

        private List<List<string>> searchTwoPrize(List<string> prizeNumer)
        {
            List<string> wonPrize = new List<string>();
            List<List<string>> result = new List<List<string>>();
            foreach (string number in prizeNumer)
            {
                string s = number.Remove(0, 1);
                wonPrize = userData.FindAll(x => x.Substring(1, x.Length - 1) == s);
                result.Add(wonPrize);
                removeWonNumber(wonPrize);
            }
            return result;
        }

        private List<List<string>> searchThreePrize(List<string> prizeNumer)
        {
            List<string> wonPrize = new List<string>();
            List<List<string>> result = new List<List<string>>();
            foreach (string number in prizeNumer)
            {
                string s = number.Remove(0, 2);
                wonPrize = userData.FindAll(x => x.Substring(2, userData[0].Length - 2) == s);
                result.Add(wonPrize);
                removeWonNumber(wonPrize);
            }
            return result;
        }

        private List<List<string>> searchFourPrize(List<string> prizeNumer)
        {
            List<string> wonPrize = new List<string>();
            List<List<string>> result = new List<List<string>>();
            foreach (string number in prizeNumer)
            {
                string s = number.Remove(0, 3);
                wonPrize = userData.FindAll(x => x.Substring(3, x.Length - 3) == s);
                result.Add(wonPrize);
                removeWonNumber(wonPrize);
            }
            return result;
        }

        private List<List<string>> searchFivePrize(List<string> prizeNumer)
        {
            List<string> wonPrize = new List<string>();
            List<List<string>> result = new List<List<string>>();
            foreach (string number in prizeNumer)
            {
                string s = number.Remove(0, 4);
                wonPrize = userData.FindAll(x => x.Substring(4, x.Length - 4) == s);
                result.Add(wonPrize);
                removeWonNumber(wonPrize);
            }
            return result;
        }

        private List<List<string>> searchSixPrize(List<string> prizeNumer)
        {
            List<string> wonPrize = new List<string>();
            List<List<string>> result = new List<List<string>>();
            foreach (string number in prizeNumer)
            {
                string s = number.Remove(0, 5);
                wonPrize = userData.FindAll(x => x.Substring(5, x.Length - 5) == s);
                result.Add(wonPrize);
                removeWonNumber(wonPrize);
            }
            return result;
        }

        private List<string> InserttoExtraPrize(List<string> prizeNumerList)
        {
            List<string> result = new List<string>();
            for (int i = 5; i < prizeNumerList.Count; i++)
            {
                result.Add(prizeNumerList[i]);
            }
            return result;
        }

        private List<List<string>> searchExtraPrize(List<string> prizeNumer)
        {
            List<string> wonPrize = new List<string>();
            List<List<string>> result = new List<List<string>>();
            foreach (string number in prizeNumer)
            {
                wonPrize = userData.FindAll(x => x.Substring(5, x.Length - 5) == number);
                result.Add(wonPrize);
                removeWonNumber(wonPrize);
            }
            return result;
        }

        private void addToPrizeResult(List<string> result, int prizeType)
        {
            foreach(string prizenum in  result)
            {
                if (prizeResult.ContainsKey(prizenum))
                    prizeResult[prizenum] = prizeType;
                else
                    prizeResult.Add(prizenum, prizeType);
            }
        }

        private void addToPrizeResult(List<List<string>> result, int prizeType)
        {
            foreach (List<string> prizenumList in result)
            {
                foreach (string prizenum in prizenumList)
                {
                    if (prizeResult.ContainsKey(prizenum))
                        prizeResult[prizenum] = prizeType;
                    else
                        prizeResult.Add(prizenum, prizeType);
                }
            }
        }

        private void removeWonNumber(List<string> wonNumberList)
        {
            foreach (string s in wonNumberList)
            {
                userData.Remove(s);
            }
        }
        #endregion

    }
}
