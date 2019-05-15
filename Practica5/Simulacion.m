clear all;
close all;
clc;

G=[0 1 1 1 1 0 0 0;
    1 1 1 0 0 1 0 0;
    1 1 0 1 0 0 1 0;
    1 0 1 1 0 0 0 1];

msg=[1 1 0 1];

code=encode(msg,8,4,'linear',G)


%%
clear all;
close all;
clc;

G=[0 1 1 1 1 0 0 0;
    1 1 1 0 0 1 0 0;
    1 1 0 1 0 0 1 0;
    1 0 1 1 0 0 0 1];

s = dec2bin(0:2^4-1);

msg=[0 0 0 0;
    0 0 0 1;
    0 0 1 0;
    0 0 1 1;
    0 1 0 0;
    0 1 0 1;
    0 1 1 0;
    0 1 1 1;
    1 0 0 0;
    1 0 0 1;
    1 0 1 0;
    1 0 1 1;
    1 1 0 0;
    1 1 0 1;
    1 1 1 0;
    1 1 1 1];

for i=1:16
    msg(i,:)
    code=encode(msg(i,:),8,4,'linear',G)
end

