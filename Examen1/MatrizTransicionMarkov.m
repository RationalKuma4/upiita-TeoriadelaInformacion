clear all;
close all;
clc;

matrizTransicion=[1/3 1/3 1/3; 1/3 0 1/2; 3/4 1/3 0];

for i=2:20
    matrizTransicion^i
end



H_c=.3*(1/3)*log2(3)+.3*(1/3)*log2(3)+.2*(1/3)*log2(3)+...
    .3*(1/2)*log2(2)+.2*(1/2)*log2(2)+.1*(1/4)*log2(4)+...
    .3*(3/4)*log2(4/3)