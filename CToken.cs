using System;
using System.Collections.Generic;
using System.Text;

namespace MyCompilerWPF
{
    enum ETokenType
    {
        ttOper,
        ttIdent,
        ttValue
    }
    enum EOperator
    {
        //Operators
        starsy, // *
        slashsy, // /
        equalsy, // =
        commasy, // ,
        semicolonsy, // ;
        colonsy, // :
        pointsy, // .
        arrowsy, // ^
        leftparsy, // (
        rightparsy, // )
        lbracketsy, // [
        rbracketsy, // ]        
        latersy, // <
        greatersy, // >
        laterequalsy, // <=
        greaterequalsy, // >=
        latergreatersy, // <>
        plussy, // +
        minussy, // -        
        assignsy, // :=
        twopointssy, // ..
        //key words
        intsy,
        realsy,
        charsy,
        stringsy,
        ifsy,
        dosy,
        ofsy,
        orsy,
        insy,
        tosy,
        endsy,
        varsy,
        divsy,
        andsy,
        notsy,
        forsy,
        modsy,
        nilsy,
        setsy,
        thensy,
        elsesy,
        casesy,
        filesy,
        gotosy,
        typesy,
        withsy,
        beginsy,
        whilesy,
        arraysy,
        constsy,
        labelsy,
        untilsy,
        downtosy,
        packedsy,
        recordsy,
        repeatsy,
        programsy,
        functionsy,
        proceduresy,
    }
    enum EValType
    {
        vtInt = 0x1,
        vtReal = 0x2,
        vtString = 0x4,
        vtChar = 0x8
    }
    class CToken    
    {        
        public ETokenType tokenType { get; private set; }
        public EOperator operation { get; private set; }
        public EValType valType { get; private set; }
        public string identName { get; private set; }
        public int ivalue { get; private set; }
        public double dvalue { get; private set; }
        public string svalue { get; private set; }
        public char cvalue { get; private set; }
        public CToken(int value) //ttValue int
        {
            ivalue = value;
            tokenType = ETokenType.ttValue;
            valType = EValType.vtInt;
        }
        public CToken(double value) //ttValue real
        {
            dvalue = value;
            tokenType = ETokenType.ttValue;
            valType = EValType.vtReal;
        }
        public CToken(char value) //ttValue char
        {
            cvalue = value;
            tokenType = ETokenType.ttValue;
            valType = EValType.vtChar;
        }
        public CToken(string value) //ttValue string
        {
            svalue = value;
            tokenType = ETokenType.ttValue;
            valType = EValType.vtString;
        }
        public CToken(EOperator op) //ttOper
        {
            operation = op;
            tokenType = ETokenType.ttOper;
        }
        public CToken(string userIdentName,int ident) //ttIdent
        {
            identName = userIdentName;
            tokenType = ETokenType.ttIdent;
        }
    }
}