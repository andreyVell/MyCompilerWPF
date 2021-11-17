using System;
using System.Windows;

namespace MyCompilerWPF
{
    class CLexicalAnalyzer
    {
        private string code = string.Empty;
        private СInputOutputModule ioModule;
        private char curLetter;
        private string curSymbol = string.Empty;
        private bool needToReadNewLetter=true;
        public CLexicalAnalyzer(string codeIn)
        {
            code = codeIn;
            ioModule = new СInputOutputModule(code);
        }
        public CToken GetNextToken()
        {
            try
            {//get new Letter from iomodule and analyze it CODE.......                
                while (true)
                {
                    if (needToReadNewLetter)
                    {
                        curLetter = ioModule.GetNextLetter();
                        curSymbol += curLetter;
                    }
                    needToReadNewLetter = true;
                    //MessageBox.Show(curLetter.ToString());
                    switch (curLetter)
                    {
                        case ' ':
                            break;
                        case '\n':
                            break;
                        case '\r':
                            break;
                        case '+': 
                                return new CToken(EOperator.plussy);
                        case '/':                            
                                return new CToken(EOperator.slashsy);                            
                        case '=':
                                return new CToken(EOperator.equalsy);
                        case ',':
                                return new CToken(EOperator.commasy);
                        case ';':
                                return new CToken(EOperator.semicolonsy);
                        case '^':
                                return new CToken(EOperator.arrowsy);
                        case ')':
                                return new CToken(EOperator.rightparsy);
                        case '[':
                                return new CToken(EOperator.lbracketsy);
                        case '*':
                                return new CToken(EOperator.starsy);
                        case ']':
                                return new CToken(EOperator.rbracketsy);
                        case '-':
                                return new CToken(EOperator.minussy);
                        case '{':
                            while (ioModule.GetNextLetter() != '}') ;
                            break;
                        case ':':
                            curLetter = ioModule.GetNextLetter();
                            switch (curLetter)
                            {                                
                                case '=':                                    
                                        return new CToken(EOperator.assignsy);
                                default:
                                    needToReadNewLetter = false;
                                    return new CToken(EOperator.colonsy);
                            }
                        case '.':
                            curLetter = ioModule.GetNextLetter();
                            switch (curLetter)
                            {                                
                                case '.':                                    
                                        return new CToken(EOperator.twopointssy);
                                default:
                                    needToReadNewLetter = false;
                                    return new CToken(EOperator.pointsy);
                            }
                        case '(':
                            curLetter = ioModule.GetNextLetter();
                            switch (curLetter)
                            {                                
                                case '*':
                                    while (ioModule.GetNextLetter() != '*' && ioModule.GetNextLetter() != ')') ;
                                    break;
                                default:
                                    needToReadNewLetter = false;
                                    return new CToken(EOperator.leftparsy);
                            }
                            break;
                        case '>':
                            curLetter = ioModule.GetNextLetter();
                            switch (curLetter)
                            {                                
                                case '=':                                    
                                        return new CToken(EOperator.greaterequalsy);
                                default:
                                    needToReadNewLetter = false;
                                    return new CToken(EOperator.greatersy);
                            }
                        case '<':
                            curLetter = ioModule.GetNextLetter();
                            switch (curLetter)
                            {    
                                case '=': 
                                    return new CToken(EOperator.laterequalsy);
                                case '>':
                                    return new CToken(EOperator.latergreatersy);
                                default:
                                    needToReadNewLetter = false;
                                    return new CToken(EOperator.latersy);
                            }
                        default:
                            ioModule.error("Unexpected symbol!");
                            break;
                    }
                }
            }
            catch (Exception exc)
            {
                //MessageBox.Show(curSymbol);
                throw new Exception(((exc.Message)+"\n"+(ioModule.errorOutput())));
            }
        }        
    }
}
