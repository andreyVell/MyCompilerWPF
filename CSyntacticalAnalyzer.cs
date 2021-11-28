using System;
using System.Windows;

namespace MyCompilerWPF
{
    class CSyntacticalAnalyzer
    {
        private CInputOutputModule ioModule;
        private CLexicalAnalyzer lexer;
        private CToken curToken = null;
        bool needToNextToken = true;
        bool needToAcceptToken = true;
        public CSyntacticalAnalyzer(CInputOutputModule io, CLexicalAnalyzer lex)
        {
            ioModule = io;
            lexer = lex;
        }
        private bool Accept(CToken expectedToken)
        {
            try
            {
                if (needToNextToken)
                    curToken = lexer.GetNextToken();
                if (needToAcceptToken)
                {
                    if (expectedToken == curToken)
                    {
                        needToNextToken = true;
                        return true;
                    }
                    else
                    {
                        needToNextToken = false;
                        NeutralizeError();
                        ioModule.error("Met " + curToken.GetTokenContent() + ", but expected " + expectedToken.GetTokenContent());
                        return false;
                    }
                }
                else
                {
                    if (IsTokenIsKeyWord(curToken))
                    {
                        needToAcceptToken = true;
                        needToNextToken = false;
                        return Accept(curToken);
                    }
                    else
                        return false;
                }
            }
            catch (Exception exc)
            {                
                if (expectedToken.operation == EOperator.pointsy)
                    ioModule.error("Expected " + expectedToken.GetTokenContent());
                throw new Exception(ioModule.errorOutput());
            }
        }
        private bool Accept(ETokenType expectedType)
        {
            try
            {
                if (needToNextToken)
                    curToken = lexer.GetNextToken();
                if (needToAcceptToken)
                {
                    if (expectedType == curToken.tokenType)
                    {
                        needToNextToken = true;
                        return true;
                    }
                    else
                    {
                        needToNextToken = false;
                        NeutralizeError();
                        ioModule.error("Met " + curToken.GetTokenContent() + ", but expected (" + expectedType.ToString().Substring(2) + ')');
                        return false;
                    }
                }
                else
                {
                    if (IsTokenIsKeyWord(curToken))
                    {
                        needToAcceptToken = true;
                        needToNextToken = false;
                        return Accept(curToken);
                    }
                    else
                        return false;
                }
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message + "\n" + ioModule.errorOutput());
            }
        }
        private void GetNextTokenManualy()
        {
            if (needToNextToken)
            {
                try
                {
                    curToken = lexer.GetNextToken();
                    needToNextToken = false;
                }
                catch (Exception exc)
                {
                    throw new Exception(exc.Message + "\n" + ioModule.errorOutput());
                }
            }
        }
        private void NeutralizeError()
        {
            needToAcceptToken = false;
        }
        private bool IsTokenIsKeyWord(CToken verifiableToken)
        {
            if (verifiableToken.operation == EOperator.programsy)
                return true;
            if (verifiableToken.operation == EOperator.varsy)
                return true;
            if (verifiableToken.operation == EOperator.beginsy)
                return true;
            if (verifiableToken.operation == EOperator.ifsy)
                return true;
            if (verifiableToken.operation == EOperator.elsesy)
                return true;
            if (verifiableToken.operation == EOperator.thensy)
                return true;
            if (verifiableToken.operation == EOperator.whilesy)
                return true;
            if (verifiableToken.operation == EOperator.dosy)
                return true;
            if (verifiableToken.operation == EOperator.endsy)
                return true;
            return false;
        }
        public void Program() //<программа>
        {            
            Accept(new CToken(EOperator.programsy));
            Name();
            Accept(new CToken(EOperator.semicolonsy));
            Block();
            Accept(new CToken(EOperator.pointsy));            
            throw new Exception(ioModule.errorOutput());
        }
        private void Name() //<имя>
        {
            Accept(ETokenType.ttIdent);
        }
        private void Block() //<блок>
        {
            VariableSection();
            OperatorsSection();
        }
        private void VariableSection() //<раздел переменных>
        {
            GetNextTokenManualy();
            if (curToken.operation == EOperator.varsy)
                if (Accept(new CToken(EOperator.varsy)))
                {
                    DescSameVariables();
                    Accept(new CToken(EOperator.semicolonsy));
                    GetNextTokenManualy();
                    while (curToken.tokenType == (ETokenType.ttIdent)) 
                    {
                        DescSameVariables();
                        Accept(new CToken(EOperator.semicolonsy));
                        GetNextTokenManualy();
                    }
                }
        }
        private void DescSameVariables() //<описание однотипных переменных>
        {
            Name();
            GetNextTokenManualy();
            while (curToken == (new CToken(EOperator.commasy)))
            {
                Accept(new CToken(EOperator.commasy));
                Name();
                GetNextTokenManualy();
            }
            Accept(new CToken(EOperator.colonsy));
            Type();
        }
        private void Type() //<тип>
        {
            SimpleType();
        }
        private void SimpleType() //<простой тип>
        {
            TypeName();
        }
        private void TypeName() //<имя типа>
        {
            GetNextTokenManualy();
            if (curToken.operation==EOperator.integersy)
            {
                Accept(new CToken(EOperator.integersy));
                return;
            }
            if (curToken.operation == EOperator.realsy)
            {
                Accept(new CToken(EOperator.realsy));
                return;
            }
            if (curToken.operation == EOperator.charsy)
            {
                Accept(new CToken(EOperator.charsy));
                return;
            }
            if (curToken.operation == EOperator.stringsy)
            {
                Accept(new CToken(EOperator.stringsy));
                return;
            }
            if (curToken.operation == EOperator.booleansy)
            {
                Accept(new CToken(EOperator.booleansy));
                return;
            }
            ioModule.error("Expected for type name");
            NeutralizeError();
        }
        private void OperatorsSection() //<раздел операторов>
        {
            CompoundOperator();
        }
        private void CompoundOperator()// <составной оператор> 
        {
            Accept(new CToken(EOperator.beginsy));
            Operator();
            GetNextTokenManualy();
            while (curToken == (new CToken(EOperator.semicolonsy))) 
            {
                Accept(new CToken(EOperator.semicolonsy));
                Operator();
                GetNextTokenManualy();
            }            
            Accept(new CToken(EOperator.endsy));
        }
        private void Operator()// <оператор>
        {
            UnlabeledOperator();
        }
        private void UnlabeledOperator()// <непомеченный оператор> 
        {
            GetNextTokenManualy();
            if (curToken.tokenType == ETokenType.ttIdent)
            { AssignOperator();return; }
            if (curToken == (new CToken(EOperator.beginsy)))
            { CompoundOperator(); return; }
            if (curToken == (new CToken(EOperator.ifsy)))
            { ConditionalOperator(); return; }
            if (curToken == (new CToken(EOperator.whilesy)))
            { PreconditionLoop(); return; }
            return;
        }
        private void AssignOperator()// <оператор присваивания>
        {
            Variable();
            Accept(new CToken(EOperator.assignsy));
            Expression();
        }
        private void Variable()// <переменная>
        {
            FullVariable();
        }
        private void FullVariable()// <полная переменная>
        {
            VariableName();
        }
        private void VariableName()// <имя переменной>
        {
            Name();
        }
        private void Expression()// <выражение> 
        {
            SimpleExpression();
            GetNextTokenManualy();
            if (curToken==(new CToken(EOperator.equalsy)) || curToken == (new CToken(EOperator.latergreatersy)) || curToken == (new CToken(EOperator.latersy)) || curToken == (new CToken(EOperator.laterequalsy)) || curToken == (new CToken(EOperator.greaterequalsy)) || curToken == (new CToken(EOperator.greatersy)))
            {                
                RelationshipOperation();
                SimpleExpression();
            }
        }
        private void SimpleExpression()// <простое выражение> 
        {            
            Sign();
            Term();
            GetNextTokenManualy();
            while (curToken == (new CToken(EOperator.plussy)) || curToken == (new CToken(EOperator.minussy)) || curToken == (new CToken(EOperator.orsy)))
            {                
                AdditiveOperation();
                Term();
                GetNextTokenManualy();
            }
        }
        private void AdditiveOperation()// <аддитивная операция>
        {
            GetNextTokenManualy();
            if (curToken.operation == EOperator.plussy)
            {
                Accept(new CToken(EOperator.plussy));
                return;
            }
            if (curToken.operation == EOperator.minussy)
            {
                Accept(new CToken(EOperator.minussy));
                return;
            }
            if (curToken.operation == EOperator.orsy)
            {
                Accept(new CToken(EOperator.orsy));
                return;
            }
            else
            { 
                ioModule.error("Additive operation expected");
                NeutralizeError();
            }
        }
        private void RelationshipOperation()// <операция отношения>
        {
            GetNextTokenManualy();
            if (curToken.operation == EOperator.equalsy)
            { Accept(new CToken(EOperator.equalsy)); return; }
            if (curToken.operation == EOperator.latergreatersy)
            { Accept(new CToken(EOperator.latergreatersy)); return; }
            if (curToken.operation == EOperator.latersy)
            { Accept(new CToken(EOperator.latersy)); return; }
            if (curToken.operation == EOperator.laterequalsy)
            { Accept(new CToken(EOperator.laterequalsy)); return; }
            if (curToken.operation == EOperator.greaterequalsy)
            { Accept(new CToken(EOperator.greaterequalsy)); return; }
            if (curToken.operation == EOperator.greatersy)
            { Accept(new CToken(EOperator.greatersy)); return; }
            else
            {
                NeutralizeError();
                ioModule.error("Expected comparison operation"); 
            }
        }
        private void Term()// <слагаемое>
        {
            Factor();
            while (MultiplicativeOperation())            
                Factor();            
        }
        private bool MultiplicativeOperation()// <мультипликативная операция>
        {
            GetNextTokenManualy();
            if (curToken.operation == EOperator.starsy)                            
                return Accept(new CToken(EOperator.starsy)); 
            if (curToken.operation == EOperator.slashsy)
                return Accept(new CToken(EOperator.slashsy));
            if (curToken.operation == EOperator.divsy)
                return Accept (new CToken(EOperator.divsy));
            if (curToken.operation == EOperator.modsy)
                return Accept(new CToken(EOperator.modsy));
            if (curToken.operation == EOperator.andsy)
                return Accept(new CToken(EOperator.andsy));
            return false;
        }
        private void Sign() //<знак>
        {
            GetNextTokenManualy();
            if (curToken.operation == EOperator.plussy)
            { Accept(new CToken(EOperator.plussy));return; }
            if (curToken.operation == EOperator.minussy)
            { Accept(new CToken(EOperator.minussy)); return; }
            //ioModule.error("Sign expected");
        }
        private void Factor()// <множитель>
        {
            GetNextTokenManualy();
            if (curToken.tokenType == ETokenType.ttIdent)
            { Variable(); return; }
            if (curToken.tokenType == ETokenType.ttValue)
            { Accept(ETokenType.ttValue); return; }
            if (curToken.operation == EOperator.notsy)
            {
                Accept(new CToken(EOperator.notsy));
                Factor();
                return;
            }
            if (curToken.operation == EOperator.leftparsy)
            {
                Accept(new CToken(EOperator.leftparsy));
                Expression();
                Accept(new CToken(EOperator.rightparsy));
                return;
            }
            else
            {
                ioModule.error("Expected for variable or const or expression or not factor");
                NeutralizeError();
            }
        }
        private void ConditionalOperator()// <условный оператор>
        {
            Accept(new CToken(EOperator.ifsy));
            Expression();
            Accept(new CToken(EOperator.thensy));
            Operator();
            GetNextTokenManualy();
            if (curToken.operation == EOperator.elsesy)
            { Accept(new CToken(EOperator.elsesy)); Operator(); }
        }
        private void PreconditionLoop()// <цикл с предусловием>
        {            
            Accept(new CToken(EOperator.whilesy));            
            Expression();            
            Accept(new CToken(EOperator.dosy));            
            Operator();
        }
    }
}