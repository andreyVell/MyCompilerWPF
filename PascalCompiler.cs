using System;

namespace MyCompilerWPF
{
    class PascalCompiler
    {
        private string input = string.Empty;
        private string output = string.Empty;
        private CInputOutputModule ioModule;
        private CLexicalAnalyzer lexer;
        private CSyntacticalAnalyzer synt;
        public void Compilate(string pascalCode)
        {
            input = pascalCode;
            ioModule = new CInputOutputModule(input + " ");
            lexer = new CLexicalAnalyzer(ioModule);
            synt = new CSyntacticalAnalyzer(ioModule, lexer);
            //CToken curToken;
            try
            {
                //while (true)
                //{
                //    curToken = lexer.GetNextToken();
                //    if (curToken.tokenType == ETokenType.ttIdent)
                //        output += curToken.identName;
                //    if (curToken.tokenType == ETokenType.ttValue)
                //        output += curToken.GetValue().ToString();
                //    output += curToken.tokenType.ToString() + ' ';
                //}
                synt.Program();
                //output += ioModule.errorOutput();
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
