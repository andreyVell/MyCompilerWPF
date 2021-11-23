using System;

namespace MyCompilerWPF
{
    class CLexicalAnalyzer
    {        
        private СInputOutputModule ioModule;
        private char curLetter;
        private string curSymbol = string.Empty;
        private bool needToReadNewLetter = true;
        public CLexicalAnalyzer(СInputOutputModule io)
        {            
            ioModule = io;
        }
        public CToken GetNextToken()
        {
            try
            {//get new Letter from iomodule and analyze it.......                
                while (true)
                {
                    curSymbol = string.Empty;
                    if (needToReadNewLetter)
                    {
                        curLetter = ioModule.GetNextLetter();
                        curSymbol += curLetter;
                    }
                    else
                        curSymbol += curLetter;
                    needToReadNewLetter = true;
                    //try to pasrse to ttValue
                    if (curLetter == Convert.ToChar("'")) 
                    {
                        curSymbol = string.Empty;
                        while (true)
                        {
                            curLetter = ioModule.GetNextLetter();
                            if (curLetter == Convert.ToChar("'"))
                                if (curSymbol.Length == 1)
                                    return new CToken(curSymbol[0]);
                                else
                                    return new CToken(curSymbol);
                            curSymbol += curLetter;
                        }
                    }//string or char
                    if (curLetter >= '0' && curLetter <= '9')
                    {
                        curLetter = ioModule.GetNextLetter();
                        needToReadNewLetter = false;
                        if (curLetter != 'e' && Char.ToLower(curLetter) >= 'a' && Char.ToLower(curLetter) <= 'z')
                            ioModule.error("Invalid identifier name");
                        else
                        {
                            while ((curLetter >= '0' && curLetter <= '9') || curLetter == '.' || Char.ToLower(curLetter) == 'e' || ((curLetter == '+' || curLetter == '-') && Char.ToLower(curSymbol[curSymbol.Length - 1]) == 'e'))
                            {
                                if (curLetter == '.')
                                    curSymbol += ',';
                                else
                                    curSymbol += curLetter;
                                curLetter = ioModule.GetNextLetter();
                                needToReadNewLetter = false;
                            }
                            if (curSymbol.Contains(',') || curSymbol.Contains('e') || curSymbol.Contains('E'))
                            {
                                try
                                {
                                    return new CToken(double.Parse(curSymbol));
                                }
                                catch (Exception exc)
                                {
                                    ioModule.error(exc.Message);
                                }
                            }
                            else
                            {
                                try
                                {
                                    return new CToken(int.Parse(curSymbol));
                                }
                                catch (Exception exc)
                                {
                                    ioModule.error(exc.Message);
                                }
                            }
                        }
                    } //integer or real
                    //try to parse to ttOper or Boolean, all that remains is ttIdent (or error)
                    switch (Char.ToLower(curLetter))
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
                            while (true)
                            {
                                curLetter = ioModule.GetNextLetter();
                                if ((curLetter >= 'A' && curLetter <= 'Z') || (curLetter >= 'a' && curLetter <= 'z') || (curLetter >= '0' && curLetter <= '9') || curLetter == '_')
                                    curSymbol += curLetter;
                                else
                                {
                                    needToReadNewLetter = false;
                                    break;
                                }
                            }
                            switch (curSymbol.ToLower())
                            {
                                case "program":
                                    return new CToken(EOperator.programsy);
                                case "true":
                                    return new CToken(true);
                                case "false":
                                    return new CToken(false);
                                case "packed":
                                    return new CToken(EOperator.packedsy);
                                case "integer":
                                    return new CToken(EOperator.integersy);
                                case "real":
                                    return new CToken(EOperator.realsy);
                                case "char":
                                    return new CToken(EOperator.charsy);
                                case "string":                                    
                                    return new CToken(EOperator.stringsy);
                                case "boolean":
                                    return new CToken(EOperator.booleansy);
                                case "if":
                                    return new CToken(EOperator.ifsy);
                                case "in":
                                    return new CToken(EOperator.insy);
                                case "do":
                                    return new CToken(EOperator.dosy);
                                case "div":
                                    return new CToken(EOperator.divsy);
                                case "downto":
                                    return new CToken(EOperator.downtosy);
                                case "of":
                                    return new CToken(EOperator.ofsy);
                                case "or":
                                    return new CToken(EOperator.orsy);
                                case "to":
                                    return new CToken(EOperator.tosy);
                                case "then":
                                    return new CToken(EOperator.thensy);
                                case "end":
                                    return new CToken(EOperator.endsy);
                                case "else":
                                    return new CToken(EOperator.elsesy);
                                case "var":
                                    return new CToken(EOperator.varsy);
                                case "and":
                                    return new CToken(EOperator.andsy);
                                case "not":
                                    return new CToken(EOperator.notsy);
                                case "nil":
                                    return new CToken(EOperator.nilsy);
                                case "file":
                                    return new CToken(EOperator.filesy);
                                case "mod":
                                    return new CToken(EOperator.modsy);
                                case "set":
                                    return new CToken(EOperator.setsy);
                                case "goto":
                                    return new CToken(EOperator.gotosy);
                                case "while":
                                    return new CToken(EOperator.whilesy);
                                case "begin":
                                    return new CToken(EOperator.beginsy);
                                case "label":
                                    return new CToken(EOperator.labelsy);
                                default:
                                    if (curSymbol.Length == 1 && !((curSymbol[0] >= 'A' && curSymbol[0] <= 'Z') || (curSymbol[0] >= 'a' && curSymbol[0] <= 'z') || curSymbol[0] == '_'))
                                    {
                                        ioModule.error("Unexpected symbol!");
                                        break;
                                    }
                                    else
                                        return new CToken(curSymbol, 1);
                            }
                            break;
                    }
                }
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message+"\n"+ioModule.errorOutput());
            }
        }
    }
}