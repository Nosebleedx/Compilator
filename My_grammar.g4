grammar My_grammar;

prog
    : block '.' EOF
    ;

block
    : consts? vars_? procedure* stat
    ;

consts
    : CONST ident '=' number (',' ident '=' number)* ';'
    ;

vars_
    : VAR ident (',' ident)* ';'
    ;

procedure
    : PROCEDURE ident ';' block ';'
    ;

stat
    : (assignstmt | callstmt | writestmt | qstmt | beginstmt | ifstmt | whilestmt | printstmt)?
    ;

printstmt 
    : '!' expression ;

assignstmt
    : ident ':=' expression
    ;

callstmt
    : CALL ident
    ;

writestmt
    : WRITE ident
    ;

qstmt
    : '?' ident
    ;


beginstmt
    : BEGIN stat (';' stat)* END
    ;

ifstmt
    : IF condition THEN stat
    ;

whilestmt
    : WHILE condition DO stat
    ;

condition
    : ODD expression
    | expression ('=' | '#' | '<' | '<=' | '>' | '>=') expression
    ;

expression
    : ('+' | '-')? term (('+' | '-') term)*
    ;

term
    : factor (('*' | '/') factor)*
    ;

factor
    : ident
    | number
    | '(' expression ')'
    ;

ident
    : STRING
    ;

number
    : NUMBER
    ;

WRITE
    : 'WRITE'
    ;

WHILE
    : 'WHILE'
    ;

DO
    : 'DO'
    ;

IF
    : 'IF'
    ;

THEN
    : 'THEN'
    ;

ODD
    : 'ODD'
    ;

BEGIN
    : 'BEGIN'
    ;

END
    : 'END'
    ;

CALL
    : 'CALL'
    ;

CONST
    : 'CONST'
    ;

VAR
    : 'VAR'
    ;

PROCEDURE
    : 'PROCEDURE'
    ;

STRING
    : [a-zA-Z] [a-zA-Z]*
    ;

NUMBER
    : [0-9]+
    ;

WS
    : [ \t\r\n] -> skip
    ;
