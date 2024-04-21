grammar My_grammar;

prog: initialization;

initialization: TYPE ID;

TYPE: INTEGER | FLOAT ;

LINE: '"' (LETTER | PUNCTUATION | EMPTY | DIGIT)+ '"';

ID: LETTER (LETTER | DIGIT)*;

NUMBER: ('-')? DIGIT+ ('.' DIGIT+)?;

fragment DIGIT : [0-9];
fragment LETTER : [a-zA-Z];
fragment PUNCTUATION : [!.,;:];

LPAREN: '(';
RPAREN: ')';

INTEGER: 'int';
FLOAT: 'float';

ADD: '+';
SUB: '-';
SEP: '/';
MUL: '*';

EQ_EQ: '==';
NOT_EQ: '!=';
GT: '>';
LT: '<';
LE : '<=';
GE: '>=';
ASSIGNMENT : '=';

END : ';';

EMPTY: [ \n\t\r] -> skip;