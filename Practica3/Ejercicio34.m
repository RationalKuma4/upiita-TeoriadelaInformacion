clear all;
close all;
clc;

cadena=['j' 'o' 'r' 'g' 'e' ' ' ...
    'a' 'n' 's' 'e' 'l' 'm' 'o' ' ' ...
    'a' 'l' 'v' 'a' 'r' 'a' 'd' 'o' ' ' ...
    'b' 'a' 'l' 'b' 'u' 'e' 'n' 'a' ' ' ...
    'p' 'a' 't' 'i' 'l' 'l' 'a'];
simbolos=unique(cadena);

propbabilidades=zeros(1, length(simbolos));
j=1;
for i=simbolos
    prob=histc(cadena, i)/length(cadena);
    propbabilidades(j)=prob;
    j=j+1;
end

mapSimbolos=double(simbolos);
dicc=huffmandict(mapSimbolos,propbabilidades);

palabra=['e' 's' 'p' 'o' 'n' 't' 'a' 'n' 'e' 'i' 'd' 'a' 'd'];
mapPalabra=double(palabra);
cadena_cod=huffmanenco(mapPalabra,dicc);