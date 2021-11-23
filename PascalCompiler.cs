using System;

namespace MyCompilerWPF
{
    class PascalCompiler
    {
        private string input = string.Empty;
        private string output = string.Empty;
        private СInputOutputModule ioModule;
        private CLexicalAnalyzer lexer;        
        public void Compilate(string pascalCode)
        {
            input = pascalCode;
            ioModule = new СInputOutputModule(input + " ");
            lexer = new CLexicalAnalyzer(ioModule);
            CToken curToken;
            try
            {
                while (true)
                {
                    curToken = lexer.GetNextToken();
                    if (curToken.tokenType == ETokenType.ttIdent)
                        output += curToken.identName;
                    output += curToken.tokenType.ToString() + ' ';
                }
            }
            catch (Exception exc)
            {
                if (exc.Message.Contains("error"))
                    output = "";
                output += exc.Message;
            }
        }
        public string GetResult()
        {
            return output;
        }
    }
}
