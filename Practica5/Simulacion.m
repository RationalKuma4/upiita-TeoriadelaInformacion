clear all;
close all;
clc;

G=[0 1 1 1 1 0 0 0;
    1 1 1 0 0 1 0 0;
    1 1 0 1 0 0 1 0;
    1 0 1 1 0 0 0 1];

msg=[1 1 0 1];

code=encode(msg,8,4,'linear',G)
