clear all;
close all;
clc;

I=eye(3);
P=[1 1 0 1;
    1 0 1 1;
    0 1 1 1];

g2=[I P]
Ht=[P;
    I]