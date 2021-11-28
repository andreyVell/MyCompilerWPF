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
        tofixsomebugs, //dont know)))
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
        integersy,
        realsy,
        charsy,
        stringsy,
        booleansy,
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
        modsy,
        nilsy,
        setsy,
        thensy,
        elsesy,
        filesy,
        gotosy,
        beginsy,
        whilesy,
        programsy,
    }
    enum EValType
    {
        vtInt = 0x1,
        vtReal = 0x2,
        vtString = 0x4,
        vtChar = 0x8,
        vtBoolean
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
        public bool bvalue { get; private set; }
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
        public CToken(bool value) //ttValue Boolean
        {
            bvalue = value;
            tokenType = ETokenType.ttValue;
            valType = EValType.vtBoolean;
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
        public static bool operator ==(CToken a, CToken b)
        {
            if (System.Object.ReferenceEquals(a, b))
                return true;
            if ((object)a == null || (object)b == null)
                return false;
            if (a.tokenType != b.tokenType)
                return false;
            else
            {
                switch (a.tokenType)
                {
                    case ETokenType.ttIdent:
                        return (a.identName == a.identName);
                    case ETokenType.ttOper:
                        return (a.operation == b.operation);
                    case ETokenType.ttValue:
                        {
                            if (a.valType == b.valType)
                                switch (a.valType)
                                {
                                    case EValType.vtInt:
                                        return (a.ivalue == b.ivalue);
                                    case EValType.vtReal:
                                        return (a.dvalue == b.dvalue);
                                    case EValType.vtChar:
                                        return (a.cvalue == b.cvalue);
                                    case EValType.vtString:
                                        return (a.svalue == b.svalue);
                                    case EValType.vtBoolean:
                                        return (a.bvalue == b.bvalue);
                                    default:
                                        return false;
                                }
                            else
                                if (a.valType == EValType.vtInt && b.valType == EValType.vtReal)
                                return a.ivalue == b.dvalue;
                            else
                                    if (a.valType == EValType.vtReal && b.valType == EValType.vtInt)
                                return a.dvalue == b.ivalue;
                            else
                                return false;
                        }
                    default:
                        return false;
                }
            }
        }
        public static bool operator !=(CToken a, CToken b)
        {
            return !(a == b);
        }
        public object GetValue()
        {
            if (this.tokenType==ETokenType.ttValue)
                switch(this.valType)
                {
                    case EValType.vtInt:
                        return ivalue;
                    case EValType.vtReal:
                        return dvalue;
                    case EValType.vtChar:
                        return cvalue;
                    case EValType.vtString:
                        return svalue;
                    case EValType.vtBoolean:
                        return bvalue;
                }
            return null;
        }
        public string GetTokenContent()
        {            
            switch(this.tokenType)
            {
                case ETokenType.ttIdent:
                    return "(Identifier name)";
                case ETokenType.ttOper:
                    return '('+operation.ToString().Substring(0, operation.ToString().Length-2)+')';
                case ETokenType.ttValue:
                    switch (this.valType)
                    {
                        case EValType.vtInt:
                            return '(' + ivalue.ToString() + ')';
                        case EValType.vtReal:
                            return '(' + dvalue.ToString() + ')';
                        case EValType.vtChar:
                            return '(' + cvalue.ToString() + ')';
                        case EValType.vtString:
                            return '(' + svalue + ')';
                        case EValType.vtBoolean:
                            return '(' + bvalue.ToString() + ')';
                    }
                    break;
            }
            return "()";
        }
    }
}