using System;
using System.Collections.Generic;
using System.Linq;

using httpmock.stringEvaluator.decoder.token;

namespace  httpmock.stringEvaluator.decoder
{
    public class EvaluatorDecoder {
        private static Dictionary<string, IStringEvaluator> decodeCache = new Dictionary<string, IStringEvaluator>();
        public static IStringEvaluator decode(string syntax) {
            List<IToken> tokens = parseToTokens(syntax);
            if (decodeCache.ContainsKey(syntax)) {
                return decodeCache[syntax];
            }
            return decodeCache[syntax] = combineTokens(tokens);
        }

        private static List<IToken> parseToTokens(string syntax) {
            string s = syntax ?? "";
            var tokens = new List<IToken>();

            IToken crurrentToken = new RootToken();

            int iPos = 0;

            while (s.Length > 0) {
                IToken nextToken = null;
                foreach (var token in crurrentToken.getFollowableTokens()) {
                    int i = token.getTokenEndIndex(s);
                    if (i >= 0) {
                        var tokenValue = s.Substring(0, i);
                        nextToken = token.newToken();
                        nextToken.setToken(tokenValue);
                        s = s.Substring(i);
                        iPos += i;
                        break;
                    }
                }

                if (nextToken == null) {
                    throw new FormatException("Unknown token. position: " + iPos + ". Unkown token: `" + s + "`");
                }
                crurrentToken = nextToken;
                tokens.Add(nextToken);
            }

            return tokens;
        }
        
        private static IStringEvaluator combineTokens(List<IToken> tokens) {
            var valStack = new Stack<IStringEvaluator>();
            var opeStack = new Stack<OperatorEvaluator>();
            bool valueLasted = false;

            Action<OperatorEvaluator> fnResolveOperator = (OperatorEvaluator ope) => {
                ope.Right = valStack.Pop();
                ope.Left = valStack.Pop();
                valStack.Push(ope);
            };

            foreach (var token in tokens) {
                IStringEvaluator evaluator = token.getEvaluator();
                if (evaluator is ValueEvaluator) {
                    if (valueLasted) {
                        throw new FormatException("Value expression duplicated.");
                    }
                    valStack.Push(evaluator);
                    valueLasted = true;
                } else if (evaluator is OperatorEvaluator) {
                    OperatorEvaluator ope = (OperatorEvaluator) evaluator;
                    if (!valueLasted) {
                        throw new FormatException("Operator expression duplicated.");
                    }
                    if (opeStack.Count > 0) {
                        if (opeStack.Last().getPriority() <= ope.getPriority()) {
                            fnResolveOperator(opeStack.Pop());
                        }
                    }
                    opeStack.Push(ope);
                    valueLasted = false;
                }
            }

            while (opeStack.Count > 0) {
                fnResolveOperator(opeStack.Pop());
            }

            if (valStack.Count == 0) {
                throw new FormatException("Operator expression is none.");
            }

            return valStack.Last();
        }
    }
}
