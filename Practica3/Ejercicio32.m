clear all;
close all;
clc;

simb=[1 2 3 4];
p=[.1 .2 .3 .4];
dicc=huffmandict(simb,p);   % Obtencion de diccionario
dicc{1,:};
dicc{2,:};
dicc{3,:};
dicc{4,:};

cadena_fte=[4 2 1 1 3];
% Codificacion
cadena_cod=huffmanenco(cadena_fte, dicc);

% Decodificacion
cadena_estudio=[1 0 1 0 0 0 0 1 1];
cadena_codi=huffmandeco(cadena_estudio,dicc);

