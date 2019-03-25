clear all;
close all;
clc;

secuencia=[4 5 6 5 7 7 7 8 9 8 8 7 6 7 5 4 7 6 5 4 5 6 7 6 7 6 4 5 ...
    8 7 7 6 7 6 7 7 6 5 9 8 4 7 6 7 7 6 7 8 6 5];
simbolos=[4 5 6 7 8 9];

propbabilidades=zeros(1, length(simbolos)); % Vector de probabilidades
for i=4:9
    prob=histc(secuencia, i)/50;    % Probabilidad por simbolo
    propbabilidades(i-3)=prob;  % Se agrega la probabilidad al vector
end

% Diccionario
dicc=huffmandict(simbolos, propbabilidades);
% Codificacion
cadena_estudio=[4 5 5 7 6 8 7 7 9 6];
cadena_cod=huffmanenco(cadena_estudio,dicc);
% Decodificacion
cad=[1 0 1 1 0 0 1 0 0 0 0 1 0 1];
cadena_deco=huffmandeco(cad, dicc);