clear all;
close all;
clc;

cadena=['j' 'o' 'r' 'g' 'e' ' ' ...
    'a' 'n' 's' 'e' 'l' 'm' 'o' ' ' ...
    'a' 'l' 'v' 'a' 'r' 'a' 'd' 'o' ' ' ...
    'b' 'a' 'l' 'b' 'u' 'e' 'n' 'a' ' ' ...
    'p' 'a' 't' 'i' 'l' 'l' 'a'];
simbolos=unique(cadena);    % Obtenemos los simbolos fuente de la cadena

propbabilidades=zeros(1, length(simbolos));
j=1;
for i=simbolos
    prob=histc(cadena, i)/length(cadena);   % Calculamos la probabilidades
    propbabilidades(j)=prob;    % y agregamos al vector
    j=j+1;
end
% Mapeamos los simbolos al codigo ascii
mapSimbolos=double(simbolos);
% Obtenemos el diccionario
dicc=huffmandict(mapSimbolos,propbabilidades);
% Codificacio de la palabra
palabra=['e' 's' 'p' 'o' 'n' 't' 'a' 'n' 'e' 'i' 'd' 'a' 'd'];
mapPalabra=double(palabra);
cadena_cod=huffmanenco(mapPalabra,dicc);