using System;
using System.Collections.Generic;
using System.Text;

namespace MyCompilerWPF
{
    class CLexicalAnalyzer
    {
        private string code = string.Empty;
        private СInputOutputModule ioModule;
        public CLexicalAnalyzer(string codeIn)
        {
            code = codeIn;
            ioModule = new СInputOutputModule(code);
        }
        public CToken getNextToken()
        {
            //get new Letter from iomodule and analyze it CODE.......
            return new CToken(1);
        }
    }
}
