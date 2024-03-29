close all;
clear all;
clc;

w=[1:5:20, 25:20:100, 130:50:300, 400:100:1000, 1250:250:5000, ...
    5500:500:10000];    % Ancho de banda
pn0_db=-20:1:30;        % SNR en decibeles
pn0=10.^(pn0_db/10);    % SNR
c=zeros(length(w), length(pn0_db));

% Se calcula la capacidad de canal
for i=1:45
    for j=1:51
        c(i,j)=w(i)*log2(1+pn0(j)/w(i));
    end
end

k=[.9 .8 .5 .6];    % Parametros adicionales
s=[-70 35];         % para la funcion surfl
surfl(w, pn0_db, c', s, k); 
title('Capacidad de canal vs. ancho de banda y SNR');
xlabel('Ancho de banda [Hz]');
ylabel('SNR [dB]');
zlabel('Capacidad de canal [bits/s�mbolo]');

