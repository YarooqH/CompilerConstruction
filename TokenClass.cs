public enum TokenClass {
    ID,             // Identifier
    DT,             // DataType
    IF,             // If - Else
    ELSE,
    WHILE,          // Loops 
    FOR,
    BREAK,          // Break
    CONT,           // Continue
    PM,              // Plus - Minus
    MDM,             // Multiply - Divide - Modulus
    COMP,            // Comparison Operators
    ASIGN,            // Assignment Operators
    NOT,            // Boolean Operators
    AND,
    OR,
    DO,             // Do - While
    FUNC,           // Function Declaration
    SEMICOL,             // Semi-Colon
    COL,             // Col
    COM,             // Comma
    DOT,            // Dot
    ORB,             // OPEN ROUND BRACKET
    CRB,             // ROUND BRACKET ClOSE
    OSB,             // OPEN SQUARE BRACKETS
    CSB,             // SQUARE BRACKET CLOSE
    OCB,             // OPEN CURRLY BRACKET
    CCB,             // CLOSE CURRLY BRACKET
    NUM,            // Number DataType for Int and Float
    SC,              // STRING CONSTANT
    CC,              // CHAR CONSTANT
    CONST,          // Final Declaration
    LET,            // Variable Declarion

    // FC,              // fLOAT CONSTANT
    // IC,              // INT CONSTANT
    /*  
        // OOP
    */   

    AM,             // Access Modifiers
    EXT,           // Extends
    ABS,            // Abstract
    AVAIL,            // Static
    NEW,            // New for constructor
    CLASS,            // Class
    CURR,           // this
    PARENT,            // Super
    CONSTRUCTOR,        // Constructor are explicitly mentioned
    INTERFACE,          // interface
    NULL,               // DataType
    ERR,             // Invalid Lexeme
    RETURN,         // Inside function
    RETURNS         // Function Declaration
}

