clear all;
close all;
clc;    
% Matriz generadora ejercicio 1
G=[1 0 0 0 1 1 0;
    0 1 0 0 0 1 1;
    0 0 1 0 1 1 1;
    0 0 0 1 1 0 1];
msg=[1 1 0 1];  % Fila 14 de u del ejercicio 1
% Funcion de codificacion
% Parametros: msg,n,k,metodod de codificacion, matriz generadora
code=encode(msg,7,4,'linear',G); % Fila 14 de c
% Funcion de decodificacion
dec=decode(code,7,4,'linear',G); % Msg inicial

I = eye(11); % Identidad
paridad=[1 0 0 1; % Paridad
    1 1 1 1;
    0 0 1 1;
    1 0 1 1;
    0 1 0 1;
    1 1 0 0;
    0 1 1 0;
    1 1 1 0;
    0 1 1 1;
    1 1 0 1;
    1 0 1 0];
g=[I paridad];

msg1=[0,0,0,0,0,0,0,1,0,0,0]; % Fila 9 u
% Fila 9 de c del ejercicio 2
code1=encode(msg1,15,11,'linear',g);
dec1=decode(code1, 15, 11, 'linear', g);

msg2=[0,0,0,0,0,0,0,1,0,0,1]; % Fila 10 u
% Fila 10 de c del ejercicio 2
code2=encode(msg2,15,11,'linear',g);
dec2=decode(code2, 15, 11, 'linear', g);

